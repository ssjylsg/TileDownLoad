using System.Collections.Generic;
//using System.Linq;
using System.Net;

namespace MapDataTools.PublicTransport
{
    public class TrastportLineAndStopDown
    {
        public delegate void TransportDowningHandler(string busLineName,BusLineModel busLineModel,List<BusStopModel> stops,int index,int count);
        public TransportDowningHandler transportDowningHandler = null;
        public delegate void TransportDownEndHandler();
        public TransportDownEndHandler transportDownEndHandler = null;
        private string[] keys = new string[] { "6064051679efa89524860e1de482e294", "ddcf114352f37eb1295049911533e97e", "05d0b6e52d9ed5da644bf6922513a73a" };
        string url = "http://restapi.amap.com/v3/bus/linename?s=rsv3&extensions=all&key={0}&output=json&pageIndex=1&city={1}&offset=1&keywords={2}";
        public TrastportLineAndStopDown()
        {
        }
        public void TrasportDown(string cityName)
        {
            List<string> busNames=TransportConfig.GetInstance().getBusNameByCityName(cityName);
            if(busNames.Count==0)
            {
                System.Windows.Forms.MessageBox.Show("当前城市暂时不支持公交线路下载");
            }
            int index = 0;
            for(int i=0;i<busNames.Count;i++)
            {
                index++;
                string tempUrl = string.Format(url, keys[i % 3], cityName, busNames[i]);
                HttpWebResponse hp = HttpHelper.CreateGetHttpResponse(tempUrl, 1000, "", null);
                string context = HttpHelper.GetResponseString(hp);
                object objContext = JsonHelper.JsonDeserialize<object>(context);
                Dictionary<string, object> dicContext = objContext as Dictionary<string, object>;
                if (dicContext == null||!dicContext.ContainsKey("status"))
                    continue;
                if (dicContext["status"]!=null&&dicContext["status"].ToString()=="1")
                {
                    object busLineObjs = dicContext["buslines"];
                    if (busLineObjs != null)
                    {
                        object[] busLines = busLineObjs as object[];
                        if (busLines.Length > 0)
                        {
                            object busLine = busLines[0];
                            Dictionary<string, object> dicBusLine = busLine as Dictionary<string, object>;
                            if (dicBusLine != null)
                            {
                                BusLineModel busLineModel = new BusLineModel();
                                busLineModel.lineId = dicBusLine["id"] != null ? dicBusLine["id"].ToString() : "";
                                busLineModel.name = dicBusLine["name"] != null ? dicBusLine["name"].ToString() : "";
                                busLineModel.type = dicBusLine["type"] != null ? dicBusLine["type"].ToString() : "";
                                busLineModel.distance = dicBusLine["distance"] != null ? dicBusLine["distance"].ToString() : "";
                                busLineModel.polyline = dicBusLine["polyline"] != null ? dicBusLine["polyline"].ToString() : "";
                                busLineModel.start_stop = dicBusLine["start_stop"] != null ? dicBusLine["start_stop"].ToString() : "";
                                busLineModel.end_stop = dicBusLine["end_stop"] != null ? dicBusLine["end_stop"].ToString() : "";
                                busLineModel.start_time = dicBusLine["start_time"] != null ? dicBusLine["start_time"].ToString() : "";
                                busLineModel.end_time = dicBusLine["end_time"] != null ? dicBusLine["end_time"].ToString() : "";
                                object[] objstops = dicBusLine["busstops"] as object[];
                                List<BusStopModel> busStopModels = new List<BusStopModel>();
                                for (int j = 0; j < objstops.Length; j++)
                                {
                                    object objstop = objstops[j];
                                    Dictionary<string, object> dicstop = objstop as Dictionary<string, object>;
                                    if (dicstop != null)
                                    {
                                        BusStopModel busStopModel = new BusStopModel();
                                        busStopModel.stopId = dicstop["id"].ToString();
                                        string location = dicstop["location"].ToString();
                                        string[] xy = location.Split(',');
                                        double.TryParse(xy[0], out busStopModel.x);
                                        double.TryParse(xy[1], out busStopModel.y);
                                        busStopModel.name = dicstop["name"] != null ? dicstop["name"].ToString() : "";
                                        busStopModel.lineId = busLineModel.lineId;
                                        busStopModels.Add(busStopModel);
                                    }
                                }
                                if (this.transportDowningHandler != null)
                                {
                                    this.transportDowningHandler(busNames[i], busLineModel, busStopModels, index, busNames.Count);
                                }
                            }
                        }
                    }
                }
            }
            if (this.transportDownEndHandler != null)
            {
                this.transportDownEndHandler();
            }
        }
    }
    public class BusLineModel
    {
        public string lineId = "";
        public string name = "";
        public string type = "";
        public string distance = "";
        public string polyline = "";
        public string start_stop = "";
        public string end_stop = "";
        public string start_time = "";
        public string end_time = "";
    }
    public class BusStopModel
    {
        public string stopId = "";
        public string name = "";
        public double x = 0.0;
        public double y = 0.0;
        public string lineId = "";
    }
}
