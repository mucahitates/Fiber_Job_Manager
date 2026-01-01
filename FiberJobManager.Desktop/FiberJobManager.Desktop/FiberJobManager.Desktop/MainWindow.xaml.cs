using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FiberJobManager.Desktop.Properties;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using FiberJobManager.Desktop.Models;
using Newtonsoft.Json;

namespace FiberJobManager.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly Brush _placeholderBrush = new SolidColorBrush(Color.FromRgb(156, 163, 175));
        private readonly Brush _textBrush = new SolidColorBrush(Color.FromRgb(75, 85, 99));
        private static readonly HttpClient _httpClient =
             new HttpClient { BaseAddress = new Uri("http://localhost:5210") }; // API PORTUNA DIKKAT


        public MainWindow()
        {
            InitializeComponent();
            SetupPlaceholders();
        }

        private void SetupPlaceholders()
        {
            // Basit placeholder: Email address
            txtEmail.Text = "Email address";
            txtEmail.Foreground = _placeholderBrush;
        }

        private void Email_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Foreground == _placeholderBrush)
            {
                txtEmail.Text = string.Empty;
                txtEmail.Foreground = _textBrush;
            }
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Email address";
                txtEmail.Foreground = _placeholderBrush;
            }
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            lblInfo.Text = "Giriş yapılıyor...";

            var email = txtEmail.Text?.Trim();
            var password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                lblInfo.Text = "Lütfen email ve şifrenizi girin.";
                return;
            }

            try
            {
                lblInfo.Text = "Giriş yapılıyor...";

                var payload = new
                {
                    Email = txtEmail.Text,
                    Password = txtPassword.Password
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:5210");

                var response = await client.PostAsync("/api/Auth/login", content);
                var respJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    lblInfo.Text = "Email veya şifre hatalı.";
                    return;
                }

                var data = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(respJson);

                // bilgiler kaydediliyor
                //Settings.JwtToken = data.Token;
                Settings.UserId = data.UserId;
                Settings.UserName = data.UserName;
                Settings.Role = data.Role;
               // Settings.Save();

                // SADECE BAŞARIDA basit mesaj
                MessageBox.Show(
                    $"Hoş geldin {data.UserName}\nRolün: {data.Role}",
                    "Giriş Başarılı",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                //dashboard açılıyor
                var dashboard = new DashboardWindow();
                dashboard.Show();

                // login penceresi kapanıyor
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "GENEL HATA",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

        }




        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pwdPlaceholder.Visibility = Visibility.Hidden;
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Password))
                pwdPlaceholder.Visibility = Visibility.Visible;
        }

    }
}


