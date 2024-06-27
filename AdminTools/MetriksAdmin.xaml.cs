using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AnalyticsSystem.AdminTools
{
    /// <summary>
    /// Логика взаимодействия для MetriksAdmin.xaml
    /// </summary>
    public partial class MetriksAdmin : Window
    {
        private SystemMetrics currentMetric = null;
        private ObservableCollection<SystemMetrics> Metrics { get; set; }
        private ObservableCollection<SystemMetrics> FilteredMetrics { get; set; }

        public MetriksAdmin()
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addMetriksWindow = new AddMetriks();
            addMetriksWindow.MetricAdded += OnMetricAdded;
            addMetriksWindow.ShowDialog();
        }

        private void OnMetricAdded(object sender, EventArgs e)
        {
            // Обновление ListView после добавления метрики
            LoadMetrics();
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                SystemMetrics metric = button.DataContext as SystemMetrics;
                if (metric != null)
                {
                    var editMetriksWindow = new EditMetriks(metric);
                    editMetriksWindow.Show();
                    this.Close();

                    LoadMetrics();
                }
            }
        }

        public void UpdateMetricInList(SystemMetrics updatedMetric)
        {
            var existingMetric = Metrics.FirstOrDefault(m => m.idMetric == updatedMetric.idMetric);
            if (existingMetric != null)
            {
                existingMetric.MetricName = updatedMetric.MetricName;
                existingMetric.MetricValue = updatedMetric.MetricValue;
                existingMetric.Timestamp = updatedMetric.Timestamp;
                existingMetric.Price = updatedMetric.Price;
                existingMetric.ImageURL = updatedMetric.ImageURL;

                FilteredMetrics = new ObservableCollection<SystemMetrics>(Metrics);
                metricsListView.ItemsSource = FilteredMetrics;
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var metric = button.DataContext as SystemMetrics;
                if (metric != null)
                {
                    if (MessageBox.Show($"Вы точно хотите удалить метрик '{metric.MetricName}'?", "Подтверждение удаления",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var context = new AnalyticsSystemEntities())
                            {
                                var existingBook = context.SystemMetrics.Find(metric.idMetric);
                                if (existingBook != null)
                                {
                                    context.SystemMetrics.Remove(existingBook);
                                    context.SaveChanges();
                                    MessageBox.Show("Книга успешно удалена");
                                    LoadMetrics();
                                }
                                else
                                {
                                    MessageBox.Show("Книга не найдена в базе данных");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка удаления книги: {ex.Message}");
                        }
                    }
                }
            }

            //if (currentMetric != null)
            //{
            //    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту метрику?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        try
            //        {
            //            AppConnect.analyticsSystemEntities.SystemMetrics.Remove(currentMetric);
            //            AppConnect.analyticsSystemEntities.SaveChanges();
            //            MessageBox.Show("Метрика успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Ошибка при удалении метрики: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Нет выбранной метрики для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
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
                                OrderDate = DateTime.Now // Add a valid date here if needed
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