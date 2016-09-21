using System;
using System.Collections.Generic;

namespace MapDataTools
{
    public class QQMap
    {
        /// <summary>
        /// 获取地图切片信息
        /// </summary>
        /// <param name="extent">区域</param>
        /// <param name="zoom">地图级别</param>
        /// <returns>切片信息</returns>
        public TitlesInfo GetTitlesInfo(Extent extent, int zoom)
        {
            TitlesInfo titleInfo = new TitlesInfo();
            double resolution = Math.Pow(2, 18 - zoom);
            titleInfo.minRow = (int)(Math.Round((extent.minX - 0) / (resolution * 256)));
            titleInfo.minCol = (int)(Math.Round((extent.minY - 23000) / (resolution * 256)));
            titleInfo.maxRow = (int)(Math.Round((extent.maxX - 0) / (resolution * 256)));
            titleInfo.maxCol = (int)(Math.Round((extent.maxY - 23000) / (resolution * 256)));
            return titleInfo;
        }
        public string GetTitleUrl(int row, int col, int zoom)
        {
            return "";
        }
        /// <summary>
        /// 根据范围和关键字获取兴趣点信息
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <param name="keyWord"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<POIInfo> GetPoiInfos(double minX, double minY, double maxX, double maxY, string keyWord, int pageIndex)
        {
            return new List<POIInfo>();
        }
        /// <summary>
        /// 根据城市编码及关键字获取兴趣点信息
        /// </summary>
        /// <param name="cityCode"></param>
        /// <param name="keyWord"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<POIInfo> GetPoiInfos(string cityCode, string keyWord, int pageIndex)
        {
            return new List<POIInfo>();
        }
    }
}
