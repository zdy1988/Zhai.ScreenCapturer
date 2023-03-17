using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Zhai.ScreenCapturer
{
    internal class ScreenCaputre
    {
        public void StartCaputre(int timeOutSeconds)
        {
            var mask = new MaskWindow(this);
            mask.Show(timeOutSeconds);
        }

        public event EventHandler<ScreenCaputredEventArgs> ScreenCaputred;
        public event EventHandler<EventArgs> ScreenCaputreCancelled;

        internal void OnScreenCaputred(object sender, BitmapSource caputredBmp)
        {
            if (ScreenCaputred != null)
            {
                ScreenCaputred(sender, new ScreenCaputredEventArgs(caputredBmp));
            }
        }

        internal void OnScreenCaputreCancelled(object sender)
        {
            if (ScreenCaputreCancelled != null)
            {
                ScreenCaputreCancelled(sender, EventArgs.Empty);
            }
        }
    }
}
