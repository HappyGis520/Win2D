using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.Media.Protection.PlayReady;
using Microsoft.UI;
using System.Threading;
using System.Diagnostics;
using Windows.Devices.HumanInterfaceDevice;
using System.Security.Cryptography.X509Certificates;
using Windows.UI.ViewManagement;
using System.Runtime.CompilerServices;
using Windows.Media.Capture;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Windows.UI.Core;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App2222
{
    /// <summary>
    /// ͼ�����
    /// </summary>
    public enum EnumGeometryType
    {
        line,
        rect,
        fillrect,
        circle,
        text,
    }

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        Thread tWorker = null;
        /// <summary>
        /// ѭ���߳��Ƿ�����
        /// </summary>
        bool _workerThreadRun = false;
        bool _dataUpdate = false;//�����Ƿ����
        bool _isLoop = true;//�Ƿ�ѭ��
        bool _tryStopLoop = false;//����ֹͣѭ��
        /// <summary>
        /// �Ƿ�ƽ��
        /// </summary>
        bool _isTiled = false;
        /// <summary>
        /// ѭ���ȴ�ʱ�䣬����
        /// </summary>
        int _Sleepscend = 1;//
        /// <summary>
        /// ѭ�����ͣ�trueΪ�̶����ͣ�falseΪ�������
        /// </summary>
        bool _LoopTypeFix = true;
        /// <summary>
        /// ��ǰ���Ƶ�ͼ������
        /// </summary>
        EnumGeometryType _CurDrawGeoType = EnumGeometryType.line;
        //������εĿ�Ⱥ͸߶ȱ���
        private int _RectWidth = 10;
        private int _RectHeight = 10;
        Myrect[] _RectArray = new Myrect[100000];
        /// <summary>
        /// �����������
        /// </summary>
        private int _MaxSpriteLimit = 100000;
        /// <summary>
        /// �����������
        /// </summary>
        private int MaxSpriteLimit
        {
            get
            {
                return _MaxSpriteLimit;
            }
            set
            {
                Interlocked.Exchange(ref _MaxSpriteLimit, value);
            }
        }
        //��־�������
        private Logger _Logger = null;
        public MainWindow()
        {
            this.InitializeComponent();
            //��ʼ����־�������,��־�ļ������ڵ�ǰĿ¼�µ�Log�ļ�����
            var dir = "C:\\WinUi3Log";
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
            _Logger = new Logger( $"{dir}\\Log.txt");
            this.Closed += MainWindow_Closed;
            _Logger.Log(LogLevel.Info, "---------------------------------���ڼ��س�ʼ�����-------------------------------------");
        }
        #region �����¼�

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            _Logger.Log(LogLevel.Info, "---------------------------------���ڹر�-------------------------------------");
            _workerThreadRun = false;
            if (null != tWorker)
            {
                if (false == tWorker.Join(1000))
                    tWorker.Abort();
                tWorker = null;
            }
            try
            {
                //������־�������
                if (null != _Logger)
                {
                    _Logger.Dispose();
                    _Logger = null;
                }
            }
            catch
            {

            }
        }
        private void myButton_Click(object sender, RoutedEventArgs e)
        {

            if (_isLoop)
            {

                StartLoop();
            }
            else
            {
                DrawRect drawBound = ReadParamFromUI();
                FillDrawData(drawBound, MaxSpriteLimit, out int size);
                _dataUpdate = true;
                //����TxtLimit��ֵ
                txtTotalGeometry.Text = size.ToString();
                this.canvas.Invalidate();
            }
        }

        private DrawRect ReadParamFromUI()
        {
            this.MaxSpriteLimit = 100;
            if (int.TryParse(txtLimit.Text, out int limit))
            {
                this.MaxSpriteLimit = limit > _RectArray.Length ? _RectArray.Length : limit;
            }
            var w = (int)canvas.ActualWidth;
            var h = (int)canvas.ActualHeight;
            DrawRect drawBound = new DrawRect(w,h);
            _RectWidth = 10;
            if (int.TryParse(txtRectWidth.Text, out int width))
            {
                _RectWidth = width;
            }
            _RectHeight = 10;
            if (int.TryParse(txtRectHeight.Text, out int height))
            {
                _RectHeight = height;
            }
            _isLoop = chkLoop.IsChecked.Value;
            _isTiled = chkTiled.IsChecked.Value;
            _Sleepscend = 1;
            if (int.TryParse(txtsleepScend.Text, out int sleep))
            {
                _Sleepscend = sleep;
            }
            _LoopTypeFix = chkTypeFix.IsChecked.Value;
            return drawBound;
        }
        private DrawRect ReadParamFromUITask()
        {
            DrawRect drawBound = null;
            this.DispatcherQueue.TryEnqueue(() =>
            {
                this.MaxSpriteLimit = 100;
                if (int.TryParse(txtLimit.Text, out int limit))
                {
                    this.MaxSpriteLimit = limit > _RectArray.Length ? _RectArray.Length : limit;
                }
                var w = (int)canvas.ActualWidth;
                var h = (int)canvas.ActualHeight;
                drawBound = new DrawRect(w, h);
                _RectWidth = 10;
                if (int.TryParse(txtRectWidth.Text, out int width))
                {
                    _RectWidth = width;
                }
                _RectHeight = 10;
                if (int.TryParse(txtRectHeight.Text, out int height))
                {
                    _RectHeight = height;
                }
                _isLoop = chkLoop.IsChecked.Value;
                _isTiled = chkTiled.IsChecked.Value;
                _Sleepscend = 1;
                if (int.TryParse(txtsleepScend.Text, out int sleep))
                {
                    _Sleepscend = sleep;
                }
                _LoopTypeFix = chkTypeFix.IsChecked.Value;
            });
            return drawBound;
        }
        private void radLine_Checked(object sender, RoutedEventArgs e)
        {
            _CurDrawGeoType = EnumGeometryType.line;
        }
        private void radRect_Checked(object sender, RoutedEventArgs e)
        {
            _CurDrawGeoType = EnumGeometryType.rect;
        }
        private void radFillRect_Checked(object sender, RoutedEventArgs e)
        {
            _CurDrawGeoType = EnumGeometryType.fillrect;

        }
        private void radCircle_Checked(object sender, RoutedEventArgs e)
        {
            _CurDrawGeoType = EnumGeometryType.circle;
        }
        private void radText_Checked(object sender, RoutedEventArgs e)
        {
            _CurDrawGeoType = EnumGeometryType.text;
        }
        private void canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            if (true == _workerThreadRun || (!_isLoop && _dataUpdate))
            {
                switch (_CurDrawGeoType)
                {
                case EnumGeometryType.line:
                    DrawLine(sender, args.DrawingSession);
                    break;
                case EnumGeometryType.rect:
                    DrawRectangle(sender, args.DrawingSession);
                    break;
                case EnumGeometryType.fillrect:
                    DrawFillRectangle(sender, args.DrawingSession);
                    break;
                case EnumGeometryType.circle:
                    DrawCircle(sender, args.DrawingSession);
                    break;
                case EnumGeometryType.text:
                    DrawText(sender, args.DrawingSession);
                    break;
                default:
                    break;
                }
                _dataUpdate = false;
            }
        }
        #endregion
        private void StartLoop()
        {

            if (null == tWorker)
            {
                DrawRect drawBound = ReadParamFromUI();
                _tryStopLoop = false;
                //����һ�����������߳�
                tWorker = new Thread(new ParameterizedThreadStart(Worker));
                tWorker.IsBackground = true;
                tWorker.Name = "Worker";
                tWorker.Start(new DrawRect((int)this.canvas.ActualWidth, (int)this.canvas.ActualHeight));
            }
            else
            {
                _workerThreadRun = false;
                _tryStopLoop = true;
                if (null != tWorker)
                {
                    if (false == tWorker.Join(10000))
                    tWorker = null;
                }
            }
        }

        /// <summary>
        /// ���ɻ���ͼ�ε�����
        /// </summary>
        /// <param name="drawrect"></param>
        private void FillDrawData(DrawRect drawrect,int MaxSpriteLimit,out int drawSize)
        {
            var timestamp = DateTime.Now;
            drawSize = MaxSpriteLimit;
            var random = new Random((int)timestamp.Ticks);
            if (_CurDrawGeoType != EnumGeometryType.line)
            {
                if (_isTiled)
                {
                    var reslut = CalculateRectangles(drawrect.Width, drawrect.Height, _RectWidth,_RectHeight, 1);
                    drawSize = reslut.Count;
                    for (int i = 0; i < reslut.Count; i++)
                    {
                        _RectArray[i] = reslut[i];
                        var mu = (float)i / MaxSpriteLimit;
                        var color = GradientColor(mu);
                        _RectArray[i].Color = color;
                    }
                }
                else
                {
                    for (int i = 0; i < MaxSpriteLimit; i++)
                    {
                        var l = random.Next(10, 100);
                        var xmax = drawrect.Width - l <= 0 ? 1 : (int)drawrect.Width - l;
                        var yMax = drawrect.Height - l <= 0 ? 1 : (int)drawrect.Height - l;
                        var x = random.Next(0, xmax);
                        var y = random.Next(0, yMax);
                        var mu = (float)i / MaxSpriteLimit;
                        var color = GradientColor(mu);
                        _RectArray[i] = new Myrect(x, y, _RectWidth, _RectHeight, color);

                    }
                    drawSize = MaxSpriteLimit;
                }
            }
            else
            {

                    for (int i = 0; i < MaxSpriteLimit; i++)
                    {
                        var startx = (float)(drawrect.Width * random.NextDouble());
                        var starty = (float)(drawrect.Height * random.NextDouble());
                        var endx = (float)(drawrect.Width * random.NextDouble());
                        var endy = (float)(drawrect.Height * random.NextDouble());
                        var mu = (float)i / MaxSpriteLimit;
                        var color = GradientColor(mu);
                        _RectArray[i] = new Myrect(startx, starty, endx - startx, endy - starty, color);
                    }
                
            }
            drawrect = null;
        }
        private void DrawLine(CanvasControl sender, CanvasDrawingSession ds)
        {
            var startTime = DateTime.Now;
            int.TryParse(txtTotalGeometry.Text, out int drawSize);
            for (int i = 0; i < drawSize; i++)
            {
                 ds.DrawLine(_RectArray[i].X, _RectArray[i].Y, _RectArray[i].X+_RectArray[i].Width, _RectArray[i].Y+_RectArray[i].Height, _RectArray[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            //��ʱ�������־�ļ���
            _Logger.Log(LogLevel.Info, $"{drawSize}�ߣ�         ��ʱ: " + timspan.TotalMilliseconds + "ms");
        }
        private void DrawFillRectangle(CanvasControl sender, CanvasDrawingSession ds)
        {
            int.TryParse(txtTotalGeometry.Text, out int drawSize);
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                    CanvasSolidColorBrush _brush = new CanvasSolidColorBrush(sender, _RectArray[i].Color);
                    ds.FillRectangle(_RectArray[i].X, _RectArray[i].Y, _RectArray[i].Width, _RectArray[i].Height, _RectArray[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            //��ʱ�������־�ļ���
            _Logger.Log(LogLevel.Info, $"{drawSize}���Σ�        ��ʱ: " + timspan.TotalMilliseconds + "ms");
        }
        private void DrawRectangle(CanvasControl sender, CanvasDrawingSession ds)
        {
            int.TryParse(txtTotalGeometry.Text, out int drawSize);
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                    ds.DrawRectangle(_RectArray[i].X, _RectArray[i].Y, _RectArray[i].Width, _RectArray[i].Height, _RectArray[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            //��ʱ�������־�ļ���
            _Logger.Log(LogLevel.Info, $"{drawSize}�߿�        ��ʱ: " + timspan.TotalMilliseconds + "ms");
        }
        private void DrawCircle(CanvasControl sender, CanvasDrawingSession ds)
        {
            int.TryParse(txtTotalGeometry.Text, out int drawSize);
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                ds.DrawCircle(_RectArray[i].X, _RectArray[i].Y, 20, _RectArray[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            //��ʱ�������־�ļ���
            _Logger.Log(LogLevel.Info, $"{drawSize}Բ��         ��ʱ: " + timspan.TotalMilliseconds + "ms");
        }
        private void DrawText(CanvasControl sender, CanvasDrawingSession ds)
        {
            int.TryParse(txtTotalGeometry.Text, out int drawSize);
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                ds.DrawText("���ڻ�����",_RectArray[i].X, _RectArray[i].Y, _RectArray[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            //��ʱ�������־�ļ���
            _Logger.Log(LogLevel.Info, $"{drawSize}���֣�        ��ʱ: " + timspan.TotalMilliseconds + "ms");
        }
        private void Worker(object param )
        {
            var drawrect =  param as DrawRect;

            _workerThreadRun = true;
            int loopIndex = 0;
            while (true == _workerThreadRun && !_tryStopLoop)
            {
                if (_isLoop && !_LoopTypeFix)
                {///������ǻ���ָ�������ͣ�������ͼ����
                    var geoType = loopIndex % 5;
                    switch (geoType)
                    {
                    case 0:
                        _CurDrawGeoType = EnumGeometryType.line;
                        break;
                    case 1:
                        _CurDrawGeoType = EnumGeometryType.rect;
                        break;
                    case 2:
                        _CurDrawGeoType = EnumGeometryType.fillrect;
                        break;
                    case 3:
                        _CurDrawGeoType = EnumGeometryType.circle;
                        break;
                    case 4:
                        _CurDrawGeoType = EnumGeometryType.text;
                        break;
                    default:
                        _CurDrawGeoType = EnumGeometryType.line;
                        break;
                    }
                    loopIndex++;
                }
                var startTime = DateTime.Now;
                FillDrawData(drawrect,MaxSpriteLimit,out int size);
                //�첽����TxtLimited��Textֵ
                this.DispatcherQueue.TryEnqueue(() =>
                {
                    txtTotalGeometry.Text = size.ToString();
                });
                this.canvas.Invalidate();
                Thread.Sleep(_Sleepscend);
            }

        }
        private static Color GradientColor(float mu)
        {
            byte c = (byte)((Math.Sin(mu * Math.PI * 2) + 1) * 127.5);

            return Color.FromArgb(255, (byte)(255 - c), c, 220);
        }
        /// <summary>
        /// ָ�������ľ���ƽ�̴���
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        /// <param name="numRectangles"></param>
        /// <param name="gap"></param>
        /// <param name="isVertical"></param>
        /// <returns></returns>
        public static List<Myrect> CalculateRectangles(int windowWidth, int windowHeight, int numRectangles, int gap, bool isVertical)
        {
            List<Myrect> rectangles = new List<Myrect>();

            // ��ʼ���� rows �� cols Ϊ numRectangles ��ƽ����
            int rows = (int)Math.Sqrt(numRectangles);
            int cols = numRectangles / rows;

            // ���ݴ��ڵĿ�߱ȵ��� rows �� cols
            float aspectRatio = (float)windowWidth / windowHeight;
            if (isVertical)
            {
                // ��ֱ�������ȣ�������������������
                while (rows * cols < numRectangles && aspectRatio < 1.0)
                {
                    rows++;
                    cols = numRectangles / rows;
                }
            }
            else
            {
                // ˮƽ�������ȣ�������������������
                while (rows * cols < numRectangles && aspectRatio > 1.0)
                {
                    cols++;
                    rows = numRectangles / cols;
                }
            }

            // ����ÿ�����εĿ�Ⱥ͸߶ȣ��������
            int rectWidth = (windowWidth - (cols - 1) * gap) / cols;
            int rectHeight = (windowHeight - (rows - 1) * gap) / rows;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // ����ÿ�����ε���������
                    int centerX = j * rectWidth + rectWidth / 2;
                    int centerY = i * rectHeight + rectHeight / 2;

                    rectangles.Add(new Myrect( centerX,  centerY, rectWidth,  rectHeight));
                }
            }

            return rectangles;
        }
        /// <summary>
        /// ָ�����εĿ�ߣ�������Է��õľ���������Ȼ��ƽ�̴���  
        /// ��λ������
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        /// <param name="rectWidth"></param>
        /// <param name="rectHeight"></param>
        /// <param name="gap">���μ��</param>
        /// <returns></returns>
        public static List<Myrect> CalculateRectangles(int windowWidth, int windowHeight, int rectWidth, int rectHeight, int gap=1)
        {
            List<Myrect> rectangles = new List<Myrect>();

            // ������Է��õ�����������
            int cols = (windowWidth + gap) / (rectWidth + gap);
            int rows = (windowHeight + gap) / (rectHeight + gap);

            // ����ÿ�����ε���������
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int centerX = j * (rectWidth + gap) + rectWidth / 2;
                    int centerY = i * (rectHeight + gap) + rectHeight / 2;

                    rectangles.Add(new Myrect( centerX, centerY,rectWidth, rectHeight ));
                }
            }
            return rectangles;
        }

        private void chkTiled_Checked(object sender, RoutedEventArgs e)
        {
            _isTiled = chkTiled.IsChecked.Value;
        }

        private void chkTiled_Unchecked(object sender, RoutedEventArgs e)
        {
            _isTiled = chkTiled.IsChecked.Value;
        }
    }


    public class DrawRect
    {
        //���캯��
        public DrawRect(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public int Width;
        public int Height;
    }
    public struct  Myrect{
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
}
