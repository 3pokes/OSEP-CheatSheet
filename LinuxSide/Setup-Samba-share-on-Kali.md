# Setup Samba share on Kali

1. Install samba, make a backup of its configuration and create a fresh configuration file.

```bash
sudo apt install samba

sudo mv /etc/samba/smb.conf /etc/samba/smb.conf.old

sudo nano /etc/samba/smb.conf
```

2. Create the new simple SMB configuration file

```bash
[visualstudio]
 path = /home/kali/data
 browseable = yes
 read only = no
```

3. Create a samba user that can access the share and then start the required services

```bash
sudo smbpasswd -a kali

sudo systemctl start smbd

sudo systemctl start nmbd

```

4. Create the shared folder and open up the permissions for Visual Studio

```bash

mkdir /home/kali/data

chmod -R 777 /home/kali/data

```

5. On the windows machine we can access the share in File Explorer by browsing it "\\192.168.x.x", then login with the samba user credentials.