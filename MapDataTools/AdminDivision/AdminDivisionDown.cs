using System;
using System.Collections.Generic;

namespace MapDataTools.AdminDivision
{
    public class AdminDivisionDown
    {
        public delegate void AdminDivisionDowningHandler(Divinsion divinsion,CityType type, int index, int count);
        public AdminDivisionDowningHandler AdminDivisionDowningEvent = null;
        public delegate void AdminDivisionDownedHandler();
        public AdminDivisionDownedHandler AdminDivisionDownedEvent = null;
        private string url = "http://restapi.amap.com/v3/config/district?subdistrict=1&extensions=all&level=district&key={0}&s=rsv3&output=json&keywords={1}";
        private string[] keys = new string[] { "6064051679efa89524860e1de482e294", "ddcf114352f37eb1295049911533e97e", "05d0b6e52d9ed5da644bf6922513a73a" };

        //private int timeOut = 2000;
        /// <summary>
        /// 下载百度行政区划图
        /// </summary>
        /// <param name="name">下载城市名称，可以是全国，省，市，县</param>
        /// <param name="areaCode"></param>
        /// <param name="type">名称所属类型，属于国家，还是省，市，区 ，县</param>
        public void DownAdminDivision(string name,string areaCode,CityType type)
        {
            switch (type)
            {
                case CityType.Country:
                    List<Province> provinces = CityConfig.GetInstance().Countryconfig.countries;
                    int index=1;
                    int count=1;
                    foreach (Province p in provinces)
                    {
                        count++;
                        foreach( City c in p.cities)
                        {
                            count++;
                            count+=c.districts.Count;
                        }
                    }
                    this.DownPositions(name,"", type, index, count,"");
                    foreach (Province p in provinces)
                    {
                        index++;
                        this.DownPositions(p.name,p.areacode,CityType.Province,index,count,p.baiduCode);
                        foreach (City c in p.cities)
                        {
                            index++;
                            this.DownPositions(c.name,c.areacode,CityType.City, index, count,c.baiduCode);
                        }
                    }
                    break;
                case CityType.Province:
                    int pindex=1;
                    int pcount=1;
                    this.DownPositions(name,areaCode, CityType.Province, pindex, pcount,"");
                    List<City> cities = CityConfig.GetCitiesByProvinceName(name);
                    foreach( City c in cities)
                    {
                        pcount++;
                        pcount += c.districts.Count;
                    }
                    foreach (City c in cities)
                    {
                        pindex++;
                        this.DownPositions(c.name,c.areacode, CityType.City, pindex, pcount,c.baiduCode);
                        foreach (District d in c.districts)
                        {
                            pindex++;
                            this.DownPositions(d.name,c.areacode, CityType.District, pindex, pcount,d.baiduCode);
                        }
                    }
                    break;
                case CityType.City:
                    List<District> districts = CityConfig.GetDistrictsByCityName(name);
                    int cindex=1;
                    int ccount = 1 + districts.Count;
                    if (!name.Contains("市")&&!name.Contains("自治"))
                    {
                        name = name + "市";
                    }
                    this.DownPositions(name,"", type, cindex, ccount,"");
                    foreach (District d in districts)
                    {
                        cindex++;
                        this.DownPositions(d.name,d.areacode, CityType.District, cindex, ccount,d.baiduCode);
                    }
                    break;
                case CityType.District:
                    this.DownPositions(name,areaCode, CityType.District, 1, 1,"");
                    break;
            }
            if (this.AdminDivisionDownedEvent != null)
                this.AdminDivisionDownedEvent();
        }
        private void DownPositions(string name,string areaCode,CityType cityType,int index,int count,string baiduCode)
        { 
            Random rd = new Random();
            int kindex = rd.Next(0, keys.Length);
            string tempUrl = string.Format(this.url, keys[kindex], name);
            //HttpWebResponse wp = HttpHelper.CreateGetHttpResponse(tempUrl, timeOut, "", GaodeMap.GetCookies());
            //string context = HttpHelper.GetResponseString(wp);
            string context = HttpHelper.GetRequestContent(tempUrl);
            object t = JsonHelper.JsonDeserialize<object>(context);
            Dictionary<string, object> dicContext = t as Dictionary<string, object>;
            if (dicContext != null)
            {
                Divinsion divinsion = new Divinsion();
                divinsion.name = name;
                divinsion.code = baiduCode;
                object[] districtsObj = dicContext["districts"] as object[];
                for (int i = 0; i < districtsObj.Length; i++)
                {
                    Dictionary<string,object> dicObj = districtsObj[i] as Dictionary<string,object>;
                    if (areaCode!=""&&dicObj["adcode"].ToString() != areaCode)
                    {
                        continue;
                    }
                    string polyline = dicObj["polyline"].ToString();
                    divinsion.polylines.Add(polyline);
                }
                if (this.AdminDivisionDowningEvent != null)
                {
                    this.AdminDivisionDowningEvent(divinsion,cityType, index, count);
                }
            }
        }
        //public void downCity()
        //{
        //    List<Province> provinces = CityConfig.GetInstance().Countryconfig.countries;
        //    foreach(Province p in provinces)
        //    {
        //        foreach (City c in p.cities)
        //        {
        //            Random rd = new Random();
        //            int kindex = rd.Next(0, 2);
        //            string tempUrl = string.Format(this.url, keys[kindex], c.name);
        //            HttpWebResponse wp = HttpHelper.CreateGetHttpResponse(tempUrl, 1000, "", null);
        //            string context = HttpHelper.GetResponseString(wp);
        //            object t = JsonHelper.JsonDeserialize<object>(context);
        //            Dictionary<string, object> dicContext = t as Dictionary<string, object>;
        //            if (dicContext != null)
        //            {
        //                object[] districtsObj = dicContext["districts"] as object[];
        //                Dictionary<string, object> dicObj = districtsObj[0] as Dictionary<string, object>;
        //                c.districts.Clear();
        //                object[] objCities = dicObj["districts"] as object[]; ;
        //                for (int j = 0; j < objCities.Length; j++)
        //                {
        //                    Dictionary<string, object> dic = objCities[j] as Dictionary<string, object>;
        //                    District d = new District();
        //                    d.name = dic["name"].ToString();
        //                    d.areacode = dic["adcode"].ToString();
        //                    c.districts.Add(d);
        //                }
        //            }
        //        }
        //        CityConfig.GetInstance().saveConfig();
        //    }
            
        //}
    }
    public enum CityType
    {
        Country=1,
        Province=2,
        City=3,
        District=4
    }
    public class Divinsion
    {
        public string name = "";

        public string code = "";
        public List<string> polylines = new List<string>();
    }
}
