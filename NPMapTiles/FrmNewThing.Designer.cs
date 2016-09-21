namespace NPMapTiles
{
    partial class FrmNewThing
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.btnScan = new DevComponents.DotNetBar.ButtonX();
            this.txbSavePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txbRightTop = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txbLeftBottom = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.checkBoxAusterityFile = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.rbVectorMap = new System.Windows.Forms.RadioButton();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.dataGridViewX = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.btnCanel = new DevComponents.DotNetBar.ButtonX();
            this.btnSelectAll = new DevComponents.DotNetBar.ButtonX();
            this.btnReverseSelection = new DevComponents.DotNetBar.ButtonX();
            this.btnClear = new DevComponents.DotNetBar.ButtonX();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbName);
            this.groupBox1.Controls.Add(this.labelX7);
            this.groupBox1.Controls.Add(this.btnScan);
            this.groupBox1.Controls.Add(this.txbSavePath);
            this.groupBox1.Controls.Add(this.labelX6);
            this.groupBox1.Controls.Add(this.labelX5);
            this.groupBox1.Controls.Add(this.labelX4);
            this.groupBox1.Controls.Add(this.txbRightTop);
            this.groupBox1.Controls.Add(this.txbLeftBottom);
            this.groupBox1.Controls.Add(this.labelX3);
            this.groupBox1.Controls.Add(this.labelX2);
            this.groupBox1.Controls.Add(this.checkBoxAusterityFile);
            this.groupBox1.Controls.Add(this.rbVectorMap);
            this.groupBox1.Controls.Add(this.labelX1);
            this.groupBox1.Location = new System.Drawing.Point(3, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 162);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "下载参数设置";
            // 
            // txbName
            // 
            // 
            // 
            // 
            this.txbName.Border.Class = "TextBoxBorder";
            this.txbName.Location = new System.Drawing.Point(74, 127);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(257, 21);
            this.txbName.TabIndex = 14;
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(10, 129);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(75, 23);
            this.labelX7.TabIndex = 13;
            this.labelX7.Text = "任务名称：";
            // 
            // btnScan
            // 
            this.btnScan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScan.Location = new System.Drawing.Point(337, 99);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 12;
            this.btnScan.Text = "浏览";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txbSavePath
            // 
            // 
            // 
            // 
            this.txbSavePath.Border.Class = "TextBoxBorder";
            this.txbSavePath.Location = new System.Drawing.Point(74, 100);
            this.txbSavePath.Name = "txbSavePath";
            this.txbSavePath.Size = new System.Drawing.Size(257, 21);
            this.txbSavePath.TabIndex = 11;
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(10, 102);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(75, 23);
            this.labelX6.TabIndex = 10;
            this.labelX6.Text = "存储目录：";
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(262, 73);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(153, 23);
            this.labelX5.TabIndex = 9;
            this.labelX5.Text = "米（最大经度，最大纬度）";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(262, 48);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(153, 23);
            this.labelX4.TabIndex = 8;
            this.labelX4.Text = "米（最小经度，最小纬度）";
            // 
            // txbRightTop
            // 
            // 
            // 
            // 
            this.txbRightTop.Border.Class = "TextBoxBorder";
            this.txbRightTop.Location = new System.Drawing.Point(74, 73);
            this.txbRightTop.Name = "txbRightTop";
            this.txbRightTop.Size = new System.Drawing.Size(182, 21);
            this.txbRightTop.TabIndex = 7;
            // 
            // txbLeftBottom
            // 
            // 
            // 
            // 
            this.txbLeftBottom.Border.Class = "TextBoxBorder";
            this.txbLeftBottom.Location = new System.Drawing.Point(74, 48);
            this.txbLeftBottom.Name = "txbLeftBottom";
            this.txbLeftBottom.Size = new System.Drawing.Size(182, 21);
            this.txbLeftBottom.TabIndex = 6;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(10, 73);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(56, 23);
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = "右上角：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(10, 48);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(56, 23);
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "左下角：";
            // 
            // checkBoxAusterityFile
            // 
            this.checkBoxAusterityFile.Checked = true;
            this.checkBoxAusterityFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAusterityFile.CheckValue = "Y";
            this.checkBoxAusterityFile.Location = new System.Drawing.Point(151, 20);
            this.checkBoxAusterityFile.Name = "checkBoxAusterityFile";
            this.checkBoxAusterityFile.Size = new System.Drawing.Size(258, 23);
            this.checkBoxAusterityFile.TabIndex = 3;
            this.checkBoxAusterityFile.Text = "是否生成紧缩文件(文件数量少，易拷贝)";
            // 
            // rbVectorMap
            // 
            this.rbVectorMap.AutoSize = true;
            this.rbVectorMap.Checked = true;
            this.rbVectorMap.Location = new System.Drawing.Point(74, 22);
            this.rbVectorMap.Name = "rbVectorMap";
            this.rbVectorMap.Size = new System.Drawing.Size(71, 16);
            this.rbVectorMap.TabIndex = 1;
            this.rbVectorMap.TabStop = true;
            this.rbVectorMap.Text = "电子地图";
            this.rbVectorMap.UseVisualStyleBackColor = true;
            this.rbVectorMap.CheckedChanged += new System.EventHandler(this.rbVectorMap_CheckedChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(10, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "下载类型：";
            // 
            // dataGridViewX
            // 
            this.dataGridViewX.AllowUserToAddRows = false;
            this.dataGridViewX.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewX.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column6,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX.Location = new System.Drawing.Point(3, 201);
            this.dataGridViewX.Name = "dataGridViewX";
            this.dataGridViewX.RowHeadersVisible = false;
            this.dataGridViewX.RowTemplate.Height = 23;
            this.dataGridViewX.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX.Size = new System.Drawing.Size(419, 212);
            this.dataGridViewX.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 64.88186F;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column6
            // 
            this.Column6.FillWeight = 108.0817F;
            this.Column6.HeaderText = "级别";
            this.Column6.Name = "Column6";
            // 
            // Column2
            // 
            this.Column2.FillWeight = 116.5307F;
            this.Column2.HeaderText = "行";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.FillWeight = 116.5307F;
            this.Column3.HeaderText = "列";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.FillWeight = 116.5307F;
            this.Column4.HeaderText = "总数";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.FillWeight = 116.5307F;
            this.Column5.HeaderText = "估计大小";
            this.Column5.Name = "Column5";
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(265, 419);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCanel
            // 
            this.btnCanel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCanel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCanel.Location = new System.Drawing.Point(347, 419);
            this.btnCanel.Name = "btnCanel";
            this.btnCanel.Size = new System.Drawing.Size(75, 23);
            this.btnCanel.TabIndex = 3;
            this.btnCanel.Text = "取消";
            this.btnCanel.Click += new System.EventHandler(this.btnCanel_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectAll.Location = new System.Drawing.Point(3, 174);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 4;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnReverseSelection
            // 
            this.btnReverseSelection.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReverseSelection.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReverseSelection.Location = new System.Drawing.Point(84, 174);
            this.btnReverseSelection.Name = "btnReverseSelection";
            this.btnReverseSelection.Size = new System.Drawing.Size(75, 23);
            this.btnReverseSelection.TabIndex = 5;
            this.btnReverseSelection.Text = "反选";
            this.btnReverseSelection.Click += new System.EventHandler(this.btnReverseSelection_Click);
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClear.Location = new System.Drawing.Point(165, 174);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "清除";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // FrmNewThing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(424, 449);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnReverseSelection);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnCanel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dataGridViewX);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNewThing";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建任务";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxAusterityFile;
        private System.Windows.Forms.RadioButton rbVectorMap;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txbRightTop;
        private DevComponents.DotNetBar.Controls.TextBoxX txbLeftBottom;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.ButtonX btnScan;
        private DevComponents.DotNetBar.Controls.TextBoxX txbSavePath;
        private DevComponents.DotNetBar.Controls.TextBoxX txbName;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.ButtonX btnCanel;
        private DevComponents.DotNetBar.ButtonX btnSelectAll;
        private DevComponents.DotNetBar.ButtonX btnReverseSelection;
        private DevComponents.DotNetBar.ButtonX btnClear;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}