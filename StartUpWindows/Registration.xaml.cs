using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AnalyticsSystem.StartUpWindows
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
            AppConnect.analyticsSystemEntities = new AnalyticsSystemEntities();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ValidateForm(null, null);
        }

        private void btnAuthorize_Click(object sender, RoutedEventArgs e)
        {
            new Authorization().Show();
            this.Close();
        }

        public void registration_process()
        {
            try
            {
                Users user = new Users()
                {
                    Login = txtLogin.Text,
                    Username = txtName.Text,
                    Password = txtPassword.Password,
                    Role = 2,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text
                };
                AppConnect.analyticsSystemEntities.Users.Add(user);
                AppConnect.analyticsSystemEntities.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                new Authorization().Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка добавления данных",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage;

            if (!ValidateFields(out errorMessage))
            {
                MessageBox.Show(errorMessage,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AppConnect.analyticsSystemEntities.Users.Any(x => x.Login == txtLogin.Text))
            {
                MessageBox.Show("Такой пользователь уже существует!",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Неверный формат почты",
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            registration_process();
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c) && c != '+')
                {
                    return false;
                }
            }

            if (!phoneNumber.StartsWith("+"))
            {
                return false;
            }

            if (phoneNumber.Length < 10 || phoneNumber.Length > 15)
            {
                return false;
            }

            return true;
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 5 || password.Length > 20)
            {
                return false;
            }

            string invalidChars = "&^?!";

            foreach (char c in password)
            {
                if (invalidChars.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidText(string text)
        {
            string invalidChars = "&^?!";

            foreach (char c in text)
            {
                if (invalidChars.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidateFields(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(txtLogin?.Text))
            {
                errorMessage = "Логин не должен быть пустым.";
                return false;
            }
            if (!IsValidText(txtLogin.Text))
            {
                errorMessage = "Логин не должен содержать символы &^?! и т.д.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName?.Text))
            {
                errorMessage = "Имя не должно быть пустым.";
                return false;
            }
            if (!IsValidText(txtName.Text))
            {
                errorMessage = "Имя не должно содержать символы &^?! и т.д.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail?.Text))
            {
                errorMessage = "Почта не должна быть пустой.";
                return false;
            }
            if (!IsValidText(txtEmail.Text))
            {
                errorMessage = "Почта не должна содержать символы &^?! и т.д.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone?.Text))
            {
                errorMessage = "Телефон не должен быть пустым.";
                return false;
            }
            if (!IsValidText(txtPhone.Text))
            {
                errorMessage = "Телефон не должен содержать символы &^?! и т.д.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword?.Password))
            {
                errorMessage = "Пароль не должен быть пустым.";
                return false;
            }
            if (!IsValidPassword(txtPassword.Password))
            {
                errorMessage = "Пароль должен быть от 5 до 20 символов и не должен содержать символы &^?! и т.д.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmPassword?.Password))
            {
                errorMessage = "Подтверждение пароля не должно быть пустым.";
                return false;
            }
            if (!IsValidPassword(txtConfirmPassword.Password))
            {
                errorMessage = "Подтверждение пароля должно быть от 5 до 20 символов и не должно содержать символы &^?! и т.д.";
                return false;
            }

            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                errorMessage = "Пароли не совпадают.";
                txtConfirmPassword.Background = Brushes.Red;
                return false;
            }

            if (!IsValidPhoneNumber(txtPhone.Text))
            {
                errorMessage = "Телефон должен содержать только цифры и начинаться с '+'.";
                return false;
            }

            return true;
        }

        private void ValidateForm(object sender, RoutedEventArgs e)
        {
            if (txtLogin != null && txtName != null && txtEmail != null &&
                txtPhone != null && txtPassword != null && txtConfirmPassword != null)
            {
                string errorMessage;
                btnRegister.IsEnabled = ValidateFields(out errorMessage);
            }
        }
    }
}
