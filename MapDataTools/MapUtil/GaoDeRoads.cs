using System.Collections.Generic;

//高德地图API的服务调用有什么限制？

//地理/逆地理编码，每日每Key调用限制200000次，每10分钟内调用次数限制10000次

//Place查询，每日每Key调用限制100000次，每10分钟内调用次数限制50000次

//输入提示，每日每Key调用限制100000次，每10分钟内调用次数限制50000次

//路线规划，每日每Key调用限制100000次，每10分钟内调用次数限制5000次

//道路查询，每日每Key调用限制25000次，每10分钟内调用次数限制2500次

//静态地图，每日每Key调用限制25000次，每10分钟内调用次数限制2500次

//定位，每日每Key调用限制100000次，每10分钟内调用次数限制5000次 。

namespace MapDataTools
{
    public class GaoDeRoads
    {
        private log4net.ILog log;
        /// <summary>
        /// 输出为火星坐标
        /// </summary>
        public GaoDeRoads()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["gaoDeKey"]))
            {
                keys = System.Configuration.ConfigurationSettings.AppSettings["gaoDeKey"].Split(',');
            }
        }

        /// <summary>
        /// 委托，当下载完成一条路线时触发
        /// </summary>
        /// <param name="road">道路信息，名称，宽度，坐标串</param>
        public delegate void RoadDateDowningHandler(RoadModel road, int index, int count);

        public delegate void RoadCrossDowningHandler(RoadCrossModel roadcross, int index, int count);

        public delegate void DownOverHandler();

        public DownOverHandler downOverHandler = null;

        public RoadDateDowningHandler roadDateDowningHandler = null;

        public RoadCrossDowningHandler roadCrossDowningHandler = null;

        private string url =
            "http://restapi.amap.com/v3/road/roadname?pageIndex=1&city={0}&offset=1000&key={1}&s=rsv3&output=json&keywords={2}";

        private string urlCross =
            "http://restapi.amap.com/v3/road/roadinter?pageIndex=1&city={0}&offset=1000&key={1}&s=rsv3&output=json&keywords={2}";

        private string[] keys = new string[]
                                    {
                                        "6064051679efa89524860e1de482e294", "ddcf114352f37eb1295049911533e97e",
                                        "05d0b6e52d9ed5da644bf6922513a73a"
                                    };

        public void DownLoadRoads(string cityCode, List<string> roadNames)
        {
            int i = 0;
            foreach (string name in roadNames)
            {
                i++;
                int index = i % keys.Length;
                string realName = name;
                if (name.Contains("-"))
                {
                    realName = realName.Split('-')[1];
                }
                string tempUrl = string.Format(url, cityCode, keys[index], realName);
                string context = HttpHelper.GetRequestContent(tempUrl);

                object t = JsonHelper.JsonDeserialize<object>(context);
                Dictionary<string, object> dicRoot = t as Dictionary<string, object>;
                if (dicRoot == null || dicRoot["status"].ToString() != "1")
                {
                    log.ErrorFormat("{0}请求失败,返回码：{1}", tempUrl, dicRoot != null ? dicRoot["status"] : "");
                    continue;
                }
                if (dicRoot["status"].ToString() == "1")
                {
                    object[] roadsObjs = dicRoot["roads"] as object[];
                    if (roadsObjs != null && roadsObjs.Length > 0)
                    {
                        for (int j = 0; j < roadsObjs.Length; j++)
                        {
                            Dictionary<string, object> dicRoad = roadsObjs[j] as Dictionary<string, object>;
                            if (dicRoad != null)
                            {
                                RoadModel roadModel = new RoadModel();
                                roadModel.name = dicRoad["name"] != null ? dicRoad["name"].ToString() : realName;
                                if (dicRoad["width"] != null)
                                {
                                    double.TryParse(dicRoad["width"].ToString(), out roadModel.width);
                                }
                                roadModel.type = dicRoad["type"] != null ? dicRoad["type"].ToString() : "";
                                object[] polyLinesObj = dicRoad["polylines"] as object[];
                                for (int k = 0; k < polyLinesObj.Length; k++)
                                {
                                    if (polyLinesObj[k] != null)
                                    {
                                        string path = polyLinesObj[k].ToString();
                                        roadModel.paths.Add(path);
                                    }
                                }
                                if (this.roadDateDowningHandler != null)
                                {
                                    this.roadDateDowningHandler(roadModel, i, roadNames.Count);
                                }
                            }
                        }
                    }
                }
                else
                {
                    log.WarnFormat("下载{0}失败", name);
                }
            }
            if (this.downOverHandler != null) this.downOverHandler();
        }

        /// <summary>
        /// 根据城市名称下载道路
        /// </summary>
        /// <param name="cityName">城市名称</param>
        public void downLoadRoadsByCityName(string cityName)
        {
            string code = this.getCodeByCityName(cityName);
            List<string> roadNames = CityRoadConfig.GetInstance().GetRoadNamesByCityName(cityName);
            if (roadNames.Count == 0) System.Windows.Forms.MessageBox.Show("没有" + cityName + "的道路信息，等待后续路网库更新");
            this.DownLoadRoads(code, roadNames);
        }

        public void downLoadRoadCrossByCityName(string cityName)
        {
            string code = this.getCodeByCityName(cityName);
            List<string> roadNames = CityRoadConfig.GetInstance().GetRoadNamesByCityName(cityName);
            if (roadNames.Count == 0) System.Windows.Forms.MessageBox.Show("没有" + cityName + "的道路信息，等待后续路网库更新");
            int i = 0;
            foreach (string name in roadNames)
            {
                i++;
                int index = i % this.keys.Length;
                string realName = name;
                if (name.Contains("-"))
                {
                    realName = realName.Split('-')[1];
                }
                string tempUrl = string.Format(urlCross, code, keys[index], realName);
                string context = HttpHelper.GetRequestContent(tempUrl);
                if (string.IsNullOrEmpty(context))
                {
                    continue;
                }
                object t = JsonHelper.JsonDeserialize<object>(context);
                Dictionary<string, object> dicRoot = t as Dictionary<string, object>;
                if (dicRoot != null && dicRoot["status"].ToString() == "1")
                {
                    object[] roadsObjs = dicRoot["roadinters"] as object[];
                    if (roadsObjs != null && roadsObjs.Length > 0)
                    {
                        for (int j = 0; j < roadsObjs.Length; j++)
                        {
                            Dictionary<string, object> dicRoadCross = roadsObjs[j] as Dictionary<string, object>;
                            if (dicRoadCross != null)
                            {
                                RoadCrossModel roadCrossModel = new RoadCrossModel();
                                roadCrossModel.id = dicRoadCross["id"] != null ? dicRoadCross["id"].ToString() : "";
                                //roadCrossModel.first_id = dicRoadCross["first_id"] != null
                                //                              ? dicRoadCross["first_id"].ToString()
                                //                              : "";
                                roadCrossModel.first_name = dicRoadCross["first_name"] != null
                                                                ? dicRoadCross["first_name"].ToString()
                                                                : "";
                                //roadCrossModel.second_id = dicRoadCross["second_id"] != null
                                //                               ? dicRoadCross["second_id"].ToString()
                                //                               : "";
                                roadCrossModel.second_name = dicRoadCross["second_name"] != null
                                                                 ? dicRoadCross["second_name"].ToString()
                                                                 : "";
                                string localtionXY = dicRoadCross["location"] != null
                                                         ? dicRoadCross["location"].ToString()
                                                         : "";
                                string[] xy = localtionXY.Split(',');
                                if (xy.Length == 2)
                                {
                                    double.TryParse(xy[0], out roadCrossModel.x);
                                    double.TryParse(xy[1], out roadCrossModel.y);

                                    var coor = CoordHelper.Gcj2Wgs(roadCrossModel.x, roadCrossModel.y);
                                    roadCrossModel.wgs_x = coor.lon;
                                    roadCrossModel.wgs_y = coor.lat;
                                }
                                if (roadCrossModel.x > 0 && roadCrossModel.y > 0)
                                {
                                    if (this.roadCrossDowningHandler != null)
                                    {
                                        this.roadCrossDowningHandler(roadCrossModel, i, roadNames.Count);
                                    }
                                }
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(50);
            }
            if (this.downOverHandler != null) this.downOverHandler();
        }

        public string getCodeByCityName(string cityName)
        {
            List<City> cities = CityConfig.GetInstance().GetCityByName(cityName);
            if (cities.Count > 0) return cities[0].gaodeCode;
            return string.Empty;
        }
    }
}
