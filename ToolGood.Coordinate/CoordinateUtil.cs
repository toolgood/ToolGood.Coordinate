using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Coordinate.Coordinates;

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
        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
        const double pi = 3.1415926535897932384626;

        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 将 GCJ-02 坐标转换成 BD-09 坐标 
        /// </summary>
        /// <param name="gg_lat">纬度</param>
        /// <param name="gg_lon">经度</param>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        public static void Gcj02_To_Bd09(double gg_lat, double gg_lon, out double bd_lat, out double bd_lon)
        {
            double x = gg_lon, y = gg_lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            bd_lon = z * Math.Cos(theta) + 0.0065;
            bd_lat = z * Math.Sin(theta) + 0.006;
        }

        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法
        /// </summary>
        /// <param name="bd_lat">纬度</param>
        /// <param name="bd_lon">经度</param>
        /// <param name="gg_lat"></param>
        /// <param name="gg_lon"></param>
        /// <returns></returns>
        public static void Bd09_To_Gcj02(double bd_lat, double bd_lon, out double gg_lat, out double gg_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            gg_lon = z * Math.Cos(theta);
            gg_lat = z * Math.Sin(theta);
        }

        /// <summary>
        /// Gps84 to 火星坐标系 (GCJ-02) World Geodetic System ==> Mars Geodetic System 
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <returns></returns>
        public static void Gps84_To_Gcj02(double lat, double lon, out double gcj_lat, out double gcj_lon)
        {
            var a = 6378245.0;
            var ee = 0.00669342162296594323;

            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            gcj_lat = lat + dLat;
            gcj_lon = lon + dLon;
        }

        /// <summary>
        /// 火星坐标系 (GCJ-02) to 84
        /// </summary>
        /// <param name="gcjLat">纬度</param>
        /// <param name="gcjLon">经度</param>
        /// <param name="wgsLat"></param>
        /// <param name="wgsLon"></param>
        public static void Gcj02_To_Gps84(double gcjLat, double gcjLon, out double wgsLat, out double wgsLon)
        {
            var initDelta = 0.01;
            var threshold = 0.000000001;
            double dLat = initDelta, dLon = initDelta;
            double mLat = gcjLat - dLat, mLon = gcjLon - dLon;
            double pLat = gcjLat + dLat, pLon = gcjLon + dLon;
            int i = 0;
            wgsLat = 0;
            wgsLon = 0;
            while (i < 10000) {
                wgsLat = (mLat + pLat) / 2;
                wgsLon = (mLon + pLon) / 2;
                double tmplat, tmpLon;
                Gps84_To_Gcj02(wgsLat, wgsLon, out tmplat, out tmpLon);
                dLat = tmplat - gcjLat;
                dLon = tmpLon - gcjLon;
                if ((Math.Abs(dLat) < threshold) && (Math.Abs(dLon) < threshold))
                    break;

                if (dLat > 0) pLat = wgsLat; else mLat = wgsLat;
                if (dLon > 0) pLon = wgsLon; else mLon = wgsLon;
            }
        }

        private static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        private static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }

        /// <summary>
        /// Gps84 to 百度坐标系 (BD-09)
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        public static void Gps84_To_Bd09(double lat, double lon, out double bd_lat, out double bd_lon)
        {
            double x, y;
            Gps84_To_Gcj02(lat, lon, out x, out y);
            Gcj02_To_Bd09(x, y, out bd_lat, out bd_lon);
        }

        /// <summary>
        /// 百度坐标系 (BD-09) to Gps84
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        public static void Bd09_To_Gps84(double bd_lat, double bd_lon, out double lat, out double lon)
        {
            double x, y;
            Gps84_To_Gcj02(bd_lat, bd_lon, out x, out y);
            Gcj02_To_Bd09(x, y, out lat, out lon);
        }


        private const double a = 6378245.0;
        private const double ee = 0.00669342162296594323;

        /// <summary>
        /// 获取两点距离(BD-09) 
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="log1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="log2">经度2</param>
        /// <returns></returns>
        public static double GetDistance_By_Bd09(double lat1, double log1, double lat2, double log2)
        {
            double p1lat, p1lng, p2lat, p2lng;
            Bd09_To_Gps84(lat1, log1, out p1lat, out p1lng);
            Bd09_To_Gps84(lat2, log2, out p2lat, out p2lng);
            return GetDistance_By_Gps84(p1lat, p1lng, p2lat, p2lng);
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
            double p1lat, p1lng, p2lat, p2lng;
            Gcj02_To_Gps84(lat1, log1, out p1lat, out p1lng);
            Gcj02_To_Gps84(lat2, log2, out p2lat, out p2lng);
            return GetDistance_By_Gps84(p1lat, p1lng, p2lat, p2lng);
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

        public static bool outOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }



        public static void Bd09_To_Point(double lat, double lng, out double x, out double y)
        {
            double plat, plng;
            Bd09_To_Gps84(lat, lng, out plat, out plng);
            Gps84_To_Point(plat, plng, out x, out y);
        }

        public static void Gcj02_To_Point(double lat, double lng, out double x, out double y)
        {
            double plat, plng;
            Gcj02_To_Gps84(lat, lng, out plat, out plng);
            Gps84_To_Point(plat, plng, out x, out y);
        }

        public static void Gps84_To_Point(double lat, double lng, out double x, out double y)
        {
            ICoordinate ic = CoordinateFactory.CreateCoordinate(Spheroid.WGS84);
            ic.Longitude0 = Convert.ToInt32(lng);
            ic.GaussPrjCalculate(lng, lat, out x, out y);
        }

    }
}
