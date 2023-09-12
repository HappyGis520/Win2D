using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCore.MapCore
{
    /// <summary>
    /// 表示世界坐标的点
    /// </summary>
    public class WorldCoordinatePoint3D
    {

        public double WorldX;
        public double WorldY;
        public double WorldZ;

        public WorldCoordinatePoint3D(double worldX, double worldY, double worldZ)
        {
            WorldX = worldX;
            WorldY = worldY;
            WorldZ = worldZ;
        }
    }
    /// <summary>
    /// 表示像素坐标的点
    /// </summary>
    public class PixelCoordinatePoint3D
    {
        public int PixelX;
        public int PixelY;
        public double PixelZ;  // Assuming Z can have decimal values

        public PixelCoordinatePoint3D(int pixelX, int pixelY, double pixelZ)
        {
            PixelX = pixelX;
            PixelY = pixelY;
            PixelZ = pixelZ;
        }
    }

}
