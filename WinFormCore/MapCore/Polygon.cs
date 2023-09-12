using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace FormCore.MapCore
{
    

    public class WorldCoordinatePolygon
    {
        public List<WorldCoordinatePoint3D> Vertices { get; private set; }

        public WorldCoordinatePolygon(List<WorldCoordinatePoint3D> vertices)
        {
            Vertices = vertices ?? new List<WorldCoordinatePoint3D>();
        }

        // Additional methods or properties specific to the polygon can be added here
    }

    public class PixelCoordinatePolygon
    {
        public List<PixelCoordinatePoint3D> Vertices { get; private set; }

        public PixelCoordinatePolygon(List<PixelCoordinatePoint3D> vertices)
        {
            Vertices = vertices ?? new List<PixelCoordinatePoint3D>();
        }
    }

}
