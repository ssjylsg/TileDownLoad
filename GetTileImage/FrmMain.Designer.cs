namespace GetTileImage
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
            this.txbMinRow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txbMaxRow = new System.Windows.Forms.TextBox();
            this.txbMinColumn = new System.Windows.Forms.TextBox();
            this.txbMaxColumn = new System.Windows.Forms.TextBox();
            this.txbMinZoom = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lsbLog = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.labMessge = new System.Windows.Forms.Label();
            this.txbUrl = new System.Windows.Forms.TextBox();
            this.btnGetURL = new System.Windows.Forms.Button();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbFalse = new System.Windows.Forms.RadioButton();
            this.txtMaxZoom = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txbMinRow
            // 
            this.txbMinRow.Location = new System.Drawing.Point(83, 39);
            this.txbMinRow.Name = "txbMinRow";
            this.txbMinRow.Size = new System.Drawing.Size(105, 21);
            this.txbMinRow.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "最小行号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "最大行号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "最小列号";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(206, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "最大列号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "最小级别数";
            // 
            // txbMaxRow
            // 
            this.txbMaxRow.Location = new System.Drawing.Point(277, 39);
            this.txbMaxRow.Name = "txbMaxRow";
            this.txbMaxRow.Size = new System.Drawing.Size(104, 21);
            this.txbMaxRow.TabIndex = 7;
            // 
            // txbMinColumn
            // 
            this.txbMinColumn.Location = new System.Drawing.Point(83, 66);
            this.txbMinColumn.Name = "txbMinColumn";
            this.txbMinColumn.Size = new System.Drawing.Size(105, 21);
            this.txbMinColumn.TabIndex = 8;
            // 
            // txbMaxColumn
            // 
            this.txbMaxColumn.Location = new System.Drawing.Point(277, 66);
            this.txbMaxColumn.Name = "txbMaxColumn";
            this.txbMaxColumn.Size = new System.Drawing.Size(104, 21);
            this.txbMaxColumn.TabIndex = 9;
            // 
            // txbMinZoom
            // 
            this.txbMinZoom.Location = new System.Drawing.Point(83, 96);
            this.txbMinZoom.Name = "txbMinZoom";
            this.txbMinZoom.Size = new System.Drawing.Size(104, 21);
            this.txbMinZoom.TabIndex = 10;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(14, 154);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(370, 15);
            this.progressBar.TabIndex = 11;
            // 
            // lsbLog
            // 
            this.lsbLog.FormattingEnabled = true;
            this.lsbLog.ItemHeight = 12;
            this.lsbLog.Location = new System.Drawing.Point(14, 198);
            this.lsbLog.Name = "lsbLog";
            this.lsbLog.Size = new System.Drawing.Size(370, 184);
            this.lsbLog.TabIndex = 12;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(309, 123);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "修复";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(228, 123);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 14;
            this.btnTest.Text = "检测";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // labMessge
            // 
            this.labMessge.AutoSize = true;
            this.labMessge.Location = new System.Drawing.Point(14, 176);
            this.labMessge.Name = "labMessge";
            this.labMessge.Size = new System.Drawing.Size(41, 12);
            this.labMessge.TabIndex = 15;
            this.labMessge.Text = "提示：";
            // 
            // txbUrl
            // 
            this.txbUrl.Enabled = false;
            this.txbUrl.Location = new System.Drawing.Point(12, 12);
            this.txbUrl.Name = "txbUrl";
            this.txbUrl.Size = new System.Drawing.Size(276, 21);
            this.txbUrl.TabIndex = 16;
            // 
            // btnGetURL
            // 
            this.btnGetURL.Location = new System.Drawing.Point(294, 11);
            this.btnGetURL.Name = "btnGetURL";
            this.btnGetURL.Size = new System.Drawing.Size(90, 23);
            this.btnGetURL.TabIndex = 17;
            this.btnGetURL.Text = "获取切片路径";
            this.btnGetURL.UseVisualStyleBackColor = true;
            this.btnGetURL.Click += new System.EventHandler(this.btnGetURL_Click);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(14, 125);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(95, 16);
            this.rbAll.TabIndex = 18;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "输出全部日志";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbFalse
            // 
            this.rbFalse.AutoSize = true;
            this.rbFalse.Checked = true;
            this.rbFalse.Location = new System.Drawing.Point(115, 125);
            this.rbFalse.Name = "rbFalse";
            this.rbFalse.Size = new System.Drawing.Size(95, 16);
            this.rbFalse.TabIndex = 19;
            this.rbFalse.TabStop = true;
            this.rbFalse.Text = "输出失败日志";
            this.rbFalse.UseVisualStyleBackColor = true;
            // 
            // txtMaxZoom
            // 
            this.txtMaxZoom.Location = new System.Drawing.Point(277, 96);
            this.txtMaxZoom.Name = "txtMaxZoom";
            this.txtMaxZoom.Size = new System.Drawing.Size(104, 21);
            this.txtMaxZoom.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(206, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "最大级别数";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 382);
            this.Controls.Add(this.txtMaxZoom);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.rbFalse);
            this.Controls.Add(this.rbAll);
            this.Controls.Add(this.btnGetURL);
            this.Controls.Add(this.txbUrl);
            this.Controls.Add(this.labMessge);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lsbLog);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.txbMinZoom);
            this.Controls.Add(this.txbMaxColumn);
            this.Controls.Add(this.txbMinColumn);
            this.Controls.Add(this.txbMaxRow);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbMinRow);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图切片检测工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbMinRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbMaxRow;
        private System.Windows.Forms.TextBox txbMinColumn;
        private System.Windows.Forms.TextBox txbMaxColumn;
        private System.Windows.Forms.TextBox txbMinZoom;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListBox lsbLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label labMessge;
        private System.Windows.Forms.TextBox txbUrl;
        private System.Windows.Forms.Button btnGetURL;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbFalse;
        private System.Windows.Forms.TextBox txtMaxZoom;
        private System.Windows.Forms.Label label6;
    }
}

