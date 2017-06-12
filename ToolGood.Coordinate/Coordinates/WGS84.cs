using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Coordinate.Coordinates
{
    /// <summary>
    /// WGS84参考椭球 参数(全球GPS监测网采用此参考椭球)
    /// 
    /// 长轴6378137.000m，
    /// 短轴6356752.314，
    /// 扁率1/298.257223563。
    /// </summary>
    public class WGS84 : GaussPrjBase
    {
        protected override double _majorAxis
        {
            get { return 6378137; }
        }

        protected override double _minorAxis
        {
            get { return 6356752.314; }
        }

        protected override double _oblateness
        {
            get { return 1.0 / 298.257223563; }
        }
    }
}
