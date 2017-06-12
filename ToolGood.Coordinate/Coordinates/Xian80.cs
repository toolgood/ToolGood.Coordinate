using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Coordinate.Coordinates
{
    /// <summary>
    /// IUGG1975参考椭球 参数(西安80坐标系采用此参考椭球)
    /// 
    /// 长半轴a=6378140±5（m）
    /// 短半轴b=6356755.2882（m）
    /// 扁 率α = 1 / 298.257
    /// 第一偏心率平方 =0.00669438499959 第二偏心率平方=0.00673950181947
    /// </summary>
    public class Xian80 : GaussPrjBase
    {
        protected override double _majorAxis
        {
            get { return 6378140; }
        }

        protected override double _minorAxis
        {
            get { return 6356755.2882; }
        }

        protected override double _oblateness
        {
            get { return 1.0 / 298.257; }
        }
    }
}
