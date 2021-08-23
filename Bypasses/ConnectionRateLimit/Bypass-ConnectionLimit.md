# Bypass connections limit

## Methode 1 - By kicking out the logged user.

List active session in the target server.

```bash
C:\Windows\system32>query session /server:SERVER_NAME
query session /server:SERVER_NAME
 SESSIONNAME       USERNAME                 ID  STATE   TYPE        DEVICE 
 services                                    0  Disc                        
 console           v0lk3n                    1  Active                      
 rdp-tcp                                 65536  Listen      
```

Reset the active session to kick out the connected user.

```bash
C:\Windows\system32>reset session 1
reset session 1
```

As we kicked a logged user, the connection limit had one connection free, this should allow us to connect througt RDP now.