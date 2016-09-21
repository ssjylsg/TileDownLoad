using System;

namespace NPMapTiles.ImageTools
{
    using MapDataTools.Util;

    public class MapTool
    {
        private double maxExtent = 20037508.34;
        private double maxResolution = 156543.03390625;
        private double topTileFromX =  -180;
        private double topTileFromY =  90;
        private double topTileToX =  180;
        private double topTileToY =  -270;

        /// <summary>
        /// 计算指定范围内在某一级别中的行列号
        /// </summary>
        /// <param name="minX">最小经度</param>
        /// <param name="minY">最小纬度</param>
        /// <param name="maxX">最大经度</param>
        /// <param name="maxY">最大纬度</param>
        /// <param name="zoom">层级数</param>
        /// <returns>行列号存储类型</returns>
        public RowColumns GetGoogleRowColomns(double minX, double minY, double maxX, double maxY, int zoom)
        {
            RowColumns rc = new RowColumns();
            rc.minRow = (int)Math.Floor((minX + maxExtent) / (maxResolution / (Math.Pow(2, zoom)) * 256.0));
            rc.maxRow = (int)Math.Ceiling((maxX + maxExtent) / (maxResolution / (Math.Pow(2, zoom)) * 256.0));
            rc.minCol = (int)Math.Floor((maxExtent - maxY) / (maxResolution / (Math.Pow(2, zoom)) * 256.0));
            rc.maxCol = (int)Math.Ceiling((maxExtent - minY) / (maxResolution / (Math.Pow(2, zoom)) * 256.0));
            rc.zoom = zoom;
            return rc;
        }
        public RowColumns GetTdtRowColomns(double minX, double minY, double maxX, double maxY, int zoom)
        {
            RowColumns rc = new RowColumns();
            double coef = 360.0 / Math.Pow(2, zoom);
            rc.minRow = (int)Math.Floor((minX - this.topTileFromX) / coef);
            rc.maxRow =(int)Math.Ceiling((maxX - this.topTileFromX) / coef);
            rc.minCol = (int)Math.Floor((this.topTileFromY-maxY) / coef);
            rc.maxCol = (int)Math.Ceiling((this.topTileFromY-minY) / coef);
            rc.zoom = zoom;
            return rc;
        }
    }
}
