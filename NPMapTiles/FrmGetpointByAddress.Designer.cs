namespace NPMapTiles
{
    partial class FrmGetpointByAddress
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
            this.cmbPointType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.btnStop = new DevComponents.DotNetBar.ButtonX();
            this.btnStart = new DevComponents.DotNetBar.ButtonX();
            this.progressBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.btnScanDirctory = new DevComponents.DotNetBar.ButtonX();
            this.txbDirctoryPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnScanFile = new DevComponents.DotNetBar.ButtonX();
            this.txbFilePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbPointType
            // 
            this.cmbPointType.DisplayMember = "Text";
            this.cmbPointType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPointType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPointType.FormattingEnabled = true;
            this.cmbPointType.ItemHeight = 15;
            this.cmbPointType.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.cmbPointType.Location = new System.Drawing.Point(103, 67);
            this.cmbPointType.Name = "cmbPointType";
            this.cmbPointType.Size = new System.Drawing.Size(121, 21);
            this.cmbPointType.TabIndex = 20;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "火星坐标";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "WGS84坐标";
            // 
            // btnStop
            // 
            this.btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(358, 120);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 19;
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStart
            // 
            this.btnStart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStart.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStart.Location = new System.Drawing.Point(276, 120);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 18;
            this.btnStart.Text = "转换";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(102, 95);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(331, 18);
            this.progressBar.TabIndex = 17;
            this.progressBar.TextVisible = true;
            // 
            // btnScanDirctory
            // 
            this.btnScanDirctory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScanDirctory.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScanDirctory.Location = new System.Drawing.Point(358, 38);
            this.btnScanDirctory.Name = "btnScanDirctory";
            this.btnScanDirctory.Size = new System.Drawing.Size(75, 23);
            this.btnScanDirctory.TabIndex = 16;
            this.btnScanDirctory.Text = "浏览...";
            this.btnScanDirctory.Click += new System.EventHandler(this.btnScanDirctory_Click);
            // 
            // txbDirctoryPath
            // 
            // 
            // 
            // 
            this.txbDirctoryPath.Border.Class = "TextBoxBorder";
            this.txbDirctoryPath.Location = new System.Drawing.Point(103, 39);
            this.txbDirctoryPath.Name = "txbDirctoryPath";
            this.txbDirctoryPath.Size = new System.Drawing.Size(248, 21);
            this.txbDirctoryPath.TabIndex = 15;
            // 
            // btnScanFile
            // 
            this.btnScanFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScanFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScanFile.Location = new System.Drawing.Point(358, 11);
            this.btnScanFile.Name = "btnScanFile";
            this.btnScanFile.Size = new System.Drawing.Size(75, 23);
            this.btnScanFile.TabIndex = 14;
            this.btnScanFile.Text = "浏览...";
            this.btnScanFile.Click += new System.EventHandler(this.btnScanFile_Click);
            // 
            // txbFilePath
            // 
            // 
            // 
            // 
            this.txbFilePath.Border.Class = "TextBoxBorder";
            this.txbFilePath.Location = new System.Drawing.Point(103, 12);
            this.txbFilePath.Name = "txbFilePath";
            this.txbFilePath.Size = new System.Drawing.Size(248, 21);
            this.txbFilePath.TabIndex = 13;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(445, 153);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 21;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(37, 65);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 14;
            this.labelX4.Text = "坐标类型";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(12, 94);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(84, 23);
            this.labelX3.TabIndex = 13;
            this.labelX3.Text = "当前处理进度";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 38);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(84, 23);
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "保存文件目录";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 11);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(84, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "导入坐标文件";
            // 
            // FrmGetpointByAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 153);
            this.Controls.Add(this.cmbPointType);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnScanDirctory);
            this.Controls.Add(this.txbDirctoryPath);
            this.Controls.Add(this.btnScanFile);
            this.Controls.Add(this.txbFilePath);
            this.Controls.Add(this.panelEx1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetpointByAddress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "逆地理编码";
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbPointType;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.DotNetBar.ButtonX btnStop;
        private DevComponents.DotNetBar.ButtonX btnStart;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBar;
        private DevComponents.DotNetBar.ButtonX btnScanDirctory;
        private DevComponents.DotNetBar.Controls.TextBoxX txbDirctoryPath;
        private DevComponents.DotNetBar.ButtonX btnScanFile;
        private DevComponents.DotNetBar.Controls.TextBoxX txbFilePath;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}