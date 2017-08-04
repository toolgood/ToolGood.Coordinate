using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolGood.Coordinate;
using PetaTest;

namespace ToolGood.Coordinate.Test
{
    [TestFixture]
    public class CoordinateTest
    {
        [Test]
        public void Gps84_To_Gcj02()
        {
            var p1 = CoordinateUtil.Gps84_To_Gcj02(39.990475, 116.481499);
            Assert.AreEqual(0, Math.Round(p1.getWgLat() - 39.991754014757, 6));
            Assert.AreEqual(0, Math.Round(p1.getWgLon() - 116.487585177952, 6));
        }


        [Test]
        public void Gcj02_To_Gps84()
        {
            var p1 = CoordinateUtil.Gcj02_To_Gps84(39.991754, 116.487585);
            Assert.AreEqual(0, Math.Round(p1.getWgLat() - 39.990475, 4));
            Assert.AreEqual(0, Math.Round(p1.getWgLon() - 116.481499, 4));

            double lat, lng;
            bd_encrypt(40.439543, 116.584119,out lat,out lng);


        }


        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

        void bd_encrypt(double gg_lat, double gg_lon,out double bd_lat, out double bd_lon)
        {
            double x = gg_lon, y = gg_lat;
            double z =Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            bd_lon = z * Math.Cos(theta) + 0.0065;
            bd_lat = z * Math.Sin(theta) + 0.006;
        }

        void bd_decrypt(double bd_lat, double bd_lon, out double gg_lat, out double gg_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            gg_lon = z * Math.Cos(theta);
            gg_lat = z * Math.Sin(theta);
        }


        [Test]
        public void Bd09_To_Gcj02()
        {
            var p1 = CoordinateUtil.Bd09_To_Gcj02(39.990475, 116.481499);
            Assert.AreEqual(0, Math.Round(p1.getWgLat() - 39.984717169345, 3));
            Assert.AreEqual(0, Math.Round(p1.getWgLon() - 116.4748955248, 3));
        }

        [Test]
        public void Gcj02_To_Bd09()
        {
            var p1 = CoordinateUtil.Gcj02_To_Bd09(39.984717169345, 116.4748955248);
            Assert.AreEqual(0, Math.Round(p1.getWgLat() - 39.990475, 3));
            Assert.AreEqual(0, Math.Round(p1.getWgLon() - 116.481499, 3));
        }

    }
}
