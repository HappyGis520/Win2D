// Copyright (c) Microsoft Corporation. 
// Licensed under the MIT License. 
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT;
using Microsoft.Graphics.Canvas;
using Microsoft.UI.Xaml.Controls;
using System.Runtime.CompilerServices;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.UI.Xaml.Media;
using System.IO;
using Button = Microsoft.UI.Xaml.Controls.Button;
using System.Drawing.Imaging;
using System.Drawing;

namespace winforms_unpackaged_app
{
    public partial class AppForm : Form
    {
        public string WindowTitle = "Windowing WinForms C# Sample";
        private AppWindow _mainAppWindow;
        private bool _isBrandedTitleBar = false;
        //CanvasControl _CanvasControl = null;
        public AppForm()
        {
            this.InitializeComponent();

            // Gets the AppWindow using the windowing interop methods (see WindowingInterop.cs for details)
            _mainAppWindow = AppWindowExtensions.GetAppWindowFromWinformsWindow(this);
            _mainAppWindow.Changed += AppWindowChangedHandler;
            _mainAppWindow.Title = WindowTitle;
            //_CanvasControl = new CanvasControl();
            //this.Controls.Add(_CanvasControl);
            //var device = CanvasDevice.GetSharedDevice();
            //using (var offscreen = new CanvasRenderTarget(device, 600, 400, 96))
            //{
            //    using(var sess=offscreen.CreateDrawingSession())
            //    {
            //        sess.DrawLine(10f,10f,1000f, 1000f,Windows.UI.Color.FromArgb(123,123,23,123));
            //    }
            //    return;
            //}
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppForm));
            toggleCompactOverlay = new System.Windows.Forms.Button();
            toggleFullScreen = new System.Windows.Forms.Button();
            presenterHeader = new Label();
            overlapped = new System.Windows.Forms.Button();
            customizeWindowHeader = new Label();
            windowTitleTextBox = new System.Windows.Forms.TextBox();
            setWindowTitleButton = new System.Windows.Forms.Button();
            widthTextBox = new System.Windows.Forms.TextBox();
            heightTextBox = new System.Windows.Forms.TextBox();
            resizeButton = new System.Windows.Forms.Button();
            customTitleBarHeader = new Label();
            titlebarBrandingBtn = new System.Windows.Forms.Button();
            titlebarCustomBtn = new System.Windows.Forms.Button();
            resetTitlebarBtn = new System.Windows.Forms.Button();
            MyTitleBar = new FlowLayoutPanel();
            MyWindowIcon = new PictureBox();
            MyWindowTitle = new Label();
            clientResizeButton = new System.Windows.Forms.Button();
            standardHeightButton = new System.Windows.Forms.Button();
            tallHeightButton = new System.Windows.Forms.Button();
            pictureBox = new PictureBox();
            MyTitleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MyWindowIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // toggleCompactOverlay
            // 
            toggleCompactOverlay.Location = new Point(12, 223);
            toggleCompactOverlay.Name = "toggleCompactOverlay";
            toggleCompactOverlay.Size = new Size(176, 27);
            toggleCompactOverlay.TabIndex = 1;
            toggleCompactOverlay.Text = "Enter Compact Overlay";
            toggleCompactOverlay.UseVisualStyleBackColor = true;
            toggleCompactOverlay.Click += SwitchPresenter;
            // 
            // toggleFullScreen
            // 
            toggleFullScreen.Location = new Point(194, 223);
            toggleFullScreen.Name = "toggleFullScreen";
            toggleFullScreen.Size = new Size(144, 27);
            toggleFullScreen.TabIndex = 2;
            toggleFullScreen.Text = "Enter Full Screen";
            toggleFullScreen.UseVisualStyleBackColor = true;
            toggleFullScreen.Click += SwitchPresenter;
            // 
            // presenterHeader
            // 
            presenterHeader.AutoSize = true;
            presenterHeader.ForeColor = Color.DodgerBlue;
            presenterHeader.Location = new Point(12, 200);
            presenterHeader.Name = "presenterHeader";
            presenterHeader.Size = new Size(270, 20);
            presenterHeader.TabIndex = 3;
            presenterHeader.Text = "Change your AppWindow Presenter";
            // 
            // overlapped
            // 
            overlapped.Location = new Point(344, 223);
            overlapped.Name = "overlapped";
            overlapped.Size = new Size(148, 27);
            overlapped.TabIndex = 4;
            overlapped.Text = "Enter Overlapped ";
            overlapped.UseVisualStyleBackColor = true;
            overlapped.Click += SwitchPresenter;
            // 
            // customizeWindowHeader
            // 
            customizeWindowHeader.AutoSize = true;
            customizeWindowHeader.FlatStyle = FlatStyle.Flat;
            customizeWindowHeader.ForeColor = Color.DodgerBlue;
            customizeWindowHeader.Location = new Point(12, 97);
            customizeWindowHeader.Name = "customizeWindowHeader";
            customizeWindowHeader.Size = new Size(149, 20);
            customizeWindowHeader.TabIndex = 5;
            customizeWindowHeader.Text = "Customize Window";
            // 
            // windowTitleTextBox
            // 
            windowTitleTextBox.Location = new Point(12, 120);
            windowTitleTextBox.Name = "windowTitleTextBox";
            windowTitleTextBox.Size = new Size(191, 27);
            windowTitleTextBox.TabIndex = 6;
            windowTitleTextBox.Text = "WinForms ❤️ AppWindow";
            // 
            // setWindowTitleButton
            // 
            setWindowTitleButton.Location = new Point(209, 120);
            setWindowTitleButton.Name = "setWindowTitleButton";
            setWindowTitleButton.Size = new Size(133, 27);
            setWindowTitleButton.TabIndex = 7;
            setWindowTitleButton.Text = "Set Window Title";
            setWindowTitleButton.UseVisualStyleBackColor = true;
            setWindowTitleButton.Click += windowTitleButton_Click;
            // 
            // widthTextBox
            // 
            widthTextBox.Location = new Point(12, 153);
            widthTextBox.Name = "widthTextBox";
            widthTextBox.Size = new Size(50, 27);
            widthTextBox.TabIndex = 9;
            // 
            // heightTextBox
            // 
            heightTextBox.Location = new Point(68, 153);
            heightTextBox.Name = "heightTextBox";
            heightTextBox.Size = new Size(50, 27);
            heightTextBox.TabIndex = 10;
            // 
            // resizeButton
            // 
            resizeButton.Location = new Point(124, 153);
            resizeButton.Name = "resizeButton";
            resizeButton.Size = new Size(119, 27);
            resizeButton.TabIndex = 11;
            resizeButton.Text = "Resize Window";
            resizeButton.UseVisualStyleBackColor = true;
            resizeButton.Click += resizeButton_Click;
            // 
            // customTitleBarHeader
            // 
            customTitleBarHeader.AutoSize = true;
            customTitleBarHeader.ForeColor = Color.DodgerBlue;
            customTitleBarHeader.Location = new Point(12, 264);
            customTitleBarHeader.Name = "customTitleBarHeader";
            customTitleBarHeader.Size = new Size(124, 20);
            customTitleBarHeader.TabIndex = 12;
            customTitleBarHeader.Text = "Custom TitleBar";
            // 
            // titlebarBrandingBtn
            // 
            titlebarBrandingBtn.Location = new Point(12, 287);
            titlebarBrandingBtn.Name = "titlebarBrandingBtn";
            titlebarBrandingBtn.Size = new Size(231, 29);
            titlebarBrandingBtn.TabIndex = 13;
            titlebarBrandingBtn.Text = "Toggle TitleBar Color Branding";
            titlebarBrandingBtn.UseVisualStyleBackColor = true;
            titlebarBrandingBtn.Click += titlebarBrandingBtn_Click;
            // 
            // titlebarCustomBtn
            // 
            titlebarCustomBtn.Location = new Point(249, 287);
            titlebarCustomBtn.Name = "titlebarCustomBtn";
            titlebarCustomBtn.Size = new Size(178, 29);
            titlebarCustomBtn.TabIndex = 14;
            titlebarCustomBtn.Text = "Toggle Custom TitleBar";
            titlebarCustomBtn.UseVisualStyleBackColor = true;
            titlebarCustomBtn.Click += titlebarCustomBtn_Click;
            // 
            // resetTitlebarBtn
            // 
            resetTitlebarBtn.Location = new Point(12, 365);
            resetTitlebarBtn.Name = "resetTitlebarBtn";
            resetTitlebarBtn.Size = new Size(114, 29);
            resetTitlebarBtn.TabIndex = 15;
            resetTitlebarBtn.Text = "Reset TitleBar";
            resetTitlebarBtn.UseVisualStyleBackColor = true;
            resetTitlebarBtn.Click += resetTitlebarBtn_Click;
            // 
            // MyTitleBar
            // 
            MyTitleBar.BackColor = Color.Blue;
            MyTitleBar.Controls.Add(MyWindowIcon);
            MyTitleBar.Controls.Add(MyWindowTitle);
            MyTitleBar.ForeColor = Color.White;
            MyTitleBar.Location = new Point(1, -1);
            MyTitleBar.Name = "MyTitleBar";
            MyTitleBar.Size = new Size(682, 32);
            MyTitleBar.TabIndex = 16;
            MyTitleBar.Visible = false;
            // 
            // MyWindowIcon
            // 
            MyWindowIcon.Image = (System.Drawing.Image)resources.GetObject("MyWindowIcon.Image");
            MyWindowIcon.InitialImage = (System.Drawing.Image)resources.GetObject("MyWindowIcon.InitialImage");
            MyWindowIcon.Location = new Point(3, 3);
            MyWindowIcon.Name = "MyWindowIcon";
            MyWindowIcon.Size = new Size(27, 26);
            MyWindowIcon.SizeMode = PictureBoxSizeMode.Zoom;
            MyWindowIcon.TabIndex = 0;
            MyWindowIcon.TabStop = false;
            // 
            // MyWindowTitle
            // 
            MyWindowTitle.AutoSize = true;
            MyWindowTitle.Dock = DockStyle.Fill;
            MyWindowTitle.Location = new Point(36, 0);
            MyWindowTitle.Name = "MyWindowTitle";
            MyWindowTitle.Size = new Size(299, 32);
            MyWindowTitle.TabIndex = 1;
            MyWindowTitle.Text = "Custom titlebar with interactive content";
            MyWindowTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // clientResizeButton
            // 
            clientResizeButton.Location = new Point(249, 153);
            clientResizeButton.Name = "clientResizeButton";
            clientResizeButton.Size = new Size(119, 27);
            clientResizeButton.TabIndex = 17;
            clientResizeButton.Text = "Resize Client Area";
            clientResizeButton.UseVisualStyleBackColor = true;
            clientResizeButton.Click += resizeButton_Click;
            // 
            // standardHeightButton
            // 
            standardHeightButton.Enabled = false;
            standardHeightButton.Location = new Point(13, 329);
            standardHeightButton.Name = "standardHeightButton";
            standardHeightButton.Size = new Size(190, 30);
            standardHeightButton.TabIndex = 18;
            standardHeightButton.Text = "Standard height title bar";
            standardHeightButton.UseVisualStyleBackColor = true;
            standardHeightButton.Click += standardHeightButton_Click;
            // 
            // tallHeightButton
            // 
            tallHeightButton.Enabled = false;
            tallHeightButton.Location = new Point(209, 329);
            tallHeightButton.Name = "tallHeightButton";
            tallHeightButton.Size = new Size(150, 30);
            tallHeightButton.TabIndex = 19;
            tallHeightButton.Text = "Taller title bar";
            tallHeightButton.UseVisualStyleBackColor = true;
            tallHeightButton.Click += tallHeightButton_Click;
            // 
            // pictureBox
            // 
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.Location = new Point(868, 97);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(1465, 856);
            pictureBox.TabIndex = 20;
            pictureBox.TabStop = false;
            // 
            // AppForm
            // 
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(2434, 1070);
            Controls.Add(pictureBox);
            Controls.Add(tallHeightButton);
            Controls.Add(standardHeightButton);
            Controls.Add(clientResizeButton);
            Controls.Add(MyTitleBar);
            Controls.Add(resetTitlebarBtn);
            Controls.Add(titlebarCustomBtn);
            Controls.Add(titlebarBrandingBtn);
            Controls.Add(customTitleBarHeader);
            Controls.Add(resizeButton);
            Controls.Add(heightTextBox);
            Controls.Add(widthTextBox);
            Controls.Add(setWindowTitleButton);
            Controls.Add(windowTitleTextBox);
            Controls.Add(customizeWindowHeader);
            Controls.Add(overlapped);
            Controls.Add(presenterHeader);
            Controls.Add(toggleFullScreen);
            Controls.Add(toggleCompactOverlay);
            Name = "AppForm";
            Activated += AppForm_Activated;
            Load += AppForm_Load;
            MyTitleBar.ResumeLayout(false);
            MyTitleBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MyWindowIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void AppWindowChangedHandler(AppWindow sender, AppWindowChangedEventArgs args)
        {
            // The presenter changed so we need to update the button captions to reflect the new state
            if (args.DidPresenterChange)
            {
                switch (_mainAppWindow.Presenter.Kind)
                {
                case AppWindowPresenterKind.CompactOverlay:
                    toggleCompactOverlay.Text = "Exit CompactOverlay";
                    toggleFullScreen.Text = "Enter FullScreen";
                    break;

                case AppWindowPresenterKind.FullScreen:
                    toggleCompactOverlay.Text = "Enter CompactOverlay";
                    toggleFullScreen.Text = "Exit FullScreen";
                    break;

                case AppWindowPresenterKind.Overlapped:
                    toggleCompactOverlay.Text = "Enter CompactOverlay";
                    toggleFullScreen.Text = "Enter FullScreen";
                    break;

                default:
                    // If we end up here the presenter was changed to something we don't know what it is.
                    // This would happen if a new presenter is introduced.
                    // We can ignore this situation since we are not aware of the presenter and have no UI that
                    // reacts to this unknown presenter.
                    break;
                }
            }
            if (args.DidSizeChange && sender.TitleBar.ExtendsContentIntoTitleBar)
            {
                // Need to update our drag region if the size of the window changes
                SetDragRegionForCustomTitleBar(sender);
            }
            if (args.DidSizeChange)
            {
                // update the size of custom titlebar as the window size changes
                MyTitleBar.Width = _mainAppWindow.Size.Width;
            }
        }

