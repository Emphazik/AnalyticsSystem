using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models;

namespace AnalyticsSystem.UsersWindows
{
    public partial class Polzovatel : Window
    {
        public ObservableCollection<Notifications> NotificationsCollection { get; set; }
        private Users CurrentUser { get; set; }

        public Polzovatel()
        {
            InitializeComponent();
            NotificationsCollection = new ObservableCollection<Notifications>();
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            // Получаем id текущего пользователя из Application Properties
            if (App.Current.Properties["idUser"] is int userId)
            {
                CurrentUser = AppConnect.analyticsSystemEntities.Users.FirstOrDefault(u => u.idUser == userId);

                if (CurrentUser != null)
                {
                    UserNameTextBlock.Text = $"Привет, {CurrentUser.Username}!";

                    var notificationsFromDb = AppConnect.analyticsSystemEntities.Notifications
                                               .Where(n => n.idUser == CurrentUser.idUser)
                                               .ToList();

                    foreach (var notification in notificationsFromDb)
                    {
                        NotificationsCollection.Add(notification);
                    }

                    NotificationsItemsControl.ItemsSource = NotificationsCollection;
                }
                else
                {
                    UserNameTextBlock.Text = "Пользователь не найден.";
                }
            }
            else
            {
                UserNameTextBlock.Text = "Не удалось определить текущего пользователя.";
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
