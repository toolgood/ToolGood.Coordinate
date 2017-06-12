using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Coordinate.Coordinates
{
    /// <summary>
    /// Krasovsky参考椭球 参数(北京54坐标系采用此参考椭球)
    /// 
    /// 长轴6378245m，
    /// 短轴6356863，
    /// 扁率1/298.3；
    /// </summary>
    public class Beijing54 : GaussPrjBase
    {
        protected override double _majorAxis
        {
            get { return 6378245; }
        }

        protected override double _minorAxis
        {
            get { return 6356863; }
        }

        protected override double _oblateness
        {
            get { return 1.0 / 298.3; }
        }
    }
}
