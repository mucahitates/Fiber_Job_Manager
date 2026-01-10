using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using FiberJobManager.Desktop.Models;
using System.Net.Http;

namespace FiberJobManager.Desktop.Views
{
    public partial class NewJobsWindow : Window
    {
        public ObservableCollection<JobRowModel> Jobs { get; set; }
        private JobRowModel _selectedJob;

        public NewJobsWindow()
        {
            InitializeComponent();
            LoadJobsFromApi();
        }

        // 🔹 Kullanıcıya atanmış new projeleri API’den çeker
        private async void LoadJobsFromApi()
        {
            try
            {
                var response = await App.ApiClient.GetAsync("/api/jobs/my-new");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Yetkisiz erişim veya projeler alınamadı.");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();

                var jobs = JsonSerializer.Deserialize<List<JobRowModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Jobs = new ObservableCollection<JobRowModel>(jobs);
                NewJobsGrid.ItemsSource = Jobs;
                TxtTotalNew.Text = Jobs.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucuya bağlanılamadı:\n" + ex.Message);
            }
        }

        // 🔹 Satır seçimi
        private void NewJobsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedJob = NewJobsGrid.SelectedItem as JobRowModel;
        }

        // 📝 Detay butonu
        private void BtnOpenNote_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedJob == null)
            {
                MessageBox.Show("Lütfen önce bir proje seçin.");
                return;
            }

            TxtProjectNote.Text = "";
            NotePopup.IsOpen = true;
        }

        // ❌ Popup kapat
        private void BtnCloseNote_Click(object sender, RoutedEventArgs e)
        {
            NotePopup.IsOpen = false;
        }

        // 💾 Kaydet → API → DB
        private async void BtnSaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedJob == null)
            {
                MessageBox.Show("Lütfen bir proje seçin.");
                return;
            }

            if (TxtProjectNote.Text.Length > 150)
            {
                MessageBox.Show("Not 150 karakteri geçemez.");
                return;
            }

            var payload = new
            {
                status = _selectedJob.FieldStatus,
                note = TxtProjectNote.Text
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var url = $"/api/jobs/{_selectedJob.Id}/field-report";

                var response = await App.ApiClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Saha notu kaydedildi.");
                    NotePopup.IsOpen = false;
                    TxtProjectNote.Text = "";
                }
                else
                {
                    MessageBox.Show("API hata verdi. Kayıt yapılamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucuya bağlanılamadı:\n" + ex.Message);
            }
        }
    }
}
