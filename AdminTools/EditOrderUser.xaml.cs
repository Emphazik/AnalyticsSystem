using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models;
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
using static AnalyticsSystem.AdminTools.AdminOrderEdit;

namespace AnalyticsSystem.AdminTools
{
    /// <summary>
    /// Логика взаимодействия для EditOrderUser.xaml
    /// </summary>
    public partial class EditOrderUser : Window
    {
        private OrderViewModel _orderViewModel;
        public EditOrderUser(OrderViewModel orderViewModel)
        {
            InitializeComponent();
            _orderViewModel = orderViewModel;
            LoadStatuses();
            StatusComboBox.SelectedValue = _orderViewModel.idStatus;
        }

        private void LoadStatuses()
        {
            StatusComboBox.ItemsSource = AppConnect.analyticsSystemEntities.Status.ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var selectedStatus = (Status)StatusComboBox.SelectedItem;
            if (selectedStatus != null)
            {
                var orderToUpdate = AppConnect.analyticsSystemEntities.Orders.FirstOrDefault(o => o.idOrder == _orderViewModel.idOrder);
                if (orderToUpdate != null)
                {
                    orderToUpdate.idStatus = selectedStatus.idStatus;

                    try
                    {
                        AppConnect.analyticsSystemEntities.SaveChanges();
                        MessageBox.Show("Статус успешно обновлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении статуса: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
