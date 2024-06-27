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

        private SystemMetrics MetricsToEdit;
        private SystemMetrics currentMetric = null;

        public EditMetriks(SystemMetrics metric)
        {
            InitializeComponent();
            analyticsSystemEntities = new AnalyticsSystemEntities();
            MetricsToEdit = metric; 
            LoadMetricForEdit();
        }

        private void LoadMetricForEdit()
        {
            if(MetricsToEdit!= null)
            {
                IdTextBox.Text = MetricsToEdit.idMetric.ToString();
                MetricNameTextBox.Text = MetricsToEdit.MetricName;
                MetricValueTextBox.Text = MetricsToEdit.MetricValue;
                TimestampDatePicker.SelectedDate = MetricsToEdit.Timestamp;
                PriceTextBox.Text = MetricsToEdit.Price.ToString();
                ImageURLTextBox.Text = MetricsToEdit.ImageURL;
            }
            
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
            if(MetricsToEdit != null)
            {
                try
                {
                    using( var context = new AnalyticsSystemEntities())
                    {
                        SystemMetrics existingMetriks = context.SystemMetrics.FirstOrDefault(s => s.idMetric == MetricsToEdit.idMetric);
                        if (existingMetriks != null)
                        {
                            existingMetriks.MetricName = MetricNameTextBox.Text;
                            existingMetriks.MetricValue = MetricValueTextBox.Text;
                            existingMetriks.Timestamp = TimestampDatePicker.SelectedDate.Value;
                            existingMetriks.Price = decimal.Parse(PriceTextBox.Text);
                            existingMetriks.ImageURL = ImageURLTextBox.Text;

                            context.SaveChanges();
                            MessageBox.Show("Метрика успешно обновлена.");
                            new MetriksAdmin().Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Метрик не найден");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при редактировании метрики: " + ex.Message);
                }
            }   
        }
    }
}