using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsIPFSController
{

    class Program
    {
        private static string pathExecutable = "ipfs.exe";
        private static IPFSManager ipfs;
        public static bool DEBUG;
        public static bool makeHardLink = false;
        public static bool linkOnly = false;

        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            //args = new string[] { @"C:/Users/igrek/Desktop/Loomis Figure Draw.pdf", "--hard-link", "--debug" };
            //args = new string[] { @"C:/Users/igrek/Epona/Added/New QR Website.lnk", "--link-only", "--debug" };
            if (args.Length > 0)
            {
                string paths = "";
                args = Escape(args);
                foreach (string path in args)
                {
                    if (path.Equals("--main"))
                    {
                        RegistryKey curUser = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64); //here you specify where exactly you want your entry

                        var settingsReg = curUser.OpenSubKey("Software\\Epona", true);
                        if (settingsReg == null)
                        {
                            settingsReg = curUser.CreateSubKey("Software\\Epona", true);
                        }
                        settingsReg.SetValue("ShowMain", "true");

                        if (!IsProcessOpen("Epona"))
                        {
                            StartEponaExecutable();
                        }
                        Environment.Exit(0);
                    }
                    if (path.Equals("--hard-link"))
                    {
                        makeHardLink = true;
                    }
                    else if (path.Equals("--link-only"))
                    {
                        linkOnly = true;
                    }
                    else if (!path.Equals("--debug"))
                    {
                        paths += path + " ";
                    }
                    else
                    {
                        DEBUG = true;
                        Console.WriteLine("DEBUG MODE: ON");
                    }
                }
                HandleNewItem(paths.Trim());

                if (!IsProcessOpen("Epona"))
                {
                    StartEponaExecutable();
                }
            }
            else
            {
                Console.WriteLine("Usage: WindowsIPFSController.exe file/folder [file/folder] ...");
            }
        }

        public static bool IsProcessOpen(string name)
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running.
                //Remember, if you have the process running more than once, 
                //say IE open 4 times the loop thr way it is now will close all 4,
                //if you want it to just close the first one it finds
                //then add a return; after the Kill
                if (clsProcess.ProcessName.Contains(name))
                {
                    //if the process is found to be running then we
                    //return a true
                    return true;
                }
            }
            //otherwise we return a false
            return false;

        }
        public static void StartEponaExecutable()
        {
            string eponaExe = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Epona.exe";
            if (System.IO.File.Exists(eponaExe))
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = eponaExe,
                        UseShellExecute = true,
                        RedirectStandardOutput = false,
                        WindowStyle = ProcessWindowStyle.Maximized,
                        CreateNoWindow = false,
                        WorkingDirectory = Path.GetDirectoryName(eponaExe)
                    }
                };

                proc.Start();
            }
        }

        static string[] Escape(string[] args)
        {
            return args.Select(s => s.Contains(" ") ? Regex.Replace(s, @"(\\+)$", @"$1$1") : s).ToArray();
        }


        static void HandleNewItem(string path)
        {
            ipfs = new IPFSManager(pathExecutable);
            ipfs.TryInit();
            ipfs.TryStartDaemon();

            string hash = ipfs.AddElement(path, makeHardLink, linkOnly);
            if (!ipfs.HasWaiters())
            {
                if (!ipfs.HasWaited)
                {
                    if (hash == null)
                    {
                        ipfs.ShowNotification("Uh-oh, something went wrong with creating the hash!", System.Drawing.SystemIcons.Application, System.Windows.Forms.ToolTipIcon.Warning, 5000);
                    }
                    else if (!hash.Equals("unshared"))
                    {
                        if (hash != null && !hash.Equals(""))
                        {
                            ipfs.CopyPathToClipboard(hash);
                        }
                        else
                        {
                            ipfs.ShowNotification("Uh-oh, something went wrong!", System.Drawing.SystemIcons.Application, System.Windows.Forms.ToolTipIcon.Warning, 5000);
                        }
                    }
                    else
                    {
                        ipfs.ShowNotification("File unshared!", System.Drawing.SystemIcons.Information, System.Windows.Forms.ToolTipIcon.Info, 5000);
                    }
                }
                else
                {
                    ipfs.ShowNotification("Multiple Files Shared!", System.Drawing.SystemIcons.Information, System.Windows.Forms.ToolTipIcon.Info, 5000);
                }
            }

        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            ipfs.DeleteLock();
        }
    }
}
