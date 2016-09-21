using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
/*
 * Place API 是一套免费使用的API接口，调用次数限制为10万次/天
 */
namespace MapDataTools
{

    public interface IDownLoad
    {
        void GetPoiByExtentKeyWords(Extent extent, string keyWords);
        
    }

    public abstract class DownLoad : IDownLoad
    {
        public abstract void GetPoiByExtentKeyWords(Extent extent, string keyWords);
        public DowningEventHandler DowningEvent = null;
        public DowningMessageHandler DowningMessageEvent = null;
        public DownEndEventHandler DownEndEvent = null;

    }
    public delegate void DowningMessageHandler(int index, int count);
    public delegate void DownEndEventHandler(string message);
    public delegate void DowningEventHandler(POIInfo p, int index, int count);
    /// <summary>
    /// 
    /// </summary>
    public class BaiduMap : DownLoad
    {
        #region 
       
        //public DowningEventHandler downingPOIHandler = null;
        //public DowningMessageHandler DowningMessageEvent = null;
        //public DownEndEventHandler DownEndEvent = null;

        private Dictionary<string, string> dicIDs = new Dictionary<string, string>();

        private string[] urls = new string[]
                                    {
                                        "http://online1.map.bdimg.com/tile/", "http://online2.map.bdimg.com/tile/",
                                        "http://online3.map.bdimg.com/tile/"
                                    };
        private string baiduPoiUrl = "http://api.map.baidu.com/place/v2/search?query={0}&bounds={1}&output=json&page_size=200&page_num={2}&ak={3}";

        private string[] keys = new string[]
                                    {
                                        "AA5708826f754eb3d65a7358b478c20a", "GOS8PlvxFWWMO4byOQd15Gld",
                                        "uV63OIQwGo3LUs5MBGaM3Wyr", "UAYYCnaiLSdvvxFWdAKcpsXa", "uV63OIQwGo3LUs5MBGaM3Wyr"
                                    };
        private int poiCount = 0;
        private log4net.ILog log;
        #endregion
        /// <summary>
        /// 输出坐标为WGS84
        /// </summary>
        public BaiduMap()
        {
            string[] baiduKey = new string[]{};
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["baiduKey"]))
            {
                baiduKey = System.Configuration.ConfigurationSettings.AppSettings["baiduKey"].Split(',');
            }
            if (baiduKey.Length > 0)
            {
                this.keys = baiduKey.ToArray();
            }
           // this.keys = new[] { "zeTqFBtbo8P9b5YeeepZouxd", "8tSet4qeEKeRLTzpL1CRLDZq" };
            log = log4net.LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// 获取地图切片信息
        /// </summary>
        /// <param name="extent">区域</param>
        /// <param name="zoom"></param>
        /// <returns>切片范围信息</returns>
        public TitlesInfo GetTitlesInfo(Extent extent, int zoom)
        {
            TitlesInfo titleInfo = new TitlesInfo();
            double resolution = Math.Pow(2, 18 - zoom);
            titleInfo.minRow = (int)(Math.Round((extent.minX - 0) / (resolution * 256)));
            titleInfo.minCol = (int)(Math.Round((extent.minY - 23000) / (resolution * 256)));
            titleInfo.maxRow = (int)(Math.Round((extent.maxX - 0) / (resolution * 256)));
            titleInfo.maxCol = (int)(Math.Round((extent.maxY - 23000) / (resolution * 256)));
            return titleInfo;
        }
        /// <summary>
        /// 获取指定切片的地址
        /// </summary>
        /// <param name="row">行号</param>
        /// <param name="col">列号</param>
        /// <param name="zoom">层级</param>
        /// <returns>切片地址</returns>
        public string GetTitleUrl(int row, int col, int zoom)
        {
            int index = (row + col) % urls.Length;
            //string url = this.urls[index] + "?qt=tile&x=" + row + "&y=" + col + "&z=" + zoom + "&styles=pl&udt=20140807";
            return string.Format("{0}?qt=tile&x={1}&y={2}&z={3}&styles=pl&udt=20140807", this.urls[index], row, col, zoom);
            //return url;
        }
        /// <summary>
        /// 根据范围和关键字获取兴趣点信息
        /// </summary>
        /// <param name="minRow">最小行</param>
        /// <param name="maxRow">最大行</param>
        /// <param name="minCol">最小列</param>
        /// <param name="maxCol">最大列</param>
        /// <param name="zoom">层级</param>
        /// <param name="keyWord">关键字</param>
        /// <returns>兴趣点信息列表</returns>
        public List<POIInfo> GetPoiInfos(int minRow, int maxRow, int minCol, int maxCol, int zoom, string keyWord)
        {
            List<POIInfo> poiInfoList = new List<POIInfo>();
            for (int i = minRow; i < maxRow; i++)
            {
                for (int j = minCol; j < maxCol; j++)
                {
                    int index = Math.Abs(i + j) % 3;
                    Extent extent = this.getExtentByTile(i, j, zoom);
                    extent.minX = extent.minX - 5000;
                    extent.minY = extent.minY - 5000;
                    extent.maxX = extent.maxX - 5000;
                    extent.maxY = extent.maxY - 5000;
                    string url = "http://gss3.map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&pcevaname=pc2&da_par=baidu&from=webmap&qt=bkg_data&ie=utf-8&wd="
                          + keyWord + "&pl_data_type=cater&l=" + zoom + "&xy=" + i.ToString() + "_" + j.ToString() +
                          "&b=(" + extent.minX.ToString() + "," + extent.minY.ToString() + ";" + extent.maxX.ToString() + "," + extent.maxY.ToString() + ")&callback=";
                    try
                    {
                        List<POIInfo> poinfos = this.GetPoiInfos(url);
                        poiInfoList.AddRange(poinfos);
                    }
                    catch(Exception ex)
                    {
                        log.ErrorFormat("请求{0} 出错:{1}", url, ex);
                    }
                }
            }
            return poiInfoList;
        }

