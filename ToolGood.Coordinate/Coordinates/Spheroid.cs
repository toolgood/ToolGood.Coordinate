using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Coordinate.Coordinates
{
    public enum Spheroid
    {
        /// <summary>
        /// GPS全球定位系统使用而建立的坐标系统
        /// </summary>
        WGS84,
        /// <summary>
        /// 北京54坐标系为参心大地坐标系
        /// </summary>
        Beijing54,
        /// <summary>
        /// 1980年西安坐标系，又简称西安大地原点
        /// </summary>
        Xian80,

    }
}
