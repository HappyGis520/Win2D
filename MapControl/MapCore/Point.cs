using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapControl
{
    /// <summary>
    /// 表示世界坐标的点
    /// </summary>
    public class WorldCoordinatePoint
    {

        public double WorldX;
        public double WorldY;

        public WorldCoordinatePoint(double worldX, double worldY)
        {
            WorldX = worldX;
            WorldY = worldY;
        }
    }
    /// <summary>
    /// 表示像素坐标的点
    /// </summary>
    public class PixelCoordinatePoint
    {
        public int PixelX;
        public int PixelY;

        public PixelCoordinatePoint(int pixelX, int pixelY)
        {
            PixelX = pixelX;
            PixelY = pixelY;
        }
    }

}
