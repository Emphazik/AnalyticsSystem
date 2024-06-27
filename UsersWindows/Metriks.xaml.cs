using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace AnalyticsSystem.UsersWindows
{
    public partial class Metriks : Window
    {
        private ObservableCollection<SystemMetrics> Metrics { get; set; }
        private ObservableCollection<SystemMetrics> FilteredMetrics { get; set; }

        public Metriks()
        {
            InitializeComponent();
            LoadMetrics();
            LoadCategories();
            AppConnect.analyticsSystemEntities = new AnalyticsSystemEntities();

            SortBox.SelectedIndex = 0;
            searchTextBox.Tag = "Поиск по названию метрики:";
        }

        private void LoadMetrics()
        {
            using (var context = new AnalyticsSystemEntities())
            {
                var metrics = context.SystemMetrics.ToList();
                Metrics = new ObservableCollection<SystemMetrics>(metrics);
                FilteredMetrics = new ObservableCollection<SystemMetrics>(metrics);
                metricsListView.ItemsSource = FilteredMetrics;
            }
        }

        private void LoadCategories()
        {
            categoryComboBox.Items.Clear();
            categoryComboBox.Items.Add(new ComboBoxItem { Content = "Все категории" });
            categoryComboBox.Items.Add(new ComboBoxItem { Content = "CPU" });
            categoryComboBox.Items.Add(new ComboBoxItem { Content = "Memory" });
            categoryComboBox.Items.Add(new ComboBoxItem { Content = "Disk" });
            categoryComboBox.SelectedIndex = 0;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Clear();
            FilteredMetrics.Clear();
            foreach (var metrics in Metrics)
            {
                FilteredMetrics.Add(metrics);
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Поиск работает по названию метрики. Введите название метрики в поле поиска.\n\n" +
                            "Сортировка позволяет упорядочить метрики по названию, значению, дате и цене. Выберите вариант сортировки в выпадающем списке и метрики будут отсортированы в соответствии с выбранным параметром.",
                            "Информация о работе поиска и сортировки",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterMetrics();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMetrics();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMetrics();
        }

        private void FilterMetrics()
        {
            string searchText = searchTextBox.Text.ToLower();
            string selectedCategory = (categoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            bool sortByNameAsc = SortBox.SelectedIndex == 1;
            bool sortByNameDesc = SortBox.SelectedIndex == 2;
            bool sortByValueAsc = SortBox.SelectedIndex == 3;
            bool sortByValueDesc = SortBox.SelectedIndex == 4;
            bool sortByDateAsc = SortBox.SelectedIndex == 5;
            bool sortByDateDesc = SortBox.SelectedIndex == 6;
            bool sortByPriceAsc = SortBox.SelectedIndex == 7;
            bool sortByPriceDesc = SortBox.SelectedIndex == 8;

            var filtered = Metrics.Where(m =>
                (selectedCategory == "Все категории" || m.MetricName.ToLower().Contains(selectedCategory.ToLower())) &&
                (string.IsNullOrEmpty(searchText) || m.MetricName.ToLower().Contains(searchText))
            );

            if (sortByNameAsc)
            {
                filtered = filtered.OrderBy(m => m.MetricName);
            }
            else if (sortByNameDesc)
            {
                filtered = filtered.OrderByDescending(m => m.MetricName);
            }
            else if (sortByValueAsc)
            {
                filtered = filtered.OrderBy(m => m.MetricValue);
            }
            else if (sortByValueDesc)
            {
                filtered = filtered.OrderByDescending(m => m.MetricValue);
            }
            else if (sortByDateAsc)
            {
                filtered = filtered.OrderBy(m => m.Timestamp);
            }
            else if (sortByDateDesc)
            {
                filtered = filtered.OrderByDescending(m => m.Timestamp);
            }
            else if (sortByPriceAsc)
            {
                filtered = filtered.OrderBy(m => m.Price);
            }
            else if (sortByPriceDesc)
            {
                filtered = filtered.OrderByDescending(m => m.Price);
            }

            FilteredMetrics.Clear();
            foreach (var item in filtered)
            {
                FilteredMetrics.Add(item);
            }
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var metric = button.DataContext;
            detailsPopup.DataContext = metric;
            detailsPopup.IsOpen = true;
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            detailsPopup.IsOpen = false;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var selectedmetric = button?.DataContext as SystemMetrics;
                if (!App.Current.Properties.Contains("idUser"))
                {
                    MessageBox.Show("Ошибка: Не удалось получить ID пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (App.Current.Properties["idUser"] == null)
                {
                    MessageBox.Show("Ошибка: ID пользователя равен null.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!int.TryParse(App.Current.Properties["idUser"].ToString(), out int userId))
                {
                    MessageBox.Show("Ошибка: Не удалось преобразовать ID пользователя в число.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (selectedmetric != null)
                {
                    using (var context = new AnalyticsSystemEntities())
                    {
                        var user = context.Users.FirstOrDefault(u => u.idUser == userId);
                        if (user == null)
                        {
                            MessageBox.Show($"Ошибка: Пользователь с ID {userId} не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var order = context.Orders.FirstOrDefault(o => o.idUser == userId && o.idStatus == 2);
                        if (order == null)
                        {
                            order = new Orders
                            {
                                idUser = userId,
                                idStatus = 2,
                                OrderDate = DateTime.Now 
                            };
                            context.Orders.Add(order);
                            context.SaveChanges();
                        }

                        var cartItem = new AnalyticsSystem.Models.Cart
                        {
                            idOrder = order.idOrder,
                            idMetric = selectedmetric.idMetric
                        };
                        context.Cart.Add(cartItem);
                        context.SaveChanges();

                        MessageBox.Show("Метрика успешно добавлена в корзину!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите метрику из списка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении метрики в корзину: " + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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