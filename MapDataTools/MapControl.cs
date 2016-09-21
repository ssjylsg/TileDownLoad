using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MapDataTools
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class MapControl : UserControl
    {
        public Extent extent = null;
        public delegate void MapMovingHandler(ArcMapEvent arcMapEvent);
        public MapMovingHandler mapMovingHandler = null;
        public delegate void MapZoomedHandler(ArcMapEvent arcMapEvent);
        public MapZoomedHandler mapZoomedHandler = null;
        public delegate void OverlayEditedHandler(Extent extent);
        public OverlayEditedHandler overlayEditedHandler = null;
        public delegate void OverlayClearedHandler();
        public OverlayClearedHandler overlayClearedHandler = null;

        private log4net.ILog log;
        public MapControl()
        {
            InitializeComponent();
            this.mapBrowser.ObjectForScripting = this;
            this.mapBrowser.ScrollBarsEnabled = false;
            this.mapBrowser.ScrollBarsEnabled = false;
            this.mapBrowser.ObjectForScripting = this;
            this.mapBrowser.Navigate(System.Windows.Forms.Application.StartupPath + @"\webmap\index.html");
            log = log4net.LogManager.GetLogger(this.GetType());
        }
        public void initMap(int MapType)//0百度地图，1google地图，2高德地图，3腾讯地图
        {
            if (MapType == 0)
            {

            }
            else if (MapType == 1)
            {
            }
            else if (MapType == 2)
            {
            }
            else if (MapType == 3)
            {
            }
            this.mapBrowser.Refresh();
        }
        /// <summary>
        /// 城市定位
        /// </summary>
        /// <param name="cityName"></param>
        public void locationCity(string cityName)
        {
            this.CallScriptMethod("locationCity", cityName);
        }
        /// <summary>
        /// 清除地图上所有覆盖物
        /// </summary>
        public void clearOverlay()
        {
            this.CallScriptMethod("clearOverlay");
            this.extent = null;
            if (this.overlayClearedHandler != null)
            {
                this.overlayClearedHandler();
            }
        }
        /// <summary>
        /// 绘制完成调用方法
        /// </summary>
        public void OnDrawComlpeted(string minX, string minY, string maxX, string maxY)
        {
            this.extent = new Extent();
            double.TryParse(minX, out this.extent.minX);
            double.TryParse(minY, out this.extent.minY);
            double.TryParse(maxX, out this.extent.maxX);
            double.TryParse(maxY, out this.extent.maxY);
            if (this.overlayEditedHandler != null)
            {
                this.overlayEditedHandler(extent);
            }
        }
        /// <summary>
        /// 添加对脚本控制委托
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="objParam"></param>
        /// <returns></returns>
        private delegate object DelegateCallScriptMethod(string methodName, params object[] objParam);

        /// <summary>
        /// 脚本控制方法
        /// </summary>
        /// <param name="methodName">要操作的脚本方法名称</param>
        /// <param name="objParam">传递参数</param>
        /// <returns></returns>
        public object CallScriptMethod(string methodName, params object[] objParam)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    DelegateCallScriptMethod handle = new DelegateCallScriptMethod(CallScriptMethod);
                    return Invoke(handle, methodName, objParam);
                }

                if (this.mapBrowser == null || this.mapBrowser.Document == null)
                    return null;

                List<object> objList = new List<object>();
                objList.AddRange(GetParams(objParam));

                return this.mapBrowser.Document.InvokeScript(methodName, objList.ToArray());
                //return null;
                //return this.webBrowserMap.Document.InvokeScript("NPMapLib.Managers.MapManager.callFromCS", objList.ToArray());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        /// <summary>
        /// 参数添加到数组当中
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private List<object> GetParams(IEnumerable<object> objs)
        {
            List<object> result = new List<object>();
            if (objs is IEnumerable<object>)
            {
                var t = objs as IEnumerable<object>;
                foreach (var temp in t)
                {
                    if (temp is IEnumerable<object>)
                        result.AddRange(GetParams(temp as IEnumerable<object>));
                    else
                        result.Add(temp);
                }
            }
            return result;
        }

        private void mapBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string x = e.Url.Fragment;
        }
        public void showLonLat(string lon, string lat)
        {
            if (this.mapMovingHandler != null)
            {
                ArcMapEvent arcMapEvent=new ArcMapEvent();
                double.TryParse(lon,out arcMapEvent.lon);
                double.TryParse(lat,out arcMapEvent.lat);
                this.mapMovingHandler(arcMapEvent);
            }
        }
        public void showZoom(string zoom)
        {
            if (this.mapZoomedHandler != null)
            {
                ArcMapEvent arcMapEvent = new ArcMapEvent();
                int.TryParse(zoom, out arcMapEvent.zoom);
                this.mapZoomedHandler(arcMapEvent);
            }
        }

        public void SetZoom(int zoom)
        {
            this.CallScriptMethod("setZoom", zoom);
        }

        public int GetZoom()
        {
            return int.Parse(this.CallScriptMethod("getZoom").ToString());
        }

        public void SetCenter(Coord point)
        {
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(point);
            this.CallScriptMethod("setCenter", json);
        }
        public void pan()
        {
            this.CallScriptMethod("pan");
        }
        public void zoomIn()
        {
            this.CallScriptMethod("zoomIn");
        }
        public void zoomOut()
        {
            this.CallScriptMethod("zoomOut");
        }
        public void selectZoomIn()
        {
            this.CallScriptMethod("selectZoomIn");
        }
        public void selectZoomOut()
        {
            this.CallScriptMethod("selectZoomOut");
        }
        public void drawRectangle()
        {
            this.CallScriptMethod("drawRectangle");
        }
        public void drawRegion()
        {
            this.CallScriptMethod("drawRegion");
        }
        public void measureDistance()
        {
            this.CallScriptMethod("measureDistance");
        }
        public void measureArea()
        {
            this.CallScriptMethod("measureArea");
        }
        public void setMap(string mapType)
        {
            this.extent = null;
            this.CallScriptMethod("destroyMap");
            if (mapType == "TianDi")
            {
                this.CallScriptMethod("InitTDTMap");
            }
            if (mapType == "Google")
            {
                this.CallScriptMethod("InitGGMap");
            }
            if (mapType == "GaoDe")
            {
                this.CallScriptMethod("InitGaoDe");
            }
            if (mapType == "GoogleImage")
            {
                this.CallScriptMethod("InitGGMapImage");
            }
            if (mapType == "TianDiImage")
            {
                this.CallScriptMethod("InitTDTMapImage");
            }
            if (mapType == "GaoDeImage")
            {
                this.CallScriptMethod("InitGaoDeImage");
            }
            if (mapType == "QQMap")
            {
                this.CallScriptMethod("InitQQMap");
            }
            if (mapType == "QQImage")
            {
                this.CallScriptMethod("InitQQMapImage");
            }
            if (mapType == "BaiduImage")
            {
                this.CallScriptMethod("InitBDMap");
            }
            if (mapType == "BaiduImageTile")
            {
                this.CallScriptMethod("InitBDMapImage");
            }
            this.CallScriptMethod(mapType);
        }

        public Coord GetCenter()
        {
            string msg = (this.CallScriptMethod("getCenter") ?? string.Empty).ToString();
            if (string.IsNullOrEmpty(msg))
            {
                return null;
            }
            return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Coord>(msg);
        }

        public void LoadEzMap(string p)
        {
            log.Info(p);
            this.CallScriptMethod("loadEzMap",p);
        }
    }
}
