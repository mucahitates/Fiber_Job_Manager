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
using System.Net.Http;
using System.Net.Http.Json;
using FiberJobManager.Desktop.Models;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;


namespace FiberJobManager.Desktop.Views
{
    /// <summary>
    /// TempDocumentsWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class TempDocumentsWindow : Window
    {
        public TempDocumentsWindow()
        {
            InitializeComponent();
        }
        private readonly HttpClient _http = new HttpClient();

        public async Task LoadDocumentsAsync()
        {
            try
            {
                var docs = await _http.GetFromJsonAsync<List<TempDocument>>(
                    "http://localhost:5210/api/tempdocuments"
                );

                dgTempDocs.ItemsSource = docs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Evraklar yüklenirken hata oluştu:\n" + ex.Message);
            }
        }

        private void dgTempDocs_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var origin = e.OriginalSource as DependencyObject;

            // Eğer tıklanan yerde Button varsa (İndir) olayı engelleme
            if (FindParent<Button>(origin) != null)
                return;

            // Hücre / satır tıklaması ise olayı iptal et
            if (FindParent<DataGridCell>(origin) != null ||
                FindParent<DataGridRow>(origin) != null)
            {
                e.Handled = true;
            }
        }

        // Görsel ağacında yukarı doğru çıkarak parent bulmak için yardımcı metod
        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T t)
                    return t;

                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }


        private void dgTempDocs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
