using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.Coordinate.Coordinates;

namespace ToolGood.Coordinate.Coordinates
{
   public  class CoordinateFactory
    {
        public static ICoordinate CreateCoordinate(Spheroid s)
        {
            ICoordinate coordinate;
            switch (s) {
                case Spheroid.Beijing54:
                    coordinate = new Beijing54();
                    break;
                case Spheroid.WGS84:
                    coordinate = new WGS84();
                    break;
                case Spheroid.Xian80:
                    coordinate = new Xian80();
                    break;
                default:
                    coordinate = null;
                    break;
            }
            return coordinate;
        }
    }
}
