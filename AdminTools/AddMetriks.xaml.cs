using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models;
using System;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using AnalyticsSystem.UsersWindows;
using System.IO;

namespace AnalyticsSystem.AdminTools
{
    public partial class AddMetriks : Window
    {
        public static AnalyticsSystemEntities analyticsSystemEntities;
        public event EventHandler MetricAdded;
        public AddMetriks()
        {
            InitializeComponent();
            AppConnect.analyticsSystemEntities = new AnalyticsSystemEntities();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
             this.Close();
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", fileName);

                try
                {
                    string resoursesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                    if (!Directory.Exists(resoursesFolder))
                    {
                        Directory.CreateDirectory(resoursesFolder);
                    }

                    File.Copy(filePath, destPath, true);
                    ImageURLTextBox.Text = $"/Resources/{fileName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копировании изображения: {ex.Message}");
                }
            }
        }

        private void AddMetric_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemMetrics newMetric = new SystemMetrics
                {
                    MetricName = MetricNameTextBox.Text,
                    MetricValue = MetricValueTextBox.Text,
                    Timestamp = TimestampDatePicker.SelectedDate.Value,
                    Price = decimal.Parse(PriceTextBox.Text),
                    ImageURL = ImageURLTextBox.Text
                };

                AppConnect.analyticsSystemEntities.SystemMetrics.Add(newMetric);
                AppConnect.analyticsSystemEntities.SaveChanges();
                MessageBox.Show("Метрик успешно добавлен.");
                ClearFields();
                MetricAdded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении метрика: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            MetricNameTextBox.Clear();
            MetricValueTextBox.Clear();
            PriceTextBox.Clear();
            ImageURLTextBox.Clear();
        }
    }
}
