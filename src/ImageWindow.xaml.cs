using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Zhai.Famil.Common.Mvvm.Messaging;
using Zhai.Famil.Controls;

namespace Zhai.ScreenCapturer
{
    /// <summary>
    /// ImageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageWindow : FamilWindow
    {
        private readonly BitmapSource bmp;

        public ImageWindow(BitmapSource bmp)
        {
            InitializeComponent();

            this.bmp = bmp;
            this.Width = bmp.Width;
            this.Height = bmp.Height;
            this.ImageContainer.Width = bmp.Width;
            this.ImageContainer.Height = bmp.Height;
            this.ImageContainer.Background = new ImageBrush(bmp);
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Clipboard.SetImage(this.bmp);
                SendNotificationMessage("拷贝成功！");
            }
            catch
            {
                SendNotificationMessage("拷贝失败！");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Famil.Win32.CommonDialog.SaveFileDialog(this, "", "", "保存...", $"{DateTime.Now.ToString("yyyy-dd-MM-hh-mm-ss")}.png", ".png", default, out string filename))
            {
                try
                {
                    SaveImageToFile(this.bmp, filename);
                    SendNotificationMessage("保存成功！");
                }
                catch
                {
                    SendNotificationMessage("保存失败！");
                }
            }
        }

        public void SendNotificationMessage(string message)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                this.Hint.Text = "";
                this.Hint.Text = message;
            }
        }

        private void SaveImageToFile(BitmapSource image, string filePath)
        {
            BitmapEncoder encoder = GetBitmapEncoder(filePath);
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        private BitmapEncoder GetBitmapEncoder(string filePath)
        {
            var extName = Path.GetExtension(filePath).ToLower();
            if (extName.Equals(".png"))
            {
                return new PngBitmapEncoder();
            }
            else
            {
                return new JpegBitmapEncoder();
            }
        }
    }
}
