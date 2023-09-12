using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace FormCore.MapCore
{
    public class WorldCoordinateLine3D
    {
        public List<WorldCoordinatePoint3D> Points { get; private set; }

        public WorldCoordinateLine3D(List<WorldCoordinatePoint3D> points)
        {
            Points = points ?? new List<WorldCoordinatePoint3D>();
        }

        // Additional methods or properties specific to the line can be added here
    }

    public class PixelCoordinateLine3D
    {
        public List<PixelCoordinatePoint3D> Points { get; private set; }

        public PixelCoordinateLine3D(List<PixelCoordinatePoint3D> points)
        {
            Points = points ?? new List<PixelCoordinatePoint3D>();
        }

        // Additional methods or properties specific to the line can be added here
    }

}
