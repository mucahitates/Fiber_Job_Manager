using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using FiberJobManager.Desktop.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;



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
                var psi = new ProcessStartInfo
                {
                    FileName = selected.Url,
                    UseShellExecute = true
                };

                Process.Start(psi);

                lstLinks.SelectedIndex = -1; // tekrar tıklanabilsin
            }
        }

    }
}
