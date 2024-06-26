using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AnalyticsSystem.UsersWindows
{
    public partial class OrderList : Window
    {
        public OrderList()
        {
            InitializeComponent();
            AppConnect.analyticsSystemEntities = new AnalyticsSystemEntities();
            LoadOrders();
        }

        private void LoadOrders()
        {
            if (App.Current.Properties.Contains("idUser") && App.Current.Properties["idUser"] != null && int.TryParse(App.Current.Properties["idUser"].ToString(), out int userId))
            {
                var orders = (from order in AppConnect.analyticsSystemEntities.Orders
                              join user in AppConnect.analyticsSystemEntities.Users on order.idUser equals user.idUser
                              join status in AppConnect.analyticsSystemEntities.Status on order.idStatus equals status.idStatus
                              where order.idUser == userId
                              select new
                              {
                                  order.idOrder,
                                  Username = user.Username,
                                  StatusName = status.StatusName,
                                  order.OrderDate
                              }).ToList();

                OrdersDataGrid.ItemsSource = orders;
            }
            else
            {
                MessageBox.Show("Ошибка: Не удалось получить ID пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
