using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace UntilTools
{
    using System.Xml;

    public class WorkConfig
    {
        /// <summary>
        /// 单例模式singleton
        /// </summary>
        static WorkConfig instance = null;
        public CommandConfig Commandconfig = new CommandConfig();
        //默认保存路径
        public static string DefaultConfigXml = Application.StartupPath + Path.DirectorySeparatorChar + "DataConfig.xml";

        /// <summary>
        /// 获取配置信息（单例模式）
        /// </summary>
        /// <returns></returns>
        public static WorkConfig GetInstance()
        {
            if (instance == null)
                instance = new WorkConfig();
            return instance;
        }

        private WorkConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            if (!File.Exists(DefaultConfigXml))
                return;
            xmler.LoadFromFile(Commandconfig, DefaultConfigXml);
        }
        public WorkInfo GetWorkInfoByID(string id)
        {
            foreach (WorkInfo workInfo in Commandconfig.workInfoList)
            {
                if (workInfo.id == id)
                    return workInfo;
            }
            return null;
        }
        public void saveConfig()
        {
            //lock (Commandconfig)
            //{
                XmlStorageHelper xmler = new XmlStorageHelper();
                xmler.SaveToFile(Commandconfig, DefaultConfigXml);
            //}
        }
    }
    public class CommandConfig
    {
       public List<WorkInfo> workInfoList = new List<WorkInfo>();
    }
    public class WorkInfo
    {
        public string id = "";
        public bool isAusterityFile = false;
        public List<RowColumns> rcList;
        public double minX = 0;
        public double minY = 0;
        public double maxX = 0;
        public double maxY = 0;
        public string filePath = "";
        public string workName = "";
        public MapType mapType = MapType.Google;
        public DownStates downStates = DownStates.stop;
        public ProcessDownImage processDownImage = new ProcessDownImage(); 
        public override string ToString()
        {
            if (this.rcList.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("minZoom:{0}", rcList[0].zoom).AppendLine();//开始写入值
            stringBuilder.AppendFormat("maxZoom:{0}", rcList[rcList.Count - 1].zoom).AppendLine();//开始写入值
            stringBuilder.AppendFormat("restrictedExtent:[{0},{1},{2},{3}]", this.minX, this.minY, this.maxX, this.maxY).AppendLine();//开始写入值
            stringBuilder.AppendFormat("type: 'png'").AppendLine();
            stringBuilder.AppendFormat("centerPoint:[{0},{1}]", (this.minX + this.maxX) / 2.0, (this.minY + this.maxY) / 2.0).AppendLine();
            if (this.mapType == MapType.Google || this.mapType == MapType.Gaode)
            {
                stringBuilder.AppendFormat("fullExtent:[-20037508.34, -20037508.34, 20037508.34, 20037508.34]").AppendLine();
            }
            else if (this.mapType == MapType.Tiandi)
            {
                stringBuilder.AppendFormat("fullExtent:[{0},{1},{2},{3}]", this.minX, this.minY, this.maxX, this.maxY).AppendLine();
            }
            stringBuilder.AppendLine("minLevel: 0");
            stringBuilder.AppendFormat("maxLevel:{0}", (rcList[rcList.Count - 1].zoom - rcList[0].zoom)).AppendLine();
            stringBuilder.AppendFormat("zoomOffset:{0}", rcList[0].zoom).AppendLine();
            stringBuilder.AppendFormat("zoomLevelSequence: 2").AppendLine();
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 写入配置文件
        /// </summary>
        public void WriteConfigurationToFile()
        {
            var file = Path.Combine(this.filePath, "配置.txt");
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
            {
                var content = this.ToString();
                var buffer = System.Text.ASCIIEncoding.UTF8.GetBytes(content);
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }
        private XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)
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
        private void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }
        /// <summary>
        /// 生成紧缩型文件配置信息
        /// </summary>
        /// <param name="workInfo">下载任务</param>
        /// <param name="austerityFilePath">保存文件</param>
        public void createAusterityConfig(string austerityFilePath)
        {
            WorkInfo workInfo = this;
            string savePath = austerityFilePath + "\\Conf.xml";
            string centerPoint = ((workInfo.maxX + workInfo.minX) / 2).ToString() + "," + ((workInfo.maxY + workInfo.minY) / 2).ToString();
            string fullExtent = workInfo.minX.ToString() + "," + workInfo.minY.ToString() + "," + workInfo.maxX.ToString() + "," + workInfo.maxY.ToString();
            int minZoom = 0;
            int maxZoom = 0;
            if (File.Exists(savePath))
            {
                XmlNode minZnode = GetXmlNodeByXpath(savePath, "//map//minZoom");
                if (minZnode != null)
                {
                    int.TryParse(minZnode.InnerText, out minZoom);
                    if (minZoom > workInfo.rcList[0].zoom)
                        minZoom = workInfo.rcList[0].zoom;
                }

                XmlNode maxZnode = GetXmlNodeByXpath(savePath, "//map//maxZoom");
                if (maxZnode != null)
                {
                    int.TryParse(maxZnode.InnerText, out maxZoom);
                    if (maxZoom < workInfo.rcList[workInfo.rcList.Count - 1].zoom)
                        maxZoom = workInfo.rcList[workInfo.rcList.Count - 1].zoom;
                }
            }
            else
            {
                minZoom = workInfo.rcList[0].zoom;
                maxZoom = workInfo.rcList[workInfo.rcList.Count - 1].zoom;
            }
            string projection = (workInfo.mapType == MapType.Tiandi || workInfo.mapType == MapType.TiandiImage) ? "4326" : "900913";
            string restrictedExtent = workInfo.minX.ToString() + "," + workInfo.minY.ToString() + "," + workInfo.maxX.ToString() + "," + workInfo.maxY.ToString();
            string type = "png";
            string zoomLevelSequence = "2";
            string tempLayerType = "tiandi";
            if (workInfo.mapType == MapType.Tiandi || workInfo.mapType == MapType.TiandiImage)
            {
                tempLayerType = "tiandi";
            }
            if (workInfo.mapType == MapType.Gaode || workInfo.mapType == MapType.GaodeImage)
            {
                fullExtent = "-20037508.34, -20037508.34, 20037508.34, 20037508.34";
                tempLayerType = "gaode";
            }
            if (workInfo.mapType == MapType.Google || workInfo.mapType == MapType.GoogleImage)
            {
                fullExtent = "-20037508.34, -20037508.34, 20037508.34, 20037508.34";
                tempLayerType = "google";
            }
            XmlDocument xmlDoc = new XmlDocument();
            //创建类型声明节点  
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            //创建根节点  
            XmlNode root = xmlDoc.CreateElement("map");
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
                    File.Delete(savePath);
                xmlDoc.Save(savePath);
            }
            catch (Exception e)
            {
                //显示错误信息  
                Console.WriteLine(e.Message);
            }
        }

        public MapTitle CreateTitle()
        {
            
        }
    }
    public enum MapType
    {
        Tiandi = 1,
        Google = 2,
        Baidu = 3,
        Gaode = 4,
        GoogleImage = 5,
        TiandiImage = 6,
        GaodeImage = 7,
        QQMap = 8,
        QQImage = 9
    }
    public enum DownStates
    {
        start = 1,
        pause = 2,
        stop = 3,
        ready = 4
    }
    /// <summary>
    /// 行列号存储类型对象，最小、最大行号，最小、最大列号
    /// </summary>
    public class RowColumns
    {
        public int minRow = 0;
        public int maxRow = 0;
        public int minCol = 0;
        public int maxCol = 0;
        public int zoom = 0;

        private object _object;

        public void SetData(object obj)
        {
            this._object = obj;
        }

        public object GetData()
        {
            return this._object;
        }

    }

    public class ProcessDownImage
    {
        public int count = 0;
        public int processIndex = 0;
        public int secess = 0;
        public int lose = 0;
    }
}
