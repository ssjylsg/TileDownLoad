using System;
using System.Collections.Generic;
using System.Text;

namespace CutDataTiles
{
    public class MapTool
    {
        private static double maxResolution = 156543.03390625;
        private static double tileOriginX = 0;
        private static double tileOriginY = 0;
        private static double[] tileSize = new double[]{256,256};

        /// <summary>
        /// 计算指定范围内在某一级别中的行列号
        /// </summary>
        /// <param name="minX">最小经度</param>
        /// <param name="minY">最小纬度</param>
        /// <param name="maxX">最大经度</param>
        /// <param name="maxY">最大纬度</param>
        /// <param name="zoom">层级数</param>
        /// <returns>行列号存储类型</returns>
        public static RowColumns GeRowColomns(double[] fullExent,double[] resolutions, int zoom)
        {
            double resolution = resolutions[zoom];
            double tilelon = resolution * tileSize[0];
            double tilelat = resolution * tileSize[1];
            double originX = fullExent[0];
            double originY = fullExent[1];
            double offsetX = fullExent[2] - fullExent[0];
            double offsetY = fullExent[3] - fullExent[1];
            int col = (int)Math.Ceiling(offsetX / tilelon);
            int row = (int)Math.Ceiling(offsetY / tilelat);
            RowColumns rc = new RowColumns();
            rc.Col = col;
            rc.Row = row;
            rc.tilelon = tilelon;
            rc.tilelat = tilelat;
            rc.zoom=zoom;
            return rc;
        }

        /// <summary>
        /// 返回指定坐标对应分辨率下的区域范围
        /// </summary>
        /// <param name="point"></param>
        /// <param name="fullExent"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static double[] GetBoundsByPoint(GeoPoint point, double[] fullExent, double resolution)
        {
            double tilelon = resolution * tileSize[0];
            double tilelat = resolution * tileSize[1];
            double originX = fullExent[0];
            double originY = fullExent[1];
            double offsetX = point.x - fullExent[0];
            double offsetY = point.y - fullExent[1];
            int col = (int)Math.Ceiling(offsetX / tilelon);
            int row = (int)Math.Ceiling(offsetY / tilelat);
            double[] bounds = new double[] { fullExent[0] + (col - 1) * tilelon, fullExent[1] + (row - 1) * tilelat, fullExent[0] + col * tilelon, fullExent[1] + row * tilelat };
            return bounds;

        }
        /// <summary>
        /// 以arcgis的格式切片，进行切片行列号的获取
        /// </summary>
        /// <param name="bounds">切片对应的区域大小，经纬度</param>
        /// <param name="origin">切片原点</param>
        /// <param name="resolution">分辨率</param>
        /// <returns>行列号</returns>
        public static RowColumns GetTileRowColomns(double[] bounds, double[] origin, double resolution)
        {
            double originTileX = (origin[0] + (resolution * tileSize[0])); 
            double originTileY = (origin[1] - (resolution * tileSize[1]));

            double centerX = (bounds[0]+bounds[2])/2;
            double centerY = (bounds[1]+bounds[3])/2;

            int x = (int)(Math.Round(Math.Abs((centerX - originTileX) / (resolution * tileSize[0]))));
            int y = (int)(Math.Round(Math.Abs((originTileY - centerY) / (resolution * tileSize[1]))));
            RowColumns rc = new RowColumns();
            rc.Col = y;
            rc.Row = x;
            return rc;
        }
    }
    /// <summary>
    /// 行列号存储类型对象，最小、最大行号，最小、最大列号
    /// </summary>
    public class RowColumns
    {
        public int Row = 0;
        public int Col = 0;
        public double tilelon = 0;
        public double tilelat = 0;
        public int zoom = 0;
    }
    /// <summary>
    /// 点位信息
    /// </summary>
    public class GeoPoint
    {
        public double x = 0;
        public double y = 0;
    }
}
