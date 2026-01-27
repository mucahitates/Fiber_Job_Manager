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
    public partial class RevisionJobsWindow : Window
    {
        public ObservableCollection<JobRowModel> Jobs { get; set; }
        private JobRowModel _activeJob;

        public RevisionJobsWindow()
        {
            InitializeComponent();
            LoadRevisionJobsFromApi();
        }

        private async void LoadRevisionJobsFromApi()
        {
            try
            {
                // Status = "Revision" olan işleri çek
                var response = await App.ApiClient.GetAsync("/api/jobs/my-revision");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Revize bekleyen işler alınamadı.");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var jobs = JsonSerializer.Deserialize<List<JobRowModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Jobs = new ObservableCollection<JobRowModel>(jobs);
                RevisionJobsGrid.ItemsSource = Jobs;
                TxtTotalRevision.Text = Jobs.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucuya bağlanılamadı:\n" + ex.Message);
            }
        }

        // 📝 Detay Butonu - Not popup'ını açar
        private async void BtnOpenNote_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is JobRowModel clickedJob)
            {
                _activeJob = clickedJob;

                // Revize nedenini ve personel notunu API'den çek
                try
                {
                    var url = $"/api/jobs/{_activeJob.Id}/revision-note";
                    var response = await App.ApiClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var revisionData = JsonSerializer.Deserialize<RevisionNoteModel>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        TxtRevisionReason.Text = revisionData?.RevisionReason ?? "Revize nedeni bulunamadı.";
                        TxtProjectNote.Text = revisionData?.WorkerNote ?? "";
                    }
                    else
                    {
                        TxtRevisionReason.Text = "Revize nedeni bulunamadı.";
                        TxtProjectNote.Text = "";
                    }
                }
                catch
                {
                    TxtRevisionReason.Text = "Revize nedeni yüklenemedi.";
                    TxtProjectNote.Text = "";
                }

                NotePopup.IsOpen = true;
                TxtProjectNote.Focus();
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

            // Tamamlandı seçiliyse NOT zorunlu
            if (_activeJob.FieldStatus == 2 && string.IsNullOrWhiteSpace(noteText))
            {
                MessageBox.Show("Projeyi tamamlamak için not girmelisiniz!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    if (_activeJob.FieldStatus == 2)
                    {
                        MessageBox.Show("Revize tamamlandı! 'Tamamlanan İşler' alanına taşındı.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Not kaydedildi.");
                    }

                    NotePopup.IsOpen = false;
                    _activeJob = null;

                    // Listeyi yenile
                    LoadRevisionJobsFromApi();
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