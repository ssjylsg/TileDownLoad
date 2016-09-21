using System;

namespace MapDataTools.Tile
{
    using System.Drawing.Imaging;
    using System.IO;
    using MapDataTools.Util;

   public class OSMTile : MapTile
    {
       #region private filed
       private string[] mapUrls = new[]
                                      {
                                          "http://a.tile.openstreetmap.org/{0}/{1}/{2}.png", //{z}/{x}/{y}
                                          "http://b.tile.openstreetmap.org/{0}/{1}/{2}.png",
                                          "http://c.tile.openstreetmap.org/{0}/{1}/{2}.png"
                                      };
          
        private double maxExtent = 20037508.34;
        private double maxResolution = 156543.03390625;
        #endregion

       public override string TemplateName
       {
           get
           {
               return "templateOSM.html";
           }
       }

       public override string TilePath
       {
           get
           {
               return "s/${z}/${x}/${y}.jpg";
           }
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
               return "streetmap";
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
            string imgType = ".jpg";
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
                        bool isSave = this.DownloadPicture(url, tempPath, 10000, ImageFormat.Jpeg);
                        if (!isSave)
                        {
                            foreach (var mapUrl in mapUrls)
                            {
                                url = string.Format(mapUrl, zoom, i, j);
                                isSave = this.DownloadPicture(url, tempPath, 10000, ImageFormat.Jpeg);
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
            var rc = new RowColumns
                         {
                             zoom = zoom,
                             minRow = (int)Math.Floor((minX + this.maxExtent)/ (this.maxResolution / (Math.Pow(2, zoom)) * 256.0)),
                             maxRow =(int)Math.Ceiling((maxX + this.maxExtent)/ (this.maxResolution / (Math.Pow(2, zoom)) * 256.0)),
                             minCol =(int)Math.Floor((this.maxExtent - maxY)/ (this.maxResolution / (Math.Pow(2, zoom)) * 256.0)),
                             maxCol =(int)Math.Ceiling((this.maxExtent - minY)/ (this.maxResolution / (Math.Pow(2, zoom)) * 256.0))
                         };
            rc.zoom = zoom;
            return rc;
        }
    }
}