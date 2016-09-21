using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace LinenceManagerController
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string textString = txbMessage.Text.Trim();
            if (textString == string.Empty)
            {
                MessageBox.Show("请输入序列号");
                return;
            }
            string value = this.Encrypt(textString);
            string path = System.Windows.Forms.Application.StartupPath + "\\Lisence.ini";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            this.Write(value, path);
            MessageBox.Show("生成成功");
        }
        public string Encrypt(string toEncrypt)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes("netposagis0123456789012345678901");
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();//using System.Security.Cryptography;    
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;//using System.Security.Cryptography;    
            rDel.Padding = PaddingMode.PKCS7;//using System.Security.Cryptography;    
            ICryptoTransform cTransform = rDel.CreateEncryptor();//using System.Security.Cryptography;    
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public void Write(string key, string path)
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
    }
}
