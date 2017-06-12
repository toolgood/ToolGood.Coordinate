using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Coordinate.Coordinates
{
    public abstract class GaussPrjBase : ICoordinate
    {
        /// <summary>
        /// 长轴
        /// </summary>
        protected abstract double _majorAxis { get; }
        /// <summary>
        /// 短轴
        /// </summary>
        protected abstract double _minorAxis { get; }
        /// <summary>
        /// 扁率
        /// </summary>
        protected abstract double _oblateness { get; }
        private int _zoneWide = 6;
        private int _longitude0 = 0;
        protected readonly double PI = 3.14159265353846;


        public void DD2DMS(double DecimalDegree, out int Degree, out int Minute, out double Second)
        {
            Degree = (int)DecimalDegree;
            Minute = (int)((DecimalDegree - Degree) * 60.0);
            Second = Math.Round((DecimalDegree * 60 - Degree * 60 - Minute) * 60 * 100) / 100.0;
        }

        public double DistanceOfTwoPoints(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double a = radLat1 - radLat2;
            double b = Rad(lng1) - Rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
            Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * _majorAxis;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
        private double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public void DMS2DD(int Degree, int Minute, double Second, out double DecimalDegree)
        {
            DecimalDegree = Degree + Minute / 60.0 + Second / 60.0 / 60.0;
        }

        public void GaussPrjCalculate(double longitude, double latitude, out double X, out double Y)
        {
            int ProjNo = 0;
            double longitude1, latitude1, longitude0, latitude0, X0, Y0, xval, yval;
            double e2, ee, NN, T, C, A, M, iPI;
            iPI = 0.0174532925199433; //3.1415926535898/180.0; 
            if (_longitude0 == 0) {
                if (_zoneWide == 3) {
                    ProjNo = (int)(longitude - _zoneWide / 2) / _zoneWide + 1;
                } else {
                    ProjNo = (int)longitude / _zoneWide + 1;
                }
                longitude0 = (ProjNo - 1) * _zoneWide + _zoneWide / 2;
                X0 = 1000000L * ProjNo + 500000L;
            } else {
                longitude0 = _longitude0;
                X0 = 500000L;
            }
            //ProjNo = (int)(longitude / _zoneWide);
            //

            longitude0 = longitude0 * iPI;
            latitude0 = 0;
            longitude1 = longitude * iPI; //经度转换为弧度
            latitude1 = latitude * iPI; //纬度转换为弧度
            e2 = 2 * _oblateness - _oblateness * _oblateness;
            ee = e2 * (1.0 - e2);
            NN = _majorAxis / Math.Sqrt(1.0 - e2 * Math.Sin(latitude1) * Math.Sin(latitude1));
            T = Math.Tan(latitude1) * Math.Tan(latitude1);
            C = ee * Math.Cos(latitude1) * Math.Cos(latitude1);
            A = (longitude1 - longitude0) * Math.Cos(latitude1);
            M = _majorAxis * ((1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256) * latitude1 - (3 * e2 / 8 + 3 * e2 * e2 / 32 + 45 * e2 * e2 * e2 / 1024) * Math.Sin(2 * latitude1) + (15 * e2 * e2 / 256 + 45 * e2 * e2 * e2 / 1024) * Math.Sin(4 * latitude1) - (35 * e2 * e2 * e2 / 3072) * Math.Sin(6 * latitude1));
            xval = NN * (A + (1 - T + C) * A * A * A / 6 + (5 - 18 * T + T * T + 72 * C - 58 * ee) * A * A * A * A * A / 120);
            yval = M + NN * Math.Tan(latitude1) * (A * A / 2 + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24 + (61 - 58 * T + T * T + 600 * C - 330 * ee) * A * A * A * A * A * A / 720);
            //

            Y0 = 0;
            X = Math.Round((xval + X0) * 100) / 100.0;
            Y = Math.Round((yval + Y0) * 100) / 100.0;
        }

        public void GaussPrjInvCalculate(double X, double Y, out double longitude, out double latitude)
        {
            int ProjNo;
            double longitude1, latitude1, longitude0, latitude0, X0, Y0, xval, yval;
            double e1, e2, ee, NN, T, C, M, D, R, u, fai, iPI;
            iPI = 0.0174532925199433; //3.1415926535898/180.0; 
            if (_longitude0 == 0) {
                ProjNo = (int)(X / 1000000L); //查找带号
                longitude0 = (ProjNo - 1) * _zoneWide + _zoneWide / 2;
                X0 = ProjNo * 1000000L + 500000L;
            } else {
                longitude0 = _longitude0;
                X0 = 500000L;
            }
            longitude0 = longitude0 * iPI; //中央经线

            Y0 = 0;
            xval = X - X0;
            yval = Y - Y0; //带内大地坐标
            e2 = 2 * _oblateness - _oblateness * _oblateness;
            e1 = (1.0 - Math.Sqrt(1 - e2)) / (1.0 + Math.Sqrt(1 - e2));
            ee = e2 / (1 - e2);
            M = yval;
            u = M / (_majorAxis * (1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256));
            fai = u + (3 * e1 / 2 - 27 * e1 * e1 * e1 / 32) * Math.Sin(2 * u) + (21 * e1 * e1 / 16 - 55 * e1 * e1 * e1 * e1 / 32) * Math.Sin(4 * u)
            + (151 * e1 * e1 * e1 / 96) * Math.Sin(6 * u) + (1097 * e1 * e1 * e1 * e1 / 512) * Math.Sin(8 * u);
            C = ee * Math.Cos(fai) * Math.Cos(fai);
            T = Math.Tan(fai) * Math.Tan(fai);
            NN = _majorAxis / Math.Sqrt(1.0 - e2 * Math.Sin(fai) * Math.Sin(fai));
            R = _majorAxis * (1 - e2) / Math.Sqrt((1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin(fai) * Math.Sin(fai)));
            D = xval / NN;
            //计算经度(Longitude) 纬度(Latitude)
            longitude1 = longitude0 + (D - (1 + 2 * T + C) * D * D * D / 6 + (5 - 2 * C + 28 * T - 3 * C * C + 8 * ee + 24 * T * T) * D * D * D * D * D / 120) / Math.Cos(fai);
            latitude1 = fai - (NN * Math.Tan(fai) / R) * (D * D / 2 - (5 + 3 * T + 10 * C - 4 * C * C - 9 * ee) * D * D * D * D / 24 + (61 + 90 * T + 298 * C + 45 * T * T - 256 * ee - 3 * C * C) * D * D * D * D * D * D / 720);
            //转换为度 DD
            longitude = Math.Round((longitude1 / iPI) * 1000000) / 1000000.0;
            latitude = Math.Round((latitude1 / iPI) * 1000000) / 1000000.0;
        }
    }
}
