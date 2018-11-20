using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using BrightIdeasSoftware;
using IPFSSystemTray.Properties;
using Microsoft.Win32;
using IWshRuntimeLibrary;
using Newtonsoft.Json;
using Timer = System.Windows.Forms.Timer;
using File = System.IO.File;

namespace IPFSSystemTray
{
    public partial class Epona : Form
    {
        public Newtonsoft.Json.Linq.JObject IpfsConfig;
        public bool isShowingHashDialog = false;
        private bool settingNodeStatus = false;
        private bool isSettingsVisible = false;
        private bool isNodeRunning = false;
        private string pathExecutable = "ipfs.exe";
        private string jsonFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona\" + Settings.Default.JSONFileName;
        private string eponaFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona";
        private string eponaSharedFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona\Shared";
        private string eponaAddedFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona\Added";
        private string lockFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Epona\files_db.lock";

        ArrayList elements = new ArrayList();

        private Timer timer1;
        private int lastElementCount = -1;

        public Epona()
        {
            settingNodeStatus = true;
            InitializeComponent();
            InitializeListView();
            TryInit();
            TryStartDaemon();
            MountIpfs();
            FetchIpfsConfig();
            SetupEpona();
            settingNodeStatus = false;
        }

        private void FetchIpfsConfig()
        {
            string configJson = ExecuteCommand("config show");
            IpfsConfig = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(configJson);
        }

        private void SetupEpona()
        {
            if (!Directory.Exists(eponaFolderPath))
            {
                Directory.CreateDirectory(eponaFolderPath);
            }
            if (!Directory.Exists(eponaSharedFolderPath))
            {
                Directory.CreateDirectory(eponaSharedFolderPath);
            }
            if (!Directory.Exists(eponaAddedFolderPath))
            {
                Directory.CreateDirectory(eponaAddedFolderPath);
            }
            isNodeRunning = IsNodeRunning();
            UpdateNodeIcon();
            File.SetAttributes(jsonFilePath, FileAttributes.Hidden);

            if (File.Exists(lockFile))
            {
                File.Delete(lockFile);
            }
        }

        public bool IsNodeRunning()
        {
            return File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.ipfs\repo.lock");
        }

        public void SetNodeStatus(bool isRunning)
        {
            settingNodeStatus = true;
            if (isNodeRunning && !isRunning)
            {
                StopDaemon();
            }
            else if (!isNodeRunning && isRunning)
            {
                TryStartDaemon();
            }
            isNodeRunning = isRunning;
            UpdateNodeIcon();
            settingNodeStatus = false;
        }

        public void UpdateNodeIcon()
        {
            if (isNodeRunning)
            {
                toolStripMenuItem2.Image = Resources.Play;
            }
            else
            {
                toolStripMenuItem2.Image = Resources.Pause;
            }
        }

        public void InitTimer(int time)
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = time; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                mainLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void mainLoop()
        {
            if (IsNodeRunning())
            {
                parseIPFSObjects();
                checkMainTrigger();
                updateNodeStatus();
                checkSharedFolder();
                handleLinkOnlyElements();
            }
        }

        private void updateNodeStatus()
        {
            if (!settingNodeStatus)
            {
                SetNodeStatus(IsNodeRunning());
            }
        }

