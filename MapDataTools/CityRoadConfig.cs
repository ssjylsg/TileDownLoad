using System;
using System.Collections.Generic;
using System.IO;

namespace MapDataTools
{
    public class CityRoadConfig
    {
         /// <summary>
        /// 单例模式singleton
        /// </summary>
        static CityRoadConfig instance = null;
        public CityRoads cityRoadConfig = new CityRoads();
        //默认保存路径
        public static string DefaultConfigXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/CityRoadConfig.xml");

        /// <summary>
        /// 获取配置信息（单例模式）
        /// </summary>
        /// <returns></returns>
        public static CityRoadConfig GetInstance(string configPath = "")
        {
            return instance ?? (instance = new CityRoadConfig(configPath));
        }

        public CityRoadConfig(string configPath = "")
        {
            if (!string.IsNullOrEmpty(configPath))
            {
                DefaultConfigXml = configPath;
            }
            XmlStorageHelper xmler = new XmlStorageHelper();
            if (!File.Exists(DefaultConfigXml))
            {
                return;
            }
            xmler.LoadFromFile(cityRoadConfig, DefaultConfigXml);
        }
        public void SaveConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            xmler.SaveToFile(cityRoadConfig, DefaultConfigXml);
        }
        public List<string> GetRoadNamesByCityName(string cityName)
        {
            cityName = cityName.TrimEnd('市');
            foreach (CityRoad r in cityRoadConfig.cityRoadList)
            {
                if (r.cityName.Contains(cityName) || cityName.Contains(r.cityName))
                {
                    return r.Roads;
                }
            }
            return new List<string>();
        }
        public CityRoad GetRoadName(string cityName)
        {
            cityName = cityName.TrimEnd('市');
            foreach (CityRoad r in cityRoadConfig.cityRoadList)
            {
                if (r.cityName.Contains(cityName) || cityName.Contains(r.cityName))
                {
                    return r;
                }
            }
            return new CityRoad();
        }
    }
    public class CityRoads
    {
        public List<CityRoad> cityRoadList = new List<CityRoad>();
    }
    public class CityRoad
    {
        public string cityName = "";
        public List<string> Roads = new List<string>();
    }
}
