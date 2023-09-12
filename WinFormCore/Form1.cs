using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration.Internal;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Windows.ApplicationModel.Background;
using Windows.Graphics.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FormCore
{
    public partial class Form1 : Form
    {
        private Mutex mutex = new Mutex();
        private Thread _RefreshThread = null;
        public Form1()
        {
            InitializeComponent();
            var dir = "C:\\WinUi3Log";
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
            _Logger = new Logger($"{dir}\\CorFormLog.txt");
            this.Load += Form1_Load;
            this.FormClosed += Form1_FormClosed;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _Logger.Log(LogLevel.Info, "---------------------------------���ڹر�-------------------------------------");
            _workerThreadRun = false;
            _TryStopLoop = true;
            if (null != _WorkerThread)
            {

                //if (false == _WorkerThread.Join(1000))
                //    _WorkerThread.Abort();
                //_WorkerThread = null;
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chkLoop.Checked = _isLoop;
            chkDrawLine.Checked = true;
            CurDrawGeoType = EnumGeometryType.line;
            radioButton1.Checked = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            _RefreshThread = new Thread(BackgroundWork);
            _RefreshThread.Priority = ThreadPriority.BelowNormal;
            _RefreshThread.IsBackground = true;
            _RefreshThread.Start();
            mapborder.MouseWheel += Mapborder_MouseWheel;
        }

        private void Mapborder_MouseWheel(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BackgroundWork()
        {
            while (true)
            {
                mutex.WaitOne(); // ������
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        if (false)
                            pictureBox1.Refresh();
                        txtTotalGeometry.Text = _CurDrawGeoSize.ToString();
                        txtfps.Text = _CurFPS.ToString();
                        txtShowTime.Text = _curShowTime.ToString();
                        txtDrawTime.Text = _curDrawTime.ToString();

                    }));
                }
                finally
                {
                    mutex.ReleaseMutex(); // �ͷ���
                }
                Thread.Sleep(100);
            }
        }

        #region MyRegion
        bool _isLoop = false;//�Ƿ�ѭ��
        private void button1_Click(object sender, EventArgs e)
        {

            DrawRect drawBound = ReadParamFromUI();
            bitmap = new Bitmap(drawBound.Width, drawBound.Height);
            pictureBox1.Image = bitmap;
            if (_isLoop)
            {
                StartLoop(pictureBox1.Width, pictureBox1.Height);
            }
            else
            {

                FillDrawData(drawBound, MaxSpriteLimit, out int size);
                txtTotalGeometry.Text = size.ToString();
                DisplayPixelData(bitmap, _GeometryArray, size, drawBound.Width, drawBound.Height, out double drawTime, out double showTime);
                _Logger.Log(LogLevel.Info, $"{size} {CurDrawGeoType.ToString()}��        �������ƺ�ʱ: " + drawTime + "ms");
                //��ʱ�������־�ļ���
                _Logger.Log(LogLevel.Info, $"{size}{CurDrawGeoType.ToString()}��         ������ƺ�ʱ: " + showTime + "ms");
            }
        }
        private void chkDrawLine_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDrawLine.Checked)
            {
                CurDrawGeoType = EnumGeometryType.line;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;
                chkTypeLoop.Checked = false;
            }
        }
        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            _isLoop = chkLoop.Checked;
        }
        private DrawRect ReadParamFromUI()
        {
            this.MaxSpriteLimit = 100;
            if (int.TryParse(txtLimit.Text, out int limit))
            {
                this.MaxSpriteLimit = limit > _GeometryArray.Length ? _GeometryArray.Length : limit;
            }
            var w = (int)pictureBox1.Width;
            var h = (int)pictureBox1.Height;
            DrawRect drawBound = new DrawRect(w, h);

            _DrawRectWidth = 10;
            if (int.TryParse(txtRectWidth.Text, out int width))
            {
                _DrawRectWidth = width;
            }
            _DrawRectHeight = 10;
            if (int.TryParse(txtRectHeight.Text, out int height))
            {
                _DrawRectHeight = height;
            }

            if (chk10W.Checked)
            {//10�������
                var w2 = 333 * _DrawRectWidth + (333 - 1) * _Grap;
                var h2 = 333 * _DrawRectHeight + (333 - 1) * _Grap;
                drawBound = new DrawRect(w2, h2);
            }

            _isLoop = chkLoop.Checked;
            _isTiled = chkTiled.Checked;
            _ThreadSleepTime = 1;
            if (int.TryParse(txtsleepScend.Text, out int sleep))
            {
                _ThreadSleepTime = sleep;
            }
            _typeLoop = chkTypeLoop.Checked;
            _Grap = 1;
            if (int.TryParse(txtbuffer.Text, out int grap))
            {
                _Grap = grap;
            }
            return drawBound;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                CurDrawGeoType = EnumGeometryType.rect;
                chkDrawLine.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;
                chkTypeLoop.Checked = false;
            }

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                CurDrawGeoType = EnumGeometryType.fillrect;
                chkDrawLine.Checked = false;
                checkBox2.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;
                chkTypeLoop.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                CurDrawGeoType = EnumGeometryType.circle;
                chkDrawLine.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                chkTypeLoop.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                CurDrawGeoType = EnumGeometryType.text;
                chkDrawLine.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
                chkTypeLoop.Checked = false;
            }
        }

        private void chkTypeLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTypeLoop.Checked)
            {
                _IsRandomType = true;
                //CurDrawGeoType = EnumGeometryType.radom;
                chkDrawLine.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;

            }

        }
        private void chkTiled_CheckedChanged(object sender, EventArgs e)
        {
            _isTiled = chkTiled.Checked;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }
        #endregion


        #region ҵ���߼�
        Thread _WorkerThread = null;                                    //��ͼ�߳� 
        bool _TryStopLoop = false;                                      //�Ƿ���Ҫ��ֹ�߳�
        int _ThreadSleepTime = 1;                                       //�߳�����ʱ��,����
        private Logger _Logger = null;                                  //��־�������
        Myrect[] _GeometryArray = new Myrect[100000];                   //��ͼ��������
        EnumGeometryType _CurDrawGeoType = EnumGeometryType.line;       //��ǰ���Ƶ�ͼ������
        Bitmap bitmap = null;                                           //�����ͼƬ
        /// <summary>
        /// ��ǰ���Ƶ�ͼ������
        /// </summary>
        public EnumGeometryType CurDrawGeoType
        {
            get
            {
                return _CurDrawGeoType;
            }
            set
            {
                _CurDrawGeoType = value;
            }
        }
        private bool _IsRandomType = false;                             //�Ƿ��������ͼ��
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
        private int _CurDrawGeoSize = 0;
        private int _CurFPS = 0;                                        //��ǰ����֡��
        private double _curShowTime = 0;
        private double _curDrawTime = 0;
        //������εĿ�Ⱥ͸߶ȱ���
        private int _DrawRectWidth = 10;                                //����εĿ��
        private int _DrawRectHeight = 10;                               //����εĸ߶�
        private int _Grap = 1;
        private bool _IsTiledGraphic = false;                           //�Ƿ��ڴ���ƽ��ͼ��
        private int _DrawedFrame = 1;


        bool _typeLoop = false;
        /// <summary>
        /// ѭ���߳��Ƿ�����
        /// </summary>
        bool _workerThreadRun = false;
        bool _dataUpdate = false;//�����Ƿ����
        /// <summary>
        /// �Ƿ�ƽ��
        /// </summary>
        bool _isTiled = false;
        /// <summary>
        /// ѭ���ȴ�ʱ�䣬����
        /// </summary>



        private void StartLoop(int width, int height)
        {

            if (null == _WorkerThread)
            {
                DrawRect drawBound = ReadParamFromUI();
                _TryStopLoop = false;
                //����һ�����������߳�
                _WorkerThread = new Thread(new ParameterizedThreadStart(Worker));
                _WorkerThread.IsBackground = true;
                _WorkerThread.Name = "DrawThreadWorker";
                _WorkerThread.Start(new DrawRect(width, height));
            }
            else
            {
                _workerThreadRun = false;
                _TryStopLoop = true;
                if (null != _WorkerThread)
                {
                    if (false == _WorkerThread.Join(10000))
                        _WorkerThread = null;
                }
            }
        }
        private void Worker(object param)
        {
            var drawrect = param as DrawRect;

            _workerThreadRun = true;
            _DrawedFrame = 0;
            var loopStartTime = DateTime.Now;
            while (true == _workerThreadRun && !_TryStopLoop)
            {
                if (_IsRandomType)
                {///������ǻ���ָ�������ͣ�������ͼ����
                    var geoType = _DrawedFrame % 5;
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
                }
                var startTime = DateTime.Now;
                FillDrawData(drawrect, MaxSpriteLimit, out int _CurDrawGeoSize);
                this.Invoke(new Action(() =>
                {
                    txtTotalGeometry.Text = _CurDrawGeoSize.ToString();
                }));
                DisplayPixelData(bitmap, _GeometryArray, _CurDrawGeoSize, pictureBox1.Width, pictureBox1.Height, out _curDrawTime, out _curShowTime, 96, true);
                _DrawedFrame++;
                var loopTime = (DateTime.Now - loopStartTime).Seconds;
                _CurFPS = loopTime > 0 ? ((_DrawedFrame / loopTime)) : 0;
                _Logger.Log(LogLevel.Info, $"{_CurDrawGeoSize} {CurDrawGeoType.ToString()}��  �������ƺ�ʱ: " + _curDrawTime + "ms  ������ƺ�ʱ: " + _curShowTime + "ms ֡��" + _CurFPS.ToString() + " fps");
                //this.Invoke(new Action(() =>
                //{

                //    pictureBox1.Refresh();
                //    txtDrawTime.Text = drawTime.ToString();
                //    txtShowTime.Text = showTime.ToString();
                //    if (loopTime > 0)
                //    {
                //        txtfps.Text = fps.ToString();
                //    }
                //}));
            }

        }
        /// <summary>
        /// ���ɻ���ͼ�ε�����
        /// </summary>
        /// <param name="drawrect"></param>
        private void FillDrawData(DrawRect drawrect, int MaxSpriteLimit, out int drawSize)
        {
            var timestamp = DateTime.Now;
            drawSize = MaxSpriteLimit;
            var random = new Random((int)timestamp.Ticks);
            if (_CurDrawGeoType != EnumGeometryType.line)
            {
                if (_isTiled)
                {
                    var reslut = CalculateRectangles(drawrect.Width, drawrect.Height, _DrawRectWidth, _DrawRectHeight, _Grap);
                    drawSize = reslut.Count;
                    for (int i = 0; i < reslut.Count; i++)
                    {
                        _GeometryArray[i] = reslut[i];
                        var mu = (float)i / MaxSpriteLimit;
                        var color = GradientColor(mu);
                        _GeometryArray[i].Color = color;
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
                        _GeometryArray[i] = new Myrect(x, y, _DrawRectWidth, _DrawRectHeight, color);

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
                    _GeometryArray[i] = new Myrect(startx, starty, endx - startx, endy - starty, color);
                }

            }
            drawrect = null;
        }
        private void DrawLine(Myrect[] DataSource, int drawSize, CanvasDrawingSession ds, out double milliseconds)
        {
            milliseconds = 0;
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                ds.DrawLine(DataSource[i].X, DataSource[i].Y, DataSource[i].X + DataSource[i].Width, DataSource[i].Y + DataSource[i].Height, DataSource[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            milliseconds = timspan.TotalMilliseconds;

        }
        private void DrawFillRectangle(Myrect[] DataSource, int drawSize, CanvasDrawingSession ds, out double milliseconds)
        {
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                ds.FillRectangle(DataSource[i].X, DataSource[i].Y, DataSource[i].Width, DataSource[i].Height, DataSource[i].Color);
                ds.DrawText($"{DataSource[i].X}-{DataSource[i].Y}", DataSource[i].X, DataSource[i].Y + DataSource[i].Height, Windows.UI.Color.FromArgb(255, 255, 255, 255));
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            milliseconds = timspan.TotalMilliseconds;
        }
        private void DrawRectangle(Myrect[] DataSource, int drawSize, CanvasDrawingSession ds, out double milliseconds)
        {
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                ds.DrawRectangle(DataSource[i].X, DataSource[i].Y, DataSource[i].Width, DataSource[i].Height, DataSource[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            milliseconds = timspan.TotalMilliseconds;
        }
        private void DrawCircle(Myrect[] DataSource, int drawSize, CanvasDrawingSession ds, out double milliseconds)
        {
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                ds.DrawCircle(DataSource[i].X, DataSource[i].Y, 20, DataSource[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            milliseconds = timspan.TotalMilliseconds;
        }
        private void DrawText(Myrect[] DataSource, int drawSize, CanvasDrawingSession ds, out double milliseconds, string text = "���ڻ�����")
        {
            var startTime = DateTime.Now;
            for (int i = 0; i < drawSize; i++)
            {
                ds.DrawText(text, DataSource[i].X, DataSource[i].Y, DataSource[i].Color);
            }
            var endTime = DateTime.Now;
            var timspan = endTime - startTime;
            milliseconds = timspan.TotalMilliseconds;
        }
        /// <summary>
        ///  ����ɫ������������PictureBox����ʾλͼ
        /// </summary>
        /// <param name="control">pictureBox�ؼ�</param>
        /// <param name="pixelData">��ɫ��������</param>
        /// <param name="width">ͼƬ�Ŀ��</param>
        /// <param name="height">ͼƬ�߶�</param>
        private void DisplayPixelData(Bitmap bitmap, Myrect[] Geometry, int drawSize, int width, int height, out double drawTime, out double showTime, float dpi = 96, bool IsThread = false)
        {
            var startTime = DateTime.Now;
            var device = CanvasDevice.GetSharedDevice();
            drawTime = 0;
            showTime = 0;
            using (var offscreen = new CanvasRenderTarget(device, width, height, dpi))         /*ʹ�������������Ļ�����ͼ��*/
            {
                using (var ds = offscreen.CreateDrawingSession())
                {
                    ds.Clear(Colors.Transparent);//�������
                    switch (_CurDrawGeoType)
                    {
                    case EnumGeometryType.line:
                        DrawLine(Geometry, drawSize, ds, out drawTime);
                        break;
                    case EnumGeometryType.rect:
                        DrawRectangle(Geometry, drawSize, ds, out drawTime);
                        break;
                    case EnumGeometryType.fillrect:
                        DrawFillRectangle(Geometry, drawSize, ds, out drawTime);
                        break;
                    case EnumGeometryType.circle:
                        DrawCircle(Geometry, drawSize, ds, out drawTime);
                        break;
                    case EnumGeometryType.text:
                        DrawText(Geometry, drawSize, ds, out drawTime);
                        break;
                    default:
                        break;
                    }
                }

                var pixelData = offscreen.GetPixelBytes();                                                        //�����������л�ȡ��������
                if (IsThread)
                {
                    mutex.WaitOne(); // ������
                    try
                    {
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),                           /*���������鸴�Ƶ�λͼ������*/
                            ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                        System.Runtime.InteropServices.Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
                        bitmap.UnlockBits(bmpData);
                        bmpData = null;
                    }
                    finally
                    {
                        mutex.ReleaseMutex(); // �ͷ���
                    }
                }
                else
                {
                    try
                    {
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),                           /*���������鸴�Ƶ�λͼ������*/
        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                        System.Runtime.InteropServices.Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
                        bitmap.UnlockBits(bmpData);
                        bmpData = null;
                        var fn = DateTime.Now.ToString("HH-mm-ss-fff");
                        offscreen.SaveAsync($"D:\\{fn}.png");
                    }
                    catch (Exception ex)
                    {
                        _Logger.Log(LogLevel.Error, ex.Message);

                    }

                }



                var endTime = DateTime.Now;
                showTime = (endTime - startTime).TotalMilliseconds;

            }

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
        public static List<Myrect> CalculateRectangles(int windowWidth, int windowHeight, int rectWidth, int rectHeight, int gap = 1)
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


        #region ����
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
            radom,
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
        public struct Myrect
        {
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