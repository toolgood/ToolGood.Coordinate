using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Coordinate
{
    /// <summary>
    /// 火星坐标系：(GCJ-02)
    ///     iOS 地图（其实是高德）
    ///     Gogole地图
    ///     搜搜、阿里云、高德地图
    /// 百度坐标系：(BD-09) 
    ///     当然只有百度地图
    /// WGS84坐标系：（Gps84）
    /// 	国际标准，谷歌国外地图、osm地图等国外的地图一般都是这个
    /// </summary>
    public class CoordinateUtil
    {
        private static double pi = 3.1415926535897932384626;
        private static double a = 6378245.0;
        private static double ee = 0.00669342162296594323;

        /// <summary>
        /// 获取两点距离(BD-09) 
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="log1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="log2">经度2</param>
        /// <returns></returns>
        public static double GetDistance_By_Bd09(double lat1, double log1, double lat2,  double log2 )
        {
            var p1 = CoordinateUtil.Bd09_To_Gps84(lat1, log1);
            var p2 = CoordinateUtil.Bd09_To_Gps84(lat2, log2);
            var coor = Coordinates.CoordinateFactory.CreateCoordinate(Coordinates.Spheroid.WGS84);
            return coor.DistanceOfTwoPoints(p1.getWgLon(), p1.getWgLat(), p2.getWgLon(), p2.getWgLat());
        }
        /// <summary>
        /// 获取两点距离(GCJ-02)
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="log1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="log2">经度2</param>
        /// <returns></returns>
        public static double GetDistance_By_Gcj02(double lat1, double log1, double lat2, double log2)
        {
            var p1 = CoordinateUtil.Gcj02_To_Gps84(lat1, log1);
            var p2 = CoordinateUtil.Gcj02_To_Gps84(lat2, log2);
            var coor = Coordinates.CoordinateFactory.CreateCoordinate(Coordinates.Spheroid.WGS84);
            return coor.DistanceOfTwoPoints(p1.getWgLon(), p1.getWgLat(), p2.getWgLon(), p2.getWgLat());
        }
        /// <summary>
        /// 获取两点距离（Gps84）
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="log1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="log2">经度2</param>
        /// <returns></returns>
        public static double GetDistance_By_Gps84(double lat1, double log1, double lat2, double log2)
        {
            var coor = Coordinates.CoordinateFactory.CreateCoordinate(Coordinates.Spheroid.WGS84);
            return coor.DistanceOfTwoPoints(log1, lat1, log2, lat2);
        }


        /// <summary>
        /// Gps84 to 火星坐标系 (GCJ-02) World Geodetic System ==> Mars Geodetic System 
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <returns></returns>
        public static Position Gps84_To_Gcj02(double lat, double lon)
        {
            if (outOfChina(lat, lon)) {
                return null;
            }
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            return new Position(mgLat, mgLon);
        }

        /// <summary>
        /// Gps84 to 百度坐标系 (BD-09)
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <returns></returns>
        public static Position Gps84_To_Bd09(double lat, double lon)
        {
            if (outOfChina(lat, lon)) {
                return null;
            }
            var p1 = Gps84_To_Gcj02(lat, lon);
            return Gcj02_To_Bd09(p1.getWgLat(), p1.getWgLon());
        }

        /// <summary>
        /// 火星坐标系 (GCJ-02) to 84
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <returns></returns>
        public static Position Gcj02_To_Gps84(double lat, double lon)
        {
            Position gps = transform(lat, lon);
            double lontitude = lon * 2 - gps.getWgLon();
            double latitude = lat * 2 - gps.getWgLat();
            return new Position(latitude, lontitude);
        }

        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 将 GCJ-02 坐标转换成 BD-09 坐标 
        /// </summary>
        /// <param name="gg_lat">纬度</param>
        /// <param name="gg_lon">经度</param>
        /// <returns></returns>
        public static Position Gcj02_To_Bd09(double gg_lat, double gg_lon)
        {
            double x = gg_lon, y = gg_lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * pi);
            double bd_lon = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;
            return new Position(bd_lat, bd_lon);
        }

        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法
        /// </summary>
        /// <param name="bd_lat">纬度</param>
        /// <param name="bd_lon">经度</param>
        /// <returns></returns>
        public static Position Bd09_To_Gcj02(double bd_lat, double bd_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * pi);
            double gg_lon = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new Position(gg_lat, gg_lon);
        }

        /// <summary>
        /// 百度坐标系(BD-09)-->84 
        /// </summary>
        /// <param name="bd_lat">纬度</param>
        /// <param name="bd_lon">经度</param>
        /// <returns></returns>
        public static Position Bd09_To_Gps84(double bd_lat, double bd_lon)
        {

            Position gcj02 = CoordinateUtil.Bd09_To_Gcj02(bd_lat, bd_lon);
            Position map84 = CoordinateUtil.Gcj02_To_Gps84(gcj02.getWgLat(),
                    gcj02.getWgLon());
            return map84;

        }

        private static bool outOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        private static Position transform(double lat, double lon)
        {
            if (outOfChina(lat, lon)) {
                return new Position(lat, lon);
            }
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            return new Position(mgLat, mgLon);
        }

        private static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                    + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        private static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                    * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0
                    * pi)) * 2.0 / 3.0;
            return ret;
        }





    }
}
