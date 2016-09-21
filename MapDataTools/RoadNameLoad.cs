using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

using HtmlAgilityPack;

namespace MapDataTools
{
    public delegate void CityRoadLoadLogHandler(string log, int process);
    public class RoadNameLoad:IRoadLine
    {

        public event CityRoadLoadLogHandler cityRoadLoadLog = null;
        int k = 0;
        int count = 0;
        public RoadNameLoad()
        {
        }
        public List<string> getCityRoads(string cityName)
        {
            List<CityRoad> cityRoads = CityRoadConfig.GetInstance().cityRoadConfig.cityRoadList;
            foreach (CityRoad road in cityRoads)
            {
                if (road.cityName == cityName)
                {
                    return road.Roads;
                }
            }
            return new List<string>();
        }
        /// <summary>
        /// 获取全国市区地图地址URL
        /// </summary>
        public void UpdataCityURL()
        {
            List<CityModel> cityModels = CityURLConfig.GetInstance().cityURLConfig.cityModels;
            cityModels.Clear();
            string url = String.Format("http://www.city8.com/#cityaf");
            try
            {
                HttpWebResponse hp = HttpHelper.CreateGetHttpResponse(url, 1000, "", null);
                string context = HttpHelper.GetResponseString(hp);
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(context);  // 加载html页面
                HtmlNode navNode = htmlDoc.DocumentNode;
                HtmlAgilityPack.HtmlNodeCollection nodes = navNode.SelectNodes("//div[@class='v5_ll_test']/ul/li/a");
                foreach (HtmlNode htmlNode in nodes)
                {
                    CityModel model = new CityModel();
                    string name = htmlNode.InnerText.Trim();
                    model.name = name;
                    model.URL = htmlNode.Attributes["href"].Value;
                    if (this.cityRoadLoadLog != null)
                    {
                        string log = "正在下载城市：" + name;
                        int process = 100;
                        this.cityRoadLoadLog(log, process);
                    }
                }
                CityURLConfig.GetInstance().SaveConfig();
            }
            catch
            {
                MessageBox.Show("更新失败");
            }
        }
        public void UpdateRoads()
        {
            List<CityModel> cityModels = CityURLConfig.GetInstance().cityURLConfig.cityModels;
            List<CityRoad> cityRoads = CityRoadConfig.GetInstance().cityRoadConfig.cityRoadList;
            cityRoads.Clear();
            k=0;
            count = cityModels.Count;
            foreach (CityModel mode in cityModels)
            {
                k++;
                  string url = mode.URL;
                CityRoad road = GetRoadsByCityName(url, mode.name, cityModels.Count);
                road.cityName = mode.name.TrimEnd(new char[] { '地', '图' });
                cityRoads.Add(road);
                //CityURLConfig.GetInstance().saveConfig();
                CityRoadConfig.GetInstance().SaveConfig();
            }
            if (this.cityRoadLoadLog != null)
            {
                string log = "下载完成";
                int process = 100;
                this.cityRoadLoadLog(log, process);
            }
        }
        public CityRoad GetRoadsByCityName(string url,string modeName,int totalCount)
        {
            CityRoad road = new CityRoad();
            road.cityName = modeName;
            string[] codes = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            for (int i = 0; i < codes.Length; i++)
            {
                string context = "";
                string tempUrl = String.Format("{0}road/{1}/", url, codes[i]);
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Encoding = new System.Text.UTF8Encoding();
                        context = webClient.DownloadString(tempUrl);
                    }
                    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(context);  // 加载html页面
                    HtmlNode navNode = htmlDoc.DocumentNode;
                    HtmlAgilityPack.HtmlNodeCollection nodes = navNode.SelectNodes("//div[@class='road_sahngjia road_zm_list']/a");
                    if (nodes == null) {
                        continue;
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
                    log4net.LogManager.GetLogger(this.GetType()).ErrorFormat("{0}请求路网名称失败,请求地址：{1},错误信息:{2}", modeName, tempUrl, ex);
                }
            }
            return road;

        }


 
    }
}
