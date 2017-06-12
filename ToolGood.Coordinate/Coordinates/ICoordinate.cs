using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Coordinate.Coordinates
{
    public interface ICoordinate
    {

        /// <summary>
        /// 十进制双精度角度转换成度分秒角度格式
        /// </summary>
        /// <param name="Decimal Degree">度，十进制型双精度</param>
        /// <param name="Degree">度，整型</param>
        /// <param name="Minute">分，整型</param>
        /// <param name="Second">秒，双精度型</param>
        void DD2DMS(double DecimalDegree, out int Degree, out int Minute, out double Second);

        /// <summary>
        /// 求两点之间的距离(根据经纬度)
        /// </summary>
        /// <param name="lng1">经度1</param>
        /// <param name="lat1">纬度1</param>
        /// <param name="lng2">经度2</param>
        /// <param name="lat2">纬度2</param>
        /// <returns>两点间距离(单位:meters)</returns>
        double DistanceOfTwoPoints(double lng1, double lat1, double lng2, double lat2);

        /// <summary>
        /// 求两点之间的距离(大地坐标)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns>单位为meters</returns>
        //double DistanceOfTwoPoints(double x1, double y1, double x2, double y2);

        /// <summary>
        /// 度分秒角度格式转换成十进制度双精度角度格式
        /// </summary>
        /// <param name="Degree">度，整型</param>
        /// <param name="Minute">分，整型</param>
        /// <param name="Second">秒，双精度型</param>
        /// <param name="Decimal Degree">度，十进制型双精度</param>   
        void DMS2DD(int Degree, int Minute, double Second, out double DecimalDegree);

        /// <summary>
        /// 高期投影正算
        /// 由经纬度（单位：Decimal Degree）正算到大地坐标（单位：Metre，含带号）
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        void GaussPrjCalculate(double longitude, double latitude, out double X, out double Y);

        /// <summary>
        /// 高斯投影反算
        /// 大地坐标（单位：Metre，含带号）反算到经纬度坐标（单位，Decimal Degree）
        /// </summary>
        /// <param name="X">大地坐标X值</param>
        /// <param name="Y">大地坐标Y值</param>
        void GaussPrjInvCalculate(double X, double Y, out double longitude, out double latitude);

    }
}
