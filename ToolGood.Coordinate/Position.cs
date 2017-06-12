using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Coordinate
{
    public class Position
    {
        private double wgLat;
        private double wgLon;

        public Position(double wgLat, double wgLon)
        {
            setWgLat(wgLat);
            setWgLon(wgLon);
        }
        /// <summary>
        /// 获取纬度
        /// </summary>
        /// <returns></returns>
        public double getWgLat()
        {
            return wgLat;
        }
        /// <summary>
        /// 设置纬度
        /// </summary>
        /// <param name="wgLat"></param>
        public void setWgLat(double wgLat)
        {
            this.wgLat = wgLat;
        }
        /// <summary>
        /// 获取经度
        /// </summary>
        /// <returns></returns>
        public double getWgLon()
        {
            return wgLon;
        }
        /// <summary>
        /// 设置经度
        /// </summary>
        /// <param name="wgLon"></param>
        public void setWgLon(double wgLon)
        {
            this.wgLon = wgLon;
        }


        public override String ToString()
        {
            return wgLat + "," + wgLon;
        }
    }
}
