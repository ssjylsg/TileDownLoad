namespace CutDataTiles
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labMessegbox = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCloseTable = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txbPassWord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbDataBase = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txbServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTableName = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txbMinX = new System.Windows.Forms.TextBox();
            this.txbMaxX = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txbMaxY = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txbminY = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.txbMaxZoom = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txbMinZoom = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txbPath = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txbResolutions = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txbOrgin = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txbExcelFile = new System.Windows.Forms.TextBox();
            this.btnFindFile = new System.Windows.Forms.Button();
            this.btnOpenExcel = new System.Windows.Forms.Button();
            this.btnCloseExcel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labMessegbox
            // 
            this.labMessegbox.AutoSize = true;
            this.labMessegbox.Location = new System.Drawing.Point(8, 467);
            this.labMessegbox.Name = "labMessegbox";
            this.labMessegbox.Size = new System.Drawing.Size(0, 12);
            this.labMessegbox.TabIndex = 0;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(10, 488);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(491, 12);
            this.progressBar.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCloseTable);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.txbPassWord);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txbUser);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txbDataBase);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txbServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 89);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据库设置";
            // 
            // btnCloseTable
            // 
            this.btnCloseTable.Location = new System.Drawing.Point(408, 50);
            this.btnCloseTable.Name = "btnCloseTable";
            this.btnCloseTable.Size = new System.Drawing.Size(93, 23);
            this.btnCloseTable.TabIndex = 9;
            this.btnCloseTable.Text = "关闭";
            this.btnCloseTable.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(408, 25);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(93, 23);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "链接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txbPassWord
            // 
            this.txbPassWord.Location = new System.Drawing.Point(268, 52);
            this.txbPassWord.Name = "txbPassWord";
            this.txbPassWord.Size = new System.Drawing.Size(125, 21);
            this.txbPassWord.TabIndex = 7;
            this.txbPassWord.Text = "123456";
            this.txbPassWord.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "PassWord:";
            // 
            // txbUser
            // 
            this.txbUser.Location = new System.Drawing.Point(55, 52);
            this.txbUser.Name = "txbUser";
            this.txbUser.Size = new System.Drawing.Size(143, 21);
            this.txbUser.TabIndex = 5;
            this.txbUser.Text = "postgres";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "User:";
            // 
            // txbDataBase
            // 
            this.txbDataBase.Location = new System.Drawing.Point(268, 25);
            this.txbDataBase.Name = "txbDataBase";
            this.txbDataBase.Size = new System.Drawing.Size(125, 21);
            this.txbDataBase.TabIndex = 3;
            this.txbDataBase.Text = "gis";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "DataBase:";
            // 
            // txbServer
            // 
            this.txbServer.Location = new System.Drawing.Point(55, 25);
            this.txbServer.Name = "txbServer";
            this.txbServer.Size = new System.Drawing.Size(143, 21);
            this.txbServer.TabIndex = 1;
            this.txbServer.Text = "192.168.60.242";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sever:";
            // 
            // cmbTableName
            // 
            this.cmbTableName.FormattingEnabled = true;
            this.cmbTableName.Location = new System.Drawing.Point(81, 159);
            this.cmbTableName.Name = "cmbTableName";
            this.cmbTableName.Size = new System.Drawing.Size(134, 20);
            this.cmbTableName.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "表名：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "最小维度：";
            // 
            // txbMinX
            // 
            this.txbMinX.Location = new System.Drawing.Point(81, 186);
            this.txbMinX.Name = "txbMinX";
            this.txbMinX.Size = new System.Drawing.Size(134, 21);
            this.txbMinX.TabIndex = 6;
            this.txbMinX.Text = "120.47858455842022";
            // 
            // txbMaxX
            // 
            this.txbMaxX.Location = new System.Drawing.Point(334, 186);
            this.txbMaxX.Name = "txbMaxX";
            this.txbMaxX.Size = new System.Drawing.Size(134, 21);
            this.txbMaxX.TabIndex = 8;
            this.txbMaxX.Text = "122.48782879957419";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(263, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "最大维度：";
            // 
            // txbMaxY
            // 
            this.txbMaxY.Location = new System.Drawing.Point(334, 213);
            this.txbMaxY.Name = "txbMaxY";
            this.txbMaxY.Size = new System.Drawing.Size(134, 21);
            this.txbMaxY.TabIndex = 12;
            this.txbMaxY.Text = "31.91994939377127";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(263, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "最大经度：";
            // 
            // txbminY
            // 
            this.txbminY.Location = new System.Drawing.Point(81, 213);
            this.txbminY.Name = "txbminY";
            this.txbminY.Size = new System.Drawing.Size(134, 21);
            this.txbminY.TabIndex = 10;
            this.txbminY.Text = "30.630244114771266";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 218);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "最小经度：";
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(410, 399);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(90, 23);
            this.btnScan.TabIndex = 13;
            this.btnScan.Text = "浏览";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txbMaxZoom
            // 
            this.txbMaxZoom.Location = new System.Drawing.Point(334, 240);
            this.txbMaxZoom.Name = "txbMaxZoom";
            this.txbMaxZoom.Size = new System.Drawing.Size(134, 21);
            this.txbMaxZoom.TabIndex = 17;
            this.txbMaxZoom.Text = "2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(263, 245);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "最大级别：";
            // 
            // txbMinZoom
            // 
            this.txbMinZoom.Location = new System.Drawing.Point(81, 240);
            this.txbMinZoom.Name = "txbMinZoom";
            this.txbMinZoom.Size = new System.Drawing.Size(134, 21);
            this.txbMinZoom.TabIndex = 15;
            this.txbMinZoom.Text = "1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 245);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 14;
            this.label11.Text = "最小级别：";
            // 
            // txbPath
            // 
            this.txbPath.Location = new System.Drawing.Point(12, 401);
            this.txbPath.Name = "txbPath";
            this.txbPath.Size = new System.Drawing.Size(383, 21);
            this.txbPath.TabIndex = 18;
            this.txbPath.Text = "F:\\SHPOI";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 383);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 19;
            this.label12.Text = "存储路径：";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(305, 437);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 23);
            this.btnStart.TabIndex = 20;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(411, 437);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // txbResolutions
            // 
            this.txbResolutions.Location = new System.Drawing.Point(81, 300);
            this.txbResolutions.Multiline = true;
            this.txbResolutions.Name = "txbResolutions";
            this.txbResolutions.Size = new System.Drawing.Size(419, 79);
            this.txbResolutions.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 305);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 22;
            this.label13.Text = "分辨率数组：";
            // 
            // txbOrgin
            // 
            this.txbOrgin.Location = new System.Drawing.Point(81, 267);
            this.txbOrgin.Name = "txbOrgin";
            this.txbOrgin.Size = new System.Drawing.Size(387, 21);
            this.txbOrgin.TabIndex = 25;
            this.txbOrgin.Text = "-400, 399.9999999999998";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 272);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 24;
            this.label15.Text = "切片原点：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 96);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(95, 12);
            this.label14.TabIndex = 26;
            this.label14.Text = "excle数据导入：";
            // 
            // txbExcelFile
            // 
            this.txbExcelFile.Location = new System.Drawing.Point(100, 91);
            this.txbExcelFile.Name = "txbExcelFile";
            this.txbExcelFile.Size = new System.Drawing.Size(293, 21);
            this.txbExcelFile.TabIndex = 27;
            // 
            // btnFindFile
            // 
            this.btnFindFile.Location = new System.Drawing.Point(409, 91);
            this.btnFindFile.Name = "btnFindFile";
            this.btnFindFile.Size = new System.Drawing.Size(92, 23);
            this.btnFindFile.TabIndex = 28;
            this.btnFindFile.Text = "浏览";
            this.btnFindFile.UseVisualStyleBackColor = true;
            this.btnFindFile.Click += new System.EventHandler(this.btnFindFile_Click);
            // 
            // btnOpenExcel
            // 
            this.btnOpenExcel.Location = new System.Drawing.Point(301, 118);
            this.btnOpenExcel.Name = "btnOpenExcel";
            this.btnOpenExcel.Size = new System.Drawing.Size(92, 23);
            this.btnOpenExcel.TabIndex = 29;
            this.btnOpenExcel.Text = "打开";
            this.btnOpenExcel.UseVisualStyleBackColor = true;
            this.btnOpenExcel.Click += new System.EventHandler(this.btnOpenExcel_Click);
            // 
            // btnCloseExcel
            // 
            this.btnCloseExcel.Location = new System.Drawing.Point(408, 118);
            this.btnCloseExcel.Name = "btnCloseExcel";
            this.btnCloseExcel.Size = new System.Drawing.Size(92, 23);
            this.btnCloseExcel.TabIndex = 30;
            this.btnCloseExcel.Text = "关闭";
            this.btnCloseExcel.UseVisualStyleBackColor = true;
            this.btnCloseExcel.Click += new System.EventHandler(this.btnCloseExcel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 506);
            this.Controls.Add(this.btnCloseExcel);
            this.Controls.Add(this.btnOpenExcel);
            this.Controls.Add(this.btnFindFile);
            this.Controls.Add(this.txbExcelFile);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txbOrgin);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txbResolutions);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txbPath);
            this.Controls.Add(this.txbMaxZoom);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txbMinZoom);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txbMaxY);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txbminY);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txbMaxX);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txbMinX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbTableName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labMessegbox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据切片工具";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labMessegbox;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCloseTable;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txbPassWord;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbDataBase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTableName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbMinX;
        private System.Windows.Forms.TextBox txbMaxX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbMaxY;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txbminY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TextBox txbMaxZoom;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txbMinZoom;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txbPath;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txbResolutions;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txbOrgin;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txbExcelFile;
        private System.Windows.Forms.Button btnFindFile;
        private System.Windows.Forms.Button btnOpenExcel;
        private System.Windows.Forms.Button btnCloseExcel;
    }
}

