using System;
using System.Text;

namespace MapDataTools.Tile
{
    using System.IO;

    using MapDataTools.Util;

    public  class LintuTile:MapTile
    {
        private double[] resolutions;

        private string url = "http://cache8.51ditu.com";
        public LintuTile()
        {
            resolutions = new double[15];
            var baseResolution = 0.703125 / 8;
            for (int i = 0; i < resolutions.Length; i++)
            {
                resolutions[i] = baseResolution;
                baseResolution /= 2;
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
              string Dpath = path + "\\" + i.ToString();
              if (!Directory.Exists(Dpath))
              {
                  Directory.CreateDirectory(Dpath);
              }
              for (int j = minCol; j < maxCol + 1; j++)
              {
                  if (workInfo.downStates == DownStates.stop) return;
                  string tempPath = Dpath + "\\" + j.ToString() + "." + imgType;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private string GetTitleUrl(int row, int col, int level)
        {
            level = level + 4;
            var offset = (int)Math.Pow(2, level - 1);
            int numX = col;
            int numY = offset - row - 1;

            var nGrade = (int)Math.Ceiling((((float)level) - 5) / 4);

            if (nGrade < 0)
            {
                nGrade = 0;
            }

            int nPreRow = 0, nPreCol = 0, nPreSize = 0;
            var sb = new StringBuilder();
            sb.Append(url);
            sb.Append("/");
            sb.Append(level);
            sb.Append("/");

            for (int i = 0; i < nGrade; i++)
            {
                int nSize = 1 << (4 * (nGrade - i));
                int nRow = (numX - nPreRow * nPreSize) / nSize;
                int nCol = (numY - nPreCol * nPreSize) / nSize;

                if (nRow <= 9)
                {
                    sb.Append("0");
                }

                sb.Append(nRow);
                if (nCol <= 9)
                {
                    sb.Append("0");
                }

                sb.Append(nCol);
                sb.Append("/");
                nPreRow = nRow;
                nPreCol = nCol;
                nPreSize = nSize;
            }

            var id =
                (long)
                ((numX & ((1 << 20) - 1)) + ((numY & ((1 << 20) - 1)) * 1048576d)
                 + ((level & ((1 << 8) - 1)) * 1099511627776d));

            sb.Append(id);
            sb.Append(".png");
            return sb.ToString();
        }

        public override RowColumns GetRowColomns(double minX, double minY, double maxX, double maxY, int zoom)
        {
            double resolution = resolutions[zoom];
            return new RowColumns
                       {
                           zoom = zoom,
                           minRow = (int)(Math.Round((minX - 0) / (resolution * 256))),
                           minCol = (int)(Math.Round((minY - 180) / (resolution * 256))),
                           maxRow = (int)(Math.Round((maxX - 0) / (resolution * 256))),
                           maxCol = (int)(Math.Round((maxY - 180) / (resolution * 256)))
                       };
        }
    }
}
