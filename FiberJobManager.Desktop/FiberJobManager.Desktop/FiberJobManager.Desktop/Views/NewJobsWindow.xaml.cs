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
        // Artık sadece o an üzerinde işlem yapılan işi tutmak için kullanıyoruz
        private JobRowModel _activeJob;

        public NewJobsWindow()
        {
            InitializeComponent();
            LoadJobsFromApi();
        }

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

        // 📝 Detay Butonu - Satır seçimine gerek duymadan çalışır
        private void BtnOpenNote_Click(object sender, RoutedEventArgs e)
        {
            // Tıklanan butonu yakalıyoruz
            var button = sender as Button;

            // Butonun DataContext'i direkt olarak o satırdaki JobRowModel'dir
            if (button?.DataContext is JobRowModel clickedJob)
            {
                _activeJob = clickedJob; // Üzerinde işlem yapacağımız işi hafızaya alıyoruz
                TxtProjectNote.Text = "";
                NotePopup.IsOpen = true;
                TxtProjectNote.Focus(); // Kullanıcı hemen yazabilsin diye focus yapıyoruz
            }
        }

        private void BtnCloseNote_Click(object sender, RoutedEventArgs e)
        {
            NotePopup.IsOpen = false;
            _activeJob = null;
        }

        // 💾 Kaydet
        private async void BtnSaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (_activeJob == null)
            {
                MessageBox.Show("İşlem yapılacak proje bulunamadı.");
                return;
            }

            string noteText = TxtProjectNote.Text.Trim();

            if (noteText.Length > 150)
            {
                MessageBox.Show("Not 150 karakteri geçemez.");
                return;
            }

            var payload = new
            {
                status = _activeJob.Status, // DataGrid içindeki ComboBox'tan gelen güncel durum
                note = noteText
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var url = $"/api/jobs/{_activeJob.Id}/field-report";
                var response = await App.ApiClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Saha notu kaydedildi.");
                    NotePopup.IsOpen = false;
                    _activeJob = null;
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