        private void SwitchPresenter(object sender, EventArgs e)
        {
            // Bail out if we don't have an AppWindow object.
            if (_mainAppWindow != null)
            {

                AppWindowPresenterKind newPresenterKind;
                switch (sender.As<Button>().Name)
                {
                case "toggleCompactOverlay":
                    newPresenterKind = AppWindowPresenterKind.CompactOverlay;
                    break;

                case "toggleFullScreen":
                    newPresenterKind = AppWindowPresenterKind.FullScreen;
                    break;

                case "overlapped":
                    newPresenterKind = AppWindowPresenterKind.Overlapped;
                    break;

                default:
                    newPresenterKind = AppWindowPresenterKind.Default;
                    break;
                }

                // If the same presenter button was pressed as the mode we're in, toggle the window back to Default.
                if (newPresenterKind == _mainAppWindow.Presenter.Kind)
                {
                    _mainAppWindow.SetPresenter(AppWindowPresenterKind.Default);
                }
                else
                {
                    // else request a presenter of the selected kind to be created and applied to the window.
                    _mainAppWindow.SetPresenter(newPresenterKind);
                }
            }
        }
        private void AppForm_Load(object sender, EventArgs e)
        {

        }
        private void AppForm_Activated(object sender, EventArgs e)
        {
        }


