namespace MapDataTools.Tile
{
    using System;
    using System.IO;

    using MapDataTools.Util;

    /// <summary>
    /// 天地影图地像
    /// </summary>
    public class TiandiTile : MapTile
    {
        #region
        private string mapType;
        private double topTileFromX = -180;

        private double topTileFromY = 90;
        private string[] mapServer;
        #endregion
        public TiandiTile(string mapType)
        {
            this.mapType = mapType;
           
            this.mapServer = (System.Configuration.ConfigurationManager.AppSettings["tiandiMapServer"] ?? "").Split(',');
            if (this.mapServer.Length == 0)
            {
                this.mapServer = new string[]
                                 {
                                     "http://t5.tianditu.com/DataServer", "http://t2.tianditu.com/DataServer",
                                     "http://t3.tianditu.com/DataServer", "http://t4.tianditu.com/DataServer"
                                 };
            }
        }

        public override string TilePath
        {
            get
            {
                return "Vector/";
            }
        }

        public override string TemplateName
        {
            get
            {
                return "tempTD.html";
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
                for (int j = minRow; j < maxRow + 1; j++)
                {
                    if (workInfo.downStates == DownStates.stop) return;
                    string tempPath = dpath + "\\" + j.ToString() + ".jpg";
                    k++;
                    workInfo.processDownImage.processIndex++;
                    if (!File.Exists(tempPath))
                    {
                        string url = string.Format(
                            "{4}?T={0}&X={1}&Y={2}&L={3}",
                            this.mapType,
                            j,
                            i,
                            zoom,
                            this.mapServer[new Random().Next(0, this.mapServer.Length)]);
                        bool isSave = this.DownloadPicture(url, tempPath, 10000);
                        if (isSave)
                        {
                            if (workInfo.isAusterityFile)
                            {
                                this.AusterityFile(zoom, j, i, tempPath, austerityFilePath + "\\_alllayers");
                            }
                            workInfo.processDownImage.secess++;
                        }
                        else
                        {
                            // 如果随机下载失败，则对所有的服务重新循环一遍重新下载数据
                            foreach (var s in this.mapServer)
                            {
                                url = string.Format("{4}?T={0}&X={1}&Y={2}&L={3}", this.mapType, j, i, zoom, s);
                                isSave = this.DownloadPicture(url, tempPath, 10000);
                                if (isSave)
                                {
                                    if (workInfo.isAusterityFile)
                                    {
                                        this.AusterityFile(zoom, j, i, tempPath, austerityFilePath + "\\_alllayers");
                                    }
                                    workInfo.processDownImage.secess++;
                                    break;
                                }
                            }
                            if (!isSave)
                            {
                                workInfo.processDownImage.lose++;
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
            double coef = 360.0 / Math.Pow(2, zoom);
            return new RowColumns
                       {
                           zoom = zoom,
                           minRow = (int)Math.Floor((minX - this.topTileFromX) / coef),
                           maxRow = (int)Math.Ceiling((maxX - this.topTileFromX) / coef),
                           minCol = (int)Math.Floor((this.topTileFromY - maxY) / coef),
                           maxCol = (int)Math.Ceiling((this.topTileFromY - minY) / coef)
                       };
        }
    }
}
