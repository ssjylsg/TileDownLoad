using System;
using System.Collections.Generic;

namespace MapDataTools
{
    using System.IO;
    using System.Net;

    using HtmlAgilityPack;

    public interface IRoadLine
    {
        void UpdateRoads();

        event CityRoadLoadLogHandler cityRoadLoadLog;

        List<string> getCityRoads(string cityName);
    }
    public class GZipWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest webrequest = (HttpWebRequest)base.GetWebRequest(address);
            webrequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return webrequest;
        }
    }

    public class MapbarRoadLine : IRoadLine
    {

        private int k = 0;

        private int count = 0;

        public void UpdateRoads()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/mapbar.xml");
            List<CityModel> cityModels = CityURLConfig.GetInstance(filePath).cityURLConfig.cityModels;
               
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/mapbarCityRoadConfig.xml");
            List<CityRoad> cityRoads = CityRoadConfig.GetInstance(filePath).cityRoadConfig.cityRoadList;
            cityRoads.Clear();
            k = 0;
            count = cityModels.Count;
            foreach (CityModel mode in cityModels)
            {
                k++;
                string url = mode.URL;
                CityRoad road = GetRoadsByCityName(url, mode.name, cityModels.Count);
                road.cityName = mode.name.TrimEnd(new char[] { '地', '图' });
                cityRoads.Add(road);
                CityRoadConfig.GetInstance().SaveConfig();
            }
            if (this.cityRoadLoadLog != null)
            {
                string log = "下载完成";
                int process = 100;
                this.cityRoadLoadLog(log, process);
            }
        }

        public event CityRoadLoadLogHandler cityRoadLoadLog;

        public List<string> getCityRoads(string cityName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config/mapbarCityRoadConfig.xml");
            List<CityRoad> cityRoads = CityRoadConfig.GetInstance(filePath).cityRoadConfig.cityRoadList;
            foreach (CityRoad road in cityRoads)
            {
                if (road.cityName == cityName)
                {
                    return road.Roads;
                }
            }
            return new List<string>();
        }

        public CityRoad GetRoadsByCityName(string url, string modeName, int totalCount)
        {
            CityRoad road = new CityRoad();
            road.cityName = modeName;

            string context = "";
            string tempUrl = String.Format("{0}/G70/", url.TrimEnd('/'));
            try
            {

                var request = System.Net.WebRequest.Create(tempUrl) as System.Net.HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

                request.Timeout = 50000;
                var stream = request.GetResponse().GetResponseStream();
                context = new System.IO.StreamReader(stream).ReadToEnd();
                stream.Close();
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(context); // 加载html页面
                HtmlNode navNode = htmlDoc.DocumentNode;
                HtmlAgilityPack.HtmlNodeCollection nodes =
                    navNode.SelectNodes("//div[@class='sortC']/dl/dd/a");
                if (nodes == null)
                {
                    return road;
                }
                foreach (HtmlNode htmlNode in nodes)
                {
                    string name = htmlNode.InnerText.Trim();
                    road.Roads.Add(name);
                    if (this.cityRoadLoadLog != null)
                    {
                        string log = "正在下载城市：" + modeName + ",道路：" + name;
                        int process = k * 100 / totalCount;
                        this.cityRoadLoadLog(log, process);
                    }
                }
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger(this.GetType())
                    .ErrorFormat("{0}请求路网名称失败,请求地址：{1},错误信息:{2}", modeName, tempUrl, ex);
            }

            return road;
        }
    }
}
