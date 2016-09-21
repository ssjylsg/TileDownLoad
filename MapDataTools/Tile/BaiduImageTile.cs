using System;

namespace MapDataTools.Tile
{
    using System.Drawing;
    using System.IO;

    using MapDataTools.Util;

   public class BaiduImageTile : MapTile
   {
       private string bdimg =
           "http://shangetu0.map.bdimg.com/it/u=x={0};y={1};z={2};v=009;type=sate&fm=46&udt=20150601";

       private string labelImg =
           "http://online0.map.bdimg.com/onlinelabel/?qt=tile&x={0}&y={1}&z={2}&styles=sl&v=083&udt=20150815&p=0";
       private int timeOut = 3000;

       private Bitmap GetMap(int j, int i, int zoom, string mapType)
       {
           var url = mapType == "bdimg" ? bdimg : labelImg;
           String v_url = string.Format(url, j, i, zoom);
           Bitmap v_image = this.DownloadPicture(v_url, this.timeOut);
           return v_image;
       }

       public override string LayerType
       {
           get
           {
               return "baidu";
           }
       }

       public override void DownLoad(int minRow, int maxRow, int minCol, int maxCol, int zoom, WorkInfo workInfo)
        {
            string path = workInfo.filePath + "\\s";
            path = path + "\\" + zoom.ToString();
            string austerityFilePath = "";
            string imgType = "jpg";
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
                    if (workInfo.downStates == DownStates.stop)
                    {
                        return;
                    }
                    string tempPath = dpath + "\\" + j.ToString() + "." + imgType;
                    workInfo.processDownImage.processIndex++;
                    bool isSave = false;
                    if (!File.Exists(tempPath))
                    {
                        isSave = this.SaveImages(
                            this.GetMap(i, j, zoom, "bdimg"),
                            this.GetMap(i, j, zoom, "labelImg"),
                            tempPath);
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
            RowColumns titleInfo = new RowColumns() { zoom = zoom };
            double resolution = Math.Pow(2, 18 - zoom);
            titleInfo.minRow = (int)(Math.Ceiling((minX - 0) / (resolution * 256)));
            titleInfo.minCol = (int)(Math.Ceiling((minY - 23000) / (resolution * 256)));
            titleInfo.maxRow = (int)(Math.Floor((maxX - 0) / (resolution * 256)));
            titleInfo.maxCol = (int)(Math.Floor((maxY - 23000) / (resolution * 256)));
            return titleInfo;
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
               return base.TilePath + "${z}/${x}/${y}.jpg";
           }
       }
   }
}
