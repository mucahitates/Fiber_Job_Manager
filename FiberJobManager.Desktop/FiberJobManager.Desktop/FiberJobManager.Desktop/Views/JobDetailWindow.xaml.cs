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


namespace FiberJobManager.Desktop.Views
{
    public partial class JobDetailWindow : Window
    {
        private int _jobId;

        public JobDetailWindow(int jobId)
        {
            InitializeComponent();
            _jobId = jobId;

            // İlk açılışta Fotoğraflar göster
            ShowPhotosContent();
        }

        private void BtnPhotos_Click(object sender, RoutedEventArgs e)
        {
            ShowPhotosContent();
        }

        private void BtnAudio_Click(object sender, RoutedEventArgs e)
        {
            ShowAudioContent();
        }

        private void BtnRevision_Click(object sender, RoutedEventArgs e)
        {
            ShowRevisionContent();
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            ShowUploadContent();
        }

        private void ShowPhotosContent()
        {
            ContentArea.Content = new PhotosDocumentNotePanel(_jobId);
        }

        private void ShowAudioContent()
        {
            ContentArea.Content = new AudioNotesPanel(_jobId);
        }

        private void ShowRevisionContent()
        {
            ContentArea.Content = new TextBlock
            {
                Text = $"📋 REVİZE GEÇMİŞİ\n\nİş ID: {_jobId}",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(15, 23, 42))
            };
        }

        private void ShowUploadContent()
        {
            ContentArea.Content = new TextBlock
            {
                Text = $"🖊️ ÇİZİM YÜKLEME\n\nİş ID: {_jobId}",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(15, 23, 42))
            };
        }
    }
}

