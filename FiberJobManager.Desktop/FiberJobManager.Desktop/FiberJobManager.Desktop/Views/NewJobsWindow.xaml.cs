using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using FiberJobManager.Desktop.Models;
using System.Net.Http;
using FiberJobManager.Desktop.Views;

namespace FiberJobManager.Desktop.Views
{
    public partial class NewJobsWindow : Window
    {
        public ObservableCollection<JobRowModel> Jobs { get; set; }
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

        
        // ============================================================
        // 📝 NOT POPUP İŞLEMLERİ (Mevcut)
        // ============================================================
        private void BtnOpenNote_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is JobRowModel job)
            {
                try
                {
                    int.TryParse(job.NVT, out int nvtValue);

                    // DetailJobWindow'u aç ve iş bilgilerini gönder
                    var detailWindow = new DetailJobWindow(
                        jobId: job.Id,           // İş ID'si
                        hk: job.HK,     // HK bilgisi (null kontrolü)
                        nvt: nvtValue,       // NVT bilgisi
                        sm: job.SM       // SM bilgisi
                    );

                    // Modal olarak aç (window kapanana kadar bekler)
                    //detailWindow.ShowDialog();

                    // VEYA modal olmadan aç:
                    detailWindow.Show();

                    // Window kapatıldıktan sonra listeyi yenile (opsiyonel)
                    LoadJobsFromApi();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Detay penceresi açılamadı:\n{ex.Message}", "Hata",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

            // Not zorunluluğu kontrolü
            if ((_activeJob.FieldStatus == 1 || _activeJob.FieldStatus == 2) && string.IsNullOrWhiteSpace(noteText))
            {
                var statusText = _activeJob.FieldStatus == 1 ? "Yapılamıyor" : "Tamamlandı";
                MessageBox.Show($"Durumu '{statusText}' olarak işaretlemek için not girmelisiniz!",
                    "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (noteText.Length > 150)
            {
                MessageBox.Show("Not 150 karakteri geçemez.");
                return;
            }

            var payload = new
            {
                status = _activeJob.FieldStatus,
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
                    // Başarı mesajı
                    if (_activeJob.FieldStatus == 1)
                    {
                        MessageBox.Show("Proje 'Yapılamıyor' olarak işaretlendi ve revizeye alındı!",
                            "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (_activeJob.FieldStatus == 2)
                    {
                        MessageBox.Show("Proje başarıyla tamamlandı! 'Tamamlanan İşler' alanına taşındı.",
                            "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Saha notu kaydedildi.");
                    }

                    NotePopup.IsOpen = false;
                    _activeJob = null;

                    // Listeyi yenile
                    LoadJobsFromApi();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"API Hatası:\n{errorContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sunucu hatası:\n{ex.Message}");
            }
        }
    }
}