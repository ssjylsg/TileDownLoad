namespace MapDataTools.Tile
{
    using System;
    using System.IO;
    using MapDataTools.Util;

    public class BaiduMapTile : MapTile
    {
        private string[] urls = new string[]
                                    {
                                        "http://online1.map.bdimg.com/tile/", "http://online2.map.bdimg.com/tile/",
                                        "http://online3.map.bdimg.com/tile/"
                                    };

        public override string LayerType
        {
            get
            {
                return "baidu";
            }
        }
        public string GetTitleUrl(int row, int col, int zoom)
        {
            int index = (row + col) % this.urls.Length;
            string url = this.urls[index] + "?qt=tile&x=" + row + "&y=" + col + "&z=" + zoom + "&styles=pl&udt=20140807";
            return url;
        }

        public override void DownLoad(int minRow, int maxRow, int minCol, int maxCol, int zoom, Util.WorkInfo workInfo)
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
                        //if (workInfo.mapType == MapType.GaodeImage) url = this.GetImgTileUrl(i, j, zoom);
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
            double resolution = Math.Pow(2, 18 - zoom);
            return new RowColumns
                       {
                           zoom = zoom,
                           minRow = (int)(Math.Ceiling((minX - 0) / (resolution * 256))),
                           minCol = (int)(Math.Ceiling((minY - 23000) / (resolution * 256))),
                           maxRow = (int)(Math.Floor((maxX - 0) / (resolution * 256))),
                           maxCol = (int)(Math.Floor((maxY - 23000) / (resolution * 256)))
                       };
        }

        public override string TemplateName
        {
            get
            {
                return "tempBaidu.html";
            }
        }

        public override string TilePath
        {
            get
            {
                return base.TilePath + "/${z}/${x}/${y}.png";
            }
        }
    }
}


