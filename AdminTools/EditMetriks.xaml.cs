using AnalyticsSystem.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System;
using System.Linq;

namespace AnalyticsSystem.AdminTools
{
    public partial class EditMetriks : Window
    {
        public static AnalyticsSystemEntities analyticsSystemEntities;

        // Поле для хранения текущей редактируемой метрики
        private SystemMetrics currentMetric = null;

        public EditMetriks(SystemMetrics metric)
        {
            InitializeComponent();
            analyticsSystemEntities = new AnalyticsSystemEntities();
            LoadMetricForEdit(metric);
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
                if (currentMetric != null)
                {
                    currentMetric.MetricName = MetricNameTextBox.Text;
                    currentMetric.MetricValue = MetricValueTextBox.Text;
                    currentMetric.Timestamp = TimestampDatePicker.SelectedDate.Value;
                    currentMetric.Price = decimal.Parse(PriceTextBox.Text);
                    currentMetric.ImageURL = ImageURLTextBox.Text;

                    analyticsSystemEntities.SaveChanges();

                    var updatedMetric = currentMetric;
                    var mainWindow = Application.Current.Windows.OfType<MetriksAdmin>().FirstOrDefault();
                    if (mainWindow != null)
                    {
                        mainWindow.UpdateMetricInList(updatedMetric);
                    }

                    MessageBox.Show("Метрика успешно обновлена.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при редактировании метрики: " + ex.Message);
            }
        }


        private void LoadMetricForEdit(SystemMetrics metric)
        {
            currentMetric = metric;

            IdTextBox.Text = metric.idMetric.ToString();
            MetricNameTextBox.Text = metric.MetricName;
            MetricValueTextBox.Text = metric.MetricValue;
            TimestampDatePicker.SelectedDate = metric.Timestamp;
            PriceTextBox.Text = metric.Price.ToString();
            ImageURLTextBox.Text = metric.ImageURL;
        }
    }
}
