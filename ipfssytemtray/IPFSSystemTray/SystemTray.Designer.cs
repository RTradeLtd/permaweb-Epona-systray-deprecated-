namespace IPFSSystemTray
{
    partial class Epona
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.IconCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.NameCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ActiveCol = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.GetHyperlink = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.OpenFolder = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Remove = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.confirmSettingsBtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.settingsPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.IconCol);
            this.objectListView1.AllColumns.Add(this.NameCol);
            this.objectListView1.AllColumns.Add(this.ActiveCol);
            this.objectListView1.AllColumns.Add(this.GetHyperlink);
            this.objectListView1.AllColumns.Add(this.OpenFolder);
            this.objectListView1.AllColumns.Add(this.Remove);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.BackColor = System.Drawing.SystemColors.Window;
            this.objectListView1.CausesValidation = false;
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IconCol,
            this.NameCol,
            this.ActiveCol,
            this.GetHyperlink,
            this.OpenFolder,
            this.Remove});
            this.objectListView1.CopySelectionOnControlC = false;
            this.objectListView1.CopySelectionOnControlCUsesDragSource = false;
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.HasCollapsibleGroups = false;
            this.objectListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.objectListView1.IsSearchOnSortColumn = false;
            this.objectListView1.Location = new System.Drawing.Point(0, 26);
            this.objectListView1.Margin = new System.Windows.Forms.Padding(2);
            this.objectListView1.MultiSelect = false;
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.SelectAllOnControlA = false;
            this.objectListView1.SelectColumnsMenuStaysOpen = false;
            this.objectListView1.SelectColumnsOnRightClick = false;
            this.objectListView1.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            this.objectListView1.ShowFilterMenuOnRightClick = false;
            this.objectListView1.ShowGroups = false;
            this.objectListView1.ShowHeaderInAllViews = false;
            this.objectListView1.ShowItemToolTips = true;
            this.objectListView1.ShowSortIndicators = false;
            this.objectListView1.Size = new System.Drawing.Size(402, 242);
            this.objectListView1.TabIndex = 0;
            this.objectListView1.TabStop = false;
            this.objectListView1.UpdateSpaceFillingColumnsWhenDraggingColumnDivider = false;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.SelectedIndexChanged += new System.EventHandler(this.ObjectListView1_SelectedIndexChanged);
            // 
            // IconCol
            // 
            this.IconCol.AspectName = "";
            this.IconCol.AutoCompleteEditor = false;
            this.IconCol.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.IconCol.Groupable = false;
            this.IconCol.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IconCol.Hideable = false;
            this.IconCol.IsEditable = false;
            this.IconCol.Searchable = false;
            this.IconCol.ShowTextInHeader = false;
            this.IconCol.Sortable = false;
            this.IconCol.Text = "";
            this.IconCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IconCol.UseFiltering = false;
            this.IconCol.Width = 40;
            // 
            // NameCol
            // 
            this.NameCol.AspectName = "Name";
            this.NameCol.AutoCompleteEditor = false;
            this.NameCol.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.NameCol.Groupable = false;
            this.NameCol.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NameCol.Hideable = false;
            this.NameCol.IsEditable = false;
            this.NameCol.MaximumWidth = 135;
            this.NameCol.MinimumWidth = 135;
            this.NameCol.Searchable = false;
            this.NameCol.ShowTextInHeader = false;
            this.NameCol.Sortable = false;
            this.NameCol.Text = "Name";
            this.NameCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NameCol.UseFiltering = false;
            this.NameCol.Width = 135;
            // 
            // ActiveCol
            // 
            this.ActiveCol.AspectName = "";
            this.ActiveCol.AutoCompleteEditor = false;
            this.ActiveCol.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.ActiveCol.Groupable = false;
            this.ActiveCol.Hideable = false;
            this.ActiveCol.IsEditable = false;
            this.ActiveCol.MaximumWidth = 50;
            this.ActiveCol.MinimumWidth = 50;
            this.ActiveCol.Searchable = false;
            this.ActiveCol.ShowTextInHeader = false;
            this.ActiveCol.Sortable = false;
            this.ActiveCol.Text = "";
            this.ActiveCol.UseFiltering = false;
            this.ActiveCol.Width = 50;
            // 
            // GetHyperlink
            // 
            this.GetHyperlink.AspectName = "";
            this.GetHyperlink.AutoCompleteEditor = false;
            this.GetHyperlink.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.GetHyperlink.Groupable = false;
            this.GetHyperlink.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GetHyperlink.Hideable = false;
            this.GetHyperlink.IsEditable = false;
            this.GetHyperlink.MaximumWidth = 50;
            this.GetHyperlink.MinimumWidth = 50;
            this.GetHyperlink.Searchable = false;
            this.GetHyperlink.ShowTextInHeader = false;
            this.GetHyperlink.Sortable = false;
            this.GetHyperlink.Text = "";
            this.GetHyperlink.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.GetHyperlink.UseFiltering = false;
            this.GetHyperlink.Width = 50;
            // 
            // OpenFolder
            // 
            this.OpenFolder.AspectName = "";
            this.OpenFolder.AutoCompleteEditor = false;
            this.OpenFolder.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.OpenFolder.Groupable = false;
            this.OpenFolder.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.OpenFolder.Hideable = false;
            this.OpenFolder.IsEditable = false;
            this.OpenFolder.MaximumWidth = 50;
            this.OpenFolder.MinimumWidth = 50;
            this.OpenFolder.Searchable = false;
            this.OpenFolder.ShowTextInHeader = false;
            this.OpenFolder.Sortable = false;
            this.OpenFolder.Text = "";
            this.OpenFolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.OpenFolder.UseFiltering = false;
            this.OpenFolder.Width = 50;
            // 
            // Remove
            // 
            this.Remove.AutoCompleteEditor = false;
            this.Remove.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.Remove.Groupable = false;
            this.Remove.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Remove.IsEditable = false;
            this.Remove.MaximumWidth = 50;
            this.Remove.MinimumWidth = 50;
            this.Remove.Searchable = false;
            this.Remove.ShowTextInHeader = false;
            this.Remove.Sortable = false;
            this.Remove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Remove.UseFiltering = false;
            this.Remove.Width = 50;
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.settingsPanel.Controls.Add(this.label1);
            this.settingsPanel.Controls.Add(this.confirmSettingsBtn);
            this.settingsPanel.Location = new System.Drawing.Point(0, 27);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(402, 241);
            this.settingsPanel.TabIndex = 1;
            this.settingsPanel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Settings";
            // 
            // confirmSettingsBtn
            // 
            this.confirmSettingsBtn.Location = new System.Drawing.Point(164, 200);
            this.confirmSettingsBtn.Name = "confirmSettingsBtn";
            this.confirmSettingsBtn.Size = new System.Drawing.Size(75, 23);
            this.confirmSettingsBtn.TabIndex = 0;
            this.confirmSettingsBtn.Text = "OK";
            this.confirmSettingsBtn.UseVisualStyleBackColor = true;
            this.confirmSettingsBtn.Click += new System.EventHandler(this.ConfirmSettingsBtn_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.xToolStripMenuItem,
            this.toolStripMenuItem4});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip1.ShowItemToolTips = true;
            this.menuStrip1.Size = new System.Drawing.Size(402, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::IPFSSystemTray.Properties.Resources.Folder;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(28, 20);
            this.toolStripMenuItem1.ToolTipText = "Open Epona Folder";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = global::IPFSSystemTray.Properties.Resources.Play;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(28, 20);
            this.toolStripMenuItem2.ToolTipText = "Start/Stop Node";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Image = global::IPFSSystemTray.Properties.Resources.Export;
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.xToolStripMenuItem.ToolTipText = "Export";
            this.xToolStripMenuItem.Click += new System.EventHandler(this.xToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Image = global::IPFSSystemTray.Properties.Resources.Add;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(28, 20);
            this.toolStripMenuItem4.ToolTipText = "Add New Hash";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.menuStrip2.Location = new System.Drawing.Point(364, 268);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.menuStrip2.Size = new System.Drawing.Size(36, 24);
            this.menuStrip2.TabIndex = 3;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Image = global::IPFSSystemTray.Properties.Resources.Settings;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(28, 20);
            this.toolStripMenuItem3.ToolTipText = "Settings";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // Epona
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 291);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.objectListView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Epona";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SystemTray_Load);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn IconCol;
        private BrightIdeasSoftware.OLVColumn NameCol;
        private BrightIdeasSoftware.OLVColumn ActiveCol;
        private BrightIdeasSoftware.OLVColumn GetHyperlink;
        private BrightIdeasSoftware.OLVColumn OpenFolder;
        private BrightIdeasSoftware.OLVColumn Remove;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Button confirmSettingsBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
    }
}

