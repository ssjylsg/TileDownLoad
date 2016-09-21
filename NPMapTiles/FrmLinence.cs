using System;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using MapDataTools;
namespace NPMapTiles
{
    public partial class FrmLinence : Office2007Form
    {
        
        public FrmLinence()
        {
            InitializeComponent();
            HardwareInfo hardwareInfo = new HardwareInfo();
            string message = hardwareInfo.GetCpuID() + hardwareInfo.GetMacAddress();
            this.txbCaputerMessage.Text = message;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "ini files (*.ini)|*.ini";
            if (of.ShowDialog() == DialogResult.OK)
                this.txbPath.Text = of.FileName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.txbPath.Text.Trim()))
            {
                MessageBox.Show("请输入正确的许可文件路径");
                return;
            }
            FileInfo fileInfo = new FileInfo(this.txbPath.Text.Trim());
            if (fileInfo.Extension.Equals(".ini") || fileInfo.Extension.Equals("ini"))
            {
                string value = LisenceManager.Read(this.txbPath.Text.Trim());
                string result = LisenceManager.Encrypt(this.txbCaputerMessage.Text.Trim());
                if (value == result)
                {
                    if (File.Exists(Application.StartupPath + "\\Lisence.ini"))
                        File.Delete(Application.StartupPath + "\\Lisence.ini");
                    this.Write(value, Application.StartupPath + "\\Lisence.ini");
                    MessageBox.Show("注册成功");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("注册失败");
                }
            }
        }

        private void Write(string key, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(key);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnToMail_Click(object sender, EventArgs e)
        {
            string subValue = "NPGIS地图下载工具 注册验证";
            subValue = System.Web.HttpUtility.UrlEncode(subValue);
            string message = @"mailto:lishigang@netposa.com;songjiang_xa@netposa.com;fanxudan@netposa.com;?subject=" + subValue + "&body=" + System.Web.HttpUtility.UrlEncode("产品线:<br/>负责人:<br/>序列号:") + this.txbCaputerMessage.Text.Trim();
            System.Diagnostics.Process.Start(message);
        }
    }
}
