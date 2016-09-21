using System.IO;
using System.Windows.Forms;

namespace NPMapTiles
{
    using MapDataTools.Util;

    public class AusterityConfig
    {
         /// <summary>
        /// 单例模式singleton
        /// </summary>
        static AusterityConfig instance = null;
        public Map map = new Map();
        //默认保存路径
        public static string DefaultConfigXml = Application.StartupPath + Path.DirectorySeparatorChar + "Conf.xml";

        /// <summary>
        /// 获取配置信息（单例模式）
        /// </summary>
        /// <returns></returns>
        public static AusterityConfig GetInstance()
        {
            return instance ?? (instance = new AusterityConfig());
        }

        private AusterityConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            if (!File.Exists(DefaultConfigXml))
            {
                return;
            }
            xmler.LoadFromFile(map, DefaultConfigXml);
        }
        public void saveConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            xmler.SaveToFile(map, DefaultConfigXml);
        }
    }
    public class Map
    {
        public int minZoom = 0;
        public int maxZoom = 0;
        public string fullExtent = "";
        public string type = "png";
        public string centerPoint = "";
        public string restrictedExtent = "";
        public string projection = "4326";
    }
}
