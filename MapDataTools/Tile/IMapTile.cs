namespace MapDataTools.Tile
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading;

    using MapDataTools.Util;

    public class Handlers
    {
        public delegate void ProcessNotifyHandler(string msg, int process);

        public delegate void LogHandler(string msg);

        public delegate void DownImageHandler(string workName, int i, int count, int sucess, int lose);
    }
    /// <summary>
    /// 地图切片下载接口
    /// </summary>
    internal interface IMapTile
    {
        void DownLoad(int minRow, int maxRow, int minCol, int maxCol, int zoom, WorkInfo workInfo);

        RowColumns GetRowColomns(double minX, double minY, double maxX, double maxY, int zoom);
    }

   

    /// <summary>
    /// 天地矢量
    /// </summary>
    public class TiandiMapTile : MapTile
    {

        private string[] mapServer;

        private const double topTileFromX = -180;

        private const double topTileFromY = 90;

        public override string TemplateName
        {
            get
            {
                return "tempTD.html";
            }
        }

        public TiandiMapTile()
        {
            this.mapServer = (System.Configuration.ConfigurationManager.AppSettings["tiandiMapServer"] ?? "").Split(',');
            if (this.mapServer.Length == 0)
            {
                this.mapServer = new string[]
                                     {
                                         "http://t5.tianditu.com/DataServer", "http://t2.tianditu.com/DataServer",
                                         "http://t3.tianditu.com/DataServer", "http://t4.tianditu.com/DataServer"
                                     };
            }
            this.random = new Random();
        }

        public override string TilePath
        {
            get
            {
                return "Vector/";
            }
        }

        private int timeOut = 3000;

        private Random random;
        private Bitmap GetTianDiMap(int j,int i,int zoom,string mapType)
        {
            String v_url = string.Format("{4}?T={0}&X={1}&Y={2}&L={3}", mapType, j, i, zoom, this.mapServer[this.random.Next(0, this.mapServer.Length)]);
            Bitmap v_image = this.DownloadPicture(v_url, this.timeOut);
            if (v_image == null)
            {
                foreach (var s in this.mapServer)
                {
                    v_url = string.Format("{4}?T={0}&X={1}&Y={2}&L={3}", mapType, j, i, zoom, s);
                    v_image = this.DownloadPicture(v_url, this.timeOut);
                    if (v_image != null)
                    {
                        break;
                    }
                }
            }
            return v_image;
        }
        private void DownLoadTitle(WorkInfo workInfo, int j, int i, int zoom, string tempPath, string austerityFilePath)
        {
            if (workInfo.downStates == DownStates.stop)
            {
                return;
            }
            Bitmap vImage = this.GetTianDiMap(j, i, zoom, "vec_c");
            if (vImage != null)
            {
                Bitmap cImage = this.GetTianDiMap(j, i, zoom, "cva_c");
                if (!this.SaveImages(vImage, cImage, tempPath, ImageFormat.Png))
                {
                    workInfo.processDownImage.lose++;
                }
                else
                {
                    if (workInfo.isAusterityFile)
                    {
                       var isAddQuequ = TaskQueue.GeTaskQueue()
                            .AddTask(zoom, j, i, tempPath, austerityFilePath + "\\_alllayers");
                        if (!isAddQuequ)
                        {
                            this.AusterityFile(zoom, j, i, tempPath, austerityFilePath + "\\_alllayers");
                        }
                    }
                    workInfo.processDownImage.secess++;
                }
            }
            else
            {
                string picUrl = string.Format("{4}?T={0}&X={1}&Y={2}&L={3}", "vec_c", j, i, zoom, this.mapServer[0]);
                    this.log.ErrorFormat("{0}", picUrl);
                workInfo.processDownImage.lose++;
            }
            if (this.DownImage != null)
            {
                this.DownImage(
                    workInfo.id,
                    workInfo.processDownImage.processIndex,
                    workInfo.processDownImage.count,
                    workInfo.processDownImage.secess,
                    workInfo.processDownImage.lose);
            }
            if (workInfo.processDownImage.processIndex % 50 == 0)
            {
                WorkConfig.GetInstance().saveConfig();
                GC.Collect();
            }
        }
        /// <summary>
        /// 切片层级
        /// </summary>
        public override int MaxTileCount
        {
            get
            {
                return 19;
            }
        }
        /// <summary>
        /// 切片大小
        /// </summary>
        public override double TileSize
        {
            get
            {
                return base.TileSize * 2;
            }
        }

        public override string LayerType
        {
            get
            {
                return "tiandi";
            }
        }

        public override void DownLoad(int minRow, int maxRow, int minCol, int maxCol, int zoom, WorkInfo workInfo)
        {
            var path = Path.Combine(workInfo.filePath, "Vector");
            path = Path.Combine(path, zoom.ToString());
            int count = (maxRow - minRow + 1) * (maxCol - minCol + 1);
            int k = 0;
            string austerityFilePath = "";
            if (workInfo.isAusterityFile)
            {
                austerityFilePath = Path.Combine(workInfo.filePath, "Layers");
            }
           
            for (int i = minCol; i < maxCol + 1; i++)
            {
                string dpath = path + "\\" + i.ToString();
                if (!Directory.Exists(dpath))
                {
                    Directory.CreateDirectory(dpath);
                }
                GC.Collect();
                for (int j = minRow; j < maxRow + 1; j++)
                {
                    if (workInfo.downStates == DownStates.stop)
                    {
                        return;
                    }
                    string tempPath = dpath + "\\" + j.ToString() + ".png";
                    k++;
                    workInfo.processDownImage.processIndex++;
                    if (!File.Exists(tempPath))
                    {
                        var tempJ = j;
                        var tempI = i;
                        this.DownLoadTitle(workInfo, tempJ, tempI, zoom, tempPath, austerityFilePath);
                        //new MethodInvoker(
                        //    () => this.DownLoadTitle(workInfo, tempJ, tempI, zoom, tempPath, austerityFilePath)).BeginInvoke(
                        //            null,
                        //            null);
                        //Thread.Sleep(15);
                    }
                    else
                    {
                        workInfo.processDownImage.secess++;
                        if (this.DownImage != null)
                        {
                            this.DownImage(
                                workInfo.id,
                                workInfo.processDownImage.processIndex,
                                workInfo.processDownImage.count,
                                workInfo.processDownImage.secess,
                                workInfo.processDownImage.lose);
                        }

                    }
                }
            }
        }

        public override RowColumns GetRowColomns(double minX, double minY, double maxX, double maxY, int zoom)
        {
            RowColumns rc = new RowColumns() { zoom = zoom };
            double coef = 360.0 / Math.Pow(2, zoom);
            rc.minRow = (int)Math.Floor((minX - topTileFromX) / coef);
            rc.maxRow = (int)Math.Ceiling((maxX - topTileFromX) / coef);
            rc.minCol = (int)Math.Floor((topTileFromY - maxY) / coef);
            rc.maxCol = (int)Math.Ceiling((topTileFromY - minY) / coef);
            rc.zoom = zoom;
            return rc;
        }
    }

    public delegate void TitleAction<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    /// <summary>
    /// 紧凑队列任务
    /// </summary>
    public class TaskQueue
    {
        private class TaskItem
        {
            public int zoom { get; set; }

            public int j { get; set; }

            public int i { get; set; }

            public string tempPath { get; set; }

            public string FilePath { get; set; }
        }

        private static TaskQueue _task;
        private static object lockObj = new object();

        public static TaskQueue GeTaskQueue()
        {
            lock (lockObj)
            {
                if (_task == null)
                {
                    _task = new TaskQueue(new TiandiMapTile().AusterityFile);
                    _task.Start();
                }
            }
            return _task;
        }

        public bool IsRunning { get; set; }

        private System.Collections.Generic.Queue<TaskItem> _titleQueue;

        private TitleAction<int, int, int, string, string> _action;

        public TaskQueue(TitleAction<int, int, int, string, string> action)
        {
            this._action = action;
            this._titleQueue = new Queue<TaskItem>();
            this.IsRunning = true;
        }

        public int GetTaskCount()
        {
            lock (lockObj)
            {
                return this._titleQueue.Count;
            }
        }
        public bool AddTask(int zoom, int j, int i, string tempPath, string austerityFilePath)
        {
            if (zoom < 14)
            {
                return false;
            }
            lock (this._titleQueue)
            {
                this._titleQueue.Enqueue(
                    new TaskItem() { FilePath = austerityFilePath, i = i, j = j, zoom = zoom, tempPath = tempPath });
            }
            return true;
        }

        private void Start()
        {
            var t = new Thread(
                () =>
                    {
                        TaskItem item = null;

                        while (this.IsRunning)
                        {
                            lock (this._titleQueue)
                            {
                                if (this._titleQueue.Count > 0)
                                {
                                    item = this._titleQueue.Dequeue();
                                }
                                else
                                {
                                    item = null;
                                }
                            }
                            if (item != null)
                            {
                                try
                                {
                                    this._action(item.zoom, item.j, item.i, item.tempPath, item.FilePath);
                                    Thread.Sleep(50);
                                }
                                catch (Exception ex)
                                {
                                    log4net.LogManager.GetLogger(this.GetType()).Error(ex);
                                }
                            }
                            else
                            {
                                Thread.Sleep(2000);
                            }
                        }
                    });
            t.IsBackground = true;
            t.Start();
        }
    }
}
