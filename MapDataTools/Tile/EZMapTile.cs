using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDataTools.Tile
{
    using System.IO;

    using MapDataTools.Util;

    public class EZMapTile : MapTile
    {
        private double[] resolutions = new double[]
                                           {
                                               2, 1, 0.5, 0.25, 0.125, 0.0625, 0.03125, 0.015625, 0.0078125, 0.00390625,
                                               0.001953125, 0.0009765625, 0.00048828125, 0.000244140625, 0.0001220703125,
                                               0.00006103515625, 0.000030517578125, 0.0000152587890625,
                                               0.00000762939453125, 0.000003814697265625, 0.0000019073486328125,
                                               9.5367431640625e-7
                                           };

        private string ezmapUrl;
        public override string LayerType
        {
            get
            {
                return "pgis";
            }
        }
        private int zoomOffset = 0;
        private string serviceVersion;

        public EZMapTile(string ezmapUrl, string serviceVersion)
        {
            this.ezmapUrl = ezmapUrl;
            this.serviceVersion = serviceVersion;
        }
        public EZMapTile()
        {
            ezmapUrl = System.Configuration.ConfigurationManager.AppSettings["ezmapUrl"];
            serviceVersion = System.Configuration.ConfigurationManager.AppSettings["serviceVersion"];
        }

        private string GetTitleUrl(int row, int col, int zoom)
        {
            //return this.ezmapUrl + "/EzMap?Service=getImage&Type=RGB&ZoomOffset=" + this.zoomOffset + "&V="
            //       + this.serviceVersion + "&Col=" + (row - 1) + "&Row=" + col + "&Zoom=" + (zoom - this.zoomOffset);

            return this.ezmapUrl + "/EzMap?Service=getImage&Type=RGB&ZoomOffset=" + this.zoomOffset + "&V="
                  + this.serviceVersion + "&Col=" + (row) + "&Row=" + (col) + "&Zoom=" + (zoom - this.zoomOffset);
        }

        public override string TemplateName
        {
            get
            {
                return "EzMap.html";
            }
        }

        public override string TilePath
        {
            get
            {
                return base.TilePath + "/${z}/${x}/${y}.png";
            }
        }

        public override void DownLoad(int minRow, int maxRow, int minCol, int maxCol, int zoom, Util.WorkInfo workInfo)
        {
           
            string path = workInfo.filePath + "\\s";
            path = path + "\\" + zoom.ToString();
            string austerityFilePath = "";
            string imgType =   "png";
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
                        isSave = this.DownloadPicture(url, tempPath, 10000);
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

        public override Util.RowColumns GetRowColomns(double minX, double minY, double maxX, double maxY, int zoom)
        {
            double resolution = this.resolutions[zoom];
            return new RowColumns
                       {
                           zoom = zoom,
                           minRow = (int)(Math.Ceiling((minX - 0) / (resolution * 256))) - 1,
                           minCol = (int)(Math.Ceiling((minY - 0) / (resolution * 256))) - 1,
                           maxRow = (int)(Math.Floor((maxX - 0) / (resolution * 256))) + 1,
                           maxCol = (int)(Math.Floor((maxY - 0) / (resolution * 256))) + 1
                       };

            //double resolution = this.resolutions[zoom];

            //var tileOrigin =
            //    new
            //    {
            //        lon = 0,
            //        lat = 0
            //    };
            //var tileSize = new { w = 256, h = 256 };
            //var res = resolution;
            //var originTileX = (tileOrigin.lon + (res * tileSize.w / 2));
            //var originTileY = (tileOrigin.lat - (res * tileSize.h / 2));

            //return new RowColumns
            //{
            //    zoom = zoom,
            //    minRow = (int)(Math.Round(Math.Abs((minX - originTileX) / (res * tileSize.w)))),
            //    minCol = (int)(Math.Round(Math.Abs((minY - originTileY) / (resolution * 256)))),
            //    maxRow = (int)(Math.Round(Math.Abs((maxX - originTileX) / (res * tileSize.w)))),
            //    maxCol = (int)(Math.Round(Math.Abs((maxY - originTileY) / (resolution * 256)))),
            //};
        }
    }
}
