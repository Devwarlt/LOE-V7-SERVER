namespace Game_Launcher
{
    partial class Launcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.webclient = new System.Windows.Forms.WebBrowser();
            this.gameLauncherStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // webclient
            // 
            this.webclient.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.webclient.AllowNavigation = false;
            this.webclient.AllowWebBrowserDrop = false;
            this.webclient.IsWebBrowserContextMenuEnabled = false;
            this.webclient.Location = new System.Drawing.Point(0, 0);
            this.webclient.Margin = new System.Windows.Forms.Padding(0);
            this.webclient.Name = "webclient";
            this.webclient.ScriptErrorsSuppressed = true;
            this.webclient.ScrollBarsEnabled = false;
            this.webclient.Size = new System.Drawing.Size(800, 600);
            this.webclient.TabIndex = 0;
            this.webclient.Url = new System.Uri("http://testing.loesoft.org:1000/testing/loe/?platform=Desktop", System.UriKind.Absolute);
            this.webclient.WebBrowserShortcutsEnabled = false;
            this.webclient.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webclient_DocumentCompleted);
            // 
            // gameLauncherStatus
            // 
            this.gameLauncherStatus.BackColor = System.Drawing.SystemColors.Control;
            this.gameLauncherStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gameLauncherStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gameLauncherStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gameLauncherStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.gameLauncherStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.gameLauncherStatus.Location = new System.Drawing.Point(0, 600);
            this.gameLauncherStatus.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.gameLauncherStatus.Name = "gameLauncherStatus";
            this.gameLauncherStatus.Size = new System.Drawing.Size(800, 24);
            this.gameLauncherStatus.TabIndex = 1;
            this.gameLauncherStatus.Text = "[Game Launcher] Status: Loading...";
            this.gameLauncherStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 624);
            this.Controls.Add(this.gameLauncherStatus);
            this.Controls.Add(this.webclient);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Launcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[GL] LoE Realm";
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.WebBrowser webclient;
        private System.Windows.Forms.Label gameLauncherStatus;
    }
}

