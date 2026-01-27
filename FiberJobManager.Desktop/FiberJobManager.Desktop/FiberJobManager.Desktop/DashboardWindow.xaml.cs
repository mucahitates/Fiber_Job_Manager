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

        // 🚪 Çıkış Yap
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Çıkış yapmak istediğinize emin misiniz?",
                "Çıkış Onayı",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                // Kullanıcı bilgilerini temizle
                Settings.UserId = 0;
                Settings.UserName = string.Empty;
                Settings.UserSurname = string.Empty;
                Settings.Email = string.Empty;
                Settings.Role = string.Empty;
                Settings.Token = string.Empty;

                // Login ekranını aç
                var loginWindow = new MainWindow();
                loginWindow.Show();

                // Dashboard'u kapat
                this.Close();
            }
        }
        // Title bar'ı sürükle
        private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        // Minimize
        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Close
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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