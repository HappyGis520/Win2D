using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCore.MapCore
{
    public class WorldCoordinateRectangle
    {
        public WorldCoordinatePoint3D TopLeft;
        public WorldCoordinatePoint3D BottomRight;

        public WorldCoordinateRectangle(WorldCoordinatePoint3D topLeft, WorldCoordinatePoint3D bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        // Additional methods or properties specific to the rectangle can be added here
    }

    public class PixelCoordinateRectangle
    {
        public PixelCoordinatePoint3D TopLeft;
        public PixelCoordinatePoint3D BottomRight;

        public PixelCoordinateRectangle(PixelCoordinatePoint3D topLeft, PixelCoordinatePoint3D bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }
    }

}
