using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Zhai.ScreenCapturer
{
    internal class ScreenCaputredEventArgs : EventArgs
    {
        public BitmapSource Bmp
        {
            get;
            private set;
        }

        public ScreenCaputredEventArgs(BitmapSource bmp)
        {
            Bmp = bmp;
        }
    }
}
