using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Zhai.ScreenCapturer
{
    internal static class HelperMethods
    {
        public static Bitmap GetScreenSnapshot()
        {
            try
            {
                Rectangle rc = SystemInformation.VirtualScreen;
                var bitmap = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);

                using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
                {
                    memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                }

                return bitmap;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public static BitmapSource ToBitmapSource(this Bitmap bmp)
        {
            BitmapSource returnSource;

            try
            {
                returnSource = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                returnSource = null;
            }

            return returnSource;

        }
    }
}
