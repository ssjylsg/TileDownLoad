namespace NPMapTiles
{
    partial class FrmRoadLineOpr
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txbPassWord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbDataBase = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txbServer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbPoi = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbCity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProvice = new System.Windows.Forms.ComboBox();
            this.rtxbRoadNames = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labMessage = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.txbPassWord);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txbUser);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txbDataBase);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txbServer);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(529, 85);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据导入";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(421, 43);
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
            this.txbDataBase.Text = "Road";
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.labMessage);
            this.groupBox2.Controls.Add(this.progressBar);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.rtxbRoadNames);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cmbPoi);
            this.groupBox2.Controls.Add(this.btnOk);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbCity);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbProvice);
            this.groupBox2.Location = new System.Drawing.Point(16, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(525, 354);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "道路";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(147, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "路网表";
            // 
            // cmbPoi
            // 
            this.cmbPoi.FormattingEnabled = true;
            this.cmbPoi.Location = new System.Drawing.Point(20, 61);
            this.cmbPoi.Name = "cmbPoi";
            this.cmbPoi.Size = new System.Drawing.Size(121, 20);
            this.cmbPoi.TabIndex = 12;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(311, 313);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "提交";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(382, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "市";
            // 
            // cmbCity
            // 
            this.cmbCity.FormattingEnabled = true;
            this.cmbCity.Location = new System.Drawing.Point(255, 28);
            this.cmbCity.Name = "cmbCity";
            this.cmbCity.Size = new System.Drawing.Size(121, 20);
            this.cmbCity.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(147, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "省";
            // 
            // cmbProvice
            // 
            this.cmbProvice.FormattingEnabled = true;
            this.cmbProvice.Location = new System.Drawing.Point(20, 28);
            this.cmbProvice.Name = "cmbProvice";
            this.cmbProvice.Size = new System.Drawing.Size(121, 20);
            this.cmbProvice.TabIndex = 0;
            this.cmbProvice.SelectedIndexChanged += new System.EventHandler(this.cmbProvice_SelectedIndexChanged);
            // 
            // rtxbRoadNames
            // 
            this.rtxbRoadNames.Location = new System.Drawing.Point(73, 103);
            this.rtxbRoadNames.Name = "rtxbRoadNames";
            this.rtxbRoadNames.Size = new System.Drawing.Size(437, 134);
            this.rtxbRoadNames.TabIndex = 14;
            this.rtxbRoadNames.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "道路名称：";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(29, 261);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(490, 23);
            this.progressBar.TabIndex = 16;
            // 
            // labMessage
            // 
            this.labMessage.AutoSize = true;
            this.labMessage.Location = new System.Drawing.Point(27, 318);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(41, 12);
            this.labMessage.TabIndex = 17;
            this.labMessage.Text = "label9";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(255, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "根据道路交叉口检查道路名称";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmRoadLineOpr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 469);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmRoadLineOpr";
            this.Text = "路网信息补全";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txbPassWord;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbDataBase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbPoi;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProvice;
        private System.Windows.Forms.RichTextBox rtxbRoadNames;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labMessage;
        private System.Windows.Forms.Button button1;

    }
}