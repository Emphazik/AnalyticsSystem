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

namespace AnalyticsSystem.AdminTools
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public Users User { get; private set; }

        public UserWindow()
        {
            InitializeComponent();
        }

        public UserWindow(Users user) : this()
        {
            User = user;
            txtLogin.Text = user.Login;
            txtUsername.Text = user.Username;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;
            cbRole.SelectedIndex = user.Role == 1 ? 0 : 1; 
            txtPassword.Password = user.Password;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (User == null)
            {
                User = new Users();
            }

            User.Login = txtLogin.Text;
            User.Username = txtUsername.Text;
            User.Email = txtEmail.Text;
            User.Phone = txtPhone.Text;
            User.Role = cbRole.SelectedIndex == 0 ? 1 : 2;
            User.Password = txtPassword.Password;

            if (!string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                User.Password = txtPassword.Password; 
            }

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateFields(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(txtLogin?.Text))
            {
                errorMessage = "Логин не должен быть пустым.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUsername?.Text))
            {
                errorMessage = "Имя не должно быть пустым.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail?.Text) || !txtEmail.Text.Contains("@"))
            {
                errorMessage = "Почта не должна быть пустой и должна содержать '@'.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone?.Text))
            {
                errorMessage = "Телефон не должен быть пустым.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPassword?.Password) && txtPassword.Password != txtConfirmPassword.Password)
            {
                errorMessage = "Пароли не совпадают.";
                return false;
            }

            return true;
        }
    }
}