        private void handleLinkOnlyElements()
        {
            try
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    IPFSElement el = ((IPFSElement)elements[i]);
                    if (el.IsLinkOnly && !el.Active)
                    {
                        new Thread(delegate ()
                        {
                            el.Active = true;
                            string output = ExecuteCommand("pin add " + el.Hash, true);
                            if (output.Equals("pinned " + el.Hash + " recursively\n"))
                            {
                                ShowNotification("File Pinned!", System.Drawing.SystemIcons.Information, System.Windows.Forms.ToolTipIcon.Info, 5000);
                            }
                            if (File.Exists(jsonFilePath))
                            {
                                string json = JsonConvert.SerializeObject(elements);
                                File.SetAttributes(jsonFilePath, FileAttributes.Normal);
                                File.WriteAllText(jsonFilePath, json);
                                File.SetAttributes(jsonFilePath, FileAttributes.Hidden);

                            }
                        }).Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void checkSharedFolder()
        {
            try
            {
                string[] entries = Directory.GetFileSystemEntries(eponaSharedFolderPath, "*", SearchOption.AllDirectories);
                bool[] fileStatus = new bool[elements.Count];

                foreach (string entry in entries)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        IPFSElement el = (IPFSElement)elements[i];
                        if (entry.Equals(el.Path) || (Path.GetFileName(entry)).Equals(Path.GetFileName(el.Path.Replace(' ', '_') + ".lnk")))
                        {
                            fileStatus[i] = true;
                        }
                    }
                }
                for (int i = 0; i < fileStatus.Length; i++)
                {
                    if (i < elements.Count)
                    {
                        IPFSElement el = (IPFSElement)elements[i];
                        if (!fileStatus[i] && !el.IsLinkOnly)
                        {
                            RemoveFile(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void checkMainTrigger()
        {
            try
            {
                RegistryKey curUser = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64); //here you specify where exactly you want your entry

                var reg = curUser.OpenSubKey("Software\\Epona", true);
                if (reg != null)
                {
                    if (reg.GetValue("ShowMain", false).Equals("true"))
                    {
                        ShowConfig(false, true);
                        reg.SetValue("ShowMain", "false");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void SystemTray_Load(object sender, EventArgs e)
        {
        }

        public void ShowConfig(bool toggle = true, bool visibility = true)
        {
            SetPopupPosition();

            if (toggle)
            {
                // If we are already showing the window, merely focus it.
                if (!IPFSSystemTray.IsVisible)
                {
                    Show();
                    BringToFront();
                    Text = "Hide";
                }
                else
                {
                    Hide();
                    Text = "Show";
                }
                IPFSSystemTray.IsVisible = !IPFSSystemTray.IsVisible;
            }
            else
            {
                // If we are already showing the window, merely focus it.
                if (visibility)
                {
                    Show();
                    BringToFront();
                    Text = "Hide";
                }
                else
                {
                    Hide();
                    Text = "Show";
                }
                IPFSSystemTray.IsVisible = visibility;
            }

            SetPopupPosition();
        }

        public void ShowSettings()
        {
            isSettingsVisible = true;
            settingsPanel.Show();
        }

        public void HideSettings()
        {
            isSettingsVisible = false;
            settingsPanel.Hide();
        }

        private void SetPopupPosition()
        {
            //Determine "rightmost" screen
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = rightmost.WorkingArea.Right - this.Width;
            this.Top = rightmost.WorkingArea.Bottom - this.Height;
        }

        private void InitializeListView()
        {
            this.objectListView1.ShowImagesOnSubItems = true;
            this.objectListView1.CellToolTipShowing += new EventHandler<ToolTipShowingEventArgs>(olv_CellToolTipShowing);
            this.objectListView1.SmallImageList = new ImageList();
            this.objectListView1.SmallImageList.ImageSize = new Size(35, 35);
            this.objectListView1.SmallImageList.Images.Add(Resources.File);
            this.objectListView1.SmallImageList.Images.Add(Resources.Folder);
            this.objectListView1.SmallImageList.Images.Add(Resources.Hyperlink);
            this.objectListView1.SmallImageList.Images.Add(Resources.Open);
            this.objectListView1.SmallImageList.Images.Add(Resources.Remove);
            this.objectListView1.SmallImageList.Images.Add(Resources.ToggleOn);
            this.objectListView1.SmallImageList.Images.Add(Resources.ToggleOff);
            this.objectListView1.CellClick += (sender, args) =>
            {
                switch (args.ColumnIndex)
                {
                    case 2:
                        ToggleActiveState(args.RowIndex);
                        break;
                    case 3:
                        SetHyperLinkToClipboard(args.RowIndex);
                        break;
                    case 4:
                        OpenFolderLocation(args.RowIndex);
                        break;
                    case 5:
                        RemoveFile(args.RowIndex);
                        break;
                };
            };

            this.IconCol.ImageGetter = delegate (object rowObject)
            {
                IPFSElement el = (IPFSElement)rowObject;
                if (el.FileType.Equals(FileType.FILE))
                    return 0;
                else
                    return 1;
            };
            this.ActiveCol.ImageGetter = delegate (object rowObject)
            {
                IPFSElement el = (IPFSElement)rowObject;
                if (!el.IsLinkOnly && !el.IsHardLink)
                {
                    if (el.Active)
                        return 5;
                    else
                        return 6;
                }
                else
                {
                    return null;
                }
            };
            this.GetHyperlink.ImageGetter = delegate (object rowObject)
            {
                return 2;
            };

            this.OpenFolder.ImageGetter = delegate (object rowObject)
            {
                IPFSElement el = (IPFSElement)rowObject;
                if (!el.IsHardLink || el.FileType.Equals(FileType.FOLDER))
                {
                    return 3;
                }
                else
                {
                    return null;
                }
            };

            this.Remove.ImageGetter = delegate (object rowObject)
            {
                return 4;
            };

            parseIPFSObjects();
            InitTimer(Properties.Settings.Default.RefreshRate);
        }

        void olv_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
        {
            string t = string.Empty;
            switch (e.ColumnIndex)
            {
                case 3: t = "Copy Link to Clipboard"; break;
                case 4: t = "Open Folder"; break;
                case 5: t = "Remove File"; break;
            }
            e.Text = t;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // if click outside dialog -> Close Dlg
            if (m.Msg == 0x86) //0x86
            {
                if (this.Focused || IPFSSystemTray.IsVisible)
                {
                    if (!this.RectangleToScreen(this.DisplayRectangle).Contains(Cursor.Position))
                    {
                        this.Hide();
                        IPFSSystemTray.IsVisible = false;
                    }
                }
            }
        }

        private void objectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void SetHyperLinkToClipboard(int index)
        {
            if (index < elements.Count)
            {
                IPFSElement el = (IPFSElement)elements[index];
                Clipboard.SetText("https://ipfs.io/ipfs/" + el.Hash);
                var notification = new System.Windows.Forms.NotifyIcon()
                {
                    Visible = true,
                    Icon = System.Drawing.SystemIcons.Information,
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
        }

        private void OpenFolderLocation(int index)
        {
            if (index < elements.Count)
            {
                IPFSElement el = (IPFSElement)elements[index];
                if (el.FileType.Equals(FileType.FOLDER))
                {
                    Process.Start("explorer.exe", el.Path.Replace(@"/", @"\"));
                }
                else
                {
                    Process.Start("explorer.exe", Path.GetDirectoryName(el.Path));
                }
            }
        }

        private void RemoveFile(int index)
        {
            if (index < elements.Count)
            {
                IPFSElement el = ((IPFSElement)elements[index]);
                UnpinElement(el);

                objectListView1.RemoveObject(elements[index]);
                elements.RemoveAt(index);
                if (File.Exists(jsonFilePath))
                {
                    string json = JsonConvert.SerializeObject(elements);
                    File.SetAttributes(jsonFilePath, FileAttributes.Normal);
                    File.WriteAllText(jsonFilePath, json);
                    File.SetAttributes(jsonFilePath, FileAttributes.Hidden);
                }
            }
        }

        public void ToggleActiveState(int index)
        {
            if (index < elements.Count)
            {
                IPFSElement el = (IPFSElement)elements[index];
                el.Active = !el.Active;
                if (File.Exists(jsonFilePath))
                {
                    string json = JsonConvert.SerializeObject(elements);
                    File.SetAttributes(jsonFilePath, FileAttributes.Normal);
                    File.WriteAllText(jsonFilePath, json);
                    File.SetAttributes(jsonFilePath, FileAttributes.Hidden);
                }
                this.objectListView1.RefreshObjects(elements);

                if (el.Active)
                {
                    AddElement(el.Path);
                }
                else
                {
                    UnpinElement(el);
                }
            }
        }

        private void parseIPFSObjects()
        {
            try
            {
                if (File.Exists(jsonFilePath))
                {
                    File.SetAttributes(jsonFilePath, FileAttributes.Normal);
                    string json = File.ReadAllText(jsonFilePath);
                    File.SetAttributes(jsonFilePath, FileAttributes.Hidden);
                    List<IPFSElement> tempElements = JsonConvert.DeserializeObject<List<IPFSElement>>(json);
                    if (lastElementCount != tempElements.Count)
                    {
                        elements = new ArrayList(tempElements);
                        this.objectListView1.SetObjects(elements);
                        lastElementCount = elements.Count;
                    }
                }
                else
                {
                    new DirectoryInfo(Path.GetDirectoryName(jsonFilePath)).Create();
                    File.SetAttributes(jsonFilePath, FileAttributes.Normal);
                    File.WriteAllText(jsonFilePath, "[]");
                    File.SetAttributes(jsonFilePath, FileAttributes.Hidden);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void TryInit()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.ipfs"))
            {
                ExecuteCommand("init");
            }
        }

        public void TryStartDaemon()
        {
            if (!IsNodeRunning())
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
                    if (proc.StandardOutput.ReadLine().Equals("Daemon is ready"))
                    {
                        break;
                    }
                }
            }
        }

        public void MountIpfs()
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pathExecutable,
                    Arguments = "mount",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            proc.WaitForExit();
        }

        public void StopDaemon()
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pathExecutable,
                    Arguments = "shutdown",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            proc.WaitForExit();
        }

        public string AddElement(string filepath)
        {
            return ExecuteCommand("add -r -Q -w " + filepath);
        }

        public string UnpinElement(IPFSElement el, bool deleteFile = true)
        {
            try
            {
                if (deleteFile)
                {
                    if (el.IsLinkOnly)
                    {
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.ipfs\" + el.Name + ".lnk");
                        File.Delete(eponaSharedFolderPath + "\\" + el.Name + ".lnk");
                    }
                    else
                    {
                        if (el.FileType.Equals(FileType.FOLDER))
                        {
                            File.Delete(eponaSharedFolderPath + "\\" + el.Name + ".lnk");
                        }
                        else
                        {
                            File.Delete(eponaSharedFolderPath + "\\" + el.Name);
                        }
                    }
                }
                return ExecuteCommand("pin rm " + el.Hash);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string ExecuteCommand(string args, bool showOutput = false, bool returnOutput = true)
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
                output += proc.StandardOutput.ReadLine() + "\n";
                if (showOutput)
                {
                    Console.WriteLine(output);
                }
            }

            if (returnOutput)
            {
                return output;
            }
            return null;
        }

        private void confirmSettingsBtn_Click(object sender, EventArgs e)
        {
            HideSettings();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", eponaFolderPath);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetNodeStatus(!isNodeRunning);
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.SetAttributes(jsonFilePath, FileAttributes.Normal);
            string json = File.ReadAllText(jsonFilePath);
            File.SetAttributes(jsonFilePath, FileAttributes.Hidden);
            SendTextToNotepad(json);
        }

        #region Imports
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        //this is a constant indicating the window that we want to send a text message
        const int WM_SETTEXT = 0X000C;
        #endregion


        public static void SendTextToNotepad(string text)
        {
            Process notepad = Process.Start(@"notepad.exe");
            System.Threading.Thread.Sleep(100);
            IntPtr notepadTextbox = FindWindowEx(notepad.MainWindowHandle, IntPtr.Zero, "Edit", null);
            SendMessage(notepadTextbox, WM_SETTEXT, 0, text);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (!isSettingsVisible)
            {
                ShowSettings();
            }
            else
            {
                HideSettings();
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            string inputHash = string.Empty, inputName = string.Empty;
            if (ShowHashInputDialog(ref inputHash, ref inputName, Location).Equals(DialogResult.OK))
            {
                string hash = ExtractHash(inputHash);
                if (hash == null)
                {
                    ShowNotification("Uh-oh, something went wrong with adding the hash!", System.Drawing.SystemIcons.Application, System.Windows.Forms.ToolTipIcon.Warning, 5000);
                }
                else
                {
                    string name = hash;
                    if (!inputName.Equals(string.Empty))
                    {
                        name = inputName;
                    }
                    CreateShortcut(name, eponaAddedFolderPath, GetIPFSMountPath() + "\\ipfs\\" + hash);
                }
            }
        }

        private string ExtractHash(string inputHash)
        {
            if (inputHash.Trim().Equals(string.Empty))
            {
                return null;
            }
            else if (inputHash.Trim().StartsWith("Qm", StringComparison.InvariantCulture) && inputHash.Length == 46)
            {
                return inputHash.Trim();
            }
            else if (inputHash.Trim().StartsWith("https://ipfs.io/ipfs/"))
            {
                return inputHash.Trim().Replace("https://ipfs.io/ipfs/", string.Empty).Replace("/", string.Empty);
            }
            return null;
        }

        private DialogResult ShowHashInputDialog(ref string inputHash, ref string inputName, System.Drawing.Point position)
        {
            isShowingHashDialog = true;
            HashInputDialog hashDialog = new HashInputDialog();
            hashDialog.Location = position;
            hashDialog.Size = new Size(380, 180);

            DialogResult result = hashDialog.ShowDialog(this);
            if (result.Equals(DialogResult.OK))
            {
                inputHash = hashDialog.hashTextbox.Text;
                inputName = hashDialog.nameTextbox.Text;
            }
            hashDialog.Dispose();
            isShowingHashDialog = false;
            return result;
        }

        private string GetIPFSMountPath()
        {
            string path = IpfsConfig["Mounts"]["IPFS"].ToString();
            if (path.Equals("/ipfs"))
            {
                return "C:\\ipfs";
            }
            else
            {
                return path;
            }
        }

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
            notification.ShowBalloonTip((int)(duration / 2));
            Thread.Sleep((int)(duration / 2));

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            notification.Dispose();
        }
    }
}
