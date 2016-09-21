namespace NPMapTiles
{
    partial class FrmPOIDown
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
            this.txbKeyWord = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txbPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnScan = new DevComponents.DotNetBar.ButtonX();
            this.progressBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.btnDown = new DevComponents.DotNetBar.ButtonX();
            this.btnStop = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labMessage = new DevComponents.DotNetBar.LabelX();
            this.checkBoxCreateShp = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.ckbExcel = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbKeyWord
            // 
            // 
            // 
            // 
            this.txbKeyWord.Border.Class = "TextBoxBorder";
            this.txbKeyWord.Location = new System.Drawing.Point(71, 12);
            this.txbKeyWord.Name = "txbKeyWord";
            this.txbKeyWord.Size = new System.Drawing.Size(125, 21);
            this.txbKeyWord.TabIndex = 1;
            // 
            // txbPath
            // 
            // 
            // 
            // 
            this.txbPath.Border.Class = "TextBoxBorder";
            this.txbPath.Location = new System.Drawing.Point(71, 42);
            this.txbPath.Name = "txbPath";
            this.txbPath.Size = new System.Drawing.Size(278, 21);
            this.txbPath.TabIndex = 3;
            // 
            // btnScan
            // 
            this.btnScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScan.Location = new System.Drawing.Point(356, 42);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 4;
            this.btnScan.Text = "浏览...";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 73);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(418, 10);
            this.progressBar.TabIndex = 5;
            this.progressBar.Text = "progressBarX1";
            // 
            // btnDown
            // 
            this.btnDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDown.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDown.Location = new System.Drawing.Point(274, 109);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "下载";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnStop
            // 
            this.btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStop.Location = new System.Drawing.Point(356, 109);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.ckbExcel);
            this.panelEx1.Controls.Add(this.labMessage);
            this.panelEx1.Controls.Add(this.checkBoxCreateShp);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(445, 141);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 11;
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.Location = new System.Drawing.Point(13, 88);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(44, 18);
            this.labMessage.TabIndex = 14;
            this.labMessage.Text = "提示：";
            // 
            // checkBoxCreateShp
            // 
            this.checkBoxCreateShp.Location = new System.Drawing.Point(13, 111);
            this.checkBoxCreateShp.Name = "checkBoxCreateShp";
            this.checkBoxCreateShp.Size = new System.Drawing.Size(132, 18);
            this.checkBoxCreateShp.TabIndex = 13;
            this.checkBoxCreateShp.Text = "下载完成后生成shp";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(13, 40);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 12;
            this.labelX2.Text = "保存路径";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(13, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 11;
            this.labelX1.Text = "关键字";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(202, 15);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(229, 18);
            this.labelX3.TabIndex = 10;
            this.labelX3.Text = "输入查询关键字即可，想查询全部则不填";
            // 
            // ckbExcel
            // 
            this.ckbExcel.Location = new System.Drawing.Point(151, 111);
            this.ckbExcel.Name = "ckbExcel";
            this.ckbExcel.Size = new System.Drawing.Size(132, 18);
            this.ckbExcel.TabIndex = 15;
            this.ckbExcel.Text = "是否生成Excel";
            // 
            // FrmPOIDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 141);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txbPath);
            this.Controls.Add(this.txbKeyWord);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(461, 179);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(461, 179);
            this.Name = "FrmPOIDown";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "兴趣点下载";
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txbKeyWord;
        private DevComponents.DotNetBar.Controls.TextBoxX txbPath;
        private DevComponents.DotNetBar.ButtonX btnScan;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBar;
        private DevComponents.DotNetBar.ButtonX btnDown;
        private DevComponents.DotNetBar.ButtonX btnStop;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labMessage;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxCreateShp;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX ckbExcel;
    }
}