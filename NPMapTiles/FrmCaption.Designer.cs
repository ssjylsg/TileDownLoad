namespace NPMapTiles
{
    partial class FrmCaption
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
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labVersion = new DevComponents.DotNetBar.LabelX();
            this.panelEx2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.labVersion);
            this.panelEx2.Controls.Add(this.labelX3);
            this.panelEx2.Controls.Add(this.labelX2);
            this.panelEx2.Controls.Add(this.labelX1);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(0, 0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(365, 136);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 18;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Location = new System.Drawing.Point(24, 83);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(322, 18);
            this.labelX3.TabIndex = 10;
            this.labelX3.Text = "著作版权：      东方网力西安研发中心基础服务部GIS组";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(24, 59);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 18);
            this.labelX2.TabIndex = 9;
            this.labelX2.Text = "软件版本：";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(24, 35);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(217, 18);
            this.labelX1.TabIndex = 8;
            this.labelX1.Text = "软件名称：       NPGIS地图下载工具";
            // 
            // labVersion
            // 
            this.labVersion.AutoSize = true;
            this.labVersion.Location = new System.Drawing.Point(128, 59);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(68, 18);
            this.labVersion.TabIndex = 11;
            // 
            // FrmCaption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 136);
            this.Controls.Add(this.panelEx2);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(381, 174);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(381, 174);
            this.Name = "FrmCaption";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于软件";
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labVersion;
    }
}