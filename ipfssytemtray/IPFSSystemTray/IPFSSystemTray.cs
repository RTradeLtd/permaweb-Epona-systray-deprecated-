using System;
using System.Diagnostics;
using System.Windows.Forms;
using IPFSSystemTray.Properties;
using Microsoft.Win32;

namespace IPFSSystemTray
{
    class IPFSSystemTray : ApplicationContext
    {
        Epona configWindow = new Epona();
        NotifyIcon notifyIcon = new NotifyIcon();
        private MenuItem configMenuItem;
        private MenuItem exitMenuItem;
        static public bool IsVisible = false;

        public IPFSSystemTray()
        {
            configMenuItem = new MenuItem("Show", new EventHandler(ShowConfig));
            exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));
            notifyIcon.Icon = Resources.AppIcon;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
                { configMenuItem, exitMenuItem });
            notifyIcon.Visible = true;
            notifyIcon.Click += new System.EventHandler(this.ShowConfig);
            configWindow.Deactivate += new System.EventHandler(this.onLostFocus);
            notifyIcon.Text = "Epona";
            AddEponaToDesktopContextMenu();
            AddEponaFileToContextMenu();
            AddEponaDirectoryToUserContextMenu();
            AddEponaFileStubToContextMenu();
            SetupCloudStorageProvider();
        }

        public void SetupCloudStorageProvider()
        {
            string cspSetupFile = "set_cloud_storage_provider.bat";
            if (System.IO.File.Exists(cspSetupFile))
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = cspSetupFile,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();
            }
        }

        public void AddEponaToDesktopContextMenu()
        {
            RegistryKey rootMachine = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);

            var reg = rootMachine.OpenSubKey("DesktopBackground\\Shell\\Show Epona", true);
            if (reg == null)
            {
                var regSettings = rootMachine.CreateSubKey("DesktopBackground\\Shell\\Show Epona", true);
                regSettings.SetValue("Icon", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\IPFSSystemTray.exe");

                var regCommand = regSettings.CreateSubKey("command", true);
                regCommand.SetValue(string.Empty, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\WindowsIPFSController.exe --main");
            }
        }

        public void AddEponaFileToContextMenu()
        {
            RegistryKey rootMachine = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);

            var reg = rootMachine.OpenSubKey("*\\shell", true);
            if (reg.OpenSubKey("Share Link") == null)
            {
                var mirrorKey = reg.CreateSubKey("Share Link", true);
                mirrorKey.SetValue("Icon", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Epona.exe");

                var commandMirrorKey = mirrorKey.CreateSubKey("command", true);
                commandMirrorKey.SetValue(string.Empty, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\WindowsIPFSController.exe --hard-link \"%1\"");
            }
            reg.OpenSubKey("Share Link", true).OpenSubKey("command", true).SetValue(string.Empty, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\WindowsIPFSController.exe --hard-link \"%1\"");
        }

        public void AddEponaFileStubToContextMenu()
        {
            RegistryKey userMachine = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            var reg = userMachine.OpenSubKey("Software\\Classes\\lnkfile", true);
            var lnkShellKey = reg.OpenSubKey("shell", true);
            if (lnkShellKey == null)
            {
                lnkShellKey = reg.CreateSubKey("shell", true);
            }

            var dlKey = lnkShellKey.OpenSubKey("Download to Epona", true);
            if (dlKey == null)
            {
                dlKey = lnkShellKey.CreateSubKey("Download to Epona", true);
            }
            dlKey.SetValue("Icon", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Epona.exe");

            var commandDlKey = dlKey.OpenSubKey("command", true);
            if (commandDlKey == null) {
                commandDlKey = dlKey.CreateSubKey("command", true);
            }
            commandDlKey.SetValue(string.Empty, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\WindowsIPFSController.exe --link-only \"%1\"");
        }

        public void AddEponaDirectoryToUserContextMenu()
        {
            RegistryKey rootMachine = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);

            var reg = rootMachine.OpenSubKey("Directory\\shell", true);
            var mirrorKey = reg.OpenSubKey("Share Link", true);

            if (mirrorKey == null)
            {
                mirrorKey = reg.CreateSubKey("Share Link", true);
            }
            mirrorKey.SetValue("Icon", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Epona.exe");

            var commandMirrorKey = mirrorKey.OpenSubKey("command", true);
            if (commandMirrorKey == null)
            {
                commandMirrorKey = mirrorKey.CreateSubKey("command", true);
            }
            commandMirrorKey.SetValue(string.Empty, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\WindowsIPFSController.exe --hard-link \"%1\"");
        }

        private void onLostFocus(object sender, EventArgs e)
        {
            if (!configWindow.isShowingHashDialog)
            {
                configWindow.HideSettings();
                configWindow.Hide();
                IsVisible = false;
            }
        }

        public void ShowConfig(object sender, EventArgs e)
        {
            configWindow.ShowConfig();
        }

        void Exit(object sender, EventArgs e)
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
