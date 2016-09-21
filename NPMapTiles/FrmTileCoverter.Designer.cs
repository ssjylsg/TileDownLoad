namespace NPMapTiles
{
    partial class FrmTileCoverter
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
            this.txbPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.txbNewPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStop = new DevComponents.DotNetBar.ButtonX();
            this.btnCoverter = new DevComponents.DotNetBar.ButtonX();
            this.progressBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.btnBJScan = new DevComponents.DotNetBar.ButtonX();
            this.txbBiaoJiPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.txbZoomLeveSequence = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.txbType = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.txbMaxZoom = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.txbMinZoom = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.txbProject = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.txbCenterPoint = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.txbFullExent = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cmbType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbTianDi = new DevComponents.Editors.ComboItem();
            this.cmbGaoDe = new DevComponents.Editors.ComboItem();
            this.cmbGoogle = new DevComponents.Editors.ComboItem();
            this.arcgis = new DevComponents.Editors.ComboItem();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labMessage = new DevComponents.DotNetBar.LabelX();
            this.pgis = new DevComponents.Editors.ComboItem();
            this.panelEx2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.expandablePanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnScan
            // 
            this.btnScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScan.Location = new System.Drawing.Point(282, 18);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(76, 23);
            this.btnScan.TabIndex = 9;
            this.btnScan.Text = "浏览...";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txbPath
            // 
            // 
            // 
            // 
            this.txbPath.Border.Class = "TextBoxBorder";
            this.txbPath.Location = new System.Drawing.Point(81, 18);
            this.txbPath.Name = "txbPath";
            this.txbPath.Size = new System.Drawing.Size(195, 21);
            this.txbPath.TabIndex = 8;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(4, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(81, 18);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "原始切片路径";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(282, 45);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(76, 23);
            this.buttonX1.TabIndex = 15;
            this.buttonX1.Text = "浏览...";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // txbNewPath
            // 
            // 
            // 
            // 
            this.txbNewPath.Border.Class = "TextBoxBorder";
            this.txbNewPath.Location = new System.Drawing.Point(81, 45);
            this.txbNewPath.Name = "txbNewPath";
            this.txbNewPath.Size = new System.Drawing.Size(195, 21);
            this.txbNewPath.TabIndex = 14;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(29, 45);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(56, 18);
            this.labelX2.TabIndex = 13;
            this.labelX2.Text = "保存路径";
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.groupBox2);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx2.Location = new System.Drawing.Point(0, 0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(392, 98);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 17;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txbPath);
            this.groupBox2.Controls.Add(this.labelX1);
            this.groupBox2.Controls.Add(this.buttonX1);
            this.groupBox2.Controls.Add(this.btnScan);
            this.groupBox2.Controls.Add(this.txbNewPath);
            this.groupBox2.Controls.Add(this.labelX2);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(15);
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox2.Size = new System.Drawing.Size(371, 76);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "区域选择";
            // 
            // btnStop
            // 
            this.btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(308, 6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 18;
            this.btnStop.Text = "停止";
            // 
            // btnCoverter
            // 
            this.btnCoverter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCoverter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCoverter.Location = new System.Drawing.Point(227, 6);
            this.btnCoverter.Name = "btnCoverter";
            this.btnCoverter.Size = new System.Drawing.Size(75, 23);
            this.btnCoverter.TabIndex = 17;
            this.btnCoverter.Text = "转换";
            this.btnCoverter.Click += new System.EventHandler(this.btnCoverter_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 35);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(378, 10);
            this.progressBar.TabIndex = 16;
            this.progressBar.Text = "progressBar";
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.expandablePanel1.Controls.Add(this.btnBJScan);
            this.expandablePanel1.Controls.Add(this.txbBiaoJiPath);
            this.expandablePanel1.Controls.Add(this.labelX11);
            this.expandablePanel1.Controls.Add(this.txbZoomLeveSequence);
            this.expandablePanel1.Controls.Add(this.labelX9);
            this.expandablePanel1.Controls.Add(this.txbType);
            this.expandablePanel1.Controls.Add(this.labelX10);
            this.expandablePanel1.Controls.Add(this.txbMaxZoom);
            this.expandablePanel1.Controls.Add(this.labelX7);
            this.expandablePanel1.Controls.Add(this.txbMinZoom);
            this.expandablePanel1.Controls.Add(this.labelX8);
            this.expandablePanel1.Controls.Add(this.txbProject);
            this.expandablePanel1.Controls.Add(this.labelX6);
            this.expandablePanel1.Controls.Add(this.txbCenterPoint);
            this.expandablePanel1.Controls.Add(this.labelX5);
            this.expandablePanel1.Controls.Add(this.txbFullExent);
            this.expandablePanel1.Controls.Add(this.labelX4);
            this.expandablePanel1.Controls.Add(this.cmbType);
            this.expandablePanel1.Controls.Add(this.labelX3);
            this.expandablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 98);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Size = new System.Drawing.Size(392, 188);
            this.expandablePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.expandablePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandablePanel1.Style.GradientAngle = 90;
            this.expandablePanel1.TabIndex = 20;
            this.expandablePanel1.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "地图参数配置";
            this.expandablePanel1.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.expandablePanel1_ExpandedChanged);
            // 
            // btnBJScan
            // 
            this.btnBJScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBJScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBJScan.Location = new System.Drawing.Point(308, 143);
            this.btnBJScan.Name = "btnBJScan";
            this.btnBJScan.Size = new System.Drawing.Size(76, 23);
            this.btnBJScan.TabIndex = 19;
            this.btnBJScan.Text = "浏览...";
            this.btnBJScan.Click += new System.EventHandler(this.btnBJScan_Click);
            // 
            // txbBiaoJiPath
            // 
            // 
            // 
            // 
            this.txbBiaoJiPath.Border.Class = "TextBoxBorder";
            this.txbBiaoJiPath.Location = new System.Drawing.Point(76, 145);
            this.txbBiaoJiPath.Name = "txbBiaoJiPath";
            this.txbBiaoJiPath.Size = new System.Drawing.Size(226, 21);
            this.txbBiaoJiPath.TabIndex = 18;
            // 
            // labelX11
            // 
            this.labelX11.Location = new System.Drawing.Point(17, 145);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(70, 23);
            this.labelX11.TabIndex = 17;
            this.labelX11.Text = "标记图层：";
            // 
            // txbZoomLeveSequence
            // 
            // 
            // 
            // 
            this.txbZoomLeveSequence.Border.Class = "TextBoxBorder";
            this.txbZoomLeveSequence.Location = new System.Drawing.Point(252, 118);
            this.txbZoomLeveSequence.Name = "txbZoomLeveSequence";
            this.txbZoomLeveSequence.ReadOnly = true;
            this.txbZoomLeveSequence.Size = new System.Drawing.Size(132, 21);
            this.txbZoomLeveSequence.TabIndex = 16;
            this.txbZoomLeveSequence.Text = "2";
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(196, 118);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(70, 23);
            this.labelX9.TabIndex = 15;
            this.labelX9.Text = "层级跨度：";
            // 
            // txbType
            // 
            // 
            // 
            // 
            this.txbType.Border.Class = "TextBoxBorder";
            this.txbType.Location = new System.Drawing.Point(76, 118);
            this.txbType.Name = "txbType";
            this.txbType.Size = new System.Drawing.Size(118, 21);
            this.txbType.TabIndex = 14;
            this.txbType.Text = "png";
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(17, 118);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(70, 23);
            this.labelX10.TabIndex = 13;
            this.labelX10.Text = "切片类型：";
            // 
            // txbMaxZoom
            // 
            // 
            // 
            // 
            this.txbMaxZoom.Border.Class = "TextBoxBorder";
            this.txbMaxZoom.Location = new System.Drawing.Point(252, 89);
            this.txbMaxZoom.Name = "txbMaxZoom";
            this.txbMaxZoom.Size = new System.Drawing.Size(132, 21);
            this.txbMaxZoom.TabIndex = 12;
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(196, 89);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(70, 23);
            this.labelX7.TabIndex = 11;
            this.labelX7.Text = "最大级别：";
            // 
            // txbMinZoom
            // 
            // 
            // 
            // 
            this.txbMinZoom.Border.Class = "TextBoxBorder";
            this.txbMinZoom.Location = new System.Drawing.Point(76, 89);
            this.txbMinZoom.Name = "txbMinZoom";
            this.txbMinZoom.Size = new System.Drawing.Size(118, 21);
            this.txbMinZoom.TabIndex = 10;
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(17, 89);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(70, 23);
            this.labelX8.TabIndex = 9;
            this.labelX8.Text = "最小级别：";
            // 
            // txbProject
            // 
            // 
            // 
            // 
            this.txbProject.Border.Class = "TextBoxBorder";
            this.txbProject.Location = new System.Drawing.Point(252, 60);
            this.txbProject.Name = "txbProject";
            this.txbProject.Size = new System.Drawing.Size(132, 21);
            this.txbProject.TabIndex = 8;
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(219, 60);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(59, 23);
            this.labelX6.TabIndex = 7;
            this.labelX6.Text = "投影：";
            // 
            // txbCenterPoint
            // 
            // 
            // 
            // 
            this.txbCenterPoint.Border.Class = "TextBoxBorder";
            this.txbCenterPoint.Location = new System.Drawing.Point(76, 60);
            this.txbCenterPoint.Name = "txbCenterPoint";
            this.txbCenterPoint.Size = new System.Drawing.Size(118, 21);
            this.txbCenterPoint.TabIndex = 6;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(28, 60);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(59, 23);
            this.labelX5.TabIndex = 5;
            this.labelX5.Text = "中心点：";
            // 
            // txbFullExent
            // 
            // 
            // 
            // 
            this.txbFullExent.Border.Class = "TextBoxBorder";
            this.txbFullExent.Location = new System.Drawing.Point(252, 33);
            this.txbFullExent.Name = "txbFullExent";
            this.txbFullExent.Size = new System.Drawing.Size(131, 21);
            this.txbFullExent.TabIndex = 4;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(219, 33);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(48, 23);
            this.labelX4.TabIndex = 3;
            this.labelX4.Text = "范围：";
            // 
            // cmbType
            // 
            this.cmbType.DisplayMember = "Text";
            this.cmbType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.ItemHeight = 15;
            this.cmbType.Items.AddRange(new object[] {
            this.cmbTianDi,
            this.cmbGaoDe,
            this.cmbGoogle,
            this.arcgis,
            this.pgis});
            this.cmbType.Location = new System.Drawing.Point(76, 33);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(118, 21);
            this.cmbType.TabIndex = 2;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // cmbTianDi
            // 
            this.cmbTianDi.Text = "天地图";
            // 
            // cmbGaoDe
            // 
            this.cmbGaoDe.Text = "高德";
            // 
            // cmbGoogle
            // 
            this.cmbGoogle.Text = "谷歌";
            // 
            // arcgis
            // 
            this.arcgis.Text = "Arcgis";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(17, 33);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(66, 23);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "地图类型：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.panel1.Controls.Add(this.labMessage);
            this.panel1.Controls.Add(this.btnCoverter);
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 286);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(392, 76);
            this.panel1.TabIndex = 21;
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.Location = new System.Drawing.Point(8, 48);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(0, 0);
            this.labMessage.TabIndex = 19;
            // 
            // pgis
            // 
            this.pgis.Text = "pgis";
            // 
            // FrmTileCoverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 362);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.expandablePanel1);
            this.Controls.Add(this.panelEx2);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTileCoverter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "切片紧缩";
            this.panelEx2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.expandablePanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnScan;
        private DevComponents.DotNetBar.Controls.TextBoxX txbPath;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txbNewPath;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.ButtonX btnStop;
        private DevComponents.DotNetBar.ButtonX btnCoverter;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBar;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbType;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txbFullExent;
        private DevComponents.DotNetBar.Controls.TextBoxX txbCenterPoint;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX txbZoomLeveSequence;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.TextBoxX txbType;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.Controls.TextBoxX txbMaxZoom;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.TextBoxX txbMinZoom;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.Controls.TextBoxX txbProject;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.Editors.ComboItem cmbTianDi;
        private DevComponents.Editors.ComboItem cmbGaoDe;
        private DevComponents.Editors.ComboItem cmbGoogle;
        private DevComponents.DotNetBar.LabelX labMessage;
        private DevComponents.DotNetBar.Controls.TextBoxX txbBiaoJiPath;
        private DevComponents.DotNetBar.ButtonX btnBJScan;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.Editors.ComboItem arcgis;
        private DevComponents.Editors.ComboItem pgis;
    }
}