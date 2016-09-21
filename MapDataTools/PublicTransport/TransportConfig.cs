using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MapDataTools.PublicTransport
{
    /// <summary>
    /// 全国公交线路配置信息,包含城市及其相关路线
    /// </summary>
    public class TransportConfig
    {
        /// <summary>
        /// 单例模式singleton
        /// </summary>
        static TransportConfig instance = null;
        public CityTransport transportCityConfig = new CityTransport();
        //默认保存路径
        public static string DefaultConfigXml = Application.StartupPath + Path.DirectorySeparatorChar + "TransportConfig.xml";

        /// <summary>
        /// 获取配置信息（单例模式）
        /// </summary>
        /// <returns></returns>
        public static TransportConfig GetInstance()
        {
            if (instance == null)
                instance = new TransportConfig();
            return instance;
        }

        private TransportConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            if (!File.Exists(DefaultConfigXml))
                return;
            xmler.LoadFromFile(transportCityConfig, DefaultConfigXml);
        }
        public void saveConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            xmler.SaveToFile(transportCityConfig, DefaultConfigXml);
        }
        public List<string> getBusNameByCityName(string name)
        {
            foreach (TransportModel model in transportCityConfig.transports)
            {
                if (model.cityName == name)
                {
                    string busNames = model.busNames;
                    string[] names = busNames.Split(',');
                    List<string> busNameList= names.ToList();
                    return busNameList;
                }
            }
            return new List<string>();
        }
    }
    public class CityTransport
    {
       public List<TransportModel> transports = new List<TransportModel>();
    }
    public class TransportModel
    {
        public string cityName="";
        public string transportURL="";
        public string busNames = "";
    }
}