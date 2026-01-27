using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using FiberJobManager.Desktop.Models;

namespace FiberJobManager.Desktop.Views
{
    public partial class CompletedJobsWindow : Window
    {
        public ObservableCollection<JobRowModel> Jobs { get; set; }

        public CompletedJobsWindow()
        {
            InitializeComponent();
            LoadCompletedJobsFromApi();
        }

        private async void LoadCompletedJobsFromApi()
        {
            try
            {
                // Status = "Completed" olan işleri çek
                var response = await App.ApiClient.GetAsync("/api/jobs/completed");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Tamamlanan işler alınamadı.");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var jobs = JsonSerializer.Deserialize<List<JobRowModel>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Jobs = new ObservableCollection<JobRowModel>(jobs);
                CompletedJobsGrid.ItemsSource = Jobs;
                TxtTotalCompleted.Text = Jobs.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucuya bağlanılamadı:\n" + ex.Message);
            }
        }

        // 📝 Not Görüntüle
        private async void BtnViewNote_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is JobRowModel job)
            {
                try
                {
                    var url = $"/api/jobs/{job.Id}/field-report";
                    var response = await App.ApiClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var report = JsonSerializer.Deserialize<JobFieldReportModel>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        TxtProjectNote.Text = report?.Note ?? "Not bulunamadı.";
                    }
                    else
                    {
                        TxtProjectNote.Text = "Not bulunamadı.";
                    }
                }
                catch
                {
                    TxtProjectNote.Text = "Not yüklenemedi.";
                }

                NotePopup.IsOpen = true;
            }
        }

        private void BtnCloseNote_Click(object sender, RoutedEventArgs e)
        {
            NotePopup.IsOpen = false;
        }
    }
}