        private void windowTitleButton_Click(object sender, EventArgs e)
        {
            _mainAppWindow.Title = windowTitleTextBox.Text;
        }

        private void resizeButton_Click(object sender, EventArgs e)
        {
            int windowWidth = 0;
            int windowHeight = 0;

            try
            {
                windowWidth = int.Parse(widthTextBox.Text);
                windowHeight = int.Parse(heightTextBox.Text);
            }
            catch (FormatException)
            {
                // Silently ignore invalid input...
            }

            if (_mainAppWindow != null)
            {
                switch (sender.As<Button>().Name)
                {
                case "resizeButton":
                    _mainAppWindow.Resize(new Windows.Graphics.SizeInt32(windowWidth, windowHeight));
                    break;

                case "clientResizeButton":
                    _mainAppWindow.ResizeClient(new Windows.Graphics.SizeInt32(windowWidth, windowHeight));
                    break;
                }
            }
        }

        private void titlebarBrandingBtn_Click(object sender, EventArgs e)
        {
            _mainAppWindow.TitleBar.ResetToDefault();

            _isBrandedTitleBar = !_isBrandedTitleBar;
            // Check to see if customization is supported. Currently only supported on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported() && _isBrandedTitleBar)
            {
                _mainAppWindow.Title = "Default titlebar with custom color customization";
                _mainAppWindow.TitleBar.ForegroundColor = Colors.White;
                _mainAppWindow.TitleBar.BackgroundColor = Colors.DarkOrange;
                _mainAppWindow.TitleBar.InactiveBackgroundColor = Colors.Blue;
                _mainAppWindow.TitleBar.InactiveForegroundColor = Colors.White;

                // Buttons
                _mainAppWindow.TitleBar.ButtonBackgroundColor = Colors.DarkOrange;
                _mainAppWindow.TitleBar.ButtonForegroundColor = Colors.White;
                _mainAppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Blue;
                _mainAppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.White;
                _mainAppWindow.TitleBar.ButtonHoverBackgroundColor = Colors.Green;
                _mainAppWindow.TitleBar.ButtonHoverForegroundColor = Colors.White;
                _mainAppWindow.TitleBar.ButtonPressedBackgroundColor = Colors.DarkOrange;
                _mainAppWindow.TitleBar.ButtonPressedForegroundColor = Colors.White;
            }
            else
            {
                _mainAppWindow.Title = WindowTitle;
            }
            MyTitleBar.Visible = false;
        }

