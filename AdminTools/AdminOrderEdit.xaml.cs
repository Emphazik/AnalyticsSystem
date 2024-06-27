using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models;

namespace AnalyticsSystem.AdminTools
{
    public partial class AdminOrderEdit : Window
    {
        public static AnalyticsSystemEntities analyticsSystemEntities;

        public AdminOrderEdit()
        {
            InitializeComponent();
            AppConnect.analyticsSystemEntities = new AnalyticsSystemEntities();
            LoadStatuses();
            LoadOrders();
            this.DataContext = this;
        }

        public List<Status> Statuses { get; set; }

        private void LoadOrders()
        {
            var orders = (from order in AppConnect.analyticsSystemEntities.Orders
                          join user in AppConnect.analyticsSystemEntities.Users on order.idUser equals user.idUser
                          join status in AppConnect.analyticsSystemEntities.Status on order.idStatus equals status.idStatus
                          select new OrderViewModel
                          {
                              idOrder = order.idOrder,
                              Username = user.Username,
                              idStatus = (int)order.idStatus,
                              StatusName = status.StatusName,
                              OrderDate = order.OrderDate
                          }).ToList();

            OrdersDataGrid.ItemsSource = orders;
        }

        private void LoadStatuses()
        {
            Statuses = AppConnect.analyticsSystemEntities.Status.ToList();
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderViewModel = button.Tag as OrderViewModel;

            if (orderViewModel != null)
            {
                var editWindow = new EditOrderUser(orderViewModel);
                editWindow.ShowDialog();
                LoadOrders();
            }
        }

        public class OrderViewModel
        {
            public int idOrder { get; set; }
            public string Username { get; set; }
            public int idStatus { get; set; }
            public string StatusName { get; set; }
            public DateTime OrderDate { get; set; }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void MetricsMenu_Click(object sender, RoutedEventArgs e)
        {
            new MetriksAdmin().Show();
            this.Close();
        }

        private void OrdersMenu_Click(object sender, RoutedEventArgs e)
        {
            new AdminOrderEdit().Show();
            this.Close();
        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            // Navigation code for settings menu
        }
    }
}
