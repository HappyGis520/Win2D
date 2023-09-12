using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCore.MapCore
{
    public class Map
    {
        public int MapWidthPixels { get;  set; }  // 实际的像素宽度
        public int MapHeightPixels { get;  set; }  // 实际的像素高度
        private List<WorldCoordinatePoint3D> points;
        private List<WorldCoordinateLine3D> lines;
        private List<WorldCoordinatePolygon> polygons;
        public Map(int mapPixelWidth,int mapPixelHeight)
        {
            mapPixelWidth = mapPixelWidth;
            mapPixelHeight = mapPixelHeight;
            points = new List<WorldCoordinatePoint3D>();
            lines = new List<WorldCoordinateLine3D>();
            polygons = new List<WorldCoordinatePolygon>();
        }

        public void AddPoint(WorldCoordinatePoint3D point)
        {
            points.Add(point);
        }
        public void AddLine(WorldCoordinateLine3D line)
        {
            lines.Add(line);
        }
        public void AddPolygon(WorldCoordinatePolygon polygon)
        {
            polygons.Add(polygon);
        }

        /// <summary>
        /// 计算地图的坐标范围
        /// </summary>
        /// <returns></returns>
        public WorldCoordinateRectangle CalculateWorldXYBound()
        {
            if (points.Count == 0 && lines.Count == 0 && polygons.Count == 0)
            {
                // Return a default area if the map has no geometries.
                return DisplayedArea;
            }

            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            // Check points
            foreach (var point in points)
            {
                minX = Math.Min(minX, point.WorldX);
                minY = Math.Min(minY, point.WorldY);
                maxX = Math.Max(maxX, point.WorldX);
                maxY = Math.Max(maxY, point.WorldY);
            }

            // Check lines
            foreach (var line in lines)
            {
                foreach (var point in line.Points)
                {
                    minX = Math.Min(minX, point.WorldX);
                    minY = Math.Min(minY, point.WorldY);
                    maxX = Math.Max(maxX, point.WorldX);
                    maxY = Math.Max(maxY, point.WorldY);
                }
            }

            // Check polygons
            foreach (var polygon in polygons)
            {
                foreach (var vertex in polygon.Vertices)
                {
                    minX = Math.Min(minX, vertex.WorldX);
                    minY = Math.Min(minY, vertex.WorldY);
                    maxX = Math.Max(maxX, vertex.WorldX);
                    maxY = Math.Max(maxY, vertex.WorldY);
                }
            }

            // Create a rectangle from the min and max values
            WorldCoordinatePoint3D topLeft = new WorldCoordinatePoint3D(minX, maxY, 0); // Using 0 for Z as a placeholder
            WorldCoordinatePoint3D bottomRight = new WorldCoordinatePoint3D(maxX, minY, 0);

            return new WorldCoordinateRectangle(topLeft, bottomRight);
        }
        public WorldCoordinateRectangle CalculateWorldXZBound()
        {

            if (points.Count == 0 && lines.Count == 0 && polygons.Count == 0)
            {
                // Return a default area if the map has no geometries.
                return DisplayedArea;
            }

            double minX = double.MaxValue;
            double minZ = double.MaxValue;
            double maxX = double.MinValue;
            double maxZ = double.MinValue;

            // Check points
            foreach (var point in points)
            {
                minX = Math.Min(minX, point.WorldX);
                minZ = Math.Min(minZ, point.WorldZ);
                maxX = Math.Max(maxX, point.WorldX);
                maxZ = Math.Max(maxZ, point.WorldZ);
            }

            // Check lines
            foreach (var line in lines)
            {
                foreach (var point in line.Points)
                {
                    minX = Math.Min(minX, point.WorldX);
                    minZ = Math.Min(minZ, point.WorldZ);
                    maxX = Math.Max(maxX, point.WorldX);
                    maxZ = Math.Max(maxZ, point.WorldZ);
                }
            }

            // Check polygons
            foreach (var polygon in polygons)
            {
                foreach (var vertex in polygon.Vertices)
                {
                    minX = Math.Min(minX, vertex.WorldX);
                    minZ = Math.Min(minZ, vertex.WorldZ);
                    maxX = Math.Max(maxX, vertex.WorldX);
                    maxZ = Math.Max(maxZ, vertex.WorldZ);
                }
            }

            // Create a rectangle from the min and max values
            WorldCoordinatePoint3D topLeft = new WorldCoordinatePoint3D(minX, 0, maxZ); // Using 0 for Z as a placeholder
            WorldCoordinatePoint3D bottomRight = new WorldCoordinatePoint3D(maxX, 0, minZ);

            return new WorldCoordinateRectangle(topLeft, bottomRight);
        }
        public PixelCoordinateRectangle WorldXYToPixel(WorldCoordinateRectangle worldRect)
        {
            var DisplayedArea  = CalculateWorldXYBound();
            double worldWidth = DisplayedArea.BottomRight.WorldX - DisplayedArea.TopLeft.WorldX;
            double worldHeight = DisplayedArea.TopLeft.WorldY - DisplayedArea.BottomRight.WorldY;

            double pixelsPerWorldUnitX = MapWidthPixels / worldWidth;
            double pixelsPerWorldUnitY = MapHeightPixels / worldHeight;

            int topLeftX = (int)((worldRect.TopLeft.WorldX - DisplayedArea.TopLeft.WorldX) * pixelsPerWorldUnitX);
            int topLeftY = (int)((DisplayedArea.TopLeft.WorldY - worldRect.TopLeft.WorldY) * pixelsPerWorldUnitY);

            int bottomRightX = (int)((worldRect.BottomRight.WorldX - DisplayedArea.TopLeft.WorldX) * pixelsPerWorldUnitX);
            int bottomRightY = (int)((DisplayedArea.TopLeft.WorldY - worldRect.BottomRight.WorldY) * pixelsPerWorldUnitY);

            return new PixelCoordinateRectangle(
                new PixelCoordinatePoint(topLeftX, topLeftY),
                new PixelCoordinatePoint(bottomRightX, bottomRightY)
            );
        }
        public PixelCoordinateRectangle WorldXZToPixel(WorldCoordinateRectangle worldRect)
        {
            var DisplayedArea = CalculateWorldXZBound();

            double worldWidth = DisplayedArea.BottomRight.WorldX - DisplayedArea.TopLeft.WorldX;
            double worldHeight = DisplayedArea.TopLeft.WorldZ - DisplayedArea.BottomRight.WordZ;

            double pixelsPerWorldUnitX = MapWidthPixels / worldWidth;
            double pixelsPerWorldUnitZ = MapHeightPixels / worldHeight;

            int topLeftX = (int)((worldRect.TopLeft.WorldX - DisplayedArea.TopLeft.WorldX) * pixelsPerWorldUnitX);
            int topLeftZ = (int)((DisplayedArea.TopLeft.WorldZ - worldRect.TopLeft.WorldZ) * pixelsPerWorldUnitZ);

            int bottomRightX = (int)((worldRect.BottomRight.WorldX - DisplayedArea.TopLeft.WorldX) * pixelsPerWorldUnitX);
            int bottomRightZ = (int)((DisplayedArea.TopLeft.WorldZ - worldRect.BottomRight.WorldZ) * pixelsPerWorldUnitZ);

            return new PixelCoordinateRectangle(
                new PixelCoordinatePoint(topLeftX, topLeftZ),
                new PixelCoordinatePoint(bottomRightX, bottomRightZ)
            );
        }
    }

}
