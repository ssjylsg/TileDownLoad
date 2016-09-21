using System;
using System.Text;

namespace MapDataTools.Tile
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;

    using log4net;

    using MapDataTools.Util;

    /// <summary>
    /// 地图切片下载
    /// </summary>
    public abstract class MapTile : IMapTile
    {
        protected ILog log;

        protected Object obj = new object();

        public Handlers.DownImageHandler DownImage;

        public Handlers.LogHandler OnLog;
        /// <summary>
        /// 切片层级
        /// </summary>
        public virtual int MaxTileCount
        {
            get
            {
                return 19;
            }
        }
        /// <summary>
        /// 模板名称
        /// </summary>
        public virtual string TemplateName
        {
            get
            {
                return "template.html";
            }
        }
        /// <summary>
        /// 切片大小
        /// </summary>
        public virtual double TileSize
        {
            get
            {
                return 10.0;
            }
        }
        /// <summary>
        /// 松散型切片地址
        /// </summary>
        public virtual string TilePath
        {
            get
            {
                return "s/";
            }
        }
        protected MapTile()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }
        public abstract string LayerType { get; }
        public abstract void DownLoad(int minRow, int maxRow, int minCol, int maxCol, int zoom, WorkInfo workInfo);

        public abstract RowColumns GetRowColomns(double minX, double minY, double maxX, double maxY, int zoom);

        public void AusterityFile(int zoom, int row, int col, string imagePath, string savePath)
        {
            int size = 128;
            String l = "0" + zoom;
            int lLength = l.Length;
            if (lLength > 2)
            {
                l = l.Substring(lLength - 2);
            }
            l = "L" + l;

            int rGroup = size * (row / size);
            String r = rGroup.ToString("X");
            int rLength = r.Length;
            if (rLength < 4)
            {
                for (int i = 0; i < 4 - rLength; i++)
                {
                    r = "0" + r;
                }
            }
            r = "R" + r;
            int cGroup = size * (col / size);
            String c = cGroup.ToString("X");
            int cLength = c.Length;
            if (cLength < 4)
            {
                for (int i = 0; i < 4 - cLength; i++)
                {
                    c = "0" + c;
                }
            }
            c = "C" + c;
            if (!Directory.Exists(savePath + "/" + l))
            {
                Directory.CreateDirectory(savePath + "/" + l);
            }
            String bundleBase = savePath + "/" + l + "/" + r + c;
            String bundlxFileName = bundleBase + ".npslx";
            String bundleFileName = bundleBase + ".npsle";
            FileStream file = null;
            FileStream fileIndex = null;
            if (!File.Exists(bundleFileName))
            {
                file = File.Create(bundleFileName);
                file.Write(new Byte[16], 0, 16);
            }
            else
            {
                file = File.OpenWrite(bundleFileName);
            }
            if (!File.Exists(bundlxFileName))
            {
                fileIndex = File.Create(bundlxFileName);
                fileIndex.Write(new Byte[16], 0, 16);
            }
            else
            {
                fileIndex = File.OpenWrite(bundlxFileName);
            }
            int index = size * (col - cGroup) + (row - rGroup);
            FileStream imageStream = File.OpenRead(imagePath);
            byte[] data = new byte[imageStream.Length];
            imageStream.Read(data, 0, data.Length);
            int l1 = data.Length / 16777216;
            int l2 = (data.Length % 16777216) / 65536;
            int l3 = ((data.Length % 16777216) % 65536) / 256;
            int l4 = ((data.Length % 16777216) % 65536) % 256;
            byte[] le = new byte[4];
            le[0] = (byte)l4;
            le[1] = (byte)l3;
            le[2] = (byte)l2;
            le[3] = (byte)l1;
            long currentLength = file.Length;
            file.Seek(currentLength, SeekOrigin.Begin);
            file.Write(le, 0, 4);
            file.Write(data, 0, data.Length);
            file.Flush();
            file.Dispose();

            long lindex0 = (currentLength / 4294967296);
            long lindex1 = (currentLength % 4294967296) / 16777216;
            long lindex2 = (currentLength % 4294967296) % 16777216 / 65536;
            long lindex3 = (currentLength % 4294967296) % 16777216 % 65536 / 256;
            long lindex4 = (currentLength % 4294967296) % 16777216 % 65536 % 256;
            byte[] indexBytes = new byte[5];
            indexBytes[0] = (byte)lindex4;
            indexBytes[1] = (byte)lindex3;
            indexBytes[2] = (byte)lindex2;
            indexBytes[3] = (byte)lindex1;
            indexBytes[4] = (byte)lindex0;
            fileIndex.Seek(16 + 5 * index, SeekOrigin.Begin);
            fileIndex.Write(indexBytes, 0, 5);
            fileIndex.Flush();
            fileIndex.Dispose();
            imageStream.Dispose();
        }

        protected static Bitmap GetImage(WebResponse response)
        {
            Stream inStream = null;
            try
            {
                inStream = response.GetResponseStream();
                Bitmap bitMap = new Bitmap(inStream);
                return bitMap;
            }
            finally
            {
                if (inStream != null)
                {
                    inStream.Close();
                }
            }
            return null;
        }

        protected Bitmap DownloadPicture(string picUrl, int timeOut)
        {
            Bitmap value = null;
            HttpWebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                if (timeOut != -1)
                {
                    request.Timeout = timeOut;
                }
                request.Method = "GET";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.KeepAlive = false;
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
                response = request.GetResponse() as HttpWebResponse;
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    value = GetImage(response);
                }
                request.Abort();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
            return value;
        }

        protected bool SaveImages(Bitmap image1, Bitmap image2, string savePath)
        {
            try
            {
                Bitmap imgTemp = new Bitmap(image1.Width, image1.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(imgTemp);
                g.DrawImage(image1, 0, 0, image1.Width, image1.Height);
                if (image2 != null)
                {
                    g.DrawImage(image2, 0, 0, image2.Width, image2.Height);
                }
                imgTemp.Save(savePath, ImageFormat.Jpeg);
                g.Dispose();
                imgTemp.Dispose();
                image1.Dispose();
                if (image2 != null) image2.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        protected bool SaveImages(Bitmap image1, Bitmap image2, string savePath, ImageFormat imageFormat)
        {
            try
            {
                Bitmap imgTemp = new Bitmap(image1.Width, image1.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(imgTemp);
                g.DrawImage(image1, 0, 0, image1.Width, image1.Height);
                if (image2 != null) g.DrawImage(image2, 0, 0, image2.Width, image2.Height);
                imgTemp.Save(savePath, imageFormat);
                image1.Dispose();
                if (image2 != null)
                {
                    image2.Dispose();
                }
                imgTemp.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        protected bool DownloadPicture(string picUrl, string savePath, int timeOut, ImageFormat imageFormat)
        {
            var bitMap = DownloadPicture(picUrl, timeOut);
            return SaveImages(bitMap, null, savePath, imageFormat);
        }
        protected bool DownloadPicture(string picUrl, string savePath, int timeOut)
        {
            bool value = false;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(picUrl);
                if (timeOut != -1)
                {
                    request.Timeout = timeOut;
                }
                request.Method = "GET";
                request.Accept = "xxx";
                request.KeepAlive = false;
                request.UserAgent = "Opera/9.25 (Windows NT 6.0; U; en)";
                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    value = SaveBinaryFile(response, savePath);
                }
                request.Abort();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0},{1}", picUrl, savePath);
                if (this.OnLog != null)
                {
                    this.OnLog(string.Format("请求{0}失败", picUrl));
                }
                return false;
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
            return value;
        }

        protected  bool SaveBinaryFile(WebResponse response, string savePath)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outStream = null;
            Stream inStream = null;
            try
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                outStream = File.Create(savePath);
                inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0) outStream.Write(buffer, 0, l);
                }
                while (l > 0);
                value = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if (outStream != null) outStream.Close();
                if (inStream != null) inStream.Close();
            }
            return value;
        }

      
        /// <summary>
        /// 创建HTML demo
        /// </summary>
        /// <param name="workInfo"></param>
        /// <param name="minZoom"></param>
        /// <param name="maxZoom"></param>
        public  void CreateHtmlDemo(WorkInfo workInfo, int minZoom, int maxZoom)
        { 
            string str = "";
            string imgType = "png";
            try
            { 
                var htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "createHTML//" + this.TemplateName);
                if (!File.Exists(htmlPath))
                {
                    throw new ArgumentNullException(this.TemplateName + "找不到");
                    return;
                }
                using (var sr = new StreamReader(htmlPath))
                {
                    str = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception err)
            {
                log.Error(err);
            }
            if (workInfo.mapType == MapType.GaodeImage 
                || workInfo.mapType == MapType.TiandiImage 
                || workInfo.mapType == MapType.GoogleImage
                || workInfo.mapType == MapType.BaiduImageTile)
            {
                imgType = "jpg";
            }
            
            try
            {
                string fname = workInfo.filePath + "//temp.html";
                str = str.Replace("$centerX$", ((workInfo.minX + workInfo.maxX) / 2).ToString());
                str = str.Replace("$centerY$", ((workInfo.minY + workInfo.maxY) / 2).ToString());
                str = str.Replace("$minX$", workInfo.minX.ToString());
                str = str.Replace("$minY$", workInfo.minY.ToString());
                str = str.Replace("$maxX$", workInfo.maxX.ToString());
                str = str.Replace("$maxY$", workInfo.maxY.ToString());
                str = str.Replace("$minZoom$", minZoom.ToString());
                str = str.Replace("$maxZoom$", maxZoom.ToString());
                str = str.Replace("$zoom$", (maxZoom - minZoom).ToString());
                str = str.Replace("$type$", imgType);
                str = str.Replace("$path$", this.TilePath);
                using (var fs = new FileStream(fname,FileMode.OpenOrCreate))
                {
                    using (var sw = new StreamWriter(fs, Encoding.GetEncoding("utf-8")))
                    {
                        sw.WriteLine(str);
                        sw.Flush();
                    }
                }
                string jsPath = System.Windows.Forms.Application.StartupPath + "//createHTML//init.js";
                if (File.Exists(jsPath))
                {
                    File.Copy(jsPath, workInfo.filePath + "//init.js");
                }
                CopyFolder(System.Windows.Forms.Application.StartupPath + "//createHTML//Netposa", workInfo.filePath + "//Netposa//");
            }
            catch (Exception err)
            {
                log.Error(err);
            }
        }
        private  void CopyFolder(string from, string to)
        {
            if (from.Contains(".svn"))
            {
                return;
            }
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
            }
            
            // 子文件夹
            foreach (string sub in Directory.GetDirectories(from))
            {
                CopyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");
            }

            // 文件
            foreach (string file in Directory.GetFiles(from))
            {
                File.Copy(file, to + Path.GetFileName(file), true);
            }
        }

        public RowColumns GetRowColomns(Extent extent,int zoom)
        {
            return this.GetRowColomns(extent.minX, extent.minY, extent.maxX, extent.maxY, zoom);
        }
    }
}
