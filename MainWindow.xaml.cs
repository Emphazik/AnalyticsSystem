using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AnalyticsSystem.AdminTools;
using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models; // Обновите имя пространства имен при необходимости
using System.Reflection;
using Microsoft.Office.Interop.Excel;


namespace AnalyticsSystem
{
    public partial class MainWindow : System.Windows.Window
    {
        private Users CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            UsersDataGrid.ItemsSource = AppConnect.analyticsSystemEntities.Users.ToList();

            if (App.Current.Properties["idAdmin"] is int userId)
            {
                CurrentUser = AppConnect.analyticsSystemEntities.Users.FirstOrDefault(u => u.idUser == userId);

                if (CurrentUser != null)
                {
                    UserNameTextBlock.Text = $"Приветствую администратора, {CurrentUser.Username}!";

                    var notificationsFromDb = AppConnect.analyticsSystemEntities.Notifications
                                               .Where(n => n.idUser == CurrentUser.idUser)
                                               .ToList();
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

        private void txtSearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearchUser.Text.ToLower();
            var filteredUsers = AppConnect.analyticsSystemEntities.Users
                .Where(u => u.Login.ToLower().Contains(filter) ||
                            u.Username.ToLower().Contains(filter) ||
                            u.Email.ToLower().Contains(filter) ||
                            u.Phone.ToLower().Contains(filter))
                .ToList();
            UsersDataGrid.ItemsSource = filteredUsers;
        }
     

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new UserWindow(); 
            if (newUserWindow.ShowDialog() == true)
            {
                var newUser = newUserWindow.User;
                AppConnect.analyticsSystemEntities.Users.Add(newUser);
                AppConnect.analyticsSystemEntities.SaveChanges();
                LoadData();
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users selectedUser)
            {
                var editUserWindow = new UserWindow(selectedUser); 
                if (editUserWindow.ShowDialog() == true)
                {
                    var updatedUser = editUserWindow.User;
                    var user = AppConnect.analyticsSystemEntities.Users.Find(updatedUser.idUser);
                    if (user != null)
                    {
                        user.Login = updatedUser.Login;
                        user.Username = updatedUser.Username;
                        user.Email = updatedUser.Email;
                        user.Phone = updatedUser.Phone;
                        user.Role = updatedUser.Role;

                        if (!string.IsNullOrWhiteSpace(updatedUser.Password))
                        {
                            user.Password = updatedUser.Password; 
                        }

                        AppConnect.analyticsSystemEntities.SaveChanges();
                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to edit.");
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users selectedUser)
            {
                AppConnect.analyticsSystemEntities.Users.Remove(selectedUser);
                AppConnect.analyticsSystemEntities.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }

     
        private void ExportToExcel<T>(DataGrid dataGrid)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FileName = "ExportedData.xlsx"
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var application = new Microsoft.Office.Interop.Excel.Application();
                application.Visible = true;
                var workbook = application.Workbooks.Add(Missing.Value);
                var worksheet = (Worksheet)workbook.ActiveSheet;
                worksheet.Name = typeof(T).Name;

                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGrid.Columns[i].Header;
                }

                var itemsSource = dataGrid.ItemsSource.Cast<T>().ToList();
                for (int i = 0; i < itemsSource.Count; i++)
                {
                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        var bindingPath = ((Binding)dataGrid.Columns[j].ClipboardContentBinding).Path.Path;
                        var property = typeof(T).GetProperty(bindingPath);
                        worksheet.Cells[i + 2, j + 1] = property.GetValue(itemsSource[i], null);
                    }
                }

                workbook.SaveAs(saveFileDialog.FileName);
                workbook.Close(SaveChanges: false);
                application.Quit();
            }
        }



        private void ExportUsersToExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel<Users>(UsersDataGrid);
        }


        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
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
            // Navigation code for orders menu
        }

        private void SettingsMenu_Click(object sender, RoutedEventArgs e)
        {
            // Navigation code for settings menu
        }
    }
}
