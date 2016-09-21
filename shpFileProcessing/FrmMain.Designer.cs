namespace ShpFileProcessing
{
    partial class FrmMain
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
            this.label5 = new System.Windows.Forms.Label();
            this.cmbTableName = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCreatPinYin = new System.Windows.Forms.Button();
            this.cmbHanZi = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labMessegbox = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(529, 85);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据导入";
            // 
            // btnCloseTable
            // 
            this.btnCloseTable.Location = new System.Drawing.Point(425, 45);
            this.btnCloseTable.Name = "btnCloseTable";
            this.btnCloseTable.Size = new System.Drawing.Size(93, 23);
            this.btnCloseTable.TabIndex = 19;
            this.btnCloseTable.Text = "关闭";
            this.btnCloseTable.UseVisualStyleBackColor = true;
            this.btnCloseTable.Click += new System.EventHandler(this.btnCloseTable_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(425, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(93, 23);
            this.btnConnect.TabIndex = 18;
            this.btnConnect.Text = "链接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txbPassWord
            // 
            this.txbPassWord.Location = new System.Drawing.Point(285, 47);
            this.txbPassWord.Name = "txbPassWord";
            this.txbPassWord.Size = new System.Drawing.Size(125, 21);
            this.txbPassWord.TabIndex = 17;
            this.txbPassWord.Text = "123456";
            this.txbPassWord.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "PassWord:";
            // 
            // txbUser
            // 
            this.txbUser.Location = new System.Drawing.Point(59, 47);
            this.txbUser.Name = "txbUser";
            this.txbUser.Size = new System.Drawing.Size(143, 21);
            this.txbUser.TabIndex = 15;
            this.txbUser.Text = "postgres";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "User:";
            // 
            // txbDataBase
            // 
            this.txbDataBase.Location = new System.Drawing.Point(285, 20);
            this.txbDataBase.Name = "txbDataBase";
            this.txbDataBase.Size = new System.Drawing.Size(125, 21);
            this.txbDataBase.TabIndex = 13;
            this.txbDataBase.Text = "gis";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "DataBase:";
            // 
            // txbServer
            // 
            this.txbServer.Location = new System.Drawing.Point(59, 20);
            this.txbServer.Name = "txbServer";
            this.txbServer.Size = new System.Drawing.Size(143, 21);
            this.txbServer.TabIndex = 11;
            this.txbServer.Text = "192.168.60.242";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "Sever:";
            // 
            // cmbTableName
            // 
            this.cmbTableName.FormattingEnabled = true;
            this.cmbTableName.Location = new System.Drawing.Point(59, 22);
            this.cmbTableName.Name = "cmbTableName";
            this.cmbTableName.Size = new System.Drawing.Size(152, 20);
            this.cmbTableName.TabIndex = 8;
            this.cmbTableName.SelectedIndexChanged += new System.EventHandler(this.cmbTableName_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "表名：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCreatPinYin);
            this.groupBox2.Controls.Add(this.cmbHanZi);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cmbTableName);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(12, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(529, 59);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "拼音生成";
            // 
            // btnCreatPinYin
            // 
            this.btnCreatPinYin.Location = new System.Drawing.Point(425, 20);
            this.btnCreatPinYin.Name = "btnCreatPinYin";
            this.btnCreatPinYin.Size = new System.Drawing.Size(93, 23);
            this.btnCreatPinYin.TabIndex = 12;
            this.btnCreatPinYin.Text = "生成";
            this.btnCreatPinYin.UseVisualStyleBackColor = true;
            this.btnCreatPinYin.Click += new System.EventHandler(this.btnCreatPinYin_Click);
            // 
            // cmbHanZi
            // 
            this.cmbHanZi.FormattingEnabled = true;
            this.cmbHanZi.Location = new System.Drawing.Point(291, 22);
            this.cmbHanZi.Name = "cmbHanZi";
            this.cmbHanZi.Size = new System.Drawing.Size(119, 20);
            this.cmbHanZi.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(220, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "汉字字段：";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 191);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(529, 12);
            this.progressBar.TabIndex = 11;
            // 
            // labMessegbox
            // 
            this.labMessegbox.AutoSize = true;
            this.labMessegbox.Location = new System.Drawing.Point(10, 170);
            this.labMessegbox.Name = "labMessegbox";
            this.labMessegbox.Size = new System.Drawing.Size(0, 12);
            this.labMessegbox.TabIndex = 10;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 210);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labMessegbox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拼音生成工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbTableName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbHanZi;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCreatPinYin;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labMessegbox;
    }
}

