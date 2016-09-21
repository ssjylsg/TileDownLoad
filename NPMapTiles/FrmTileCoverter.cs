using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using System.Threading;
using System.Xml;
using System.Drawing.Imaging;
namespace NPMapTiles
{
    using log4net;

    public partial class FrmTileCoverter : Office2007Form
    {
        string oldDirPath = "";
        string newDirPath = "";
        string biaoZhuPath = "";
        int LayerType = 1;
        bool CovertStatus = false;
        string fullExtent = "";
        private delegate void ProcessNotifyHandler(string msg, int process);
        private ProcessNotifyHandler OnProcessNotifyCoverte;

        private ILog log;
        public FrmTileCoverter()
        {
           log = log4net.LogManager.GetLogger(this.GetType());
            InitializeComponent();
            this.OnProcessNotifyCoverte += new ProcessNotifyHandler(this.CoverteMsg);
            if (this.cmbType.Items.Count > 0)
            {
                this.cmbType.SelectedIndex = 0;
            }
        }

        private void expandablePanel1_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            this.Height = this.Height == 237 ? 371 : 237;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbType.SelectedItem == this.cmbTianDi)
            {
                txbFullExent.Text = "";
                txbProject.Text = "4326";
                txbBiaoJiPath.Text = "";
            }
            if (this.cmbType.SelectedItem == this.cmbGaoDe || this.cmbType.SelectedItem == this.cmbGoogle)
            {
                txbFullExent.Text = "-20037508.3427892, -20037508.3427892, 20037508.3427892, 20037508.3427892";
                txbProject.Text = "900913";
                txbBiaoJiPath.Text = "";
            }
        }

        private void btnCoverter_Click(object sender, EventArgs e)
        {
            this.oldDirPath = this.txbPath.Text.Trim();
            if (!Directory.Exists(this.oldDirPath))
            {
                MessageBox.Show("路径不正确");
                return;
            }
            this.newDirPath = this.txbNewPath.Text.Trim();
            if (!Directory.Exists(this.newDirPath))
            {
                MessageBox.Show("路径不正确");
                return;
            }
            if (this.cmbType.SelectedItem == this.cmbTianDi)
            {
                this.LayerType = 1;
            }
            else if (this.cmbType.SelectedItem == this.cmbGoogle)
            {
                this.LayerType = 3;
            }
            else if (this.cmbType.SelectedItem == this.cmbGaoDe)
            {
                this.LayerType = 2;
            }
            else if (this.cmbType.SelectedItem == this.arcgis) {
                this.LayerType = 4;
            }
            else if (this.cmbType.SelectedItem == this.pgis)
            {
                this.LayerType = 5;
            }
            this.biaoZhuPath = this.txbBiaoJiPath.Text.Trim();
            this.CovertStatus = true;
            Thread thread = new Thread(
                () =>
                    {
                        try
                        {
                            covertTiles();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            MessageBox.Show(ex.Message);
                        }
                    });
            
            thread.Start();
        }
        private void covertTiles()
        {
            string austerityFilePath = Path.Combine(this.newDirPath, "Layers");
            if (!Directory.Exists(austerityFilePath))
                Directory.CreateDirectory(austerityFilePath);
            this.createConfig(austerityFilePath);
            string[] ds = Directory.GetDirectories(this.oldDirPath);
            for (int i = 0; i < ds.Length; i++)
            {
                string[] rs = Directory.GetDirectories(ds[i]);
                DirectoryInfo d = new DirectoryInfo(ds[i]);
                string zstring = d.Name;
                int zoom = 0;
                int currentIndex = 0;
                if (int.TryParse(zstring.Replace("L", ""), out zoom))
                {
                    for (int j = 0; j < rs.Length; j++)
                    {
                        string[] cs = Directory.GetFiles(rs[j]);
                        DirectoryInfo r = new DirectoryInfo(rs[j]);
                        string rstring = r.Name;
                        int row = 0;
                        if (rstring.Contains("R"))
                        {
                            rstring = this.removeChar(rstring, 'R');
                            rstring = this.removeChar(rstring, '0');
                            row = System.Convert.ToInt32(rstring, 16);
                        }
                        else
                        {
                            int.TryParse(rstring, out row);
                        }
                        for (int k = 0; k < cs.Length; k++)
                        {
                            FileInfo file = new FileInfo(cs[k]);
                            string cstring = file.Name.Split('.')[0];
                            int col = 0;
                            if (cstring.Contains("C"))
                            {
                                cstring = this.removeChar(cstring, 'C');
                                cstring = this.removeChar(cstring, '0');
                                try
                                {
                                    col = System.Convert.ToInt32(cstring, 16);
                                }
                                catch (Exception exception)
                                {
                                    log.Error(exception);
                                    var msg = string.Format("zoom={0},x={1},y={2}", zoom, rstring, cstring);
                                    log.InfoFormat(msg);
                                    continue;
                                    //throw new Exception(msg,exception.InnerException);
                                }
                            }
                            else
                            {
                                int.TryParse(cstring, out col);
                            }
                            if (Directory.Exists(this.biaoZhuPath))
                            {
                                string biaoZhuPath = cs[k].Replace(this.oldDirPath, this.biaoZhuPath);
                                if (File.Exists(biaoZhuPath))
                                {
                                    biaoZhuPath = cs[k].Replace(this.oldDirPath, this.biaoZhuPath);
                                    Image image1 = Bitmap.FromFile(cs[k]);
                                    Image image2 = Bitmap.FromFile(biaoZhuPath);
                                    Bitmap imgTemp = new System.Drawing.Bitmap(image1.Width, image1.Height, PixelFormat.Format24bppRgb);
                                    Graphics g = Graphics.FromImage(imgTemp);
                                    g.DrawImage(image1, 0, 0, image1.Width, image1.Height);
                                    g.DrawImage(image2, 0, 0, image2.Width, image2.Height);
                                    GC.Collect();
                                    if (LayerType == 1)
                                    {
                                        this.austerityFile(zoom, col, row, imgTemp, austerityFilePath + "\\_alllayers");
                                    }
                                    else
                                    {
                                        this.austerityFile(zoom, row, col, imgTemp, austerityFilePath + "\\_alllayers");
                                    }
                                }
                                else
                                {
                                    if (LayerType == 1)
                                    {
                                        this.austerityFile(zoom, col, row, cs[k], austerityFilePath + "\\_alllayers");
                                    }
                                    else
                                    {
                                        this.austerityFile(zoom, row, col, cs[k], austerityFilePath + "\\_alllayers");
                                    }
                                }
                            }
                            else
                            {
                                if (LayerType == 1)
                                {
                                    this.austerityFile(zoom, col, row, cs[k], austerityFilePath + "\\_alllayers");
                                }
                                else
                                {
                                    this.austerityFile(zoom, row, col, cs[k], austerityFilePath + "\\_alllayers");
                                }
                            }
                            currentIndex++;
                            if (this.CovertStatus)
                            {
                                if (this.OnProcessNotifyCoverte != null)
                                {
                                    this.OnProcessNotifyCoverte("处理第" + zoom.ToString() + "级", currentIndex * 100 / (rs.Length * cs.Length));
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            }
            if (this.OnProcessNotifyCoverte != null)
            {
                this.OnProcessNotifyCoverte("处理完成",100);
            }
        }
        private void CopyArcgisCofig(string titlePath) 
        {
            if (this.oldDirPath.IndexOf(@"\_alllayers") > -1) {

                var target = this.oldDirPath.Substring(0,this.oldDirPath.IndexOf(@"\_alllayers"));
                var files = System.IO.Directory.GetFiles(target).Where(m => m.Contains("conf.cdi") || m.Contains("conf.xml")).ToList();
                if (files.Count != 0)
                {
                    files.ForEach(m =>
                    {
                        var tempFile = new FileInfo(m).Extension == ".cdi" ? "conf.cdi" : "conf.xml";
                        new FileInfo(m).CopyTo(System.IO.Path.Combine(titlePath, tempFile), true);
                    });
                }
            } 
        }
        private void createConfig(string austerityFilePath)
        {
            string tempLayerType = "tiandi";
            if (this.LayerType == 1)
            {
                tempLayerType = "tiandi";
            }
            if (this.LayerType == 2)
            {
                tempLayerType = "gaode";
            }
            if (this.LayerType == 3)
            {
                tempLayerType = "google";
            }
            if (this.LayerType == 4)
            {
                tempLayerType = "pgis";
            }
            else if (this.LayerType == 4)
            {
                CopyArcgisCofig(austerityFilePath);
                return;
            }
            string savePath = austerityFilePath + "\\Conf.xml";
            XmlDocument xmlDoc = new XmlDocument();
            //创建类型声明节点  
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            //创建根节点  
            XmlNode root = xmlDoc.CreateElement("map");
            string minZoom = txbMinZoom.Text.Trim();
            string maxZoom = txbMaxZoom.Text.Trim();
            string fullExtent = txbFullExent.Text.Trim();
            string centerPoint = txbCenterPoint.Text.Trim();
            string projection = txbProject.Text.Trim();
            string restrictedExtent = fullExtent;
            string type = this.txbType.Text.Trim();
            string zoomLevelSequence = this.txbZoomLeveSequence.Text.Trim();
           
            xmlDoc.AppendChild(root);
            CreateNode(xmlDoc, root, "layerType", tempLayerType);
            CreateNode(xmlDoc, root, "minZoom", minZoom.ToString());
            CreateNode(xmlDoc, root, "maxZoom", maxZoom.ToString());
            CreateNode(xmlDoc, root, "fullExtent", fullExtent);
            CreateNode(xmlDoc, root, "centerPoint", centerPoint);
            CreateNode(xmlDoc, root, "projection", projection);
            CreateNode(xmlDoc, root, "restrictedExtent", restrictedExtent);
            CreateNode(xmlDoc, root, "type", type);
            CreateNode(xmlDoc, root, "zoomLevelSequence", zoomLevelSequence);
            try
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                xmlDoc.Save(savePath);
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
            } 
        }
        public XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                return xmlNode;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex; //这里可以定义你自己的异常处理
            }
        }
        public void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            parentNode.AppendChild(node);
        } 
        private void CoverteMsg(string message, int process)
        {
            string threadName = Thread.CurrentThread.Name;
            MethodInvoker invoker = delegate
            {
                this.labMessage.Text = message;
                this.progressBar.Value = process;
                this.progressBar.Update();
            };
            if ((!base.IsDisposed) && base.InvokeRequired)
            {
                base.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }
        private string removeChar(string str, char c)
        {
            int index = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == c)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }
            string result = "";
            for (int i = index; i < str.Length; i++)
            {
                result += str[i];
            }
            return result;
        }
        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
        private void austerityFile(int zoom, int row, int col, Image image, string savePath)
        {
            byte[] data = this.ImageToBytes(image);
            this.austerityFile(zoom, row, col, data, savePath);
        }
        private void austerityFile(int zoom, int row, int col, string imagePath, string savePath)
        {
            FileStream imageStream = File.OpenRead(imagePath);
            byte[] data = new byte[imageStream.Length];
            imageStream.Read(data, 0, data.Length);
            this.austerityFile(zoom, row, col, data, savePath);
        }
        private void austerityFile(int zoom, int row, int col, byte[] data, string savePath)
        {
            int size = 128;
            String l = "0" + zoom;
            int lLength = l.Length;
            if (lLength > 2)
            {
                l = l.Substring(lLength - 2);
            }
            l = "L" + l;

            int rGroup = size * (row / size);
            String r = rGroup.ToString("X");
            int rLength = r.Length;
            if (rLength < 4)
            {
                for (int i = 0; i < 4 - rLength; i++)
                {
                    r = "0" + r;
                }
            }
            r = "R" + r;
            int cGroup = size * (col / size);
            String c = cGroup.ToString("X");
            int cLength = c.Length;
            if (cLength < 4)
            {
                for (int i = 0; i < 4 - cLength; i++)
                {
                    c = "0" + c;
                }
            }
            c = "C" + c;
            if (!Directory.Exists(savePath + "/" + l))
            {
                Directory.CreateDirectory(savePath + "/" + l);
            }
            String bundleBase = savePath + "/" + l + "/" + r + c;
            String bundlxFileName = bundleBase + ".npslx";
            String bundleFileName = bundleBase + ".npsle";
            FileStream file = null;
            FileStream fileIndex = null;
            if (!File.Exists(bundleFileName))
            {
                file = File.Create(bundleFileName);
                file.Write(new Byte[16], 0, 16);
            }
            else
            {
                file = File.OpenWrite(bundleFileName);
            }
            if (!File.Exists(bundlxFileName))
            {
                fileIndex = File.Create(bundlxFileName);
                fileIndex.Write(new Byte[16], 0, 16);
            }
            else
            {
                fileIndex = File.OpenWrite(bundlxFileName);
            }
            int index = size * (col - cGroup) + (row - rGroup);
            int l1 = data.Length / 16777216;
            int l2 = (data.Length % 16777216) / 65536;
            int l3 = ((data.Length % 16777216) % 65536) / 256;
            int l4 = ((data.Length % 16777216) % 65536) % 256;
            byte[] le = new byte[4];
            le[0] = (byte)l4;
            le[1] = (byte)l3;
            le[2] = (byte)l2;
            le[3] = (byte)l1;
            long currentLength = file.Length;
            file.Seek(currentLength, SeekOrigin.Begin);
            file.Write(le, 0, 4);
            file.Write(data, 0, data.Length);
            file.Flush();
            file.Dispose();

            long lindex0 = (long)(currentLength / 4294967296);
            long lindex1 = (long)(currentLength % 4294967296) / 16777216;
            long lindex2 = (long)(currentLength % 4294967296) % 16777216 / 65536;
            long lindex3 = (long)(currentLength % 4294967296) % 16777216 % 65536 / 256;
            long lindex4 = (long)(currentLength % 4294967296) % 16777216 % 65536 % 256;
            byte[] indexBytes = new byte[5];
            indexBytes[0] = (byte)lindex4;
            indexBytes[1] = (byte)lindex3;
            indexBytes[2] = (byte)lindex2;
            indexBytes[3] = (byte)lindex1;
            indexBytes[4] = (byte)lindex0;
            fileIndex.Seek(16 + 5 * index, SeekOrigin.Begin);
            fileIndex.Write(indexBytes, 0, 5);
            fileIndex.Flush();
            fileIndex.Dispose();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder =
                               new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.txbPath.Text = folder.SelectedPath;
                if (File.Exists(folder.SelectedPath + "\\..\\配置.txt"))
                {
                    StreamReader sr = new StreamReader(folder.SelectedPath + "\\..\\配置.txt", Encoding.Default);
                    String line;
                    line = sr.ReadLine();
                    Dictionary<string,string> dic = new Dictionary<string,string>();
                    while (line != null)
                    {
                        string[] s = line.Split(':');
                        dic.Add(s[0], s[1]);
                        line = sr.ReadLine();
                    }
                    if (dic.ContainsKey("centerPoint"))
                        txbCenterPoint.Text = dic["centerPoint"].Replace("[","").Replace("]","");
                    if (dic.ContainsKey("minZoom"))
                        txbMinZoom.Text = dic["minZoom"];
                    if (dic.ContainsKey("maxZoom"))
                        txbMaxZoom.Text = dic["maxZoom"];
                    if (dic.ContainsKey("fullExtent"))
                        txbFullExent.Text = dic["fullExtent"].Replace("[", "").Replace("]", "");
                }
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog folder =
                               new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.txbNewPath.Text = folder.SelectedPath;
            }
        }

        private void btnBJScan_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder =
                   new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.txbBiaoJiPath.Text = folder.SelectedPath;
            }
        }
    }
}
