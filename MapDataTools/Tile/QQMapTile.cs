namespace MapDataTools.Tile
{
    using System;
    using System.Drawing.Imaging;
    using System.IO;

    using MapDataTools.Util;

    public class QQMapTile:MapTile
    {
        private string[] mapUrls = new[]
                                      {
                                          "http://rt0.map.gtimg.com/realtimerender?z={0}&x={1}&y={2}&type=vector&style=0&v=1.1.2", 
                                          "http://rt1.map.gtimg.com/realtimerender?z={0}&x={1}&y={2}&type=vector&style=0&v=1.1.2", 
                                          "http://rt2.map.gtimg.com/realtimerender?z={0}&x={1}&y={2}&type=vector&style=0&v=1.1.2",
                                          "http://rt3.map.gtimg.com/realtimerender?z={0}&x={1}&y={2}&type=vector&style=0&v=1.1.2"
                                      };
        private double topTileFromX = -180;
        private double topTileFromY = 90;
        public override string TemplateName
        {
            get
            {
                return "qq.html";
            }
        }

        public override string TilePath
        {
            get
            {
                return "s/${z}/${x}/${y}.png";
            }
        }

        public override int MaxTileCount
        {
            get
            {
                return 18;
            }
        }

        public override string LayerType
        {
            get
            {
                throw new NotImplementedException();
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
           string path = workInfo.filePath + folderName;
           path = path + "\\" + zoom.ToString();
           for (int i = minRow; i < maxRow + 1; i++)
           {
               string Dpath = path + "\\" + i.ToString();
               if (!Directory.Exists(Dpath))
               {
                   Directory.CreateDirectory(Dpath);
               }
               for (int j = minCol; j < maxCol + 1; j++)
               {
                   if (workInfo.downStates == DownStates.stop) return;
                   string tempPath = Dpath + "\\" + j.ToString() + imgType;
                   workInfo.processDownImage.processIndex++;
                   if (!File.Exists(tempPath))
                   {
                       string url = string.Format(mapUrls[(i + j) % mapUrls.Length], zoom, i, j);
                       var tempUrl = url;
                       bool isSave = this.DownloadPicture(url, tempPath, 10000, ImageFormat.Png);
                       if (!isSave)
                       {
                           foreach (var mapUrl in mapUrls)
                           {
                               url = string.Format(mapUrl, zoom, i, j);
                               isSave = this.DownloadPicture(url, tempPath, 10000, ImageFormat.Png);
                               if (isSave)
                               {
                                   break;
                               }
                           }
                       }

                       if (isSave)
                       {
                           if (workInfo.isAusterityFile)
                               this.AusterityFile(zoom, i, j, tempPath, austerityFilePath + "\\_alllayers");
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
