using System;
using System.Collections.Generic;
using System.Net;

namespace MapDataTools
{
    using System.Linq;

    /// <summary>
    /// 输出坐标为WGS84
    /// </summary>
    public class GaodeMap : DownLoad//: IMap
    {
        #region private 
        private double maxExtent = 20037508.3427892;
        private int index = 0;
        private int count = 0;
        private int POICount = 0;
        public bool isStop = false;
     
        //public DowningEventHandler downingPOIHandler = null;
     
        //public DowningMessageHandler DowningMessageEvent = null;
      
        //public DownEndEventHandler DownEndEvent = null;


        private string[] imgUrls = new string[] { "http://webst01.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=6", "http://webst02.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=6", "http://webst03.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=6", "http://webst04.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=6" };
        private string url = "http://restapi.amap.com/v3/place/polygon?polygon={0}&s=rsv3&key={1}&offset=50&page={2}&keywords={3}";
        private string[] keys = new string[] { "6064051679efa89524860e1de482e294", "ddcf114352f37eb1295049911533e97e", "05d0b6e52d9ed5da644bf6922513a73a" };
        private Dictionary<string, string> dic = new Dictionary<string, string>();
        string[] urls = new string[] { "http://webrd01.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=7", "http://webrd02.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=7", "http://webrd03.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=7", "http://webrd04.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=7" };
        private string addressUrl = "http://restapi.amap.com/v3/geocode/geo?key={0}&s=rsv3&city={1}&address={2}";
        private string locationUrl = "http://restapi.amap.com/v3/geocode/regeo?location={0}&key={1}&s=rsv3";

        private double[] resolutions = new double[]
                                           {
                                               156543.0339, 78271.516953125, 39135.7584765625, 19567.87923828125,
                                               9783.939619140625, 4891.9698095703125, 2445.9849047851562,
                                               1222.9924523925781, 611.4962261962891, 305.74811309814453,
                                               152.87405654907226, 76.43702827453613, 38.218514137268066,
                                               19.109257068634033, 9.554628534317016, 4.777314267158508, 2.388657133579254,
                                               1.194328566789627, 0.5971642833948135,
                                           };
        private log4net.ILog log;

        #endregion

