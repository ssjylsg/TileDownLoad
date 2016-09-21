using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;

namespace MapDataTools.PublicTransport
{
    public class TransportNamesLoad
    {
        public delegate void BusNameDowningHandler(string message,int process);
        public BusNameDowningHandler busNameDowningHandler = null;        
        public void UpdataTransportLines()
        {
 
        }
        public void UpdataCityURL()
        {
            string url = String.Format("http://bus.cncn.com/change.php");
            try
            {
                TransportConfig.GetInstance().transportCityConfig.transports.Clear();
                HttpWebResponse hp = HttpHelper.CreateGetHttpResponse(url, 1000, "", null);
                string context = HttpHelper.GetResponseString(hp);
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(context);  // 加载html页面
                HtmlNode navNode = htmlDoc.DocumentNode;
                HtmlAgilityPack.HtmlNodeCollection nodes = navNode.SelectNodes("//div[@class='main mg_t6']/div/dl/dd/a");
                int index = 0;
                foreach (HtmlNode htmlNode in nodes)
                {
                    index++;
                    TransportModel model = new TransportModel();
                    string name = htmlNode.InnerText.Trim();
                    model.cityName = name;
                    model.transportURL = htmlNode.Attributes["href"].Value;
                    model.transportURL = "http://bus.cncn.com" + model.transportURL + "/gongjiao-xianlu";
                    List<string> urls = this.getURLs(model.transportURL);
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (string ul in urls)
                    {
                        string temp = "http://bus.cncn.com" + ul;
                        HttpWebResponse busHP = HttpHelper.CreateGetHttpResponse(temp, 1000, "", null);
                        string busContext = HttpHelper.GetResponseString(busHP);
                        HtmlAgilityPack.HtmlDocument busHtmlDoc = new HtmlAgilityPack.HtmlDocument();
                        busHtmlDoc.LoadHtml(busContext);  // 加载html页面
                        //HtmlNode busNode = htmlDoc.DocumentNode;
                        HtmlNode busNode = busHtmlDoc.GetElementbyId("data");
                        if (busNode == null)
                            continue;
                        HtmlAgilityPack.HtmlNodeCollection busnodes = busNode.SelectNodes("//tr/td/a");
                        if (busnodes == null)
                            continue;
                        foreach (HtmlNode n in busnodes)
                        {
                            string busName = n.InnerText.Trim();
                            busName=busName.Replace("(","");
                            busName = busName.Replace(")", "");
                            if (busName.Contains("["))
                                busName = busName.Substring(0, busName.IndexOf("["));
                            if (busName.Contains("（"))
                                busName = busName.Substring(0, busName.IndexOf("（"));
                            
                            if (!dic.ContainsKey(busName))
                            {
                                dic.Add(busName, busName);
                            }
                        }
                    }
                    foreach(KeyValuePair<string,string> k in dic)
                    {
                        if(model.busNames=="")
                            model.busNames=k.Key;
                        else
                            model.busNames=model.busNames+","+k.Key;
                    }
                    TransportConfig.GetInstance().transportCityConfig.transports.Add(model);
                    if (this.busNameDowningHandler != null)
                    {
                        string log = "正在下载城市：" + name;
                        this.busNameDowningHandler(log, index * 100 / nodes.Count);
                    }
                    TransportConfig.GetInstance().saveConfig();
                }
            }
            catch(Exception ex)
            { System.Windows.Forms.MessageBox.Show(ex.Message); }
        }
        private List<string> getURLs(string url)
        {
            List<string> urls = new List<string>();
            try
            {
                HttpWebResponse hp = HttpHelper.CreateGetHttpResponse(url, 1000, "", null);
                string context = HttpHelper.GetResponseString(hp);
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(context);  // 加载html页面
                HtmlNode navNode = htmlDoc.DocumentNode;
                HtmlAgilityPack.HtmlNodeCollection nodes = navNode.SelectNodes("//div[@class='letter']/a");
                foreach (HtmlNode htmlNode in nodes)
                {
                    string href = htmlNode.Attributes["href"].Value;
                    urls.Add(href);
                }
            }
            catch(Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return urls;
        }
    }
}
