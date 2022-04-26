namespace WindowsFormsApp1
{
    partial class Crypto
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

        #region Windows Form Designer Code

        /// <summary>
        /// Required method for Designer support
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Crypto));
            this.tbFilePath = new System.Windows.Forms.TextBox();
            this.lbFilePath = new System.Windows.Forms.Label();
            this.btBrowseFile = new System.Windows.Forms.Button();
            this.rbDecode = new System.Windows.Forms.RadioButton();
            this.rbEncode = new System.Windows.Forms.RadioButton();
            this.lbPassword = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btStart = new System.Windows.Forms.Button();
            this.cbShowPassword = new System.Windows.Forms.CheckBox();
            this.pbLoading = new System.Windows.Forms.ProgressBar();
            this.lbProgress = new System.Windows.Forms.Label();
            this.lbFileSize = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.tbFolderPath = new System.Windows.Forms.TextBox();
            this.btBrowseFolder = new System.Windows.Forms.Button();
            this.lbFolderPath = new System.Windows.Forms.Label();
            this.lbMode = new System.Windows.Forms.Label();
            this.ToolTipError = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // tbFilePath
            // 
            this.tbFilePath.AllowDrop = true;
            resources.ApplyResources(this.tbFilePath, "tbFilePath");
            this.tbFilePath.Name = "tbFilePath";
            this.tbFilePath.TextChanged += new System.EventHandler(this.tbFilePath_TextChanged);
            this.tbFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFilePath_DragDrop);
            this.tbFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFilePath_DragEnter);
            // 
            // lbFilePath
            // 
            resources.ApplyResources(this.lbFilePath, "lbFilePath");
            this.lbFilePath.Name = "lbFilePath";
            // 
            // btBrowseFile
            // 
            resources.ApplyResources(this.btBrowseFile, "btBrowseFile");
            this.btBrowseFile.Name = "btBrowseFile";
            this.btBrowseFile.UseVisualStyleBackColor = true;
            this.btBrowseFile.Click += new System.EventHandler(this.btBrowseFile_Click);
            // 
            // rbDecode
            // 
            resources.ApplyResources(this.rbDecode, "rbDecode");
            this.rbDecode.Name = "rbDecode";
            this.rbDecode.UseVisualStyleBackColor = true;
            this.rbDecode.CheckedChanged += new System.EventHandler(this.rbDecode_CheckedChanged);
            // 
            // rbEncode
            // 
            resources.ApplyResources(this.rbEncode, "rbEncode");
            this.rbEncode.Checked = true;
            this.rbEncode.Name = "rbEncode";
            this.rbEncode.TabStop = true;
            this.rbEncode.UseVisualStyleBackColor = true;
            this.rbEncode.CheckedChanged += new System.EventHandler(this.rbEncode_CheckedChanged);
            // 
            // lbPassword
            // 
            resources.ApplyResources(this.lbPassword, "lbPassword");
            this.lbPassword.Name = "lbPassword";
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.Name = "tbPassword";
            // 
            // btStart
            // 
            this.btStart.Cursor = System.Windows.Forms.Cursors.Arrow;
            resources.ApplyResources(this.btStart, "btStart");
            this.btStart.Name = "btStart";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // cbShowPassword
            // 
            resources.ApplyResources(this.cbShowPassword, "cbShowPassword");
            this.cbShowPassword.Name = "cbShowPassword";
            this.cbShowPassword.UseVisualStyleBackColor = true;
            this.cbShowPassword.CheckedChanged += new System.EventHandler(this.cbShowPassword_CheckedChanged);
            // 
            // pbLoading
            // 
            this.pbLoading.ForeColor = System.Drawing.Color.LimeGreen;
            resources.ApplyResources(this.pbLoading, "pbLoading");
            this.pbLoading.Maximum = 10;
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // lbProgress
            // 
            resources.ApplyResources(this.lbProgress, "lbProgress");
            this.lbProgress.Name = "lbProgress";
            // 
            // lbFileSize
            // 
            resources.ApplyResources(this.lbFileSize, "lbFileSize");
            this.lbFileSize.Name = "lbFileSize";
            // 
            // lbStatus
            // 
            this.lbStatus.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbStatus, "lbStatus");
            this.lbStatus.Name = "lbStatus";
            // 
            // tbFolderPath
            // 
            this.tbFolderPath.AllowDrop = true;
            resources.ApplyResources(this.tbFolderPath, "tbFolderPath");
            this.tbFolderPath.Name = "tbFolderPath";
            this.tbFolderPath.TextChanged += new System.EventHandler(this.tbFolderPath_TextChanged);
            this.tbFolderPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbFolderPath_DragDrop);
            this.tbFolderPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbFolderPath_DragEnter);
            // 
            // btBrowseFolder
            // 
            resources.ApplyResources(this.btBrowseFolder, "btBrowseFolder");
            this.btBrowseFolder.Name = "btBrowseFolder";
            this.btBrowseFolder.UseVisualStyleBackColor = true;
            this.btBrowseFolder.Click += new System.EventHandler(this.btBrowseFolder_Click);
            // 
            // lbFolderPath
            // 
            resources.ApplyResources(this.lbFolderPath, "lbFolderPath");
            this.lbFolderPath.Name = "lbFolderPath";
            // 
            // lbMode
            // 
            resources.ApplyResources(this.lbMode, "lbMode");
            this.lbMode.Name = "lbMode";
            // 
            // ToolTipError
            // 
            this.ToolTipError.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.ToolTipError.ToolTipTitle = "Warning";
            // 
            // Crypto
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.lbMode);
            this.Controls.Add(this.lbFolderPath);
            this.Controls.Add(this.btBrowseFolder);
            this.Controls.Add(this.tbFolderPath);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbFileSize);
            this.Controls.Add(this.lbProgress);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.cbShowPassword);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.rbEncode);
            this.Controls.Add(this.rbDecode);
            this.Controls.Add(this.btBrowseFile);
            this.Controls.Add(this.lbFilePath);
            this.Controls.Add(this.tbFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Crypto";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Crypto_HelpButtonClicked);
            this.Load += new System.EventHandler(this.Crypto_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFilePath;
        private System.Windows.Forms.Label lbFilePath;
        private System.Windows.Forms.Button btBrowseFile;
        private System.Windows.Forms.RadioButton rbDecode;
        private System.Windows.Forms.RadioButton rbEncode;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.CheckBox cbShowPassword;
        private System.Windows.Forms.ProgressBar pbLoading;
        private System.Windows.Forms.Label lbProgress;
        private System.Windows.Forms.Label lbFileSize;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TextBox tbFolderPath;
        private System.Windows.Forms.Button btBrowseFolder;
        private System.Windows.Forms.Label lbFolderPath;
        private System.Windows.Forms.Label lbMode;
        private System.Windows.Forms.ToolTip ToolTipError;
    }
}

