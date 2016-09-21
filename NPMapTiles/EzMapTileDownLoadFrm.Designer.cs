namespace NPMapTiles
{
    partial class EzMapTileDownLoadFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EzMapTileDownLoadFrm));
            this.saveConfigBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.downLoadTxb = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txbSavePath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.minZoomTxb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.maxZoomTxb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.extentTxb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.versionTxb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.urlTxb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mapControl1 = new MapDataTools.MapControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.绘制下载区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除覆盖物ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveConfigBtn
            // 
            this.saveConfigBtn.Location = new System.Drawing.Point(189, 418);
            this.saveConfigBtn.Name = "saveConfigBtn";
            this.saveConfigBtn.Size = new System.Drawing.Size(75, 23);
            this.saveConfigBtn.TabIndex = 1;
            this.saveConfigBtn.Text = "保存并浏览地图";
            this.saveConfigBtn.UseVisualStyleBackColor = true;
            this.saveConfigBtn.Click += new System.EventHandler(this.saveConfigBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.mapControl1);
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Location = new System.Drawing.Point(480, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(520, 387);
            this.panel2.TabIndex = 2;
            // 
            // downLoadTxb
            // 
            this.downLoadTxb.Location = new System.Drawing.Point(672, 418);
            this.downLoadTxb.Name = "downLoadTxb";
            this.downLoadTxb.Size = new System.Drawing.Size(75, 23);
            this.downLoadTxb.TabIndex = 3;
            this.downLoadTxb.Text = "下载地图";
            this.downLoadTxb.UseVisualStyleBackColor = true;
            this.downLoadTxb.Click += new System.EventHandler(this.downLoadTxb_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(462, 163);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作说明";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 17);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(456, 143);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.txbSavePath);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.minZoomTxb);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.maxZoomTxb);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.extentTxb);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.versionTxb);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.urlTxb);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(15, 177);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 231);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "地图参数设置";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(378, 189);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "浏览";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txbSavePath
            // 
            this.txbSavePath.Location = new System.Drawing.Point(117, 191);
            this.txbSavePath.Name = "txbSavePath";
            this.txbSavePath.Size = new System.Drawing.Size(260, 21);
            this.txbSavePath.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "切片保存地址:";
            // 
            // minZoomTxb
            // 
            this.minZoomTxb.Location = new System.Drawing.Point(117, 158);
            this.minZoomTxb.Name = "minZoomTxb";
            this.minZoomTxb.Size = new System.Drawing.Size(311, 21);
            this.minZoomTxb.TabIndex = 22;
            this.minZoomTxb.Text = "10";
            this.minZoomTxb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maxZoomTxb_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "最小地图层级：";
            // 
            // maxZoomTxb
            // 
            this.maxZoomTxb.Location = new System.Drawing.Point(117, 119);
            this.maxZoomTxb.Name = "maxZoomTxb";
            this.maxZoomTxb.Size = new System.Drawing.Size(311, 21);
            this.maxZoomTxb.TabIndex = 20;
            this.maxZoomTxb.Text = "18";
            this.maxZoomTxb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maxZoomTxb_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "最大地图层级：";
            // 
            // extentTxb
            // 
            this.extentTxb.Location = new System.Drawing.Point(117, 85);
            this.extentTxb.Name = "extentTxb";
            this.extentTxb.Size = new System.Drawing.Size(311, 21);
            this.extentTxb.TabIndex = 18;
            this.extentTxb.Text = "106.56395859374,33.179194843749,108.88134140624,34.301265156249";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "地图范围：";
            // 
            // versionTxb
            // 
            this.versionTxb.Location = new System.Drawing.Point(117, 45);
            this.versionTxb.Name = "versionTxb";
            this.versionTxb.Size = new System.Drawing.Size(311, 21);
            this.versionTxb.TabIndex = 16;
            this.versionTxb.Text = "0.3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "版本：";
            // 
            // urlTxb
            // 
            this.urlTxb.Location = new System.Drawing.Point(117, 19);
            this.urlTxb.Name = "urlTxb";
            this.urlTxb.Size = new System.Drawing.Size(311, 21);
            this.urlTxb.TabIndex = 14;
            this.urlTxb.Text = "http://10.173.2.20/PGIS_S_TileMapServer/Maps/V";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "服务RUL:";
            // 
            // mapControl1
            // 
            this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl1.Location = new System.Drawing.Point(0, 25);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.Size = new System.Drawing.Size(520, 362);
            this.mapControl1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.绘制下载区域ToolStripMenuItem,
            this.清除覆盖物ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(520, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 绘制下载区域ToolStripMenuItem
            // 
            this.绘制下载区域ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("绘制下载区域ToolStripMenuItem.Image")));
            this.绘制下载区域ToolStripMenuItem.Name = "绘制下载区域ToolStripMenuItem";
            this.绘制下载区域ToolStripMenuItem.Size = new System.Drawing.Size(108, 21);
            this.绘制下载区域ToolStripMenuItem.Text = "绘制下载区域";
            this.绘制下载区域ToolStripMenuItem.Click += new System.EventHandler(this.button2_Click);
            // 
            // 清除覆盖物ToolStripMenuItem
            // 
            this.清除覆盖物ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("清除覆盖物ToolStripMenuItem.Image")));
            this.清除覆盖物ToolStripMenuItem.Name = "清除覆盖物ToolStripMenuItem";
            this.清除覆盖物ToolStripMenuItem.Size = new System.Drawing.Size(96, 21);
            this.清除覆盖物ToolStripMenuItem.Text = "清除覆盖物";
            this.清除覆盖物ToolStripMenuItem.Click += new System.EventHandler(this.清除覆盖物ToolStripMenuItem_Click);
            // 
            // EzMapTileDownLoadFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 459);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.downLoadTxb);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.saveConfigBtn);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1028, 497);
            this.MinimizeBox = false;
            this.Name = "EzMapTileDownLoadFrm";
            this.Text = "PGIS 地图配置";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveConfigBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button downLoadTxb;
        private MapDataTools.MapControl mapControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txbSavePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox minZoomTxb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox maxZoomTxb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox extentTxb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox versionTxb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox urlTxb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 绘制下载区域ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除覆盖物ToolStripMenuItem;
    }
}