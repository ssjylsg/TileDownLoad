using System;
using System.Collections.Generic;
using System.Net;

namespace MapDataTools
{
    public interface IMap
    {
        /// <summary>
        /// 获取地图切片信息
        /// </summary>
        /// <param name="minX">最小纬度</param>
        /// <param name="minY">最小经度</param>
        /// <param name="maxX">最大纬度</param>
        /// <param name="maxY">最大经度</param>
        /// <param name="zoom">地图级别</param>
        /// <returns>切信息，切片行列号起止信息</returns>
        TitlesInfo getTitlesInfo(Extent extent, int zoom);

        /// <summary>
        /// 根据类型获取切片地址信息（影像，矢量）
        /// </summary>
        /// <param name="row">行号</param>
        /// <param name="col">列号</param>
        /// <param name="zoom">地图级别</param>
        /// <param name="layerType">0为矢量，1为影像</param>
        /// <returns>切片地址</returns>
        string GetTitleUrl(int row, int col, int zoom,int layerType);

        /// <summary>
        /// 根据范围和关键字获取兴趣点信息
        /// </summary>
        /// <param name="minRow">最小行号</param>
        /// <param name="maxRow">最大行号</param>
        /// <param name="minCol">最小列号</param>
        /// <param name="maxCol">最大列号</param>
        ///<param name="zoom">地图层级</param>
        /// <returns>兴趣点信息列表</returns>
        List<POIInfo> GetPoiInfos(int minRow, int maxRow, int minCol, int maxCol, int zoom);

        /// <summary>
        /// 根据id获取兴趣点详细信息
        /// </summary>
        /// <param name="id">兴趣点唯一ID</param>
        /// <returns></returns>
        POIDeInfo getDetailInfo(string id);

        /// <summary>
        /// 根据关键字获取兴趣点信息
        /// </summary>
        /// <param name="minRow">最小行号</param>
        /// <param name="maxRow">最大行号</param>
        /// <param name="minCol">最小列号</param>
        /// <param name="maxCol">最大列号</param>
        ///<param name="zoom">地图层级</param>
        /// <param name="keyWord">关键字</param>
        /// <returns>兴趣点列表</returns>
        List<POIInfo> GetPoiInfos(int minRow, int maxRow, int minCol, int maxCol, int zoom, string keyWord);
    }

    public class GoogleMap : IMap
    {
        private double maxExtent = 20037508.34;
        private double maxResolution = 156543.03390625;
        /// <summary>
        /// 没下载多少条数据保存一次，默认100条
        /// </summary>
        private int saveStep = 100;
        public delegate void SaveDataHandler(List<POIInfo> poiInfos);
        /// <summary>
        /// 委托，间断性保存数据
        /// </summary>
        public SaveDataHandler SaveDataEvent = null;

