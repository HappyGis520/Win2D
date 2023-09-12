using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapControl
{
    public class Map
    {
        /// <summary>
        /// 地图实际的像素宽度
        /// </summary>
        public int MapWidthPixels { get;  set; }  
        /// <summary>
        /// 地图实际的像素高度
        /// </summary>
        public int MapHeightPixels { get;  set; }  
        public WorldCoordinatePoint[] Points;
        public WorldCoordinateLine[] Lines;
        public WorldCoordinateRectangle[] Rectangles;
        public Map(int mapPixelWidth,int mapPixelHeight)
        {
            MapWidthPixels = mapPixelWidth;
            MapHeightPixels = mapPixelHeight;
            Points = new WorldCoordinatePoint[100000];
            Lines = new WorldCoordinateLine[100000];
            Rectangles = new WorldCoordinateRectangle[100000];
        }

        /// <summary>
        /// 计算地图XY世界坐标范围
        /// </summary>
        /// <returns></returns>
        public WorldCoordinateRectangle CalculateWorldXYBound()
        {
            if (Points.Length == 0 && Lines.Length == 0||Rectangles.Length== 0)
            {
                // Return a default area if the map has no geometries.
                return new WorldCoordinateRectangle(0d,0d,0d,0d);
            }

            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            // Check points

            //foreach (var point in _Points)
            //{
            //    minX = Math.Min(minX, point.WorldX);
            //    minY = Math.Min(minY, point.WorldY);
            //    maxX = Math.Max(maxX, point.WorldX);
            //    maxY = Math.Max(maxY, point.WorldY);
            //}

            //for (int i = 0; i < Points.Length; i++)
            //{
            //    var point = Points[i];
            //    minX = Math.Min(minX, point.WorldX);
            //    minY = Math.Min(minY, point.WorldY);
            //    maxX = Math.Max(maxX, point.WorldX);
            //    maxY = Math.Max(maxY, point.WorldY);
            //}

            //// Check lines
            //foreach (var line in Lines)
            //{
            //    foreach (var point in line.Points)
            //    {
            //        minX = Math.Min(minX, point.WorldX);
            //        minY = Math.Min(minY, point.WorldY);
            //        maxX = Math.Max(maxX, point.WorldX);
            //        maxY = Math.Max(maxY, point.WorldY);
            //    }
            //}

            // Check polygons
            //foreach (var polygon in Polygons)
            //{
            //    foreach (var vertex in polygon.Vertices)
            //    {
            //        minX = Math.Min(minX, vertex.WorldX);
            //        minY = Math.Min(minY, vertex.WorldZ);
            //        maxX = Math.Max(maxX, vertex.WorldX);
            //        maxY = Math.Max(maxY, vertex.WorldZ);
            //    }
            //}

            foreach (var rect in Rectangles)
            {
                if (rect == null)
                    continue;
                var points = new WorldCoordinatePoint[] { rect.TopLeft, rect.BottomRight };
                foreach (var p in points)
                {
                   
                    minX = Math.Min(minX, p.WorldX);
                    minY = Math.Min(minY, p.WorldY);
                    maxX = Math.Max(maxX, p.WorldX);
                    maxY = Math.Max(maxY, p.WorldY);
                }
            }
            WorldCoordinatePoint topLeft = new WorldCoordinatePoint(minX, maxY); // Using 0 for Z as a placeholder
            WorldCoordinatePoint bottomRight = new WorldCoordinatePoint(maxX, minY);

            return new WorldCoordinateRectangle(topLeft, bottomRight);
        }
        /// <summary>
        /// 计算地图XY像素坐标范围
        /// </summary>
        /// <returns></returns>
        public PixelCoordinateRectangle CalculatePixelXYBound()
        {
            var bound = CalculateWorldXYBound();
            return  WorldXYToPixel(bound);
        }
        WorldCoordinateRectangle _WorldXYBounds = null;
        private PixelCoordinateRectangle _PixelXYBound = null;
        /// <summary>
        /// 视图像素范围
        /// </summary>
        private PixelCoordinateRectangle _ViewBound = null;
        /// <summary>
        /// 视图中心像素坐标
        /// </summary>
        private PixelCoordinatePoint _ViewPixelCenter = null;
        public PixelCoordinatePoint ViewPixelCenter
        {
            get
            {
                return _ViewPixelCenter;
            }
            set
            {
                _ViewPixelCenter = value;
                if(value!=null)
                {
                }
            }
        }
        /// <summary>
        /// 视图中心世界坐标
        /// </summary>
        private WorldCoordinatePoint _ViewWorldCenter = null;
        public WorldCoordinatePoint ViewWorldCenter
        {
            get
            {
                return _ViewWorldCenter;
            }
        }


        public void UpdateBound()
        {

            _WorldXYBounds = CalculateWorldXYBound();
            _PixelXYBound = CalculatePixelXYBound();
        }

        public bool ZoomToPixelBound(int mapPixelWidth, int mapPixelHeight)
        {
            MapWidthPixels = mapPixelWidth;
            MapHeightPixels = mapPixelHeight;
            return true;
        }
        public PixelCoordinateRectangle WorldXYToPixel(WorldCoordinateRectangle worldRect)
        {
            var DisplayedArea  = CalculateWorldXYBound();                                               //计算地图XY世界坐标范围
            double worldWidth = DisplayedArea.Width;       //横向跨度
            double worldHeight = DisplayedArea.Width;      //纵向跨度

            double pixelsPerWorldUnitX = MapWidthPixels / worldWidth;                                 //横向每个世界单位对应的像素数
            double pixelsPerWorldUnitY = MapHeightPixels / worldHeight;                               //纵向每个世界单位对应的像素数

            int topLeftX = (int)((worldRect.TopLeft.WorldX - DisplayedArea.TopLeft.WorldX) * pixelsPerWorldUnitX);//计算对象与地图左上角的横向距离
            int topLeftY = (int)((DisplayedArea.TopLeft.WorldY - worldRect.TopLeft.WorldY) * pixelsPerWorldUnitY);//计算对象与地图左上角的纵向距离

            int bottomRightX = (int)((worldRect.BottomRight.WorldX - DisplayedArea.TopLeft.WorldX) * pixelsPerWorldUnitX);
            int bottomRightY = (int)((DisplayedArea.TopLeft.WorldY - worldRect.BottomRight.WorldY) * pixelsPerWorldUnitY);

            return new PixelCoordinateRectangle(
                new PixelCoordinatePoint(topLeftX, topLeftY),
                new PixelCoordinatePoint(bottomRightX, bottomRightY)
            );
        }

        public PixelCoordinatePoint WorldXYToPixel( WorldCoordinatePoint worldPoint, Point? viewportOffset = null)
        {

            if (_WorldXYBounds == null || _PixelXYBound == null)
                return null;
            int offsetX = viewportOffset?.X ?? 0;
            int offsetY = viewportOffset?.Y ?? 0;

            int pixelX = (int)((worldPoint.WorldX - _WorldXYBounds.TopLeft.WorldX) * (_PixelXYBound.Width / _WorldXYBounds.Width) + offsetX);
            int pixelY = (int)((worldPoint.WorldY - _WorldXYBounds.BottomRight.WorldY) * (_PixelXYBound.Height / _WorldXYBounds.Width) + offsetY);

            return new PixelCoordinatePoint(pixelX, pixelY);
        }


    }

}
