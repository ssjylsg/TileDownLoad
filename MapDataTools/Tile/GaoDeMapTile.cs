namespace MapDataTools.Tile
{
    using System;
    using System.IO;

    using MapDataTools.Util;

    /// <summary>
    /// 高德地图切片
    /// </summary>
    public class GaoDeMapTile : MapTile
    {
        #region private filed
        private readonly string[] urls = new string[]
                                    {
                                        "http://webrd01.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=7",
                                        "http://webrd02.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=7",
                                        "http://webrd03.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=7",
                                        "http://webrd04.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=7"
                                    };

        private readonly string[] imgUrls = new string[]
                                       {
                                           "http://webst01.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=6",
                                           "http://webst02.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=6",
                                           "http://webst03.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=6",
                                           "http://webst04.is.autonavi.com/appmaptile??lang=zh_cn&size=1&scale=1&style=6"
                                       };
        private double[] resolutions = new double[]
                                           {
                                               156543.0339, 78271.516953125, 39135.7584765625, 19567.87923828125,
                                               9783.939619140625, 4891.9698095703125, 2445.9849047851562,
                                               1222.9924523925781, 611.4962261962891, 305.74811309814453,
                                               152.87405654907226, 76.43702827453613, 38.218514137268066,
                                               19.109257068634033, 9.554628534317016, 4.777314267158508, 2.388657133579254,
                                               1.194328566789627, 0.5971642833948135,
                                           };
        private double maxExtent = 20037508.3427892;
        #endregion
       
        private string GetTitleUrl(int row, int col, int zoom)
        {
            int index = (row + col) % this.urls.Length;
            return string.Format("{0}&z={1}&y={2}&x={3}", this.urls[index], zoom, col, row);
        }

        private string GetImgTileUrl(int row, int col, int zoom)
        {
            int index = (row + col) % this.imgUrls.Length;
            return string.Format("{0}&z={1}&y={2}&x={3}", this.imgUrls[index], zoom, col, row);
        }

        public override string LayerType
        {
            get
            {
                return "gaode";
            }
        }

        public override void DownLoad(int minRow, int maxRow, int minCol, int maxCol, int zoom, WorkInfo workInfo)
        {
            string path = workInfo.filePath + "\\s";
            path = path + "\\" + zoom.ToString();
            string austerityFilePath = "";
            string imgType = (workInfo.mapType == MapType.GaodeImage) ? "jpg" : "png";
            if (workInfo.isAusterityFile)
            {
                austerityFilePath = Path.Combine(workInfo.filePath, "Layers");
            }
            for (int i = minRow; i < maxRow + 1; i++)
            {
                string dpath = path + "\\" + i.ToString();
                if (!Directory.Exists(dpath))
                {
                    Directory.CreateDirectory(dpath);
                }
                for (int j = minCol; j < maxCol + 1; j++)
                {
                    if (workInfo.downStates == DownStates.stop) return;
                    string tempPath = dpath + "\\" + j.ToString() + "." + imgType;
                    workInfo.processDownImage.processIndex++;
                    bool isSave = false;
                    if (!File.Exists(tempPath))
                    {
                        string url = this.GetTitleUrl(i, j, zoom);
                        if (workInfo.mapType == MapType.GaodeImage) url = this.GetImgTileUrl(i, j, zoom);
                        isSave = this.DownloadPicture(url, tempPath, 10000);
                        if (isSave)
                        {
                            if (workInfo.isAusterityFile)
                            {
                                this.AusterityFile(zoom, i, j, tempPath, austerityFilePath + "\\_alllayers");
                            }
                            workInfo.processDownImage.secess++;
                        }
                        else workInfo.processDownImage.lose++;

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
        public override RowColumns GetRowColomns(double minX, double minY, double maxX, double maxY, int zoom)
        {
            double resolution = this.resolutions[zoom];
            return new RowColumns
                       {
                           zoom = zoom,
                           minRow = (int)(Math.Ceiling((minX + this.maxExtent) / (resolution * 256))),
                           maxCol = (int)(Math.Floor((this.maxExtent - minY) / (resolution * 256))),
                           maxRow = (int)(Math.Floor((maxX + this.maxExtent) / (resolution * 256))),
                           minCol = (int)(Math.Ceiling((this.maxExtent - maxY) / (resolution * 256)))
                       };
        }

        public override string TemplateName
        {
            get
            {
                return "tempGaoDe.html";
            }
        }
    }
}
