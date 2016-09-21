namespace MapDataTools.Tile
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using MapDataTools.Util;

    /// <summary>
    /// 谷歌切片数据
    /// </summary>
    public class GoogleMapTile : MapTile
    {
        #region private filed
        private MapType mapType;

        private const string mapUrl = "http://mt{0}.google.cn/vt?pb=!1m4!1m3!1i{1}!2i{2}!3i{3}!2m3!1e0!2sm!3i271000000!3m11!2szh-cn!3scn!5e18!12m1!1e47!12m3!1e37!2m1!1ssmartmaps!12m1!1e47!4e0";

        private const string imageUrl =
            "http://mt{0}.google.cn/maps/vt?lyrs=s@192&hl=zh-Hans-CN&gl=CN&x={2}&y={3}&z={1}&token=25250";
            // "http://mt{0}.google.cn/vt?lyrs=s@175&hl=zh-CN&gl=CN&x={2}&y={3}&z={1}";
     

        private const string biaozhuUrl = "http://mt{0}.google.cn/vt?pb=!1m4!1m3!1i{1}!2i{2}!3i{3}!2m3!1e0!2sm!3i316000000!3m9!2szh-CN!3sCN!5e18!12m1!1e50!12m3!1e37!2m1!1ssmartmaps!4e0";
        private double maxExtent = 20037508.34;
        private double maxResolution = 156543.03390625;
        #endregion 

        public GoogleMapTile(MapType mapType)
        {
            this.mapType = mapType;
        }

        public override int MaxTileCount
        {
            get
            {
                return 21;
            }
        }

        public override string LayerType
        {
            get
            {
                return "google";
            }
        }

        public override void DownLoad(int minRow, int maxRow, int minCol, int maxCol, int zoom, WorkInfo workInfo)
        {

            string folderName = "\\s";
            string austerityFilePath = "";
            if (workInfo.isAusterityFile)
            {
                austerityFilePath = Path.Combine(workInfo.filePath, "Layers");
            }
            string imgType = ".png";
            if (this.mapType == MapType.GoogleImage)
            {
                imgType = ".jpg";
            }
            string path = workInfo.filePath + folderName;
            path = path + "\\" + zoom.ToString();
            for (int i = minRow; i < maxRow + 1; i++)
            {
                string dpath = path + "\\" + i.ToString();
                if (!Directory.Exists(dpath))
                {
                    Directory.CreateDirectory(dpath);
                }
                for (int j = minCol; j < maxCol + 1; j++)
                {
                    if (workInfo.downStates == DownStates.stop)
                    {
                        return;
                    }
                    string tempPath = dpath + "\\" + j.ToString() + imgType;
                    workInfo.processDownImage.processIndex++;
                    bool isSave = false;
                    if (!File.Exists(tempPath))
                    {
                        string url = string.Format(mapUrl, (i + j) % 2, zoom, i, j);
                        var tempUrl = url;
                        if (this.mapType == MapType.GoogleImage)
                        {
                            
                            string vUrl = string.Format(imageUrl, (i + j) % 2, zoom, i, j);
                            tempUrl = vUrl;
                            string cUrl = string.Format(biaozhuUrl, (i + j) % 2, zoom, i, j);
                            Bitmap vImage = this.DownloadPicture(vUrl, 10000);
                            if (vImage == null)
                            {
                                vUrl = string.Format(imageUrl, (i + j + 1) % 2, zoom, i, j);
                                vImage = this.DownloadPicture(vUrl, 10000);
                            }
                            if (vImage != null)
                            {
                                Bitmap cImage = this.DownloadPicture(cUrl, 10000);
                                if (cImage == null)
                                {
                                    cUrl = string.Format(biaozhuUrl, (i + j + 1) % 2, zoom, i, j);
                                    cImage = this.DownloadPicture(cUrl, 10000);
                                }
                                if (cImage != null)
                                {
                                    if (this.SaveImages(vImage, cImage, tempPath, ImageFormat.Jpeg))
                                    {
                                        isSave = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isSave = this.DownloadPicture(url, tempPath, 10000);
                            if (!isSave)
                            {
                                url = string.Format(mapUrl, (i + j + 1) % 2, zoom, i, j);
                                isSave = this.DownloadPicture(url, tempPath, 10000);
                            }
                        }
                        if (isSave)
                        {
                            if (workInfo.isAusterityFile)
                            {
                                this.AusterityFile(zoom, i, j, tempPath, austerityFilePath + "\\_alllayers");
                            }
                            workInfo.processDownImage.secess++;
                        }
                        else
                        {
                            workInfo.processDownImage.lose++;
                            if (this.OnLog != null)
                            {
                                this.OnLog(tempUrl);
                            }
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
                            lock (this.obj)
                            {
                                WorkConfig.GetInstance().saveConfig();
                            }
                        }
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
            return new RowColumns
                       {
                           zoom = zoom,
                           minRow =(int)Math.Floor((minX + this.maxExtent) / (this.maxResolution / (Math.Pow(2, zoom)) * 256.0)),
                           maxRow =(int)Math.Ceiling((maxX + this.maxExtent) / (this.maxResolution / (Math.Pow(2, zoom)) * 256.0)),
                           minCol =(int)Math.Floor((this.maxExtent - maxY) / (this.maxResolution / (Math.Pow(2, zoom)) * 256.0)),
                           maxCol =(int)Math.Ceiling((this.maxExtent - minY) / (this.maxResolution / (Math.Pow(2, zoom)) * 256.0))
                       };
        }
    }
}