        /// <summary>
        /// 输出坐标为WGS84
        /// </summary>
        public GaodeMap()
        {
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["gaoDeKey"]))
            {
                keys = System.Configuration.ConfigurationSettings.AppSettings["gaoDeKey"].Split(',');
            }
            log = log4net.LogManager.GetLogger(this.GetType());
        }
        /// <summary>
        /// 获取地图切片信息
        /// </summary>
        /// <param name="extent">区域</param>
        /// <returns>切片范围信息</returns>
        public TitlesInfo getTitlesInfo(Extent extent, int zoom)
        {
            double resolution = this.resolutions[zoom];
            TitlesInfo titleInfo = new TitlesInfo();
            titleInfo.minRow = (int)(Math.Floor((extent.minX + maxExtent) / (resolution * 256)));
            titleInfo.maxCol = (int)(Math.Ceiling((maxExtent - extent.minY) / (resolution * 256)));
            titleInfo.maxRow = (int)(Math.Ceiling((extent.maxX + maxExtent) / (resolution * 256)));
            titleInfo.minCol = (int)(Math.Floor((maxExtent - extent.maxY) / (resolution * 256)));
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
            string url = this.urls[index] + "&z=" + zoom.ToString() + "&y=" + col.ToString() + "&x=" + row.ToString();
            return url;
        }
        public string GetImgTileUrl(int row, int col, int zoom)
        {
            int index = (row + col) % imgUrls.Length;
            string url = this.imgUrls[index] + "&z=" + zoom.ToString() + "&y=" + col.ToString() + "&x=" + row.ToString();
            return url;
        }
        /// <summary>
        /// 获取指定范围的所有兴趣点信息
        /// </summary>
        /// <param name="minRow">最小行</param>
        /// <param name="maxRow">最大行</param>
        /// <param name="minCol">最小列</param>
        /// <param name="MaxCol">最大列</param>
        /// <param name="zoom">层级</param>
        public void GetPoiInfos(int minRow, int maxRow, int minCol, int MaxCol, int zoom)
        {
            this.count = (maxRow - minRow + 1) * (MaxCol - minCol + 1);
            for (int i = minRow; i < maxRow; i++)
            {
                for (int j = minCol; j < MaxCol; j++)
                {
                    this.index++;
                    try
                    {
                        int index = Math.Abs(i + j) % 3;
                        string rcz = zoom.ToString() + "," + i.ToString() + "," + j.ToString();
                        string url = "http://vdata.amap.com/tiles?v=2&style=1&t=" + rcz + "&callback=";
                        this.GetPoiInfos(url);
                    }
                    catch
                    {
                    }
                    if (this.DowningMessageEvent != null)
                    {
                        this.DowningMessageEvent(this.index, this.count);
                    }
                }
            }
            if (this.DownEndEvent != null)
            {
                this.DownEndEvent("下载完成");
            }
        }

        /// <summary>
        /// 根据地址查询兴趣点信息
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns>兴趣点信息</returns>
        private void GetPoiInfos(string url)
        {
            try
            {
                HttpWebResponse hwr = HttpHelper.CreateGetHttpResponse(url, 1000, "Opera/9.25 (Windows NT 6.0; U; en)", GetCookies());
                string context = HttpHelper.GetResponseString(hwr);
                object t = JsonHelper.JsonDeserialize<object>(context);
                object[] xs = t as object[];
                for (int i = 0; i < xs.Length; i++)
                {
                    object ps = xs[i];
                    object[] pps = ps as object[];
                    object[] objects = pps[4] as object[];
                    for (int j = 0; j < objects.Length; j++)
                    {
                        object[] oos = objects[j] as object[];
                        object[] xos = oos[0] as object[];
                        for (int k = 0; k < xos.Length; k++)
                        {
                            object[] obs = xos[k] as object[];
                            string name = (obs.Length > 0 && obs[0] != null) ? obs[0].ToString() : "";
                            string id = (obs.Length > 0 && obs[obs.Length - 1] != null) ? obs[obs.Length - 1].ToString() : "";
                            if (id != "")
                            {
                                POIInfo poiInfo = new POIInfo();
                                POIDeInfo poiDeInfo = this.getDetailInfo(id);
                                if (poiDeInfo == null || poiDeInfo.name == "")
                                    continue;
                                poiInfo.name = poiDeInfo.name;
                                poiInfo.address = poiDeInfo.address.Replace(",", "");
                                //poiInfo.address.Replace(",", "");
                                poiInfo.type = poiDeInfo.type;
                                poiInfo.phone = poiDeInfo.phone;
                                poiInfo.x = poiDeInfo.x;
                                poiInfo.y = poiDeInfo.y;
                                Coord c = CoordHelper.Gcj2Wgs(poiInfo.x, poiInfo.y);
                                poiInfo.cx = c.lon;
                                poiInfo.cy = c.lat;
                                poiInfo.pName = poiDeInfo.pName;
                                poiInfo.cName = poiDeInfo.cName;
                                poiInfo.dName = poiDeInfo.dName;
                                poiInfo.roadName = poiDeInfo.roadName;
                                this.POICount++;
                                if (this.DowningEvent != null)
                                {
                                    this.DowningEvent(poiInfo, this.index, this.count);
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private  static CookieCollection coll;
        public static CookieCollection GetCookies()
        {
            if (coll == null || coll.Count == 0)
            {
                Cookie ck = new Cookie("__utma", "240281747.1415545504.1422095776.1422095776.1422095776.1", "/", "www.amap.com");
                Cookie ck2 = new Cookie("__utmz", "240281747.1422095776.1.1.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=%E9%AB%98%E5%BE%B7%E5%9C%B0%E5%9B%BE", "/", "www.amap.com");
                Cookie ck3 = new Cookie("key", "6064051679efa89524860e1de482e294", "/", "www.amap.com");
                Cookie ck4 = new Cookie("_ga", "GA1.2.1067756989.1418979928", "/", "www.amap.com");
                Cookie ck5 = new Cookie("cna", "FFcMDTQwN38CAXHIn2TVRmsg", "/", "www.amap.com");
                Cookie ck6 = new Cookie("isg", "AQV7Aqd4hRtFW1DOdDRh/EVbhZTHz4Uf", "/", "www.amap.com");
                Cookie ck7 = new Cookie("l", "AQV7Aqd4hRtFW1DOdDRh", "/", "www.amap.com");
                Cookie ck8 = new Cookie("Kdw4_5279_lastvisit", "1429667476", "/", "www.amap.com");
                Cookie ck9 = new Cookie("Kdw4_5279_saltkey", "eKQxp65W", "/", "www.amap.com");
                Cookie ck10 = new Cookie("Kdw4_5279_visitedfid", "37D40D48", "/", "www.amap.com");
                coll = new CookieCollection();
                coll.Add(ck);
                coll.Add(ck2);
                coll.Add(ck3);
                coll.Add(ck4);
                coll.Add(ck5);
                coll.Add(ck6);
                coll.Add(ck7);
                coll.Add(ck8);
                coll.Add(ck9);
                coll.Add(ck10);
            }
            return coll;
        }

        /// <summary>
        /// 根据地址获取兴趣点信息（关键字查询）
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        private void getPoiInfosKeyWord(string url)
        {
            try
            {

                HttpWebResponse hwr = HttpHelper.CreateGetHttpResponse(url, 1000, "Opera/9.25 (Windows NT 6.0; U; en)", GetCookies());
                string context = HttpHelper.GetResponseString(hwr);
                object t = JsonHelper.JsonDeserialize<object>(context);
                Dictionary<string, object> ts = t as Dictionary<string, object>;
                if (ts.ContainsKey("tiles"))
                {
                    object[] objs = ts["tiles"] as object[];
                    Dictionary<string, object> ob = objs[0] as Dictionary<string, object>;
                    object[] osx = ob["tile"] as object[];
                    for (int i = 0; i < osx.Length; i++)
                    {
                        POIInfo poiInfo = new POIInfo();
                        Dictionary<string, object> dicox = osx[i] as Dictionary<string, object>;
                        string id = dicox["id"].ToString();
                        poiInfo.name = dicox["name"].ToString();
                        POIDeInfo poiDeInfo = this.getDetailInfo(id);
                        if (poiDeInfo != null && poiDeInfo.name != "")
                        {
                            poiInfo.name = poiDeInfo.name;
                            poiInfo.address = poiDeInfo.address;
                            poiInfo.type = poiDeInfo.type;
                            poiInfo.phone = poiDeInfo.phone;
                            poiInfo.x = poiDeInfo.x;
                            poiInfo.y = poiDeInfo.y;
                            Coord c = CoordHelper.Gcj2Wgs(poiInfo.x, poiInfo.y);
                            poiInfo.cx = c.lon;
                            poiInfo.cy = c.lat;
                            poiInfo.roadName = poiDeInfo.roadName;
                            poiInfo.pName = poiDeInfo.pName;
                            poiInfo.cName = poiDeInfo.cName;
                            poiInfo.dName = poiDeInfo.dName;
                            poiInfo.address.Replace(",", "");
                            this.POICount++;
                            if (this.DowningEvent != null)
                            {
                                this.DowningEvent(poiInfo, this.index, this.count);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("请求{0} 出错:{1}", url, ex);
            }
        }
        int poiindex = 0;

        /// <summary>
        /// 根据id查询兴趣点详细信息
        /// </summary>
        /// <param name="id">兴趣点id</param>
        /// <returns>兴趣点详细信息</returns>
        public POIDeInfo getDetailInfo(string id)
        {
            POIDeInfo dinfo = new POIDeInfo();

            poiindex++;
            Random r = new Random();
            int index = r.Next(0, this.keys.Length);
            string url = "http://restapi.amap.com/v3/place/detail?id=" + id + "&s=rsv3&key="
                         + this.keys[index];
            try
            {
                HttpWebResponse ht = HttpHelper.CreateGetHttpResponse(
                    url,
                    1000,
                    "Opera/9.25 (Windows NT 6.0; U; en)",
                    null);
                string ff = HttpHelper.GetResponseString(ht);
                object detail = JsonHelper.JsonDeserialize<object>(ff);
                Dictionary<string, object> dicContext = detail as Dictionary<string, object>;
                if (!dicContext.ContainsKey("pois")) return dinfo;
                object obj = dicContext["pois"];
                if (obj != null)
                {
                    object[] objs = obj as object[];
                    if (objs.Length > 0)
                    {
                        Dictionary<string, object> objects = objs[0] as Dictionary<string, object>;
                        string name = objects["name"] != null ? objects["name"].ToString() : "";
                        string address = objects["address"] != null ? objects["address"].ToString() : "";
                        address = address == "System.Object[]" ? string.Empty : address;
                        string tel = "";
                        if (objects["tel"] is Array)
                        {
                            object[] otels = objects["tel"] as object[];
                            for (int i = 0; i < otels.Length; i++)
                            {
                                if (tel != "")
                                {
                                    tel = tel + ";" + otels[i] != null ? otels[i].ToString() : "";
                                }
                                else
                                {
                                    tel = otels[i] != null ? otels[i].ToString() : "";
                                }
                            }

                        }
                        else
                        {
                            tel = objects["tel"] != null ? objects["tel"].ToString() : "";
                        }
                        string xy = objects["location"] != null ? objects["location"].ToString() : "";
                        string str_longitude = xy.Split(',')[0];
                            //objects["longitude"] != null ? objects["longitude"].ToString() : "";
                        string str_latitude = xy.Split(',')[1];
                            //objects["latitude"] != null ? objects["latitude"].ToString() : "";
                        //string typecode = objects["typecode"] != null ? objects["typecode"].ToString() : "";
                        string typeString = objects["type"] != null ? objects["type"].ToString() : "";
                        dinfo.pName = objects["pname"] != null ? objects["pname"].ToString() : "";
                        dinfo.cName = objects["cityname"] != null ? objects["cityname"].ToString() : "";
                        dinfo.dName = objects["adname"] != null ? objects["adname"].ToString() : "";
                        dinfo.name = name;
                        dinfo.address = address;
                        dinfo.type = typeString;
                        dinfo.phone = tel;
                        double.TryParse(str_longitude, out dinfo.x);
                        double.TryParse(str_latitude, out dinfo.y);
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("请求{0} 出错:{1}", url, ex);
            }
            return dinfo;
        }

        /// <summary>
        /// 根据范围和关键字获取兴趣点信息
        /// </summary>
        /// <param name="minRow">最小行</param>
        /// <param name="maxRow">最大行</param>
        /// <param name="minCol">最小列</param>
        /// <param name="MaxCol">最大列</param>
        /// <param name="zoom">层级</param>
        /// <param name="keyWord">关键字</param>
        public void GetPoiInfos(int minRow, int maxRow, int minCol, int MaxCol, int zoom, string keyWord)
        {
            this.count = (maxRow - minRow + 1) * (MaxCol - minCol + 1);
            for (int i = minRow; i < maxRow + 1; i++)
            {
                for (int j = minCol; j < MaxCol + 1; j++)
                {
                    this.index++;
                    string url = "http://vector.amap.com/mass/" + keyWord + "/" + zoom + "/" + i + "/" + j + ".json?&cbk=";
                    this.getPoiInfosKeyWord(url);
                    if (this.DowningMessageEvent != null)
                        this.DowningMessageEvent(this.index, this.count);
                }
            }
            if (this.DownEndEvent != null)
            {
                this.DownEndEvent("下载完成");
            }
        }
        public void GetPoIbyExtent(Extent extent, string keyWords)
        {
            Coord topLeft = new Coord(extent.minX, extent.minY);
            topLeft = CoordHelper.WebMercator2lonLat(topLeft);
            Coord bottomRight = new Coord(extent.maxX, extent.maxY);
            bottomRight = CoordHelper.WebMercator2lonLat(bottomRight);
            double minx = topLeft.lon;
            double miny = topLeft.lat;
            double maxx = bottomRight.lon;
            double maxy = bottomRight.lat;
            int xCount = (int)Math.Ceiling((maxx - minx) / 0.01);
            int yCount = (int)Math.Ceiling((maxy - miny) / 0.01);
            int index = 0;
            keyWords = keyWords != "" ? keyWords : "汽车|摩托|餐饮|购物|生活|体育|医疗|住宿|风景|住宅|政府|科教|交通|金融|公司|行政地名|自然地名";
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    index++;
                    double x1 = minx + i * 0.01;
                    double x2 = minx + (i + 1) * 0.01;
                    double y1 = miny + j * 0.01;
                    double y2 = miny + (j + 1) * 0.01;
                    if (i + 1 == xCount)
                    {
                        x2 = maxx;
                    }
                    if (j + 1 == yCount)
                        y2 = maxy;
                    this.DownPoIbyExtent(x1, y1, x2, y2, keyWords, index, xCount * yCount);
                }
            }
            if (this.DownEndEvent != null)
            {
                this.DownEndEvent("下载完成");
            }
        }

        
        private void DownPoIbyExtent(double x1, double y1, double x2, double y2, string keyWord, int curentIndex, int count)
        {
            keyWord = !String.IsNullOrEmpty(keyWord) ? keyWord : "汽车|摩托|餐饮|购物|生活|体育|医疗|住宿|风景|住宅|政府|科教|交通|金融|公司|行政地名|自然地名";
            for (int pindex = 1; pindex < 21; pindex++)
            {
                string url = string.Format(
                       this.url,
                       x1.ToString() + "," + y1.ToString() + ";" + x2.ToString() + "," + y2.ToString(),
                       this.keys[pindex % this.keys.Length],
                       pindex,
                       keyWord);
                try
                {
                    HttpWebResponse ht = HttpHelper.CreateGetHttpResponse(
                        url,
                        1000,
                        "Opera/9.25 (Windows NT 6.0; U; en)",
                        null);
                    string ff = HttpHelper.GetResponseString(ht);
                    object detail = JsonHelper.JsonDeserialize<object>(ff);
                    Dictionary<string, object> dicResult = detail as Dictionary<string, object>;
                    //{errcode:30000,errmsg:"Denied",data:{host:"wangsu.traffitor.amap.com"}}
                    if (dicResult != null && dicResult.ContainsKey("errcode"))
                    {
                        log.WarnFormat("请求{0}出现错误，详细:{1}",url,ff);
                        continue;
                    }
                    if (dicResult != null && dicResult["status"] != null  && dicResult["status"].ToString() == "1")
                    {
                        object poiObjs = dicResult["pois"];
                        object[] pois = poiObjs as object[];
                        if (pois.Length == 0)
                        {
                            break;
                        }
                        else
                        {
                            for (int i = 0; i < pois.Length; i++)
                            {
                                POIInfo poiInfo = new POIInfo();
                                this.POICount++;
                                Dictionary<string, object> objects = pois[i] as Dictionary<string, object>;
                                string id = objects["id"] != null ? objects["id"].ToString() : "";
                                poiInfo.name = objects["name"] != null ? objects["name"].ToString() : "";
                                if (objects["address"] is Array)
                                {
                                    string address = "";
                                    object[] addresses = objects["tel"] as object[];
                                    for (int j = 0; j < addresses.Length; j++)
                                    {
                                        if (address != "")
                                        {
                                            address = address + ";" + addresses[j] != null ? addresses[j].ToString() : "";
                                        }
                                        else
                                        {
                                            address = addresses[j] != null ? addresses[j].ToString() : "";
                                        }
                                    }
                                }
                                else
                                {
                                    poiInfo.address = objects["address"] != null ? objects["address"].ToString() : "";
                                }
                                poiInfo.address.Replace(",", "");
                                string tel = "";
                                if (objects["tel"] is Array)
                                {
                                    object[] otels = objects["tel"] as object[];
                                    for (int j = 0; j < otels.Length; j++)
                                    {
                                        if (tel != "")
                                        {
                                            tel = tel + ";" + otels[j] != null ? otels[j].ToString() : "";
                                        }
                                        else
                                        {
                                            tel = otels[j] != null ? otels[j].ToString() : "";
                                        }
                                    }

                                }
                                else
                                {
                                    tel = objects["tel"] != null ? objects["tel"].ToString() : "";
                                }
                                string xy = objects["location"] != null ? objects["location"].ToString() : "";
                                string str_longitude = xy.Split(',')[0];
                                    //objects["longitude"] != null ? objects["longitude"].ToString() : "";
                                string str_latitude = xy.Split(',')[1];
                                    //objects["latitude"] != null ? objects["latitude"].ToString() : "";
                                poiInfo.type = objects["type"] != null ? objects["type"].ToString() : "";
                                poiInfo.phone = tel;
                                double.TryParse(str_longitude, out poiInfo.x);
                                double.TryParse(str_latitude, out poiInfo.y);
                                Coord c = CoordHelper.Gcj2Wgs(poiInfo.x, poiInfo.y);
                                poiInfo.cx = c.lon;
                                poiInfo.cy = c.lat;

                                if (this.DowningEvent != null)
                                {
                                    this.DowningEvent(poiInfo, curentIndex, count);
                                }
                                if (this.DowningMessageEvent != null) this.DowningMessageEvent(curentIndex, count);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("请求{0} 出错:{1}",url,ex);
                    Console.WriteLine(ex.Message);
                }
                System.Threading.Thread.Sleep(500);
            }
        }
        /// <summary>
        /// 根据类型编号获取类型信息
        /// </summary>
        /// <param name="code">类型编号</param>
        /// <returns>类型信息</returns>
        public string GetTypeByCode(string code)
        {
            return this.dic.ContainsKey(code) ? this.dic[code] : null;
        }

        public string GetAddressByLocation(double x, double y)
        {
            return this.GetAddressByLocation(x, y, false);
        }
        public string GetAddressByLocation(double x, double y,bool isWgs84)
        {
         
            Coord c = new Coord(x, y);
            if (isWgs84)
            {
                c = CoordHelper.Transform(x, y);
            }
            string tempURL = string.Format(this.locationUrl, c.lon.ToString() + "," + c.lat.ToString(), this.keys[System.DateTime.Now.Ticks % this.keys.Length]);
            try
            {
                System.Threading.Thread.Sleep(500);
                HttpWebResponse webresponse = HttpHelper.CreateGetHttpResponse(tempURL, 1000, null, null);
                string context = HttpHelper.GetResponseString(webresponse);
                object resultObj = JsonHelper.JsonDeserialize<object>(context);
                if (resultObj != null)
                {
                    Dictionary<string, object> resultDic = resultObj as Dictionary<string, object>;
                    if (resultDic != null && resultDic.ContainsKey("regeocode"))
                    {
                        object addressObj = resultDic["regeocode"];
                        string address = (addressObj as Dictionary<string, object>)["formatted_address"].ToString();
                        return address;
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("请求{0} 出错:{1}", tempURL, ex);
            }
            return "";
        }
        public Coord GetLocationByAddress(string address, string cityCode)
        {
            return this.GetLocationByAddress(address, cityCode, false);
        }
        public Coord GetLocationByAddress(string address, string cityCode,bool jiupian)
        { 
            string tempURL = string.Format(this.addressUrl, this.keys[System.DateTime.Now.Ticks % this.keys.Length], cityCode, address);
            try
            {
                System.Threading.Thread.Sleep(500);
                HttpWebResponse webresponse = HttpHelper.CreateGetHttpResponse(tempURL, 1000, null, null);
                string context = HttpHelper.GetResponseString(webresponse);
                object resultObj = JsonHelper.JsonDeserialize<object>(context);
                if (resultObj != null)
                {
                    Dictionary<string, object> resultDic = resultObj as Dictionary<string, object>;
                    if (resultDic != null && resultDic.ContainsKey("geocodes"))
                    {
                        object locationObj = resultDic["geocodes"];
                        if ((locationObj as object[]).Length > 0)
                        {
                            string location = ((locationObj as object[])[0] as Dictionary<string, object>)["location"].ToString();
                            string[] xy = location.Split(',');
                            if (xy.Length == 2)
                            {
                                Coord c = new Coord();
                                if (double.TryParse(xy[0], out c.lon) && double.TryParse(xy[1], out c.lat))
                                {
                                    if (jiupian)
                                    {
                                        c = CoordHelper.Gcj2Wgs(c.lon, c.lat);
                                    }
                                    return c;
                                }
                                else
                                    return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("请求{0} 出错:{1}", tempURL, ex);
            }
            return null;
        }


        public string GetTitleUrl(int row, int col, int zoom, int layerType)
        {
            return this.GetTitleUrl(row, col, zoom);
        } 


        public override void GetPoiByExtentKeyWords(Extent extent, string keyWords)
        {
              double minx = extent.minX;
            double miny = extent.minY;
            double maxx = extent.maxX;
            double maxy = extent.maxY;
            int xCount = (int)Math.Floor((maxx - minx) / 0.03);
            int yCount = (int)Math.Floor((maxy - miny) / 0.03);
            int index = 0;
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    index++;
                    double x1 = minx + i * 0.03;
                    double x2 = minx + (i + 1) * 0.03;
                    double y1 = miny + j * 0.03;
                    double y2 = miny + (j + 1) * 0.03;
                    this.DownPoiInfobyExtent(x1, y1, x2, y2, keyWords, index, xCount * yCount);
                }
            }
            if (this.DownEndEvent != null)
            {
                this.DownEndEvent("下载完成");
            }
        }

        private string poiUrl =
            "http://ditu.amap.com/service/poiInfo?query_type=TQUERY&city=&keywords={0}&pagesize=20&pagenum={1}&qii=true&cluster_state=5&need_utd=true&utd_sceneid=1000&div=PC1000&addr_poi_merge=true&is_classify=true&geoobj={2}";
        private void DownPoiInfobyExtent(double x1, double y1, double x2, double y2, string keyWord, int curentIndex, int count)
        {
            keyWord = !String.IsNullOrEmpty(keyWord) ? keyWord : "汽车|摩托|餐饮|购物|生活|体育|医疗|住宿|风景|住宅|政府|科教|交通|金融|公司|行政地名|自然地名|小区|学校";
            for (int pindex = 1; pindex < 21; pindex++)
            {
                string tempUrl = string.Format(
                    this.poiUrl,
                    keyWord,
                    pindex,
                    string.Join("|", new[] { x1.ToString(), y1.ToString(), x2.ToString(), y2.ToString() }));
                try
                {
                    HttpWebResponse ht = HttpHelper.CreateGetHttpResponse(
                        tempUrl,
                        1000,
                        "Opera/9.25 (Windows NT 6.0; U; en)",
                        null);
                    string ff = HttpHelper.GetResponseString(ht);
                    object detail = JsonHelper.JsonDeserialize<object>(ff);
                    Dictionary<string, object> dicResult = detail as Dictionary<string, object>;
                    //{errcode:30000,errmsg:"Denied",data:{host:"wangsu.traffitor.amap.com"}}
                    if (dicResult != null && dicResult.ContainsKey("status") && dicResult.ContainsKey("status").ToString() != "1")
                    {
                        log.WarnFormat("请求{0}出现错误，详细:{1}", tempUrl, ff);
                        continue;
                    }
                    if (dicResult != null && dicResult["status"] != null && dicResult["status"].ToString() == "1")
                    {
                        object poiObjs = dicResult["list"];
                        object[] pois = poiObjs as object[];
                        if (pois.Length == 0)
                        {
                            break;
                        }
                        else
                        {
                            for (int i = 0; i < pois.Length; i++)
                            {
                                POIInfo poiInfo = new POIInfo();
                                this.POICount++;
                                Dictionary<string, object> objects = pois[i] as Dictionary<string, object>;
                               
                                poiInfo.name = objects["name"] != null ? objects["name"].ToString() : "";
                                if (objects["address"] is Array)
                                {
                                    string address = "";
                                    object[] addresses = objects["tel"] as object[];
                                    for (int j = 0; j < addresses.Length; j++)
                                    {
                                        if (address != "")
                                        {
                                            address = address + ";" + addresses[j] != null ? addresses[j].ToString() : "";
                                        }
                                        else
                                        {
                                            address = addresses[j] != null ? addresses[j].ToString() : "";
                                        }
                                    }
                                }
                                else
                                {
                                    poiInfo.address = objects["address"] != null ? objects["address"].ToString() : "";
                                }
                                poiInfo.address.Replace(",", "");
                                string tel = "";
                                if (objects["tel"] is Array)
                                {
                                    object[] otels = objects["tel"] as object[];
                                    for (int j = 0; j < otels.Length; j++)
                                    {
                                        if (tel != "")
                                        {
                                            tel = tel + ";" + otels[j] != null ? otels[j].ToString() : "";
                                        }
                                        else
                                        {
                                            tel = otels[j] != null ? otels[j].ToString() : "";
                                        }
                                    }
                                }
                                else
                                {
                                    tel = objects["tel"] != null ? objects["tel"].ToString() : "";
                                }
                                poiInfo.type = objects["type"] != null ? objects["type"].ToString() : "";
                                poiInfo.phone = tel;


                                string str_longitude = objects["longitude"].ToString();
                                string str_latitude = objects["latitude"].ToString();
                               
                                double.TryParse(str_longitude, out poiInfo.x);
                                double.TryParse(str_latitude, out poiInfo.y);

                                Coord c = CoordHelper.Gcj2Wgs(poiInfo.x, poiInfo.y);
                                poiInfo.cx = c.lon;
                                poiInfo.cy = c.lat;

                                if (objects.ContainsKey("templateData"))
                                {
                                    var templateData = (Dictionary<string, object>)objects["templateData"];
                                    if (templateData.ContainsKey("aoi"))
                                    {
                                        var aoi = ((Dictionary<string, object>)objects["templateData"])["aoi"].ToString();
                                        var tempAois = aoi.Split('_').ToList().Select(
                                            m => CoordHelper.Gcj2Wgs(
                                                double.Parse(m.Split(',')[0]),
                                                double.Parse(m.Split(',')[1])).ToString()).ToArray();
                                        poiInfo.aoi = string.Join("_", tempAois);
                                    }
                                }

                                if (this.DowningEvent != null)
                                {
                                    this.DowningEvent(poiInfo, curentIndex, count);
                                }
                                if (this.DowningMessageEvent != null) this.DowningMessageEvent(curentIndex, count);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("请求{0} 出错:{1}", url, ex);
                    Console.WriteLine(ex.Message);
                }
                System.Threading.Thread.Sleep(500);
            }
        }
    }
    
}
