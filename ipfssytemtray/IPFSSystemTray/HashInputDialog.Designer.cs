using System;
using System.Runtime.InteropServices;

namespace IPFSSystemTray
{
    partial class HashInputDialog
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
            this.confirmButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.hashTextbox = new System.Windows.Forms.TextBox();
            this.nameTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(71, 106);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(80, 23);
            this.confirmButton.TabIndex = 0;
            this.confirmButton.Text = "OK";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(226, 106);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            // 
            // hashTextbox
            // 
            this.hashTextbox.Location = new System.Drawing.Point(28, 27);
            this.hashTextbox.Name = "hashTextbox";
            this.hashTextbox.Size = new System.Drawing.Size(310, 20);
            this.hashTextbox.TabIndex = 1;
            this.SetPlaceholderMessage(this.hashTextbox.Handle, "Hash");
            // 
            // nameTextbox
            // 
            this.nameTextbox.Location = new System.Drawing.Point(28, 64);
            this.nameTextbox.Name = "nameTextbox";
            this.nameTextbox.Size = new System.Drawing.Size(310, 20);
            this.nameTextbox.TabIndex = 2;
            this.SetPlaceholderMessage(this.nameTextbox.Handle, "Name (Optional)");
            // 
            // HashInputDialog
            // 
            this.AcceptButton = this.confirmButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(364, 141);
            this.ControlBox = false;
            this.Controls.Add(this.nameTextbox);
            this.Controls.Add(this.hashTextbox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.confirmButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HashInputDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Add New Hash";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button confirmButton;
        public System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.TextBox hashTextbox;
        public System.Windows.Forms.TextBox nameTextbox;
        
        // To set placeholder for textboxes
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        public void SetPlaceholderMessage(IntPtr handlePtr, string placeholderText)
        {
            SendMessage(handlePtr, EM_SETCUEBANNER, 0, placeholderText);
        }
    }
}