namespace NPMapTiles
{
    partial class FrmCoverteRectify
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txbPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnScan = new DevComponents.DotNetBar.ButtonX();
            this.progressBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.btnCoverter = new DevComponents.DotNetBar.ButtonX();
            this.btnStop = new DevComponents.DotNetBar.ButtonX();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(81, 18);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "百度坐标数据";
            // 
            // txbPath
            // 
            // 
            // 
            // 
            this.txbPath.Border.Class = "TextBoxBorder";
            this.txbPath.Location = new System.Drawing.Point(99, 9);
            this.txbPath.Name = "txbPath";
            this.txbPath.Size = new System.Drawing.Size(217, 21);
            this.txbPath.TabIndex = 1;
            // 
            // btnScan
            // 
            this.btnScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScan.Location = new System.Drawing.Point(322, 8);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "浏览...";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 37);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(386, 10);
            this.progressBar.TabIndex = 3;
            this.progressBar.Text = "progressBar";
            // 
            // btnCoverter
            // 
            this.btnCoverter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCoverter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCoverter.Location = new System.Drawing.Point(241, 53);
            this.btnCoverter.Name = "btnCoverter";
            this.btnCoverter.Size = new System.Drawing.Size(75, 23);
            this.btnCoverter.TabIndex = 4;
            this.btnCoverter.Text = "转换";
            this.btnCoverter.Click += new System.EventHandler(this.btnCoverter_Click);
            // 
            // btnStop
            // 
            this.btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(322, 53);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // checkBoxX1
            // 
            this.checkBoxX1.Location = new System.Drawing.Point(12, 53);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(131, 23);
            this.checkBoxX1.TabIndex = 6;
            this.checkBoxX1.Text = "完成后生成Shp文件";
            // 
            // FrmCoverteRectify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 81);
            this.Controls.Add(this.checkBoxX1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnCoverter);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txbPath);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(426, 119);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(426, 119);
            this.Name = "FrmCoverteRectify";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坐标转换";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txbPath;
        private DevComponents.DotNetBar.ButtonX btnScan;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBar;
        private DevComponents.DotNetBar.ButtonX btnCoverter;
        private DevComponents.DotNetBar.ButtonX btnStop;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
    }
}