using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;
using FiberJobManager.Desktop.Models;

namespace FiberJobManager.Desktop.Views
{
    public partial class NewJobsWindow : Window
    {
        public ObservableCollection<JobRowModel> Jobs { get; set; }

        public NewJobsWindow()
        {
            InitializeComponent();
            LoadFakeData();
            NewJobsGrid.ItemsSource = Jobs;
            TxtTotalNew.Text = Jobs.Count.ToString();
        }

        private void LoadFakeData()
        {
            Jobs = new ObservableCollection<JobRowModel>
            {
                new JobRowModel{ Id=101, Firma="Berlin Mitte", Bolge="Berlin", HK="HK12", SM="SM5", NVT="NVT-1", Erstmessung="01.01.2026", FieldStatus=0 },
                new JobRowModel{ Id=102, Firma="Hamburg Nord", Bolge="Hamburg", HK="HK7", SM="SM2", NVT="NVT-3", Erstmessung="02.01.2026", FieldStatus=1 },
                new JobRowModel{ Id=103, Firma="Munich Süd", Bolge="Munich", HK="HK3", SM="SM9", NVT="NVT-7", Erstmessung="03.01.2026", FieldStatus=2 }
            };
        }

        private void OpenDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            var job = (sender as FrameworkElement).Tag as JobRowModel;

            MessageBox.Show(
                $"Proje {job.Id} için detay girilecek.\n(Demo modundayız)",
                "Detay",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        // Bu metodun ismi XAML'deki Click="BtnOpenNote_Click" ile birebir aynı olmalı
        private void BtnOpenNote_Click(object sender, RoutedEventArgs e)
        {
            // Popup'ı görünür kılan komut budur
            NotePopup.IsOpen = true;
        }

        // İptal butonu için
        private void BtnCloseNote_Click(object sender, RoutedEventArgs e)
        {
            NotePopup.IsOpen = false;
        }
    }
}

