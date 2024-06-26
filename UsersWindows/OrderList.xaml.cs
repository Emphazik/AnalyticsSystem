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

namespace AnalyticsSystem.UsersWindows
{
    /// <summary>
    /// Логика взаимодействия для OrderList.xaml
    /// </summary>
    public partial class OrderList : Window
    {
        public OrderList()
        {
            InitializeComponent();
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            new Polzovatel().Show();
            this.Close();
        }

        private void MetricsMenu_Click(object sender, RoutedEventArgs e)
        {
            new Metriks().Show();
            this.Close();

        }

        private void CartMenu_Click(object sender, RoutedEventArgs e)
        {
            new Cart().Show();
            this.Close();

        }

        private void OrdersMenu_Click(object sender, RoutedEventArgs e)
        {
            new OrderList().Show();
            this.Close();

        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно Настроек

        }
    }
}
