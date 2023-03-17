using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zhai.Famil.Controls;

namespace Zhai.ScreenCapturer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FamilWindow
    {
        private readonly ScreenCaputre screenCaputre = new ScreenCaputre();
        public MainWindow()
        {
            InitializeComponent();

            screenCaputre.ScreenCaputred += OnScreenCaputred;
            screenCaputre.ScreenCaputreCancelled += OnScreenCaputreCancelled;
        }

        private void OnScreenCaputreCancelled(object sender, System.EventArgs e)
        {
            Show();
            Focus();
        }

        private void OnScreenCaputred(object sender, ScreenCaputredEventArgs e)
        {
            Show();

            var win = new ImageWindow(e.Bmp);

            win.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Thread.Sleep(300);
            screenCaputre.StartCaputre(30);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AboutWindow
            {
                Owner = App.Current.MainWindow
            };

            window.ShowDialog();
        }
    }
}
