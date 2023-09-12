using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace MapControl
{
    public class WorldCoordinateLine
    {
        public WorldCoordinatePoint StartPoint;
        public WorldCoordinatePoint EndPoint;

        public WorldCoordinateLine(WorldCoordinatePoint startPoint,WorldCoordinatePoint endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
    }

    public class PixelCoordinateLine
    {
        public PixelCoordinatePoint StartPoint;
        public PixelCoordinatePoint EndPoint;

        public PixelCoordinateLine(PixelCoordinatePoint startPoint, PixelCoordinatePoint endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        // Additional methods or properties specific to the line can be added here
    }

}
