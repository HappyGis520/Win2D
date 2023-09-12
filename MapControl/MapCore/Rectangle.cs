using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapControl
{
    public class WorldCoordinateRectangle
    {
        public WorldCoordinatePoint TopLeft;
        public WorldCoordinatePoint BottomRight;
        /// <summary>
        /// X横向跨度
        /// </summary>
        public double Width { 
            get { 
                return BottomRight.WorldX - TopLeft.WorldX; 
            } 
        }
        /// <summary>
        /// Y纵向跨度
        /// </summary>
        public double Height
        {
            get
            {
                return TopLeft.WorldY - BottomRight.WorldY;
            }
        }
        public WorldCoordinateRectangle(WorldCoordinatePoint topLeft, WorldCoordinatePoint bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }
        public WorldCoordinateRectangle(double topLeftX, double topLeftY, double bottomRightX , double bottomRightY)
        {
            TopLeft = new WorldCoordinatePoint(topLeftX, topLeftY);
            BottomRight = new WorldCoordinatePoint(bottomRightX, bottomRightY);
        }

    }

    public class PixelCoordinateRectangle
    {
        public PixelCoordinatePoint TopLeft;
        public PixelCoordinatePoint BottomRight;
        public double Width
        {
            get
            {
                return BottomRight.PixelX - TopLeft.PixelX;
            }
        }
        public  double Height
        {
            get
            {
                return BottomRight.PixelY - TopLeft.PixelY;
            }
        }

        public PixelCoordinateRectangle(PixelCoordinatePoint topLeft, PixelCoordinatePoint bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }
    }

}
