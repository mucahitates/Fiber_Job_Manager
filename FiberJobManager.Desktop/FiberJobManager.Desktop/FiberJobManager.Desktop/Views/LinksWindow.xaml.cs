using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using FiberJobManager.Desktop.Models;

namespace FiberJobManager.Desktop.Views
{
    public partial class LinksWindow : Window
    {
        private readonly HttpClient _http;

        public LinksWindow()
        {
            InitializeComponent();

            _http = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5210")
            };

            // 🔐 Login sonrası kaydedilen JWT burada header’a giriliyor
            var token = FiberJobManager.Desktop.Properties.Settings.Default.Token;

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var links = await _http.GetFromJsonAsync<List<LinkModel>>("/api/links");
                var people = await _http.GetFromJsonAsync<List<PersonModel>>("/api/people");

                lstLinks.ItemsSource = links;
                lstPeople.ItemsSource = people;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yüklenirken hata oluştu:\n" + ex.Message);
            }
        }

        private void lstLinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstLinks.SelectedItem is LinkModel selected)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = selected.Url,
                    UseShellExecute = true
                });

                lstLinks.SelectedIndex = -1;
            }
        }
    }
}
