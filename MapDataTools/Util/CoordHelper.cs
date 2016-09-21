using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDataTools
{
    public class CoordHelper
    {
        private static double pi = 3.14159265358979324D;// 圆周率
        private static double a = 6378245.0D;// WGS 长轴半径
        private static double ee = 0.00669342162296594323D;// WGS 偏心率的平方
        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
        /// <summary>
        /// 84->火星
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static Coord Transform(double lon, double lat)
        {
            Coord localHashMap = new Coord();
            if (OutofChina(lat, lon))
            {
                localHashMap.lon = lon;
                localHashMap.lat = lat;
                return localHashMap;
            }
            double dLat = TransformLat(lon - 105.0, lat - 35.0);
            double dLon = TransformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            localHashMap.lon = mgLon;
            localHashMap.lat = mgLat;
            return localHashMap;
        }

        private static bool OutofChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        private static double TransformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                    + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        private static double TransformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                    * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0
                    * pi)) * 2.0 / 3.0;
            return ret;
        }
        /// <summary>
        /// gcj02-84
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static Coord Gcj2Wgs(double lon, double lat)
        {
            Coord p = new Coord();
            double lontitude = lon
                    - (Transform(lon, lat).lon - lon);
            double latitude = lat - (Transform(lon, lat).lat - lat);
            p.lon = lontitude;
            p.lat = latitude;
            return p;
        }
        /// <summary>
        /// 火星坐标转百度坐标
        /// </summary>
        /// <param name="gg_lat"></param>
        /// <param name="gg_lon"></param>
        /// <returns></returns>
        public static Coord BdEncrypt(double gg_lat, double gg_lon)
        {
            double x = gg_lon, y = gg_lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            double bd_lon = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;
            return new Coord(bd_lon, bd_lat);
        }
        /// <summary>
        /// 百度坐标转火星坐标
        /// </summary>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        /// <returns></returns>
        public static Coord BdDecrypt(double bd_lat, double bd_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            double gg_lon = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new Coord(gg_lon, gg_lat);
        }
        /// <summary>
        /// 经纬度转墨卡托投影
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static Coord WebMoctorJw2Pm(double lon,double lat)
        {
            Coord c=new Coord();
            c.lon = (lon / 180.0) * 20037508.34;
            if (lat > 85.05112) {
            lat = 85.05112;
            }
            if (lat < -85.05112) {
                lat = -85.05112;
            }
            lat = (Math.PI / 180.0) * lat;
            double tmp = Math.PI / 4.0 + lat / 2.0;
            c.lat = 20037508.34 * Math.Log(Math.Tan(tmp)) / Math.PI;
            return c;
        }
        /// <summary>
        /// 墨卡托投影转经纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Coord Mercator2lonLat(double x, double y)
        {
            Coord c = new Coord();
            c.lon = x / 20037508.34 * 180;
            y = y / 20037508.34 * 180;
            c.lat = 180 / Math.PI * (2 * Math.Atan(Math.Exp(y * Math.PI / 180)) - Math.PI / 2);
            return c;
        }
        /// <summary>
        /// Web墨卡托转经纬度
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public static Coord WebMercator2lonLat(Coord coord)
        {
            Coord c = new Coord();
            double x = coord.lon / 20037508.34 * 180;
            double y = coord.lat / 20037508.34 * 180;
            y = 180 / pi * (2 * Math.Atan(Math.Exp(y * pi / 180)) - pi / 2);
            c.lon = x;
            c.lat = y;
            return c;
        }
    }
    public class Coord
    {
        public double lon = 0;
        public double lat = 0;
        public Coord(double lon, double lat)
        {
            this.lon = lon;
            this.lat = lat;
        }
        public Coord()
        {
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.lon, this.lat);
        }
    }
}
