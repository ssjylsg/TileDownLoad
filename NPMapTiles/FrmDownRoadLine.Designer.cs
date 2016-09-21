namespace NPMapTiles
{
    partial class FrmDownRoadLine
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
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnDown = new DevComponents.DotNetBar.ButtonX();
            this.progressBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.labMessage = new DevComponents.DotNetBar.LabelX();
            this.btnScan = new DevComponents.DotNetBar.ButtonX();
            this.txbPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbCity = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbProvince = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.btnDown);
            this.panelEx1.Controls.Add(this.progressBar);
            this.panelEx1.Controls.Add(this.labMessage);
            this.panelEx1.Controls.Add(this.btnScan);
            this.panelEx1.Controls.Add(this.txbPath);
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Controls.Add(this.groupBox1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(410, 193);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(326, 159);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDown
            // 
            this.btnDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDown.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDown.Location = new System.Drawing.Point(245, 159);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 6;
            this.btnDown.Text = "下载";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 121);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(389, 13);
            this.progressBar.TabIndex = 5;
            this.progressBar.Text = "progressBarX1";
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.Location = new System.Drawing.Point(12, 140);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(44, 18);
            this.labMessage.TabIndex = 4;
            this.labMessage.Text = "提示：";
            // 
            // btnScan
            // 
            this.btnScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScan.Location = new System.Drawing.Point(328, 89);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(73, 23);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "浏览...";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txbPath
            // 
            // 
            // 
            // 
            this.txbPath.Border.Class = "TextBoxBorder";
            this.txbPath.Location = new System.Drawing.Point(68, 90);
            this.txbPath.Name = "txbPath";
            this.txbPath.Size = new System.Drawing.Size(254, 21);
            this.txbPath.TabIndex = 1;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(12, 92);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 2;
            this.labelX4.Text = "存储路径";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbCity);
            this.groupBox1.Controls.Add(this.labelX2);
            this.groupBox1.Controls.Add(this.cmbProvince);
            this.groupBox1.Controls.Add(this.labelX1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "区域选择";
            // 
            // cmbCity
            // 
            this.cmbCity.DisplayMember = "Text";
            this.cmbCity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCity.FormattingEnabled = true;
            this.cmbCity.ItemHeight = 15;
            this.cmbCity.Location = new System.Drawing.Point(198, 40);
            this.cmbCity.Name = "cmbCity";
            this.cmbCity.Size = new System.Drawing.Size(185, 21);
            this.cmbCity.TabIndex = 2;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(199, 18);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(121, 23);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "城市";
            // 
            // cmbProvince
            // 
            this.cmbProvince.DisplayMember = "Text";
            this.cmbProvince.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProvince.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvince.FormattingEnabled = true;
            this.cmbProvince.ItemHeight = 15;
            this.cmbProvince.Location = new System.Drawing.Point(6, 40);
            this.cmbProvince.Name = "cmbProvince";
            this.cmbProvince.Size = new System.Drawing.Size(174, 21);
            this.cmbProvince.TabIndex = 0;
            this.cmbProvince.SelectedIndexChanged += new System.EventHandler(this.cmbProvince_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(7, 18);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "省份";
            // 
            // FrmDownRoadLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 193);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(426, 231);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(426, 231);
            this.Name = "FrmDownRoadLine";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "路网数据下载";
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnDown;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBar;
        private DevComponents.DotNetBar.LabelX labMessage;
        private DevComponents.DotNetBar.ButtonX btnScan;
        private DevComponents.DotNetBar.Controls.TextBoxX txbPath;
        private DevComponents.DotNetBar.LabelX labelX4;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCity;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbProvince;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}