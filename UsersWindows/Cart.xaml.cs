using AnalyticsSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Window = System.Windows.Window;

namespace AnalyticsSystem.UsersWindows
{
    /// <summary>
    /// Логика взаимодействия для Cart.xaml
    /// </summary>
    public partial class Cart : Window
    {
        internal int idMetric;

        public int IdOrder { get; internal set; }

        public Cart()
        {
            InitializeComponent();
            LoadCartItems();
        }


        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
                if (button != null && button.Tag is int cartItemId)
                {
                    using (var context = new AnalyticsSystemEntities())
                    {
                        var itemToRemove = context.Cart.FirstOrDefault(x => x.idCart == cartItemId);
                        if (itemToRemove != null)
                        {
                            context.Cart.Remove(itemToRemove);
                            context.SaveChanges();
                            LoadCartItems();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка: Неверный формат ID элемента корзины.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при удалении элемента из корзины: " + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CreatePDF()
        {
            Document doc = new Document();
            try
            {
                string fileName = System.IO.Path.Combine("C:\\Users\\andre\\Downloads", $"order_{DateTime.Now:yyyyMMddHHmmss}.pdf");
                PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
                doc.Open();

                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font titleFont = new Font(baseFont, 18, Font.BOLD);

                doc.Add(new Paragraph("Список товаров", titleFont));

                int userId = Convert.ToInt32(App.Current.Properties["idUser"]);
                using (var context = new AnalyticsSystemEntities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.idUser == userId && o.idStatus == 2);
                    if (order != null)
                    {
                        var cartItems = context.Cart.Where(c => c.idOrder == order.idOrder).ToList();
                        decimal total = 0;

                        foreach (var item in cartItems)
                        {
                            doc.Add(new Paragraph("Название: " + item.SystemMetrics.MetricName, font));
                            doc.Add(new Paragraph("Значение: " + item.SystemMetrics.MetricValue, font));
                            doc.Add(new Paragraph("Цена: " + item.SystemMetrics.Price.ToString("F2") + " руб.", font));
                            doc.Add(new Paragraph(" ", font));
                            total += item.SystemMetrics.Price;
                        }

                        doc.Add(new Paragraph($"Итого: {total.ToString("F2")} руб.", titleFont));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании PDF: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                doc.Close();
            }
        }

        private void UpdateOrderStatus()
        {
            int userId = Convert.ToInt32(App.Current.Properties["idUser"]);
            using (var context = new AnalyticsSystemEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.idUser == userId && o.idStatus == 2);
                if (order != null)
                {
                    order.idStatus = 1;
                    context.SaveChanges();
                }
            }
        }
        private void ClearCart()
        {
            int userId = Convert.ToInt32(App.Current.Properties["idUser"]);
            using (var context = new AnalyticsSystemEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.idUser == userId && o.idStatus == 2);
                if (order != null)
                {
                    var cartItems = context.Cart.Where(c => c.idOrder == order.idOrder).ToList();
                    context.Cart.RemoveRange(cartItems);
                    context.SaveChanges();
                }
            }
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите сформировать заказ?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    CreatePDF();
                    UpdateOrderStatus();
                    ClearCart();
                    MessageBox.Show("PDF документ заказа успешно сформирован!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCartItems();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при формировании заказа: " + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void LoadCartItems()
        {
            int userId = Convert.ToInt32(App.Current.Properties["idUser"]);
            using (var context = new AnalyticsSystemEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.idUser == userId && o.idStatus == 2);
                if (order != null)
                {
                    var cartItems = context.Cart.Where(c => c.idOrder == order.idOrder)
                        .Select(c => new CartViewModel
                        {
                            id = c.idCart,
                            MetricName = c.SystemMetrics.MetricName,
                            MetricValue = c.SystemMetrics.MetricValue,
                            Price = c.SystemMetrics.Price,
                            TimeStamp = c.SystemMetrics.Timestamp,
                            ImageURL = c.SystemMetrics.ImageURL
                        }).ToList();
                    cartListView.ItemsSource = cartItems;
                }
                else
                {
                    cartListView.ItemsSource = null;
                }
            }
        }

        public class CartViewModel
        {
            public int id { get; set; }
            public string MetricName { get; set; }
            public string MetricValue { get; set; }
            public decimal Price { get; set; }
            public System.DateTime TimeStamp { get; set; }
            public string ImageURL { get; set; }
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
