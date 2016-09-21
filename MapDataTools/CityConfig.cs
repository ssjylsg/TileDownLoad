using System;
using System.Collections.Generic;
using System.IO;

namespace MapDataTools
{
    public class CityConfig
    {
        /// <summary>
        /// 单例模式singleton
        /// </summary>
        static CityConfig instance = null;
        public Country Countryconfig = new Country();
        //默认保存路径
        public static string DefaultConfigXml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/CityConfig.xml");

        /// <summary>
        /// 获取配置信息（单例模式）
        /// </summary>
        /// <returns></returns>
        public static CityConfig GetInstance()
        {
            return instance ?? (instance = new CityConfig());
        }

        private CityConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            if (!File.Exists(DefaultConfigXml))
                return;
            xmler.LoadFromFile(Countryconfig, DefaultConfigXml);
        }
        public void SaveConfig()
        {
            XmlStorageHelper xmler = new XmlStorageHelper();
            xmler.SaveToFile(Countryconfig, DefaultConfigXml);
        }
        public List<City> GetCityByName(string name)
        {
            List<City> cities = new List<City>();
            foreach (Province p in Countryconfig.countries)
            {
                foreach (City c in p.cities)
                {
                    if (c.name.Contains(name))
                    {
                        cities.Add(c);
                    }
                }
            }
            return cities;
        }
        public static List<City> GetCitiesByProvinceName(string name)
        {
            foreach (Province p in instance.Countryconfig.countries)
            {
                if (p.name == name)
                {
                    return p.cities;
                }
            }
            return new List<City>();
        }
        public static List<District> GetDistrictsByCityName(string name)
        {
            foreach (Province p in instance.Countryconfig.countries)
            {
                foreach (City c in p.cities)
                {
                    if (c.name == name)
                        return c.districts;
                }
            }
            return new List<District>();
        }
    }
    public class Country
    {
        public List<Province> countries = new List<Province>();
    }
    public class Province
    {
        public string name = "";
        public string areacode = "";

        public string baiduCode = "";
        public List<City> cities = new List<City>();
    }
    public class City
    {
        public string name = "";
        public string areacode = "";
        public string baiduCode = "";
        public List<District> districts = new List<District>();
        public string gaodeCode = "";
        public string pinyin = "";
    }
    public class District
    {
        public string name = "";
        public string areacode = "";
        public string baiduCode = "";
        public List<Business> business = new List<Business>(); 
    }

    public class Business
    {
        public string baiduCode = "";

        public string name = "";
    }
}