        /// <summary>
        /// 获取地图切片信息
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="zoom">地图级别</param>
        /// <returns>切信息，切片行列号起止信息</returns>
        public TitlesInfo getTitlesInfo(Extent extent, int zoom)
        {
            TitlesInfo titleInfos = new TitlesInfo();
            titleInfos.minRow = (int)Math.Floor((extent.minX + maxExtent) / (maxResolution / (Math.Pow(2, zoom)) * 256.0));
            titleInfos.maxRow = (int)Math.Ceiling((extent.maxX + maxExtent) / (maxResolution / (Math.Pow(2, zoom)) * 256.0));
            titleInfos.minCol = (int)Math.Floor((maxExtent - extent.maxY) / (maxResolution / (Math.Pow(2, zoom)) * 256.0));
            titleInfos.maxCol = (int)Math.Ceiling((maxExtent - extent.minY) / (maxResolution / (Math.Pow(2, zoom)) * 256.0));
            return titleInfos;
        }
        /// <summary>
        /// 根据类型获取切片地址信息（影像，矢量）
        /// </summary>
        /// <param name="row">行号</param>
        /// <param name="col">列号</param>
        /// <param name="zoom">地图级别</param>
        /// <param name="layerType">0为矢量，1为影像</param>
        /// <returns>切片地址</returns>
        public string GetTitleUrl(int row, int col, int zoom,int layerType)
        {
            int index = (row + col) % 2;
            string url = string.Format("http://mt{0}.google.cn/vt?pb=!1m4!1m3!1i{1}!2i{2}!3i{3}!2m3!1e0!2sm!3i271000000!3m11!2szh-cn!3scn!5e18!12m1!1e47!12m3!1e37!2m1!1ssmartmaps!12m1!1e47!4e0",index,zoom,row,col);
            if (layerType == 1)
            {
                url = string.Format("http://mt{0}.google.cn/vt?lyrs=s@168&hl=zh-CN&gl=CN&x={1}&y={2}&z={3}",index,row,col,zoom);
            }
            return url;
        }
        /// <summary>
        /// 根据范围和关键字获取兴趣点信息
        /// </summary>
        /// <param name="minRow">最小行号</param>
        /// <param name="maxRow">最大行号</param>
        /// <param name="minCol">最小列号</param>
        /// <param name="maxCol">最大列号</param>
        ///<param name="zoom">地图层级</param>
        /// <returns>兴趣点信息列表</returns>
        public List<POIInfo> GetPoiInfos(int minRow, int maxRow, int minCol, int maxCol, int zoom)
        {
            List<POIInfo> poiInfoList = new List<POIInfo>();
            for (int i = minRow; i < maxRow; i++)
            {
                for (int j = minCol; j < maxCol; j++)
                {
                    try
                    {
                        string url = "http://mt0.google.cn/vt?pb=";
                        string n = "!1m4!1m3!1i" + zoom.ToString() + "!2i" + i.ToString() + "!3i" + j.ToString();
                        url += n;
                        url += "!2m3!1e0!2sm!3i!3m9!2szh-CN!3sCN!5e18!12m1!1e50!12m3!1e37!2m1!1ssmartmaps!4e3&callback";
                        Cookie ck = new Cookie("NID", "67=NFg3Uhf4DA2v8o5ocSGVOL-aRgTgIQuu60KMagzDXXio5NfyWfZ1GdnCiOD0oiMZ5KNcgLjw80g_lIRN7qduKn8DaObXjWUI6T_ikmqcBVkqYPaXHyefhb1leb1c4oEI", "/", "mt0.google.cn");
                        Cookie ck2 = new Cookie("PREF", "ID=49a50c62c452e4b2:U=8fb8ca3a2befd2dd:NW=1:TM=1419327444:LM=1419327658:S=sFI0jDHVntczyAsT", "/", "mt0.google.cn");
                        CookieCollection coll = new CookieCollection();
                        coll.Add(ck);
                        coll.Add(ck2);
                        HttpWebResponse coordshr = HttpHelper.CreateGetHttpResponse(url, 5000, "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36", coll);
                        string cf = HttpHelper.GetResponseString(coordshr);
                        object fObj = JsonHelper.JsonDeserialize<object>(cf);
                        foreach (object b in (fObj as Array))
                        {
                            Dictionary<string, object> dic = b as Dictionary<string, object>;
                            object o = dic["features"];
                            foreach (object xo in (o as Array))
                            {
                                Dictionary<string, object> dxo = xo as Dictionary<string, object>;
                                POIInfo poiInfo = new POIInfo();
                                string id = dxo["id"].ToString();
                                object nobj = JsonHelper.JsonDeserialize<object>(dxo["c"].ToString());
                                Dictionary<string, object> dicname = nobj as Dictionary<string, object>;
                                Dictionary<string, object> dicName2 = dicname["1"] as Dictionary<string, object>;
                                poiInfo.name = dicName2["title"].ToString();
                                POIDeInfo pDeinfo = this.getDetailInfo(id);
                                poiInfo.x = pDeinfo.x;
                                poiInfo.y = pDeinfo.y;
                                poiInfo.type = pDeinfo.type;
                                poiInfo.phone = pDeinfo.phone;
                                poiInfo.address = pDeinfo.address;
                                poiInfoList.Add(poiInfo);
                                if (this.SaveDataEvent != null&&poiInfoList.Count%this.saveStep==0)
                                {
                                    this.SaveDataEvent(poiInfoList);
                                }
                            }
                        }
                    }
                    catch
                    { }
                }
            }
            return poiInfoList;
        }
        /// <summary>
        /// 根据id获取兴趣点详细信息
        /// </summary>
        /// <param name="id">兴趣点唯一ID</param>
        /// <returns></returns>
        public POIDeInfo getDetailInfo(string id)
        {
            POIDeInfo dinfo = new POIDeInfo();
            string url = "http://www.google.cn/maps/preview/entity?authuser=0&hl=zh-CN&pb=!1m14!1s0x0000000000000000%3A0x";
            string temp = "!3m9!1m3!1d33333!2d0!3d0!2m0!3m2!1i1440!2i777!4f13.1!4m2!3d0!4d0!12m9!1e1!1e2!1e5!1e9!1e3!1e10!1e12!1e15!4smaps_sv.tactile!13m14!1e1!1e2!1e5!1e11!1e4!1e3!1e9!1e10!1e12!1e15!2m2!1i203!2i100!5smaps_sv.tactile!14m6!1s0!3b1!4m1!2i5210!7e81!12e9!22m1!1e810";
            url = url + id + temp;
            Cookie ck = new Cookie("NID", "67=NFg3Uhf4DA2v8o5ocSGVOL-aRgTgIQuu60KMagzDXXio5NfyWfZ1GdnCiOD0oiMZ5KNcgLjw80g_lIRN7qduKn8DaObXjWUI6T_ikmqcBVkqYPaXHyefhb1leb1c4oEI", "/", "mt0.google.cn");
            Cookie ck2 = new Cookie("PREF", "ID=49a50c62c452e4b2:U=8fb8ca3a2befd2dd:NW=1:TM=1419327444:LM=1419327658:S=sFI0jDHVntczyAsT", "/", "mt0.google.cn");
            CookieCollection coll = new CookieCollection();
            coll.Add(ck);
            coll.Add(ck2);
            HttpWebResponse coordshr = HttpHelper.CreateGetHttpResponse(url, 5000, "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36", coll);
            string cf = HttpHelper.GetResponseString(coordshr);
            cf = cf.Substring(5);
            object fObj = JsonHelper.JsonDeserialize<object>(cf);
            object x = fObj;
            object[] xs = fObj as object[];
            object xs1 = xs[0];
            object[] xs1s = xs1 as object[];
            object xs2 = xs1s[1];
            object[] xs2s = xs2 as object[];
            object xs3 = xs2s[0];
            object[] xs3s = xs3 as object[];
            object xs4 = xs3s[14];
            object[] xs4s = xs4 as object[];
            object xs5 = xs4s[2];
            object[] xs5s = xs5 as object[];
            if (xs5s.Length > 0)
            {
                for (int i = 0; i < xs5s.Length; i++)
                {
                    if (xs5s != null)
                    {
                        dinfo.address += xs5s[i].ToString();
                    }
                }
            }
            object xs10 = xs4s[3];
            object[] xs10s = xs10 as object[];
            if (xs10s!=null && xs10s.Length > 0 && xs10s[0] != null)
            {
                dinfo.phone = xs10s[0].ToString();
            }
            object xs6 = xs4s[9];
            object[] xs6s = xs6 as object[];
            if (xs6s[3] != null)
            {
                double.TryParse(xs6s[3].ToString(), out dinfo.x);
            }
            if (xs6s[2] != null)
            {
                double.TryParse(xs6s[2].ToString(), out dinfo.y);
            }
            object xs7 = xs4s[13];
            object[] xs7s = xs7 as object[];
            object xs8 = xs7s[0];
            if (xs8 != null)
            {
                dinfo.type = xs8.ToString();
            }
            return dinfo;
        }
        /// <summary>
        /// 根据关键字获取兴趣点信息
        /// </summary>
        /// <param name="minRow">最小行号</param>
        /// <param name="maxRow">最大行号</param>
        /// <param name="minCol">最小列号</param>
        /// <param name="maxCol">最大列号</param>
        ///<param name="zoom">地图层级</param>
        /// <param name="keyWord">关键字</param>
        /// <returns>兴趣点列表</returns>
        public List<POIInfo> GetPoiInfos(int minRow, int maxRow, int minCol, int maxCol, int zoom, string keyWord)
        {
            List<POIInfo> poiInfoList = new List<POIInfo>();
            for (int i = minRow; i < maxRow; i++)
            {
                for (int j = minCol; j < maxCol; j++)
                {
                    try
                    {
                        string url = "http://mt0.google.cn/vt?pb=";
                        string n = "!1m4!1m3!1i" + zoom.ToString() + "!2i" + i.ToString() + "!3i" + j.ToString();
                        url += n;
                        url += "!2m3!1e0!2sm!3i!3m9!2szh-CN!3sCN!5e18!12m1!1e50!12m3!1e37!2m1!1ssmartmaps!4e3&callback";
                        Cookie ck = new Cookie("NID", "67=NFg3Uhf4DA2v8o5ocSGVOL-aRgTgIQuu60KMagzDXXio5NfyWfZ1GdnCiOD0oiMZ5KNcgLjw80g_lIRN7qduKn8DaObXjWUI6T_ikmqcBVkqYPaXHyefhb1leb1c4oEI", "/", "mt0.google.cn");
                        Cookie ck2 = new Cookie("PREF", "ID=49a50c62c452e4b2:U=8fb8ca3a2befd2dd:NW=1:TM=1419327444:LM=1419327658:S=sFI0jDHVntczyAsT", "/", "mt0.google.cn");
                        CookieCollection coll = new CookieCollection();
                        coll.Add(ck);
                        coll.Add(ck2);
                        HttpWebResponse coordshr = HttpHelper.CreateGetHttpResponse(url, 5000, "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36", coll);
                        string cf = HttpHelper.GetResponseString(coordshr);
                        object fObj = JsonHelper.JsonDeserialize<object>(cf);
                        foreach (object b in (fObj as Array))
                        {
                            Dictionary<string, object> dic = b as Dictionary<string, object>;
                            object o = dic["features"];
                            foreach (object xo in (o as Array))
                            {
                                Dictionary<string, object> dxo = xo as Dictionary<string, object>;
                                POIInfo poiInfo = new POIInfo();
                                string id = dxo["id"].ToString();
                                object nobj = JsonHelper.JsonDeserialize<object>(dxo["c"].ToString());
                                Dictionary<string, object> dicname = nobj as Dictionary<string, object>;
                                Dictionary<string, object> dicName2 = dicname["1"] as Dictionary<string, object>;
                                poiInfo.name = dicName2["title"].ToString();
                                POIDeInfo pDeinfo = this.getDetailInfo(id);
                                if (pDeinfo.type.Contains(keyWord) || poiInfo.name.Contains(keyWord))
                                {
                                    poiInfo.x = pDeinfo.x;
                                    poiInfo.y = pDeinfo.y;
                                    poiInfo.type = pDeinfo.type;
                                    poiInfo.phone = pDeinfo.phone;
                                    poiInfo.address = pDeinfo.address;
                                    poiInfoList.Add(poiInfo);
                                    if (this.SaveDataEvent != null && poiInfoList.Count % this.saveStep == 0)
                                    {
                                        this.SaveDataEvent(poiInfoList);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    { }
                }
            }
            return poiInfoList;
        }
    }
}