        private void titlebarCustomBtn_Click(object sender, EventArgs e)
        {
            _mainAppWindow.TitleBar.ExtendsContentIntoTitleBar = !_mainAppWindow.TitleBar.ExtendsContentIntoTitleBar;

            // Check to see if customization is supported. Currently only supported on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported() && _mainAppWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                // Show the custom titlebar
                MyTitleBar.Visible = true;

                // Enable title bar height toggle buttons
                this.standardHeightButton.Enabled = true;
                this.tallHeightButton.Enabled = true;

                // Set Button colors to match the custom titlebar
                _mainAppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                _mainAppWindow.TitleBar.ButtonForegroundColor = Colors.White;
                _mainAppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                _mainAppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.White;
                _mainAppWindow.TitleBar.ButtonHoverBackgroundColor = Colors.Green;
                _mainAppWindow.TitleBar.ButtonHoverForegroundColor = Colors.White;
                _mainAppWindow.TitleBar.ButtonPressedBackgroundColor = Colors.Green;
                _mainAppWindow.TitleBar.ButtonPressedForegroundColor = Colors.White;

                // Set the drag region for the custom TitleBar
                SetDragRegionForCustomTitleBar(_mainAppWindow);
            }
            else
            {
                // Bring back the default titlebar
                MyTitleBar.Visible = false;

                // Disable title bar height toggle buttons
                this.standardHeightButton.Enabled = false;
                this.tallHeightButton.Enabled = false;

                // reset the title bar to default state
                _mainAppWindow.TitleBar.ResetToDefault();
            }
        }

        private void SetDragRegionForCustomTitleBar(AppWindow appWindow)
        {
            //Infer titlebar height
            int titleBarHeight = appWindow.TitleBar.Height;
            MyTitleBar.Height = titleBarHeight;

            // Get caption button occlusion information
            // Use LeftInset if you've explicitly set your window layout to RTL or if app language is a RTL language
            int CaptionButtonOcclusionWidth = appWindow.TitleBar.RightInset;

            // Define your drag Regions
            int windowIconWidthAndPadding = MyWindowIcon.Width + MyWindowIcon.Margin.Right;
            int dragRegionWidth = appWindow.Size.Width - (CaptionButtonOcclusionWidth + windowIconWidthAndPadding);

            Windows.Graphics.RectInt32[] dragRects = new Windows.Graphics.RectInt32[] { };
            Windows.Graphics.RectInt32 dragRect;

            dragRect.X = windowIconWidthAndPadding;
            dragRect.Y = 0;
            dragRect.Height = titleBarHeight;
            dragRect.Width = dragRegionWidth;

            appWindow.TitleBar.SetDragRectangles(dragRects.Append(dragRect).ToArray());
        }
        CanvasControl _canvas = null;

        private byte[] pixelData;
        private void resetTitlebarBtn_Click(object sender, EventArgs e)
        {
            _mainAppWindow.TitleBar.ResetToDefault();
            _mainAppWindow.Title = WindowTitle;
            MyTitleBar.Visible = false;
            var device = CanvasDevice.GetSharedDevice();
            using (var offscreen = new CanvasRenderTarget(device, pictureBox.Width, pictureBox.Height, 96))
            {
                using (var ds = offscreen.CreateDrawingSession())
                {
                    ds.Clear(Colors.Transparent);
                    ds.DrawRectangle(100f, 200f, 5f, 6f, Colors.Red);
                }
                pixelData = offscreen.GetPixelBytes();
                DisplayPixelData(pictureBox, pixelData, pictureBox.Width, pictureBox.Height);
                //offscreen.SaveAsync("D:\\offscreen11.png");
                return;
            }
            //_canvas.Draw += _canvas_Draw; ;
            //this.Controls.Add(_canvas);
        }

        /// <summary>
        ///  将颜色像素数组在在PictureBox中显示位图
        /// </summary>
        /// <param name="control">pictureBox控件</param>
        /// <param name="pixelData">颜色像素数组</param>
        /// <param name="width">图片的宽度</param>
        /// <param name="height">图片高度</param>
        private void DisplayPixelData(PictureBox pictureBox, byte[] pixelData, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            // 将像素数组复制到位图对象中
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(pixelData, 0, bmpData.Scan0, pixelData.Length);
            bitmap.UnlockBits(bmpData);
            // 在PictureBox中显示位图
            pictureBox.Image = bitmap;
        }
        private void _canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            using (var ds = args.DrawingSession)
            {
                ds.DrawLine(0f, 0f, 500f, 500f, Windows.UI.Color.FromArgb(255, 255, 0, 0));
            }
        }

        private void standardHeightButton_Click(object sender, EventArgs e)
        {
            if (AppWindowTitleBar.IsCustomizationSupported() && _mainAppWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                // Set the title bar height to standard height
                _mainAppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Standard;

                // Adjust title and icon margins
                this.MyWindowTitle.Margin = new Padding(3, 0, 3, 0);
                this.MyWindowIcon.Margin = new Padding(3, 3, 3, 3);

                // Reset the drag region for the custom title bar
                SetDragRegionForCustomTitleBar(_mainAppWindow);
            }
            else
            {
                this.standardHeightButton.Enabled = false;
            }

        }
        private void tallHeightButton_Click(object sender, EventArgs e)
        {
            if (AppWindowTitleBar.IsCustomizationSupported() && _mainAppWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                // Use a tall title bar to provide more room for interactive elements like searchboxes, person pictures e.t.c
                _mainAppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

                // Adjust title and icon top margin
                this.MyWindowTitle.Margin = new Padding(3, 8, 3, 0);
                this.MyWindowIcon.Margin = new Padding(3, 8, 3, 3);

                // Reset the drag region for the custom title bar
                SetDragRegionForCustomTitleBar(_mainAppWindow);

            }
            else
            {
                this.tallHeightButton.Enabled = false;
            }
        }
    }
}
