using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiandiMap
{
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            //var layer = new TidanMapLayer(430, 84, 427, 82, 9, 17);
            //layer.SavePath = @"F:\MapTileDownload\tianditu"; //AppDomain.CurrentDomain.BaseDirectory;
            //layer.OnLog += (message) => { Console.WriteLine(message); };
            //layer.Start();
            new OmpHelper(@"E:\map\shanghai\shanghai\MAP00001").Deal();
            Console.WriteLine("处理OK");
            Console.Read();
        }
    }
    /// <summary>
    /// 地图获取
    /// </summary>
    public interface IMapPicture
    {
        int Max_X { get; }
        int Max_Y { get; }
        int Min_X { get; }
        int Min_Y { get; }
        int LowZoom { get; }
        int HightZoom { get; }
        MapType MapType { get; }
        string SavePath { get; set; }

        event Action<string> OnLog;

        event Action<string, int> OnProcessNotify;

        void Start();

        void Stop();
    }

    public enum MapType
    {
        Tiandi,
        Google,
        Baidu
    }
    /// <summary>
    /// 天地地图获取
    /// </summary>
    public class TidanMapLayer : IMapPicture
    {
        public int Max_X { get; private set; }
        public int Max_Y { get; private set; }
        public int Min_X { get; private set; }
        public int Min_Y { get; private set; }
        public int LowZoom { get; private set; }
        public int HightZoom { get; private set; }

        public MapType MapType
        {
            get
            {
                return MapType.Tiandi;
            }
        }

        public string SavePath { get; set; }

        private string[] mapServer = new[]
                                         {
                                             "http://tile0.tianditu.com/DataServer",
                                             "http://tile1.tianditu.com/DataServer",
                                             "http://tile2.tianditu.com/DataServer",
                                             "http://tile3.tianditu.com/DataServer",
                                             "http://tile4.tianditu.com/DataServer",
                                             "http://tile5.tianditu.com/DataServer",
                                             "http://tile6.tianditu.com/DataServer"
                                         };
        public TidanMapLayer(int maxX, int maxY, int minX, int minY, int lowZoom, int heightZoom)
        {
            this.Max_X = maxX;
            this.Max_Y = maxY;

            this.Min_X = minX;
            this.Min_Y = minY;

            this.LowZoom = lowZoom;
            this.HightZoom = heightZoom;
        }

        public event Action<string> OnLog;

        public event Action<string, int> OnProcessNotify;

        private Thread mapThread;
        public void Start()
        {
            mapThread = new Thread(() => { this.HreadStart(); });
            this.mapThread.IsBackground = true;
            this.mapThread.Start();
        }

        public void Stop()
        {

        }



        private void HreadStart()
        {
            var maxRow = this.Max_Y;
            var minRow = this.Min_Y;

            var maxCol = this.Max_X;
            var minCol = this.Min_X;

            if (Max_X > 0 && Max_Y > 0 && Min_X > 0 && Min_Y > 0 && LowZoom > 0 && HightZoom > 0)
            {
                for (int i = LowZoom; i < HightZoom + 1; i++)
                {
                    int rowCount = maxRow - minRow + 1;
                    int colCount = maxCol - minCol + 1;
                    if (i > LowZoom)
                    {
                        minRow = minRow * 2;
                        maxRow = minRow + rowCount * 2 - 1;
                        minCol = minCol * 2;
                        maxCol = minCol + colCount * 2 - 1;
                    }
                    if (this.OnLog != null)
                    {
                        this.OnLog("开始第" + i.ToString() + "级切片下载...");
                    }

                    Parallel.Invoke(
                        new Action[]
                            {
                                new Action(
                                    () =>
                                        {
                                            this.GetTiles(minRow, maxRow, minCol, maxCol, i, this.GetMapType("EMap", i));
                                        }),
                                new Action(
                                    () =>
                                        {
                                            this.GetTiles(minRow,maxRow,minCol,maxCol,i,this.GetMapType("ESatellite", i));
                                        })
                            });
                    if (this.OnLog != null)
                    {
                        this.OnLog("完成第" + i.ToString() + "级切片下载");
                    }
                }
            }
        }
        private string GetMapType(string type, int level)
        {
            if (type == "EMap")
            {
                if (level >= 1 && level <= 10)
                {
                    type = "vec_c";
                }
                else if (level == 11 || level == 12)
                {
                    type = "vec_c";
                }
                else if (level >= 13 && level <= 18)
                {
                    type = "vec_c";
                }
            }
            if (type == "ESatellite")
            {
                if (level >= 1 && level <= 10)
                {
                    type = "cva_c";
                }
                else if (level == 11 || level == 12 || level == 13)
                {
                    type = "cva_c";
                }
                else if (level == 14)
                {
                    type = "cva_c";
                }
                else if (level >= 15 && level <= 18)
                {
                    type = "cva_c";
                }
            }
            return type;
        }
        private void GetTiles(int minRow, int maxRow, int minCol, int maxCol, int zoom, string mapType)
        {
            var path = Path.Combine(SavePath, mapType);
            path = Path.Combine(path, zoom.ToString());
            int count = (maxRow - minRow + 1) * (maxCol - minCol + 1);
            int k = 0;
            for (int i = minRow; i < maxRow + 1; i++)
            {
                string dpath = path + "\\" + i.ToString();
                if (!Directory.Exists(dpath))
                {
                    Directory.CreateDirectory(dpath);
                }
                for (int j = minCol; j < maxCol + 1; j++)
                {
                    string tempPath = dpath + "\\" + j.ToString() + ".png";
                    k++;
                    bool isSave = false;
                    if (!File.Exists(tempPath))
                    {
                        var url = string.Format("{4}?T={0}&X={1}&Y={2}&L={3}", mapType, j, i, zoom, mapServer[new Random().Next(0, 6)]);
                        isSave = this.DownloadPicture(url, tempPath, 10000);
                        if (this.OnLog != null && !isSave)
                        {
                            this.OnLog(tempPath + "下载失败");
                        }
                    }
                    string msg = string.Format("提示：已处理第{0}级{1}条,共{2}条", zoom, k, count);
                    if (this.OnProcessNotify != null)
                    {
                        this.OnProcessNotify(msg, (k * 100) / count);
                    }
                }
            }
        }
        private bool DownloadPicture(string picUrl, string savePath, int timeOut)
        {
            bool value = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                if (timeOut != -1)
                {
                    request.Timeout = timeOut;
                }
                request.Method = "GET";
                request.Accept = "xxx";
                request.KeepAlive = false;
                request.UserAgent = "Opera/9.25 (Windows NT 6.0; U; en)";
                using (var response = request.GetResponse())
                {
                    if (!response.ContentType.ToLower().StartsWith("text/"))
                        value = SaveBinaryFile(response, savePath);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return value;
        }
        private static bool SaveBinaryFile(WebResponse response, string savePath)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outStream = null;
            Stream inStream = null;
            try
            {
                if (File.Exists(savePath)) File.Delete(savePath);
                outStream = System.IO.File.Create(savePath);
                inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0) outStream.Write(buffer, 0, l);
                } while (l > 0);
                value = true;
            }
            finally
            {
                if (outStream != null) outStream.Close();
                if (inStream != null) inStream.Close();
            }
            return value;
        }

    }
    /// <summary>
    /// OMP 文件转为png
    /// </summary>
    public class OmpHelper
    {
        private string filePath;

        private Thread timer;
        public OmpHelper(string filePath)
        {
            this.filePath = filePath;
        }

        public void Deal()
        {
            timer = new Thread((o) => { StartDeal(); });
            timer.IsBackground = true;
            timer.Start();
        }

        private void StartDeal()
        {
            if (!Directory.Exists(this.filePath))
            {
                return;
            }
            foreach (string zoom in Directory.GetDirectories(this.filePath))
            {
                foreach (string x in Directory.GetDirectories(zoom))
                {
                    foreach (string y in Directory.GetFiles(x, "*.omp"))
                    {
                        var fileInfo = new FileInfo(y);
                        using (var stream = new FileInfo(y).OpenRead())
                        {
                            var buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                            var path = fileInfo.FullName.Split('_')[2].Replace(".omp", ".png");

                            using (var newFile = new FileStream(Path.Combine(x, path), FileMode.OpenOrCreate))
                            {
                                var startIndex = 11;
                                if (buffer.Length == 0)
                                {
                                    startIndex = 0;
                                }
                                newFile.Write(buffer, startIndex, buffer.Length - startIndex);
                            }
                        }
                        File.Delete(y);
                    }
                }
                Console.WriteLine(zoom + "处理完成");
            }
        }
    }
}
