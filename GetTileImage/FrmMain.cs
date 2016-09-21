using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GetTileImage
{
    public partial class FrmMain : Form
    {
        private delegate void ProcessNotifyHandler(string msg1, int process);

        private delegate void LogHandler(string msg, bool isSave);

        private ProcessNotifyHandler OnProcessNotify;
        private LogHandler OnLog;
        public FrmMain()
        {
            InitializeComponent();
            this.OnProcessNotify += new ProcessNotifyHandler(this.ShowMsg);
            this.OnLog += new LogHandler(this.WriteLog);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.lsbLog.Items.Clear();
            new Thread(new ThreadStart(this.DoSomething)).Start();
        }

        private void DoSomething()
        {
            int minRow = 0;
            string strMinRow = this.txbMinRow.Text.Trim();
            int.TryParse(strMinRow, out minRow);

            int maxRow = 0;
            string strMaxRow = this.txbMaxRow.Text.Trim();
            int.TryParse(strMaxRow, out maxRow);

            int minCol = 0;
            string strMinCol = this.txbMinColumn.Text.Trim();
            int.TryParse(strMinCol, out minCol);

            int maxCol = 0;
            string strMaxCol = this.txbMaxColumn.Text.Trim();
            int.TryParse(strMaxCol, out maxCol);

            int minZoom = 0;
            string strminZoom = this.txbMinZoom.Text.Trim();
            int.TryParse(strminZoom, out minZoom);

            int maxZoom = 0;
            string strmaxZoom = this.txtMaxZoom.Text.Trim();
            int.TryParse(strmaxZoom, out maxZoom);

            if (minRow > 0 && maxRow > 0 && minCol > 0 && maxCol > 0 && minZoom > 0 && maxZoom > 0)
            {
                for (int i = minZoom; i < maxZoom + 1; i++)
                {
                    int rowCount = maxRow - minRow + 1;
                    int colCount = maxCol - minCol + 1;
                    if (i > minZoom)
                    {
                        minRow = minRow * 2;
                        maxRow = minRow + rowCount * 2 - 1;
                        minCol = minCol * 2;
                        maxCol = minCol + colCount * 2 - 1;
                    }
                    this.GetTiles(minRow, maxRow, minCol, maxCol, i);
                }
            }
        }

        private void GetTiles(int minRow, int maxRow,int minCol ,int maxCol ,int zoom)
        {
            string path = this.txbUrl.Text;
            path = path + "\\" + zoom.ToString();
            int count = (maxRow - minRow + 1) * (maxCol - minCol + 1);
            int k = 0;
            for (int i = minRow; i < maxRow + 1; i++)
            {
                string Dpath = path + "\\" + i.ToString();
                if (!Directory.Exists(Dpath))
                {
                    Directory.CreateDirectory(Dpath);
                }
                for (int j = minCol; j < maxCol + 1; j++)
                {
                    string tempPath = Dpath + "\\" + j.ToString() + ".png";
                    k++;
                    bool isSave = false;
                    if (!File.Exists(tempPath))
                    {
                        string url = "http://mt0.google.cn/vt?pb=!1m4!1m3!1i"+zoom.ToString()+"!2i"+i.ToString()+"!3i"+j.ToString()+"!2m3!1e0!2sm!3i271000000!3m11!2szh-cn!3scn!5e18!12m1!1e47!12m3!1e37!2m1!1ssmartmaps!12m1!1e47!4e0";
                        isSave = this.DownloadPicture(url, tempPath, 2000);
                        if (this.OnLog != null)
                        {
                            this.OnLog(tempPath, isSave);
                        }
                    }
                    string msg = "提示：已处理第"+zoom.ToString()+"级," + k.ToString() + "条,共" + count.ToString() + "条";
                    if (this.OnProcessNotify != null)
                    {
                        this.OnProcessNotify(msg, (k * 100) / count);
                    }
                }
            }
        }

        private bool DownloadPicture(string picUrl, string savePath, int timeOut)
        {
            bool value = false;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                if (timeOut != -1) request.Timeout = timeOut;
                request.UserAgent = "xxx";
                request.Method = "GET";
                request.Accept = "xxx";
                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                    value = SaveBinaryFile(response, savePath);
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
            return value;
        }

        private static bool SaveBinaryFile(WebResponse response, string savePath)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outStream = null;
            Stream inStream = null;
            try
            {
                if (File.Exists(savePath)) File.Delete(savePath);
                outStream = System.IO.File.Create(savePath);
                inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0) outStream.Write(buffer, 0, l);
                } while (l > 0);
                value = true;
            }
            finally
            {
                if (outStream != null) outStream.Close();
                if (inStream != null) inStream.Close();
            }
            return value;
        }

        private void ShowMsg(string msg1, int process)
        {
            string threadName = Thread.CurrentThread.Name;
            MethodInvoker invoker = delegate
            {
                this.labMessge.Text = msg1;
                this.progressBar.Value = process;
                this.progressBar.Update();
            };
            if (base.InvokeRequired)
            {
                base.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }
        private void WriteLog(string msg, bool isSave)
        {
            string threadName = Thread.CurrentThread.Name;
            string m="";
            if (isSave)
            {
                m = ":下载成功";
            }
            else
            {
                m = ":下载失败";
            }
            MethodInvoker invoker = delegate
            {
                if (this.rbFalse.Checked)
                {
                    if(!isSave)
                    this.lsbLog.Items.Add(msg + m);
                }
                else if (this.rbAll.Checked)
                {
                    this.lsbLog.Items.Add(msg + m);
                }
            };
            if (base.InvokeRequired)
            {
                base.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }

        private void btnGetURL_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder =
                                new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.txbUrl.Text = folder.SelectedPath;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.lsbLog.Items.Clear();
            new Thread(new ThreadStart(this.Test)).Start();
        }

        private void Test()
        {
            int minRow = 0;
            string strMinRow = this.txbMinRow.Text.Trim();
            int.TryParse(strMinRow, out minRow);

            int maxRow = 0;
            string strMaxRow = this.txbMaxRow.Text.Trim();
            int.TryParse(strMaxRow, out maxRow);

            int minCol = 0;
            string strMinCol = this.txbMinColumn.Text.Trim();
            int.TryParse(strMinCol, out minCol);

            int maxCol = 0;
            string strMaxCol = this.txbMaxColumn.Text.Trim();
            int.TryParse(strMaxCol, out maxCol);

            int minZoom = 0;
            string strminZoom = this.txbMinZoom.Text.Trim();
            int.TryParse(strminZoom, out minZoom);

            int maxZoom = 0;
            string strmaxZoom = this.txtMaxZoom.Text.Trim();
            int.TryParse(strmaxZoom, out maxZoom);


            if (minRow > 0 && maxRow > 0 && minCol > 0 && maxCol > 0 && minZoom > 0 && maxZoom > 0)
            {
                for (int i = minZoom; i < maxZoom + 1; i++)
                {
                    int rowCount = maxRow - minRow + 1;
                    int colCount = maxCol - minCol + 1;
                    if (i > minZoom)
                    {
                        minRow = minRow * 2;
                        maxRow = minRow + rowCount * 2 - 1;
                        minCol = minCol * 2;
                        maxCol = minCol + colCount * 2 - 1;
                    }
                    this.TestTiles(minRow, maxRow, minCol, maxCol, i);
                }
            }
        }
        private void TestTiles(int minRow, int maxRow, int minCol, int maxCol, int zoom)
        {
            string path = this.txbUrl.Text;
            path = path + "\\" + zoom.ToString();
            int count = (maxRow - minRow + 1) * (maxCol - minCol + 1);
            int k = 0;
            for (int i = minRow; i < maxRow + 1; i++)
            {
                string Dpath = path + "\\" + i.ToString();
                if (!Directory.Exists(Dpath))
                {
                    if (this.OnLog != null)
                    {
                        this.OnLog(Dpath, false);
                    }
                    
                }
                for (int j = minCol; j < maxCol + 1; j++)
                {
                    string tempPath = Dpath + "\\" + j.ToString() + ".png";
                    k++;
                    bool isSave = false;
                    if (!File.Exists(tempPath))
                    {
                        if (this.OnLog != null)
                        {
                            this.OnLog(tempPath, isSave);
                        }
                    }
                    string msg = "提示：已处理第" + zoom.ToString() + "级," + k.ToString() + "条,共" + count.ToString() + "条";
                    if (this.OnProcessNotify != null)
                    {
                        this.OnProcessNotify(msg, (k * 100) / count);
                    }
                }
            }
        }
    }
}
