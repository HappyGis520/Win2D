using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MapControl
{
    //像素坐标的坐标原点是基于屏幕左上角的，X轴向右，Y轴向下,即屏幕像素坐标的0，0点位于屏幕左上角
    //世界坐标的坐标原点是位于屏幕左下角的，X轴向右，Y轴向上,即世界坐标的0，0点位于屏幕左下角
    public class Map
    {
        /*------------------------------------------------------------视图相关参数-----------------------------------------------*/
        /// <summary>
        /// 视图的像素宽度
        /// </summary>
        private int _ViewPixelWidth=0;
        /// <summary>
        /// 视图的像素高度
        /// </summary>
        private int _viewPixelHeight = 0;

        /// <summary>
        /// 视图在地图图片上的范围
        /// </summary>
        private PixelCoordinateRectangle _ViewBoundInMapImage = null;
        /// <summary>
        /// 视图在地图图片上的范围
        /// </summary>
        public PixelCoordinateRectangle ViewBoundInMapImage
        {
            get
            {
                return _ViewBoundInMapImage;
            }
        }

        /// <summary>
        /// 视图在地图上世界坐标范围
        /// </summary>
        private WorldCoordinateRectangle _ViewBoudInMapWorld = null;
        /// <summary>
        /// 视图在地图上世界坐标范围
        /// </summary>
        public WorldCoordinateRectangle ViewBoudInMapWorld
        {
            get
            {
                return _ViewBoudInMapWorld;
            }
        }
        /// <summary>
        /// 视图中心在地图上的世界坐标
        /// </summary>
        private WorldCoordinatePoint _ViewCenterInMapXY = null;
        /// <summary>
        /// 视图中心在地图上的世界坐标
        /// </summary>
        public WorldCoordinatePoint ViewCenterInMapXY
        {
            get
            {
                return _ViewCenterInMapXY;
            }
        }
        /// <summary>
        /// 视图中心像素坐标
        /// </summary>
        private PixelCoordinatePoint _ViewCenterInMapImageXY = null;
        /// <summary>
        /// 视图中心在地图图片中的像素坐标
        /// </summary>
        public PixelCoordinatePoint ViewCenterInMapImageXY
        {
            get
            {
                return _ViewCenterInMapImageXY;
            }
            set
            {
                _ViewCenterInMapImageXY = value;
                if (value != null)
                {
                    //像素坐标转世界坐标
                    //_ViewWorldCenter = PixelXYToWorldXY(_ViewPixelCenter);
                }
            }
        }


        /// <summary>
        /// 缩放层级
        /// </summary>
        public float ZoomLevel
        {
            get
            {
                return _ZoomLevel;
            }
            set
            {
                _ZoomLevel = value < 0 ? 1 : value > 6 ? 6 : value;

            }
        }
        private float _ZoomLevel = 3;
        /*------------------------------------------------------------地图相关参数-----------------------------------------------*/
        /// <summary>
        /// 地图世界坐标范围
        /// </summary>
        private WorldCoordinateRectangle _MapWorldXYBounds = null;
        /// <summary>
        /// 地图图片像素宽度
        /// </summary>
        private int _MapImageWidthPixels = 0;
        /// <summary>
        /// 地图图片的像素高度
        /// </summary>
        private int _MapImageHeightPixels = 0;
        /// <summary>
        /// 地图像素坐标范围
        /// </summary>
        public PixelCoordinateRectangle MapImagePixelXYBound
        {
            get
            {
                return new PixelCoordinateRectangle(new PixelCoordinatePoint(0, 0), new PixelCoordinatePoint(_MapImageWidthPixels, _MapImageHeightPixels));
            }
        }
        /// <summary>
        /// 每个世界坐标单位对应的像素数
        /// </summary>
        public double PixelsPerWorldUnit
        {
            get;private set;
        }


        public WorldCoordinatePoint[] Points;
        public WorldCoordinateLine[] Lines;
        public WorldCoordinateRectangle[] Rectangles;

        public Map()
        {
            Points = new WorldCoordinatePoint[100000];
            Lines = new WorldCoordinateLine[100000];
            ZoomLevel = 1;
            Rectangles = new WorldCoordinateRectangle[100000];
        }
        /// <summary>
        /// 计算地图XY世界坐标范围
        /// 在添加或删除图形时需要重新计算
        /// </summary>
        /// <returns></returns>
        public WorldCoordinateRectangle CalculateMapWorldXYBound()
        {
            if (Points.Length == 0 && Lines.Length == 0 || Rectangles.Length == 0)
            {
                // Return a default area if the map has no geometries.
                return new WorldCoordinateRectangle(0d, 0d, 0d, 0d);
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
        ///地图缩放至指定像素范围
        /// </summary>
        /// <param name="mapPixelWidth"></param>
        /// <param name="mapPixelHeight"></param>
        /// <returns></returns>
        public bool ZoomToPixelBound(int viewPixelWidth, int viewPixelHeight)
        {
            ZoomLevel = 1;
            _viewPixelHeight = viewPixelHeight;
            _ViewPixelWidth = viewPixelWidth;

            _MapImageWidthPixels = viewPixelWidth;
            _MapImageHeightPixels = viewPixelHeight;

            ViewCenterInMapImageXY = new PixelCoordinatePoint(_MapImageWidthPixels / 2, _MapImageHeightPixels / 2);

            _MapWorldXYBounds = CalculateMapWorldXYBound();

            double pixelsPerWorldUnitX = _MapImageWidthPixels / _MapWorldXYBounds.Width;                                 //横向每个世界单位对应的像素数
            double pixelsPerWorldUnitY = _MapImageHeightPixels / _MapWorldXYBounds.Height;                               //纵向每个世界单位对应的像素数
            //为了画下所有的图形，比例使用较小的值; 这样地图填充区域，是一个小于或等于视图大小的矩形
            var pixelPerWorldUnit = pixelsPerWorldUnitX > pixelsPerWorldUnitY ? pixelsPerWorldUnitY : pixelsPerWorldUnitX;//转换比例
            PixelsPerWorldUnit = pixelPerWorldUnit;

            _ViewCenterInMapXY = MapImagePixelXYToWorldXY(ViewCenterInMapImageXY);
            _ViewBoundInMapImage = new PixelCoordinateRectangle(new PixelCoordinatePoint(_ViewCenterInMapImageXY.PixelX - _ViewPixelWidth / 2, _ViewCenterInMapImageXY.PixelY-_viewPixelHeight/2),
                new PixelCoordinatePoint(_ViewCenterInMapImageXY.PixelX + _ViewPixelWidth / 2, _ViewCenterInMapImageXY.PixelY + _viewPixelHeight / 2));
            return true;
        }
        /// <summary>
        /// 地图图片上的像素坐标转世界坐标
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public WorldCoordinatePoint MapImagePixelXYToWorldXY(PixelCoordinatePoint point)
        {
            if (_MapWorldXYBounds == null || MapImagePixelXYBound == null)
                return null;
            var pointx = point.PixelX / PixelsPerWorldUnit;
            var pointy = point.PixelY / PixelsPerWorldUnit;
            return new WorldCoordinatePoint(pointx, pointy);
        }


        /// <summary>
        /// 地图的世界坐标范围转换成地图图片像素坐标范围
        /// </summary>
        /// <param name="worldRect"></param>
        /// <param name="mapOriginPixCoord">地图原点的像素坐标</param>
        /// <returns></returns>
        private PixelCoordinateRectangle MapWorldBoundToMapImagePixelBound(WorldCoordinateRectangle worldRect)
        {
            if (_MapWorldXYBounds == null )
                return null;

            double worldWidth = _MapWorldXYBounds.Width;            //地图横向跨度
            double worldHeight = _MapWorldXYBounds.Width;           //地图纵向跨度
            var mapImagePixelWidth =(int) _MapImageWidthPixels * ZoomLevel;        //地图全图图片像素宽度
            var mapImagePixelHeight = (int)_MapImageHeightPixels * ZoomLevel;      //地图全图图片像素高度


            double pixelsPerWorldUnitX = mapImagePixelWidth / worldWidth;                                 //横向每个世界单位对应的像素数
            double pixelsPerWorldUnitY = mapImagePixelHeight / worldHeight;                               //纵向每个世界单位对应的像素数

            //为了画下所有的图形，比例使用较小的值; 这样地图填充区域，是一个小于或等于视图大小的矩形
            var pixelPerWorldUnit = pixelsPerWorldUnitX>pixelsPerWorldUnitY?pixelsPerWorldUnitY: pixelsPerWorldUnitX;//转换比例
            PixelsPerWorldUnit = pixelPerWorldUnit;
            //var mapFilledPixWidth = pixelPerWorldUnit * (worldRect.BottomRight.WorldX - worldRect.TopLeft.WorldX);//地图部分像素宽度
            //var mapFilledPixHeight = pixelPerWorldUnit * (worldRect.TopLeft.WorldY - worldRect.BottomRight.WorldY);//地图部分高度

            return new PixelCoordinateRectangle(
                new PixelCoordinatePoint(0,0),
                new PixelCoordinatePoint((int)mapImagePixelWidth, (int)mapImagePixelHeight)
            ); 
        }
        /// <summary>
        /// 世界坐标转换成地图片的像素坐标
        /// </summary>
        /// <param name="worldPoint"></param>
        /// <param name="viewportOffset"></param>
        /// <returns></returns>
        public PixelCoordinatePoint MapWorldXYToMapImagePixel(WorldCoordinatePoint worldPoint, Point? viewportOffset = null)
        {

            if (_MapWorldXYBounds == null || MapImagePixelXYBound == null)
                return null;
            int offsetX = viewportOffset?.X ?? 0;
            int offsetY = viewportOffset?.Y ?? 0;

            int pixelX = (int)(worldPoint.WorldX * PixelsPerWorldUnit) + offsetX;
            int pixelY = (int)(worldPoint.WorldY * PixelsPerWorldUnit) + offsetY;

            return new PixelCoordinatePoint(pixelX, pixelY);
        }

        public bool ZoomIn()
        {
           return  ZoomAtViewCenter(ZoomLevel*0.5f);
        }
        public bool ZoomOut()
        {
           return ZoomAtViewCenter(ZoomLevel * 1.5f);
        }

        /// <summary>
        /// 以视图为中心缩放
        /// </summary>
        /// <param name="zoomLevel"></param>
        /// <returns></returns>
        public bool ZoomAtViewCenter(float zoomLevel)
        {
            if(zoomLevel<1||zoomLevel>10)
            {
                return false;
            }
            var preZoomLevel = ZoomLevel;
            _MapImageWidthPixels =(int)( _MapImageWidthPixels * zoomLevel / preZoomLevel);
            _MapImageHeightPixels = (int)(_MapImageHeightPixels * zoomLevel / preZoomLevel);


            double pixelsPerWorldUnitX = _MapImageWidthPixels / _MapWorldXYBounds.Width;                                 //横向每个世界单位对应的像素数
            double pixelsPerWorldUnitY = _MapImageHeightPixels / _MapWorldXYBounds.Height;                               //纵向每个世界单位对应的像素数
                                                                                                                         //为了画下所有的图形，比例使用较小的值; 这样地图填充区域，是一个小于或等于视图大小的矩形
            var pixelPerWorldUnit = pixelsPerWorldUnitX > pixelsPerWorldUnitY ? pixelsPerWorldUnitY : pixelsPerWorldUnitX;//转换比例
            PixelsPerWorldUnit = pixelPerWorldUnit;


            ViewCenterInMapImageXY = MapWorldXYToMapImagePixel(_ViewCenterInMapXY);//视图中心的世界坐标不变，转换成地图图片上的像素坐标

            _ViewBoundInMapImage = new PixelCoordinateRectangle(new PixelCoordinatePoint(_ViewCenterInMapImageXY.PixelX - _ViewPixelWidth / 2, _ViewCenterInMapImageXY.PixelY - _viewPixelHeight / 2),
                new PixelCoordinatePoint(_ViewCenterInMapImageXY.PixelX + _ViewPixelWidth / 2, _ViewCenterInMapImageXY.PixelY + _viewPixelHeight / 2));

            ZoomLevel = zoomLevel;

            return true;
        }
        //0--------------------------------------------------------------------------------------




        //public CoordinateRectangle GetZoomedRectangle(double zoomFactor)
        //{
        //    var center = ViewCenterWorldXY;

        //    double halfWidth = (BottomRight.X - TopLeft.X) / 2 / zoomFactor;
        //    double halfHeight = (TopLeft.Y - BottomRight.Y) / 2 / zoomFactor;

        //    return new CoordinateRectangle(
        //        new Coordinate(center.X - halfWidth, center.Y + halfHeight),
        //        new Coordinate(center.X + halfWidth, center.Y - halfHeight)
        //    );
        //}









    }

}
