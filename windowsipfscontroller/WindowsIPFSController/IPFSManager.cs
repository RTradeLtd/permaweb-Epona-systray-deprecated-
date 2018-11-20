using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using Newtonsoft.Json;
using Shell32;
using File = System.IO.File;

namespace WindowsIPFSController
{
    class IPFSManager
    {
        private string pathExecutable;
        ArrayList elements = new ArrayList();
        private string jsonFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona\" +
                                      Properties.Settings.Default.JSONFileName;
        private string eponaFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona";
        private string eponaSharedFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona\Shared";

        private string lockFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona\files_db.lock";
        private string waitFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona\files_db.wait";

        public bool HasWaited = false;

        public IPFSManager(string exePath)
        {
            bool canRead = !File.Exists(lockFile);

            while(!canRead)
            {
                Thread.Sleep(100);
                canRead = !File.Exists(lockFile);
                if(!canRead)
                {
                    using (File.Create(waitFile))
                    {
                    }
                    HasWaited = true;
                }
            }
            File.Delete(waitFile);
            CreateLock();

            pathExecutable = exePath;
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                elements = new ArrayList(JsonConvert.DeserializeObject<List<IPFSElement>>(json));
            }
        }

        public bool HasWaiters()
        {
            return File.Exists(waitFile);
        }

        public void CreateLock()
        {
            using (File.Create(lockFile))
            {
            }
        }

        public void DeleteLock()
        {
            var fi = new System.IO.FileInfo(lockFile);
            if (fi.Exists)
            {
                fi.Delete();
                fi.Refresh();
                while (fi.Exists)
                {
                    System.Threading.Thread.Sleep(100);
                    fi.Refresh();
                }
            }
        }

