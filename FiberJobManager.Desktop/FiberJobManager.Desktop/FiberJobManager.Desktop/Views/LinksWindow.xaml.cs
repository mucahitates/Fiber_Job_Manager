using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using FiberJobManager.Desktop.Models;

namespace FiberJobManager.Desktop.Views
{
    public partial class LinksWindow : Window
    {
        public LinksWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var links = await App.ApiClient.GetFromJsonAsync<List<LinkModel>>("/api/links");
                var people = await App.ApiClient.GetFromJsonAsync<List<PersonModel>>("/api/people");

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
            if (lstLinks.SelectedItem is LinkModel selected && !string.IsNullOrWhiteSpace(selected.Url))
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
