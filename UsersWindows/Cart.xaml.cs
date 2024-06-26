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
    /// Логика взаимодействия для Cart.xaml
    /// </summary>
    public partial class Cart : Window
    {
        internal int idMetric;

        public int IdOrder { get; internal set; }

        public Cart()
        {
            InitializeComponent();
        }


        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int itemId)
            {
                // Найдите и удалите элемент из коллекции корзины по itemId
                // Пример:
                // var itemToRemove = CartItems.FirstOrDefault(item => item.id == itemId);
                // if (itemToRemove != null)
                // {
                //     CartItems.Remove(itemToRemove);
                // }
            }
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            // Логика для формирования заказа
            // Пример:
            // MessageBox.Show("Заказ сформирован успешно!");
            // Очистка корзины после формирования заказа
            // CartItems.Clear();
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
