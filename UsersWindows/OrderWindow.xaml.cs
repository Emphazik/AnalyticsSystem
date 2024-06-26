using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aspose.BarCode.Generation;

namespace AnalyticsSystem.UsersWindows
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
        }

        int a = 0;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            BarcodeGenerator gen = new BarcodeGenerator(EncodeTypes.QR, "https://kartinkof.club/uploads/posts/2022-03/1648267624_29-kartinkof-club-p-mem-postavte-5-29.jpg");
            gen.Parameters.Barcode.XDimension.Pixels = 34;
            string dataDir = @"C:\Users\andre\Downloads\";
            gen.Save(dataDir + a.ToString() + "1.png", BarCodeImageFormat.Png);
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(dataDir + a.ToString() + "1.png");
            bitmap.EndInit();
            image.Source = bitmap;
            a++;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
