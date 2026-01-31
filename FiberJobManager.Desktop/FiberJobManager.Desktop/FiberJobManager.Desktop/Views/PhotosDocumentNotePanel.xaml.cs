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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FiberJobManager.Desktop.Views
{
    public partial class PhotosDocumentNotePanel : UserControl
    {
        private int _jobId;
        private bool _isPhotoLocked = false;
        private Border _currentPhotoBorder = null;

        public PhotosDocumentNotePanel(int jobId)
        {
            InitializeComponent();
            _jobId = jobId;

            // Canvas'ı pencere boyutuna ayarla
            this.Loaded += (s, e) => ResizeOverlay();
            this.SizeChanged += (s, e) => ResizeOverlay();

            // Canvas'a tıklayınca kapat
            OverlayCanvas.MouseDown += OverlayCanvas_MouseDown;
        }

        private void ResizeOverlay()
        {
            OverlayCanvas.Width = this.ActualWidth;
            OverlayCanvas.Height = this.ActualHeight;

            // Overlay'i ortala
            if (BigPhotoOverlay != null)
            {
                Canvas.SetLeft(BigPhotoOverlay, (this.ActualWidth - 800) / 2);
                Canvas.SetTop(BigPhotoOverlay, (this.ActualHeight - 1000) / 2);
            }
        }

        // FOTOĞRAF HOVER - Büyük göster
        private void Photo_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_isPhotoLocked) return; // Kilitliyse hover çalışmasın

            var border = sender as Border;
            if (border != null)
            {
                _currentPhotoBorder = border;
                ShowBigPhoto(border);
            }
        }

        // FOTOĞRAF LEAVE - Gizle (eğer kilitli değilse)
        private void Photo_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_isPhotoLocked) return;

            HideBigPhoto();
        }

        // FOTOĞRAF CLICK - Kilitle/Aç
        private void Photo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            e.Handled = true; // Event'in Canvas'a geçmesini engelle

            if (_isPhotoLocked)
            {
                // Zaten kilitli - aç
                _isPhotoLocked = false;
                HideBigPhoto();
            }
            else
            {
                // Kilitle
                _isPhotoLocked = true;
                _currentPhotoBorder = border;
                ShowBigPhoto(border);
            }
        }

        // Canvas'a (dışarıya) tıklayınca kapat
        private void OverlayCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isPhotoLocked)
            {
                _isPhotoLocked = false;
                HideBigPhoto();
            }
        }

        // Büyük fotoğrafı göster
        private void ShowBigPhoto(Border photoBorder)
        {
            // TODO: Gerçek fotoğrafı yükle
            // string photoPath = photoBorder.Tag.ToString();
            // BigPhotoImage.Source = new BitmapImage(new Uri(photoPath));

            // Şimdilik placeholder
            var rect = new System.Windows.Shapes.Rectangle
            {
                Fill = Brushes.LightGray,
                Width = 780,
                Height = 980
            };

            OverlayCanvas.Visibility = Visibility.Visible;
        }

        // Büyük fotoğrafı gizle
        private void HideBigPhoto()
        {
            OverlayCanvas.Visibility = Visibility.Collapsed;
            _currentPhotoBorder = null;
        }

        // PDF İndirme
        private void PDFDownload_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("PDF indirme fonksiyonu eklenecek", "İndirme", MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO: API'den PDF indir
        }
    }
}
