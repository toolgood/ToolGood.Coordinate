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