        private Extent getExtentByTile(int row, int col, int zoom)
        {
            Extent extent = new Extent();
            double resolution = Math.Pow(2, zoom - 18);
            extent.minX = row * 256 * resolution;
            extent.minY = col * 256 * resolution;
            extent.maxX = (row + 1) * 256 * resolution;
            extent.maxY = (col + 1) * 256 * resolution;
            return extent;
        }

        /// <summary>
        /// 根据范围获取百度所有信息坐标点
        /// </summary>
        /// <param name="minRow">最小行</param>
        /// <param name="maxRow">最大行</param>
        /// <param name="minCol">最小列</param>
        /// <param name="maxCol">最大列</param>
        /// <param name="zoom">层级</param>
        /// <returns>返回兴趣点信息列表</returns>
        public List<POIInfo> GetPoiInfos(int minRow, int maxRow, int minCol, int maxCol, int zoom)
        {
            List<POIInfo> poiInfoList = new List<POIInfo>();
            for (int i = minRow; i < maxRow; i++)
            {
                for (int j = minCol; j < maxCol; j++)
                {
                    int index = Math.Abs(i + j) % 3;
                    string url = "http://online" + index.ToString() + ".map.bdimg.com/js/?qt=vQuest&styles=pl&x=" + i + "&y=" + j + "&z=" + zoom.ToString() + "&v=064&fn=MPC_Mgr.getPoiDataCbk";
                    try
                    {
                        List<POIInfo> poinfos = this.GetPoiInfos(url);
                        poiInfoList.AddRange(poinfos);
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("请求{0} 出错:{1}", url, ex);
                    }
                }
            }
            return poiInfoList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<POIInfo> GetPoiInfos(string url)
        {
            List<POIInfo> poiList = new List<POIInfo>();
            HttpWebResponse hwr = HttpHelper.CreateGetHttpResponse(url, 1000, "Opera/9.25 (Windows NT 6.0; U; en)", null);
            string context = HttpHelper.GetResponseString(hwr);
            context = context.Substring(22, context.Length - 24);
            object t = JsonHelper.JsonDeserialize<object>(context);
            Dictionary<string, object> dicT = t as Dictionary<string, object>;
            Dictionary<string, object> dicInfos = (dicT["content"] as object[])[0] as Dictionary<string, object>;
            object[] infos = dicInfos["uids"] as object[];
            if (infos.Length == 0)
                return poiList;
            foreach (object i in infos.ToList())
            {
                Dictionary<string, object> icon = (i as Dictionary<string, object>)["icon"] as Dictionary<string, object>;
                object x = icon["x"];
                object y = icon["y"];
                object ty = (i as Dictionary<string, object>)["type"];
                object name = (i as Dictionary<string, object>)["name"];
                object uid = (i as Dictionary<string, object>)["uid"];
                double doubleX = double.Parse(x.ToString());
                double doubleY = double.Parse(y.ToString());
                string id = uid.ToString();
                POIInfo poiInfo = new POIInfo();
                poiInfo.name = name.ToString();
                poiInfo.type = ty.ToString();
                poiInfo.x = doubleX;
                poiInfo.y = doubleY;
                Coord c = CoordHelper.BdDecrypt(poiInfo.y, poiInfo.x);
                c = CoordHelper.Gcj2Wgs(c.lon, c.lat);
                poiInfo.cx = c.lon;
                poiInfo.cy = c.lat;
                if (this.dicIDs.ContainsKey(id))
                {
                    continue;
                }
                else
                {
                    POIDeInfo poiDeInfo = this.GetDetailInfo(id);
                    Coord coord = this.coordProjection(doubleX, doubleY);
                    if (coord != null)
                    {
                        poiInfo.x = coord.lon;
                        poiInfo.y = coord.lat;
                    }
                    if (poiDeInfo != null)
                    {
                        poiInfo.phone = poiDeInfo.phone;
                        poiInfo.type = poiDeInfo.type;
                        poiInfo.address = poiDeInfo.address;
                    }
                }
                poiList.Add(poiInfo);
            }
            return poiList;
        }
        public POIDeInfo GetDetailInfo(string poiID)
        {
            POIDeInfo poiDetailInfo = new POIDeInfo();
            ///根据id去请求相信信息，
            string tempUrl = "http://map.baidu.com/?qt=inf&uid=" + poiID + "&t=1428040686134";
            try
            {
              
                HttpWebResponse ht = HttpHelper.CreateGetHttpResponse(tempUrl, 1000, "Opera/9.25 (Windows NT 6.0; U; en)", null);
                string ff = HttpHelper.GetResponseString(ht);
                object detail = JsonHelper.JsonDeserialize<object>(ff);
                string ty = "";
                if (detail == null || (detail as Dictionary<string, object>)["content"] == null)
                {
                    return poiDetailInfo;
                }
                Dictionary<string, object> dicContext = (detail as Dictionary<string, object>)["content"] as Dictionary<string, object>;
                if (dicContext.ContainsKey("std_tag") && dicContext["std_tag"] != null)
                {
                    object cla = dicContext["std_tag"];
                    ty = cla.ToString();
                }
                string address = "";
                if (dicContext.ContainsKey("addr") && dicContext["addr"] != null)
                    address = dicContext["addr"].ToString();
                string tel = "";
                if (dicContext.ContainsKey("tel") && dicContext["tel"] != null)
                    tel = dicContext["tel"].ToString();
                string n = "";
                Dictionary<string, object> dicResult = (detail as Dictionary<string, object>)["result"] as Dictionary<string, object>;
                if (dicResult != null && dicResult.ContainsKey("wd") && dicResult["wd"] != null)
                    n = dicResult["wd"].ToString();

                string city = "";
                Dictionary<string, object> dicCity = (detail as Dictionary<string, object>)["current_city"] as Dictionary<string, object>;
                if (dicCity != null && dicCity["name"] != null)
                {
                    city = dicCity["name"].ToString();
                }
                string province = "";
                if (dicCity != null && dicCity["up_province_name"] != null)
                {
                    province = dicCity["up_province_name"].ToString();
                }
                poiDetailInfo.address = address;
                poiDetailInfo.id = poiID;
                poiDetailInfo.phone = tel;
                poiDetailInfo.type = ty;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("请求{0} 出错:{1}", tempUrl, ex);
            }
            return poiDetailInfo;
        }
        /// <summary>
        /// 百度平面坐标转换经纬度坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Coord coordProjection(double x, double y)
        {
            //进行坐标转换，将平面坐标转换成经纬度坐标
            string coordsUrl = "http://api.map.baidu.com/geoconv/v1/?coords=" + x + "," + y + "&from=6&to=5&ak=AA5708826f754eb3d65a7358b478c20a";
            HttpWebResponse coordshr = HttpHelper.CreateGetHttpResponse(coordsUrl, 1000, "Opera/9.25 (Windows NT 6.0; U; en)", null);
            string cf = HttpHelper.GetResponseString(coordshr);
            object coord = JsonHelper.JsonDeserialize<object>(cf);
            if (coord != null)
            {
                Dictionary<string, object> dicCoord = coord as Dictionary<string, object>;
                if (dicCoord["result"] != null && (dicCoord["result"] as object[])[0] != null)
                {
                    Dictionary<string, object> c = (dicCoord["result"] as object[])[0] as Dictionary<string, object>;
                    if (c["x"] != null && c["y"] != null)
                    {
                        Coord coordInfo = new Coord();
                        double.TryParse(c["x"].ToString(), out coordInfo.lon);
                        double.TryParse(c["y"].ToString(), out coordInfo.lat);
                        return coordInfo;
                    }
                }
            }
            return null;
        }

        private List<POIInfo> getKeywordPOI(string url)
        {
            List<POIInfo> poiList = new List<POIInfo>();
            HttpWebResponse hwr = HttpHelper.CreateGetHttpResponse(url, 1000, "Opera/9.25 (Windows NT 6.0; U; en)", null);
            string context = HttpHelper.GetResponseString(hwr);
            object t = JsonHelper.JsonDeserialize<object>(context);
            Dictionary<string, object> dicT = t as Dictionary<string, object>;
            object[] dicInfos = dicT["uids"] as object[];
            for (int i = 0; i < dicInfos.Length; i++)
            {
                Dictionary<string, object> dic = dicInfos[i] as Dictionary<string, object>;
                POIInfo poiInfo = new POIInfo();
                object objX = dic["x"];
                object objY = dic["y"];
                object objID = dic["uid"];
                object objName = dic["name"];
                if (objX != null && objY != null)
                {
                    double.TryParse(objX.ToString(), out poiInfo.x);
                    double.TryParse(objY.ToString(), out poiInfo.y);
                }
                if (objName != null)
                {
                    poiInfo.name = objName.ToString();
                }
                if (objID != null)
                {
                    string id = objID.ToString();
                    POIDeInfo poiDetailInfo = this.getDetailInfoKeyWord(id);
                    poiInfo.address = poiDetailInfo.address;
                    poiInfo.phone = poiDetailInfo.phone;
                    poiInfo.type = poiDetailInfo.type;
                }
                Coord coord = this.coordProjection(poiInfo.x, poiInfo.y);
                if (coord != null)
                {
                    poiInfo.x = coord.lon;
                    poiInfo.y = coord.lat;
                    Coord c = CoordHelper.BdDecrypt(poiInfo.y, poiInfo.x);
                    c = CoordHelper.Gcj2Wgs(c.lon, c.lat);
                    poiInfo.cx = c.lon;
                    poiInfo.cy = c.lat;
                }
                poiList.Add(poiInfo);
            }
            return poiList;
        }

        private POIDeInfo getDetailInfoKeyWord(string id)
        {
            POIDeInfo poiDetailInfo = new POIDeInfo();
            poiDetailInfo.id = id;
            string url = "http://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&pcevaname=pc2&da_par=baidu&from=webmap&qt=inf&uid=" + id + "&ie=utf-8";
            HttpWebResponse hwr = HttpHelper.CreateGetHttpResponse(url, 1000, "Opera/9.25 (Windows NT 6.0; U; en)", null);
            string context = HttpHelper.GetResponseString(hwr);
            object t = JsonHelper.JsonDeserialize<object>(context);
            Dictionary<string, object> detail = t as Dictionary<string, object>;
            if (detail == null || (detail as Dictionary<string, object>)["content"] == null)
            {
                return poiDetailInfo;
            }
            Dictionary<string, object> dicContext = (detail as Dictionary<string, object>)["content"] as Dictionary<string, object>;
            if (dicContext.ContainsKey("addr") && dicContext["addr"] != null)
            {
                poiDetailInfo.address = dicContext["addr"].ToString();
            }
            if (dicContext.ContainsKey("tag") && dicContext["tag"] != null)
            {
                poiDetailInfo.type = dicContext["tag"].ToString();
            }
            if (dicContext.ContainsKey("tel") && dicContext["tel"] != null)
            {
                poiDetailInfo.phone = dicContext["tel"].ToString();
            }
            return poiDetailInfo;
        }
        private POIDeInfo getDetailInfo2(string id)
        {
            POIDeInfo poiDetailInfo = new POIDeInfo();
            poiDetailInfo.id = id;
            string url = "http://api.map.baidu.com/place/v2/detail?uid={0}&ak={1}&output=json";
            Random rd = new Random();
            int kindex = rd.Next(0, this.keys.Length);
            url = string.Format(url, id, this.keys[kindex]);
            HttpWebResponse hwr = HttpHelper.CreateGetHttpResponse(url, 1000, "Opera/9.25 (Windows NT 6.0; U; en)", null);
            string context = HttpHelper.GetResponseString(hwr);
            object t = JsonHelper.JsonDeserialize<object>(context);
            Dictionary<string, object> detail = t as Dictionary<string, object>;
            if (detail == null || (detail as Dictionary<string, object>)["result"] == null)
            {
                return poiDetailInfo;
            }
            Dictionary<string, object> dicContext = (detail as Dictionary<string, object>)["result"] as Dictionary<string, object>;
            if (dicContext.ContainsKey("name") && dicContext["name"] != null)
            {
                poiDetailInfo.name = dicContext["name"].ToString();
            }
            if (dicContext.ContainsKey("location") && dicContext["location"] != null)
            {
                Dictionary<string, object> dicxy = dicContext["location"] as Dictionary<string, object>;
                string stringX = dicxy["lng"] != null ? dicxy["lng"].ToString() : "";
                string stringY = dicxy["lat"] != null ? dicxy["lat"].ToString() : "";
                double.TryParse(stringX, out poiDetailInfo.x);
                double.TryParse(stringY, out poiDetailInfo.y);
            }
            if (dicContext.ContainsKey("address") && dicContext["address"] != null)
            {
                poiDetailInfo.address = dicContext["address"].ToString();
            }
            if (dicContext.ContainsKey("telephone") && dicContext["telephone"] != null)
            {
                poiDetailInfo.phone = dicContext["telephone"].ToString();
            }
            return poiDetailInfo;
        }
        public override void GetPoiByExtentKeyWords(Extent extent, string keyWords)
        {

            //string logpath = System.Windows.Forms.Application.StartupPath + "\\" + DateTime.Now.ToShortDateString().ToString().Replace("/", "") + ".txt";
            //FileStream fs = new FileStream(logpath, FileMode.Append);
            double minx = extent.minX;
            double miny = extent.minY;
            double maxx = extent.maxX;
            double maxy = extent.maxY;
            var step = 0.05; //0.03
            int xCount = (int)Math.Floor((maxx - minx) / step);
            int yCount = (int)Math.Floor((maxy - miny) / step);
            int index = 0;
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    index++;
                    double x1 = minx + i * step;
                    double x2 = minx + (i + 1) * step;
                    double y1 = miny + j * step;
                    double y2 = miny + (j + 1) * step;
                    this.DownPoIbyExtent(x1, y1, x2, y2, keyWords, index, xCount * yCount);
                }
            }
            if (this.DownEndEvent != null)
            {
                this.DownEndEvent("下载完成");
            }
        }
        private void DownPoIbyExtent(double x1, double y1, double x2, double y2, string keyWords, int index, int count)
        {
            for (int pindex = 0; pindex < 38; pindex++)
            {
                var keyWordsList = new List<string>();
                if (keyWords == string.Empty)
                {
                    keyWordsList.Add("美食$宾馆$购物$汽车$生活$结婚$丽人$金融$休闲娱乐$医疗");
                    keyWordsList.Add("旅游$教育$房地产$政府机构$公司企业$公交站$学校$小区");
                }
                else
                {
                    keyWordsList.Add(keyWords);
                }

                foreach (string keyword in keyWordsList)
                {
                    Random rd = new Random();
                    int kindex = rd.Next(0, this.keys.Length);
                    string url = string.Format(
                        this.baiduPoiUrl,
                        keyword,
                        y1 + "," + x1 + "," + y2 + "," + x2,
                        pindex,
                        this.keys[kindex]);

                    try
                    {
                        HttpWebResponse ht = HttpHelper.CreateGetHttpResponse(
                            url,
                            1000,
                            "Opera/9.25 (Windows NT 6.0; U; en)",
                            null);
                        string ff = HttpHelper.GetResponseString(ht);
                        var detail = JsonHelper.JsonDeserialize<object>(ff);
                        var dicResult = detail as Dictionary<string, object>;
                        if (dicResult.ContainsKey("status") && dicResult["status"].ToString() != "0")
                        {
                            log.ErrorFormat("{0}请求失败,返回码：{1}", url, dicResult["status"]);
                            continue;
                        }
                        if (dicResult != null && dicResult.ContainsKey("results"))
                        {
                            object poiObjs = dicResult["results"];
                            var pois = poiObjs as object[];
                            if (pois.Length == 0)
                            {
                                if (this.DowningMessageEvent != null)
                                {
                                    this.DowningMessageEvent(index, count);
                                }
                                break;
                            }
                            foreach (object poi in pois)
                            {
                                var poiInfo = new POIInfo();
                                this.poiCount++;
                                var objects = poi as Dictionary<string, object>;
                                if (objects.ContainsKey("name") && objects["name"] != null)
                                {
                                    poiInfo.name = objects["name"].ToString();
                                }
                                if (objects.ContainsKey("location") && objects["location"] != null)
                                {
                                    var dicxy = objects["location"] as Dictionary<string, object>;
                                    string stringX = dicxy["lng"] != null ? dicxy["lng"].ToString() : "";
                                    string stringY = dicxy["lat"] != null ? dicxy["lat"].ToString() : "";
                                    double.TryParse(stringX, out poiInfo.x);
                                    double.TryParse(stringY, out poiInfo.y);
                                    Coord c = CoordHelper.BdDecrypt(poiInfo.y, poiInfo.x);
                                    c = CoordHelper.Gcj2Wgs(c.lon, c.lat);
                                    poiInfo.cx = c.lon;
                                    poiInfo.cy = c.lat;
                                }
                                if (objects.ContainsKey("address") && objects["address"] != null) poiInfo.address = objects["address"].ToString();

                                if (objects.ContainsKey("telephone") && objects["telephone"] != null)
                                {
                                    poiInfo.phone = objects["telephone"].ToString();
                                }
                                if (this.DowningEvent != null)
                                {
                                    this.DowningEvent(poiInfo, index, count);
                                }
                            }
                        }
                        else
                        {
                            log.WarnFormat("请求{0} 出错", url);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("请求{0} 出错:{1}", url, ex);
                    }
                }

                if (this.DowningMessageEvent != null)
                {
                    this.DowningMessageEvent(index, count);
                }
            }
        }
    }
}