        public void TryInit()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.ipfs") || !Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.ipfs\keystore"))
            {
                ExecuteCommand("init");
            }
        }

        public void TryStartDaemon()
        {
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.ipfs\repo.lock"))
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pathExecutable,
                        Arguments = "daemon",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    if (Program.DEBUG)
                    {
                        Console.WriteLine(line);
                    }
                    if (line.Equals("Daemon is ready"))
                    {
                        break;
                    }
                }
            }
        }

        public IPFSElement ElementExists(string fpath)
        {
            foreach (IPFSElement el in elements)
            {
                string fname = Path.GetFileName(fpath);
                if (el.Name.Equals(fname) && (el.Path.Equals(fpath)) || el.Path.Equals(eponaSharedFolderPath + "\\" + fname))
                {
                    return el;
                }
            }
            return null;
        }

        public string AddElement(string filepath, bool makeHardLink, bool isLinkOnly)
        {
            string hash = null;
            string fname = Path.GetFileName(filepath);
            IPFSElement prevEl = ElementExists(filepath);
            if (prevEl != null && !isLinkOnly)
            {
                string pathToRemove = null;

                if(prevEl.IsHardLink)
                {
                    pathToRemove = eponaSharedFolderPath + "\\" + fname;
                }
                else if(!prevEl.IsLinkOnly)
                {
                    pathToRemove = prevEl.Path;
                }

                if (pathToRemove != null)
                {
                    if (prevEl.FileType.Equals(FileType.FILE))
                    {
                        File.Delete(pathToRemove);
                    }
                    else
                    {
                        Directory.Delete(pathToRemove);
                    }
                }
                elements.Remove(prevEl);
                hash = "unshared";
            }
            else
            {
                FileType ft;
                FileAttributes attr;
                if (!isLinkOnly)
                {
                    attr = File.GetAttributes(filepath);
                } else
                {
                    attr = File.GetAttributes(GetShortcutTargetFile(filepath));
                }

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    ft = FileType.FOLDER;
                }
                else
                {
                    ft = FileType.FILE;
                }
                string newFilePath = eponaSharedFolderPath + "\\" + fname;
                if (makeHardLink)
                {
                    if(ft.Equals(FileType.FILE))
                    {
                        CreateHardLink(newFilePath, filepath, IntPtr.Zero);
                        hash = ExecuteCommand("add -r -Q -w \"" + newFilePath + "\"");
                        elements.Add(new IPFSElement(fname, newFilePath, hash, true, ft, makeHardLink, isLinkOnly));
                    } else
                    {
                        // TODO: Temporarily Disabling junctions
                        //JunctionPoint.Create(eponaSharedFolderPath + "\\" + fname, filepath, false);
                        CreateShortcut(fname, eponaSharedFolderPath, filepath);
                        hash = ExecuteCommand("add -r -Q -w \"" + filepath + "\"");
                        elements.Add(new IPFSElement(fname, filepath, hash, true, ft, makeHardLink, isLinkOnly));
                    }
                    //hash = ExecuteCommand("add -r -Q -w " + eponaSharedFolderPath + "\\" + fname);
                    //elements.Add(new IPFSElement(fname, eponaSharedFolderPath + "\\" + fname, hash, true, ft, makeHardLink, isLinkOnly));
                }
                else if (isLinkOnly)
                {
                    if(fname.EndsWith(".lnk") && IsValidIPFSPath(GetShortcutTargetFile(filepath)))
                    {
                        hash = GetIPFSHashFromPath(GetShortcutTargetFile(filepath));
                        elements.Add(new IPFSElement(fname, GetShortcutTargetFile(filepath), hash, false, ft, makeHardLink, isLinkOnly));
                    }
                }
            }

            if (!File.Exists(jsonFilePath))
            {
                File.Create(jsonFilePath);
            }

            string json = JsonConvert.SerializeObject(elements);
            File.SetAttributes(jsonFilePath, FileAttributes.Normal);
            File.WriteAllText(jsonFilePath, json);
            File.SetAttributes(jsonFilePath, FileAttributes.Hidden);
            return hash;
        }

        public bool IsValidIPFSPath(string path)
        {
            string[] pathParts = path.Split('\\');
            return pathParts[pathParts.Length - 2].Equals("ipfs") && IsValidIPFSHash(GetIPFSHashFromPath(path));
        }

        public string GetIPFSHashFromPath(string path)
        {
            string[] pathParts = path.Split('\\');
            return pathParts[pathParts.Length - 1];
        }

        public bool IsValidIPFSHash(string hash)
        {
            return hash.StartsWith("Qm", StringComparison.InvariantCulture) && hash.Length == 46;
        }

        public string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Shell32.Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        public void CopyPathToClipboard(string hash)
        {
            Clipboard.SetText(ConvertHashToPath(hash));
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                BalloonTipText = "Link copied to Clipboard"
            };

            // Display for 5 seconds.
            notification.ShowBalloonTip(2000);
            Thread.Sleep(2500);

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            notification.Dispose();
        }

        public void ShowNotification(string text, System.Drawing.Icon systemIcon, System.Windows.Forms.ToolTipIcon icon, int duration)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = systemIcon,
                BalloonTipIcon = icon,
                BalloonTipText = text
            };

            // Display for 5 seconds.
            notification.ShowBalloonTip((int)(duration/2));
            Thread.Sleep((int)(duration / 2));

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            notification.Dispose();
        }

        private string ConvertHashToPath(string hash)
        {
            if (hash.Equals(string.Empty))
            {
                if (Program.DEBUG)
                {
                    Console.WriteLine("Empty hash...");
                }
                return string.Empty;
            }
            return "https://ipfs.io/ipfs/" + hash;
        }

        private string ExecuteCommand(string args, bool returnOutput = true)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pathExecutable,
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string output = string.Empty;
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                if (Program.DEBUG)
                {
                    Console.WriteLine(line);
                }
                output += line;
            }

            if (returnOutput)
            {
                return output;
            }
            return null;
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(
            string lpFileName,
            string lpExistingFileName,
            IntPtr lpSecurityAttributes
         );

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = shortcutName + " on Epona";   // The description of the shortcut
            shortcut.IconLocation = targetFileLocation;           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
    }
}
