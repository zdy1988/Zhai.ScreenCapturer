using System;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using Zhai.Famil.Common.ExtensionMethods;
using System.Windows.Forms;
using Zhai.Famil.Controls;

namespace Zhai.ScreenCapturer
{
    internal class MaskWindow : Window
    {
        private ScreenSelector screenSelector;
        private Bitmap screenSnapshot;
        private DispatcherTimer timeOutTimmer;
        private readonly ScreenCaputre screenCaputreOwner;

        public MaskWindow(ScreenCaputre screenCaputreOwner)
        {
            this.screenCaputreOwner = screenCaputreOwner;

            //Topmost = true;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;

            var rect = SystemInformation.VirtualScreen;
            Left = rect.X;
            Top = rect.Y;
            Width = rect.Width;
            Height = rect.Height;


            screenSnapshot = HelperMethods.GetScreenSnapshot();
            if (screenSnapshot != null)
            {
                var bmp = screenSnapshot.ToBitmapSource();
                bmp.Freeze();
                Background = new ImageBrush(bmp);
            }

            screenSelector = new ScreenSelector();
            screenSelector.IndicatorDoubleClick += ScreenSelector_IndicatorDoubleClick;
            Content = screenSelector;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.RightButton == MouseButtonState.Pressed && e.ClickCount >= 2)
            {
                CancelCaputre();
            }
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (timeOutTimmer != null && timeOutTimmer.IsEnabled)
            {
                timeOutTimmer.Stop();
                timeOutTimmer.Start();
            }
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
            {
                CancelCaputre();
            }
        }

        private void ScreenSelector_IndicatorDoubleClick(object sender, Rect e)
        {
            ClipSnapshot(e);
        }

        private void CancelCaputre()
        {
            Close();
            screenCaputreOwner.OnScreenCaputreCancelled(null);
        }

        private void ClipSnapshot(Rect clipRegion)
        {
            BitmapSource caputredBmp = CopyFromScreenSnapshot(clipRegion);

            if (caputredBmp != null)
            {
                screenCaputreOwner.OnScreenCaputred(null, caputredBmp);
            }

            //close mask window
            Close();
        }

        private BitmapSource CopyFromScreenSnapshot(Rect region)
        {
            var sourceRect = region.ToRectangle();
            var destRect = new Rectangle(0, 0, sourceRect.Width, sourceRect.Height);

            if (screenSnapshot != null)
            {
                var bitmap = new Bitmap(sourceRect.Width, sourceRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(screenSnapshot, destRect, sourceRect, GraphicsUnit.Pixel);
                }

                return bitmap.ToBitmapSource();
            }

            return null;
        }

        internal void Show(int timeOutSecond)
        {
            if (timeOutSecond > 0)
            {
                if (timeOutTimmer == null)
                {
                    timeOutTimmer = new DispatcherTimer();
                    timeOutTimmer.Tick += TimeOutTimmer_Tick;
                }

                timeOutTimmer.Interval = TimeSpan.FromMilliseconds(timeOutSecond * 1000);
                timeOutTimmer.Start();
            }

            Show();
            Focus();
        }

        private void TimeOutTimmer_Tick(object? sender, EventArgs e)
        {
            timeOutTimmer.Stop();
            CancelCaputre();
        }
    }
}
