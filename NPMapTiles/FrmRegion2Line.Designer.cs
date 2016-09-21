namespace NPMapTiles
{
    partial class FrmRegion2Line
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnStop = new DevComponents.DotNetBar.ButtonX();
            this.btnCoverter = new DevComponents.DotNetBar.ButtonX();
            this.progressBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.btnScan = new DevComponents.DotNetBar.ButtonX();
            this.txbFilePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnScanDirctory = new DevComponents.DotNetBar.ButtonX();
            this.txbDirctoryPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.btnScanDirctory);
            this.panelEx1.Controls.Add(this.txbDirctoryPath);
            this.panelEx1.Controls.Add(this.labelX6);
            this.panelEx1.Controls.Add(this.btnStop);
            this.panelEx1.Controls.Add(this.btnCoverter);
            this.panelEx1.Controls.Add(this.progressBar);
            this.panelEx1.Controls.Add(this.labelX8);
            this.panelEx1.Controls.Add(this.btnScan);
            this.panelEx1.Controls.Add(this.txbFilePath);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(445, 125);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(358, 93);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 11;
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnCoverter
            // 
            this.btnCoverter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCoverter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCoverter.Location = new System.Drawing.Point(276, 93);
            this.btnCoverter.Name = "btnCoverter";
            this.btnCoverter.Size = new System.Drawing.Size(75, 23);
            this.btnCoverter.TabIndex = 10;
            this.btnCoverter.Text = "转换";
            this.btnCoverter.Click += new System.EventHandler(this.btnCoverter_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(75, 70);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(358, 18);
            this.progressBar.TabIndex = 9;
            this.progressBar.TextVisible = true;
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(32, 70);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(75, 23);
            this.labelX8.TabIndex = 8;
            this.labelX8.Text = "提示：";
            // 
            // btnScan
            // 
            this.btnScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScan.Location = new System.Drawing.Point(358, 11);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "浏览...";
            this.btnScan.Click += new System.EventHandler(this.btnScanFile_Click);
            // 
            // txbFilePath
            // 
            // 
            // 
            // 
            this.txbFilePath.Border.Class = "TextBoxBorder";
            this.txbFilePath.Location = new System.Drawing.Point(75, 12);
            this.txbFilePath.Name = "txbFilePath";
            this.txbFilePath.Size = new System.Drawing.Size(276, 21);
            this.txbFilePath.TabIndex = 1;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(13, 12);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "Shp文件：";
            // 
            // btnScanDirctory
            // 
            this.btnScanDirctory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScanDirctory.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScanDirctory.Location = new System.Drawing.Point(358, 40);
            this.btnScanDirctory.Name = "btnScanDirctory";
            this.btnScanDirctory.Size = new System.Drawing.Size(75, 23);
            this.btnScanDirctory.TabIndex = 14;
            this.btnScanDirctory.Text = "浏览...";
            this.btnScanDirctory.Click += new System.EventHandler(this.btnScanDirctory_Click);
            // 
            // txbDirctoryPath
            // 
            // 
            // 
            // 
            this.txbDirctoryPath.Border.Class = "TextBoxBorder";
            this.txbDirctoryPath.Location = new System.Drawing.Point(75, 41);
            this.txbDirctoryPath.Name = "txbDirctoryPath";
            this.txbDirctoryPath.Size = new System.Drawing.Size(276, 21);
            this.txbDirctoryPath.TabIndex = 13;
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(13, 41);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(75, 23);
            this.labelX6.TabIndex = 12;
            this.labelX6.Text = "保存路径：";
            // 
            // FrmShp2Excel
            // 
            this.ClientSize = new System.Drawing.Size(445, 125);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmShp2Excel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shp转Excel";
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnScan;
        private DevComponents.DotNetBar.Controls.TextBoxX txbFilePath;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBar;
        private DevComponents.DotNetBar.ButtonX btnStop;
        private DevComponents.DotNetBar.ButtonX btnCoverter;
        private DevComponents.DotNetBar.ButtonX btnScanDirctory;
        private DevComponents.DotNetBar.Controls.TextBoxX txbDirctoryPath;
        private DevComponents.DotNetBar.LabelX labelX6;
    }
}