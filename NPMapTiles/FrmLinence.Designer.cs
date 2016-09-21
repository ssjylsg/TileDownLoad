namespace NPMapTiles
{
    partial class FrmLinence
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
            this.btnScan = new DevComponents.DotNetBar.ButtonX();
            this.txbCaputerMessage = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txbPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.btnToMail = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // btnScan
            // 
            this.btnScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScan.Location = new System.Drawing.Point(253, 35);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(73, 23);
            this.btnScan.TabIndex = 6;
            this.btnScan.Text = "浏览...";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txbCaputerMessage
            // 
            this.txbCaputerMessage.Location = new System.Drawing.Point(80, 8);
            this.txbCaputerMessage.Name = "txbCaputerMessage";
            this.txbCaputerMessage.Size = new System.Drawing.Size(246, 21);
            this.txbCaputerMessage.TabIndex = 4;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(25, 7);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 5;
            this.labelX4.Text = "序列号：";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 35);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "许可文件：";
            // 
            // txbPath
            // 
            // 
            // 
            // 
            this.txbPath.Border.Class = "TextBoxBorder";
            this.txbPath.Location = new System.Drawing.Point(80, 35);
            this.txbPath.Name = "txbPath";
            this.txbPath.Size = new System.Drawing.Size(167, 21);
            this.txbPath.TabIndex = 8;
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(174, 70);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(73, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "注册";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClose.Location = new System.Drawing.Point(253, 70);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(73, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnToMail
            // 
            this.btnToMail.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnToMail.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnToMail.Location = new System.Drawing.Point(95, 70);
            this.btnToMail.Name = "btnToMail";
            this.btnToMail.Size = new System.Drawing.Size(73, 23);
            this.btnToMail.TabIndex = 13;
            this.btnToMail.Text = "发送邮件";
            this.btnToMail.Click += new System.EventHandler(this.btnToMail_Click);
            // 
            // FrmLinence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 105);
            this.Controls.Add(this.btnToMail);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txbPath);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txbCaputerMessage);
            this.Controls.Add(this.labelX4);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(352, 143);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(352, 143);
            this.Name = "FrmLinence";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "注册";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnScan;
        private DevComponents.DotNetBar.Controls.TextBoxX txbCaputerMessage;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txbPath;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.ButtonX btnClose;
        private DevComponents.DotNetBar.ButtonX btnToMail;
    }
}