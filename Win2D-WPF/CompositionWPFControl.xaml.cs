//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using Windows.Graphics.DirectX;
using Windows.UI.Composition;

namespace Win2D_WPF
{
    /// <summary>
    /// Interaction logic for CompositionHostControl.xaml
    /// </summary> 
    public partial class CompositionWPFControl : UserControl
    {
        private CompositionWPFHost _CompositionHost;
        private Compositor _Compositor;
        /// <summary>
        /// 可视对象容器--可视对象根节点,所有可视对象和层的父级
        /// </summary>
        private Windows.UI.Composition.ContainerVisual _VisualRoot;
        SpriteVisual _Win2DLayer = null;      //绘制文字的可视对象
        //Win2D
        CanvasDevice _CanvasDevice = null;//设备上下文
        CompositionGraphicsDevice _compositionGraphicsDevice = null; //合成图形绘制
        CanvasDrawingSession _Win2DDrawSession = null;

        /// <summary>
        /// 当前分辨率
        /// </summary>
        private DpiScale _currentDpiX;
        public CompositionWPFControl()
        {
            InitializeComponent();
            Loaded += CompositionHostControl_Loaded;
            this.SizeChanged += CompositionHostControl_SizeChanged;
        }

        #region 公有方法-接口
        /// <summary>
        /// 初始化控件参数
        /// </summary>
        public void Inition()
        {
            if (_CompositionHost is null)
            {
                _currentDpiX = System.Windows.Media.VisualTreeHelper.GetDpi(this);
                //创建宿主
                _CompositionHost = new CompositionWPFHost(CompositionHostElement.ActualWidth, CompositionHostElement.ActualHeight);
                CompositionHostElement.Child = _CompositionHost;
                //创建可视对象容器
                _Compositor = _CompositionHost.Compositor;
                _VisualRoot = _Compositor.CreateContainerVisual();
                _CompositionHost.Child = _VisualRoot;
            }
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
        }

        #endregion

        #region UI事件处理
        private void CompositionHostControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }
        private void CompositionHostControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (_VisualRoot != null)
            {
                _VisualRoot.Size = new Vector2((float)CompositionHostElement.ActualWidth, (float)CompositionHostElement.ActualHeight);
                _VisualRoot.Offset = new Vector3(0, 0, 0);
            }
        }

        


        #endregion

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            base.OnDpiChanged(oldDpi, newDpi);
            _currentDpiX = newDpi;
            Vector3 newScale = new Vector3((float)newDpi.DpiScaleX, (float)newDpi.DpiScaleY, 1);

            foreach (SpriteVisual child in _VisualRoot.Children)
            {
                child.Scale = newScale;
                var newOffsetX = child.Offset.X * ((float)newDpi.DpiScaleX / (float)oldDpi.DpiScaleX);
                var newOffsetY = child.Offset.Y * ((float)newDpi.DpiScaleY / (float)oldDpi.DpiScaleY);
                child.Offset = new Vector3(newOffsetX, newOffsetY, 1);
                AnimateSquare(child, 0);
            }
        }
        public void BeginDraw()
        {

            BeginDrawText();

        }
        public void EndDraw()
        {
            EndDrawText();
        }

        public void DrawLine()
        {
            _Win2DDrawSession.DrawLine(10f,10f,100f,300f,Windows.UI.Colors.Red);
        }

        #region 绘制文字

        /// <summary>
        /// 开始绘制文字--初始化文字绘制相关参数
        /// </summary>
        protected void BeginDrawText()
        {
            //if(_CanvasDevice==null)//注意：必须每次都重新创建，否则会有重影现象
                _CanvasDevice = CanvasDevice.GetSharedDevice();//设备上下文
            //if(_compositionGraphicsDevice==null)//注意：必须每次都重新创建，否则会有重影现象
                _compositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(_Compositor, _CanvasDevice); //合成图形绘制

            /*1.创建绘制文字的可视对象，因大小会变动，所以必须每次创建新的*/
            _Win2DLayer = _Compositor.CreateSpriteVisual();
            _Win2DLayer.Size = new Vector2((float)this.ActualWidth, (float)this.ActualHeight);
            _Win2DLayer.Offset = new Vector3(0, 0, 0);
            _VisualRoot.Children.InsertAtTop(_Win2DLayer);

            /*2创建一个画刷*/
            var _surfaceBrush = _Compositor.CreateSurfaceBrush();//画文字的画刷
            CompositionDrawingSurface _drawingSurface = _compositionGraphicsDevice.CreateDrawingSurface(new Windows.Foundation.Size((float)this.ActualWidth, (float)this.ActualHeight),     /*3.画刷绘制层*/
                    DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
            _surfaceBrush.Surface = _drawingSurface; //画文字的图层

            /*3将画文字的画刷赋给可视对象*/
            _Win2DLayer.Brush = _surfaceBrush;
            /*4.可视对象采用文字画刷填充*/
            _Win2DDrawSession = CanvasComposition.CreateDrawingSession(_drawingSurface);
            
        }
        /// <summary>
        /// 文字绘制结束--销毁文字绘制相关参数
        /// </summary>
        protected void EndDrawText()
        {
            //_CanvasContainer.Children.InsertAtTop(_DrawingTextVisualLayer);//注意：不要在最后加入到可视树中，这样比先加入闪屏更明显
            _Win2DDrawSession?.Dispose();
            _Win2DDrawSession = null;

            /************以下几个对象不能经常性地销毁，否则内存增长很快********************/
            //_compositionGraphicsDevice?.Dispose();
            //_compositionGraphicsDevice = null;
            //_CanvasDevice?.Dispose();
            //_CanvasDevice = null;
            /**********************************************************************/
        }



        #endregion


        private Windows.UI.Color GetRandomColor()
        {
            Random random = new Random();
            byte r = (byte)random.Next(0, 255);
            byte g = (byte)random.Next(0, 255);
            byte b = (byte)random.Next(0, 255);
            return Windows.UI.Color.FromArgb(255, r, g, b);
        }

        /// <summary>
        /// 简单动效
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="delay"></param>
        private void AnimateSquare(SpriteVisual visual, int delay)
        {
            float offsetX = (float)(visual.Offset.X); // Already adjusted for DPI.

            // Adjust values for DPI scale, then find the Y offset that aligns the bottom of the square
            // with the bottom of the host container. This is the value to animate to.
            var hostHeightAdj = CompositionHostElement.ActualHeight * _currentDpiX.DpiScaleY;
            var squareSizeAdj = visual.Size.Y * _currentDpiX.DpiScaleY;
            float bottom = (float)(hostHeightAdj - squareSizeAdj);

            // Create the animation only if it's needed.
            if (visual.Offset.Y != bottom)
            {
                Vector3KeyFrameAnimation animation = _Compositor.CreateVector3KeyFrameAnimation();
                animation.InsertKeyFrame(1f, new Vector3(offsetX, bottom, 0f));
                animation.Duration = TimeSpan.FromSeconds(2);
                animation.DelayTime = TimeSpan.FromSeconds(delay);
                visual.StartAnimation("Offset", animation);
            }
        }

    }
}
