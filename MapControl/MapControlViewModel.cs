using Microsoft.Graphics.Canvas;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI.WebUI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MapControl
{
    public class MapControlViewModel
    {
        private PictureBox _PictureBox = null;
        private MapLogger _Logger = null;
        private Map _Map = null;

        /// <summary>
        /// 更新Bitmap图片的线程
        /// </summary>
        private Thread _UpdateBitMapThread = null;
        private bool _TryStopThread = false;
        /// <summary>
        /// 绘图数据数组
        /// </summary>
        private Myrect[] _GeometryBufferArray = new Myrect[10000];             
        /// <summary>
        /// 当前绘图数据数组大小
        /// </summary>
        private int _CurDrawGeoSize = 0;                                        
        private Mutex _bitmapMutex = new Mutex();
        /// <summary>
        /// 已绘制帧数
        /// </summary>
        private int _DrawedFrame = 1;                                           
        /// <summary>
        /// 当前绘制帧率
        /// </summary>
        private int _CurFPS = 0;  
        public int FPS
        {
            get
            {
                return _CurFPS;
            }
        }
        public  MapControlViewModel(PictureBox pictureBox)
        {
            _PictureBox = pictureBox;
            var file = Path.Combine(Environment.CurrentDirectory, "MapControl.log");  //在当前工作目录下创建日志文件
            _Logger = new MapLogger(file);
            BindPictureBoxEvent();
            _Map = new Map();
            FillDrawData(_PictureBox.Width, _PictureBox.Height, 0.4f, 0.25f, 0.125f, 100000, out _CurDrawGeoSize, EnumGeometryType.Point, true);

            _Map.ZoomToPixelBound(_PictureBox.Width, _PictureBox.Height);
            StartDrawMap();
        }

        #region 鼠标事件
        private bool _isMouseDown = false;
        private bool _dragging = false;
        private Point _startPoint = new Point(0, 0);
        private Point _endPoint = new Point(0, 0);
        private void BindPictureBoxEvent()
        {
            _PictureBox.MouseDown += PictureBox1_MouseDown;
            _PictureBox.MouseMove += PictureBox1_MouseMove;
            _PictureBox.MouseUp += PictureBox1_MouseUp;
            _PictureBox.SizeChanged += PictureBox_SizeChanged;
            _PictureBox.MouseWheel += _PictureBox_MouseWheel;
        }

        private void _PictureBox_MouseWheel(object? sender, MouseEventArgs e)
        {
           if(e.Delta>0)
            {
                _Map.ZoomIn();
            }
            else
            {
                _Map.ZoomOut();
            }
        }

        private void PictureBox_SizeChanged(object? sender, EventArgs e)
        {
            //_Map._MapImageWidthPixels = _PictureBox.Width;
            //_Map._MapImageHeightPixels = _PictureBox.Height;
            _Map.ZoomToPixelBound(_PictureBox.Width, _PictureBox.Height);
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isMouseDown = true;
                _startPoint = e.Location;
                _PictureBox.Cursor = Cursors.Hand;
            }
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
                _dragging = true;
            if (_dragging)
            {
                Point change = new Point(e.X - _startPoint.X, e.Y - _startPoint.Y);
            }
        }
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
            _isMouseDown = false;
            _PictureBox.Cursor = Cursors.Default;
        }
        #endregion

        private void StartDrawMap()
        {

            _Logger.Log(MapLogLevel.Info, "图片控件加载完成");
            _UpdateBitMapThread = new Thread(UpdateBitMapWork);
            _UpdateBitMapThread.Priority = ThreadPriority.Normal;
            _UpdateBitMapThread.IsBackground = true;
            _UpdateBitMapThread.Start();
        }
        /// <summary>
        /// 更新Bitmap图片
        /// </summary>
        /// <param name="param"></param>
        private void UpdateBitMapWork()
        {
            var loopStartTime = DateTime.Now;
            while (!_TryStopThread)
            {
                if (_Map.Points.Length < 1)
                    continue;
                DisplayPixelData( _Map.Points.Length, out double _curDrawTime, out double _curShowTime);
                _DrawedFrame++;
                var loopTime = (DateTime.Now - loopStartTime).Seconds;
                _CurFPS = loopTime > 0 ? ((_DrawedFrame / loopTime)) : 0;
                _Logger.Log(MapLogLevel.Info, $"{_CurDrawGeoSize} ，  离屏绘制耗时: " + _curDrawTime + "ms  整体绘制耗时: " + _curShowTime + "ms 帧率" + _CurFPS.ToString() + " fps");
                //Thread.Sleep(10);
            }
        }
        /// <summary>
        ///  将颜色像素数组在在PictureBox中显示位图
        /// </summary>
        /// <param name="control">pictureBox控件</param>
        /// <param name="pixelData">颜色像素数组</param>
        /// <param name="bitmapWidth">图片的宽度</param>
        /// <param name="bitmapHeight">图片高度</param>
        private void DisplayPixelData(int drawSize, out double drawTime, out double showTime, float dpi = 96)
        {
            var startTime = DateTime.Now;
            drawTime = 0;
            showTime = 0;
            

            //var _MapImageBound = _Map.MapImagePixelXYBound;
            var mapImageBound = new Rectangle(_Map.MapImagePixelXYBound.TopLeft.PixelX, _Map.MapImagePixelXYBound.TopLeft.PixelY, (int)_Map.MapImagePixelXYBound.Width, (int)_Map.MapImagePixelXYBound.Height);
            var viewImageBound = new Rectangle(_Map.ViewBoundInMapImage.TopLeft.PixelX, _Map.ViewBoundInMapImage.TopLeft.PixelY, (int)_Map.ViewBoundInMapImage.Width, (int)_Map.ViewBoundInMapImage.Height);
            var cutImageBound = Rectangle.Intersect(mapImageBound, viewImageBound);

            if (mapImageBound==null || mapImageBound.Width<=0 || mapImageBound.Height<=0)
            {
                return;
            }   
            var width =(int) mapImageBound.Width;
            var height = (int)mapImageBound.Height;
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);                //创建位图对象
            var device = CanvasDevice.GetSharedDevice();
            using (var offscreen = new CanvasRenderTarget(device, width, height, dpi))         /*使用离离对象在屏幕外绘制图形*/
            {
                using (var ds = offscreen.CreateDrawingSession())
                {
                    ds.Clear(Windows.UI.Color.FromArgb(100, 255, 255, 255));//清除画布
                    //_Map.UpdateMapBound();
                    for (int i = 0; i < drawSize; i++)
                    {
                        if (_Map.Rectangles[i] == null)
                            continue;
                        switch (EnumGeometryType.fillrect)
                        {
                        case EnumGeometryType.Point:
                            var p = _Map.MapWorldXYToMapImagePixel(_Map.Points[i]);
                            ds.DrawCircle(p.PixelX, p.PixelY, 10, Windows.UI.Color.FromArgb(255, 123, 45, 23));
                            break;
                        case EnumGeometryType.line:
                            ds.DrawLine(_GeometryBufferArray[i].X, _GeometryBufferArray[i].Y,
                                _GeometryBufferArray[i].X + _GeometryBufferArray[i].Width, _GeometryBufferArray[i].Y + _GeometryBufferArray[i].Height, _GeometryBufferArray[i].Color);
                            break;
                        case EnumGeometryType.rect:
                            ds.DrawRectangle(_GeometryBufferArray[i].X, _GeometryBufferArray[i].Y, _GeometryBufferArray[i].Width, _GeometryBufferArray[i].Height, _GeometryBufferArray[i].Color);
                            break;
                        case EnumGeometryType.fillrect:
                            var p1 = _Map.MapWorldXYToMapImagePixel(_Map.Rectangles[i].TopLeft);
                            var p2 = _Map.MapWorldXYToMapImagePixel(_Map.Rectangles[i].BottomRight);
                            ds.FillRectangle((float)p1.PixelX, (float)p1.PixelY, p2.PixelX-p1.PixelX,p2.PixelY-p1.PixelY, Windows.UI.Color.FromArgb(255, 123, 45, 23));
                            break;
                        case EnumGeometryType.circle:
                            ds.DrawCircle(_GeometryBufferArray[i].X, _GeometryBufferArray[i].Y, 20, _GeometryBufferArray[i].Color);
                            break;
                        case EnumGeometryType.text:
                            ds.DrawText("立镖机器人", _GeometryBufferArray[i].X, _GeometryBufferArray[i].Y, _GeometryBufferArray[i].Color);
                            break;
                        default:
                            break;
                        }
                    }
                }
                //drawTime = (DateTime.Now - startTime).TotalMilliseconds;
                var pixelData = offscreen.GetPixelBytes();                           //从离屏对象中获取像素数组
                
                var bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),    /*将像素数组复制到位图对象中*/
                                                                                     ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
               
                System.Runtime.InteropServices.Marshal.Copy(pixelData, 0, bmpData.Scan0,pixelData.Length);
                bitmap.UnlockBits(bmpData);
                Bitmap img = bitmap;
                if (cutImageBound.Width>0 && cutImageBound.Height>0)
                {
                   img = bitmap.Clone(cutImageBound, PixelFormat.Format32bppRgb);
                }
                
                _PictureBox.Invoke(() =>
                {
                    _PictureBox.Image = img;
                });

            }
            var endTime = DateTime.Now;
            showTime = (endTime - startTime).TotalMilliseconds;

         }

        #region 测试数据
        /// <summary>
        /// 生成世界坐标矩形 
        /// </summary>
        /// <param name="drawrect"></param>
        private void FillDrawData(int width,int height, float geoWidth, float geoHeight, float gap, int MaxSpriteLimit, out int drawSize, EnumGeometryType geoType = EnumGeometryType.Point, bool isTiled = false)
        {
            var timestamp = DateTime.Now;
            drawSize = MaxSpriteLimit;
            var random = new Random((int)timestamp.Ticks);

            //填充10万个矩形 ,X =0.25~50,Y=0.18!13
            for (int i = 0; i < MaxSpriteLimit; i++)
            {
                var startx = random.Next(1, 200)*0.25;
                var startY = random.Next(1, 100) * 0.13;
                var topLeft = new WorldCoordinatePoint(startx, startY + geoHeight);
                var bottomRight = new WorldCoordinatePoint(startx + geoWidth, startY );

                _Map.Rectangles[i] = new WorldCoordinateRectangle(topLeft, bottomRight);
                //_GeometryBufferArray[i] = new Myrect(startx, starty, endx - startx, endy - starty, color);
            }
            var rects = CalculateRectangles(200f, 100f, 0.4f, 0.3f, 0.1f);
            var size = MaxSpriteLimit>=rects.Count?rects.Count:MaxSpriteLimit;
            for (int i = 0; i < size; i++)
            {
                var topLeftX = rects[i].X - rects[i].Width/2;
                var topLeftY = rects[i].Y + rects[i].Height / 2;
                var bottomRightX = rects[i].X + rects[i].Width / 2;
                var bottomRightY = rects[i].Y - rects[i].Height / 2;
                _Map.Rectangles[i] = new WorldCoordinateRectangle(new WorldCoordinatePoint(topLeftX,topLeftY),new WorldCoordinatePoint(bottomRightX,bottomRightY));
            }
                //if (geoType != EnumGeometryType.rect)
                //{
                //    if (isTiled)
                //    {
                //        var reslut = CalculateRectangles(width,height, geoWidth, geoHeight, gap);
                //        drawSize = reslut.Count;
                //        for (int i = 0; i < reslut.Count; i++)
                //        {
                //            _GeometryBufferArray[i] = reslut[i];
                //            var mu = (float)i / MaxSpriteLimit;
                //            var color = GradientColor(mu);
                //            _GeometryBufferArray[i].Color = color;
                //        }
                //    }
                //    else
                //    {
                //        for (int i = 0; i < MaxSpriteLimit; i++)
                //        {
                //            var l = random.Next(10, 100);
                //            var xmax = width - l <= 0 ? 1 : (int)width - l;
                //            var yMax =height - l <= 0 ? 1 : (int)height - l;
                //            var x = random.Next(0.25, xmax);
                //            var y = random.Next(0.18, yMax);
                //            var mu = (float)i / MaxSpriteLimit;
                //            var color = GradientColor(mu);
                //            _GeometryBufferArray[i] = new Myrect(x, y, geoWidth, geoHeight, color);

                //        }
                //        drawSize = MaxSpriteLimit;
                //    }
                //}
                //else
                //{

                //    for (int i = 0; i < MaxSpriteLimit; i++)
                //    {
                //        var startx = (float)(width * random.NextDouble());
                //        var starty = (float)(height * random.NextDouble());
                //        var endx = (float)(width * random.NextDouble());
                //        var endy = (float)(height * random.NextDouble());
                //        var mu = (float)i / MaxSpriteLimit;
                //        var color = GradientColor(mu);

                //        //_Map.AddPoint(new WorldCoordinatePoint3D(startx, starty, 0));
                //        _Map.Points[i] = new WorldCoordinatePoint3D(startx, starty, 0);
                //        //_GeometryBufferArray[i] = new Myrect(startx, starty, endx - startx, endy - starty, color);
                //    }

                //}
            }

        /// <summary>
        /// 指定矩形的宽高，计算可以放置的矩形数量，然后平铺窗口  
        /// 单位：像素
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        /// <param name="rectWidth"></param>
        /// <param name="rectHeight"></param>
        /// <param name="gap">矩形间隔</param>
        /// <returns></returns>
        public static List<Myrect> CalculateRectangles(float windowWidth, float windowHeight, float rectWidth, float rectHeight, float gap = 1)
        {
            List<Myrect> rectangles = new List<Myrect>();

            // 计算可以放置的行数和列数
            int cols =(int)( (windowWidth) / (rectWidth + gap));
            int rows =(int)( (windowHeight) / (rectHeight + gap));

            // 计算每个矩形的中心坐标
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var  centerX = j * (rectWidth + gap) + rectWidth / 2.0f;
                    var  centerY = i * (rectHeight + gap) + rectHeight / 2.0f;

                    rectangles.Add(new Myrect(centerX, centerY, rectWidth, rectHeight));
                }
            }
            return rectangles;
        }
        private static Windows.UI.Color GradientColor(float mu)
        {
            byte c = (byte)((Math.Sin(mu * Math.PI * 2) + 1) * 127.5);

            return Windows.UI.Color.FromArgb(255, (byte)(255 - c), c, 220);
        }

        #endregion

        #region 数据
        /// <summary>
        /// 图形类别
        /// </summary>
        public enum EnumGeometryType
        {
            Point,
            line,
            rect,
            fillrect,
            circle,
            text,
            radom,
        }
        public struct Myrect
        {
            public EnumGeometryType GeometryType = EnumGeometryType.line;
            public Myrect(float x, float y, float width, float height, Windows.UI.Color color)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Color = color;
            }
            public Myrect(float x, float y, float width, float height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Color = Windows.UI.Color.FromArgb(255, (byte)(255 - 123), 125, 220);
            }
            public float X;
            public float Y;
            public float Width;
            public float Height;
            public Windows.UI.Color Color;

        }
        #endregion

    }
}
