using System.Collections.Generic;
using System.IO;

namespace MapDataTools
{
    public class CityURLConfig
    {
          /// <summary>
        /// 单例模式singleton
        /// </summary>
        static CityURLConfig instance = null;
        public CityURL cityURLConfig = new CityURL();
        //默认保存路径
        public static string DefaultConfigXml = Path.Combine(
            System.AppDomain.CurrentDomain.BaseDirectory,
            "config/CityURLConfig.xml");

        /// <summary>
        /// 获取配置信息（单例模式）
        /// </summary>
        /// <returns></returns>
        public static CityURLConfig GetInstance(string path = "")
        {
            return instance ?? (instance = new CityURLConfig(path));
        }

        private CityURLConfig(string path="")
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            if (!string.IsNullOrEmpty(path))
            {
                DefaultConfigXml = path;
            }
            if (!File.Exists(DefaultConfigXml))
                return;
            xmler.LoadFromFile(cityURLConfig, DefaultConfigXml);
        }
        public void SaveConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            xmler.SaveToFile(cityURLConfig, DefaultConfigXml);
        }
    }
    public class CityURL
    {
        public List<CityModel> cityModels = new List<CityModel>();
    }
    public class CityModel
    {
        public string name="";
        public string URL="";
    }
}
