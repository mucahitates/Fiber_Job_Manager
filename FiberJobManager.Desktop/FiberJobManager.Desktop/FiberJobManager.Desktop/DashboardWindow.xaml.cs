using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FiberJobManager.Desktop.Views;
using FiberJobManager.Desktop.Models;

namespace FiberJobManager.Desktop
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
            lblUserName.Text = $"{Settings.UserName} {Settings.UserSurname}";
            lblEmail.Text = Settings.Email;

            // 🔥 YENİ: Sayaçları yükle
            LoadJobCounts();
        }

        // 🔥 YENİ: Sayaçları API'den çek
        private async void LoadJobCounts()
        {
            try
            {
                var response = await App.ApiClient.GetAsync("/api/jobs/counts");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var counts = JsonSerializer.Deserialize<JobCountsModel>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // TextBlock'ları güncelle (XAML'deki isimlere göre)
                    lblCompleted.Text = counts.CompletedJobs.ToString();
                    lblRevisions.Text = counts.RevisionJobs.ToString();
                    lblAssignedDrawings.Text = counts.NewJobs.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sayaçlar yüklenemedi: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.Role?.ToLower() != "boss")
            {
                MessageBox.Show(
                    "Bu alana yalnızca yöneticiler erişebilir.",
                    "Yetkisiz Erişim",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var win = new LinksWindow();
            win.Show();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var win = new TempDocumentsWindow();
            await win.LoadDocumentsAsync();
            win.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var win = new NewJobsWindow();
            win.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var win = new CompletedJobsWindow();
            win.Show();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var win = new RevisionJobsWindow();
            win.Show();
        }
    }
}