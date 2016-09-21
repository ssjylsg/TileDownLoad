namespace NPMapTiles
{
    partial class FrmRoadNameUpdata
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
            this.btnStop = new DevComponents.DotNetBar.ButtonX();
            this.btnDown = new DevComponents.DotNetBar.ButtonX();
            this.progressBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labMessage = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(356, 40);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnDown
            // 
            this.btnDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDown.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDown.Location = new System.Drawing.Point(274, 40);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 11;
            this.btnDown.Text = "更新";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // progressBar
            // 
            this.progressBar.FocusCuesEnabled = false;
            this.progressBar.Location = new System.Drawing.Point(12, 9);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(418, 15);
            this.progressBar.TabIndex = 9;
            this.progressBar.Text = "progressBarX1";
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.labMessage);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(445, 72);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 13;
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.Location = new System.Drawing.Point(12, 26);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(44, 18);
            this.labMessage.TabIndex = 11;
            this.labMessage.Text = "提示：";
            // 
            // FrmRoadNameUpdata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 72);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(461, 110);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(461, 110);
            this.Name = "FrmRoadNameUpdata";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "路名更新";
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnStop;
        private DevComponents.DotNetBar.ButtonX btnDown;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBar;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labMessage;
    }
}