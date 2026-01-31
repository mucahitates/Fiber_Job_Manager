using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FiberJobManager.Desktop.Views
{
    public partial class DetailJobWindow : Window
    {
        private Button _selectedButton;
        private AudioNotesPanel _currentAudioPanel;

        // İş bilgileri
        private int _jobId;
        private string _hk;
        private int _nvt;
        private string _sm;

        // Constructor - NewJobWindow'dan bu bilgiler gelecek
        public DetailJobWindow(int jobId, string hk, int nvt, string sm)
        {
            InitializeComponent();

            _jobId = jobId;
            _hk = hk;
            _nvt = nvt;
            _sm = sm;

            // Sol üst paneli doldur
            LoadJobDetails();

            // İlk buton seçili gelsin (Fotoğraflar)
            SetSelectedButton(BtnFotograflar);
            ShowPhotosContent(); // veya istediğiniz ilk panel
        }

        // Sol üst iş detaylarını yükle
        private void LoadJobDetails()
        {
            TxtId.Text = _jobId.ToString();
            TxtHK.Text = _hk;
            TxtNVT.Text = _nvt.ToString();
            TxtSM.Text = _sm;
        }

        // ============================================================
        // SOL PANEL BUTON CLİCK EVENTLERİ
        // ============================================================

        private void BtnFotograflar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            SetSelectedButton(button);
            ShowPhotosContent();
        }

        private void BtnSesliNotlar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            SetSelectedButton(button);
            ShowAudioContent();
        }

        private void BtnRevizeGecmisi_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            SetSelectedButton(button);
            ShowRevisionContent();
        }

        private void BtnCizimYukleme_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            SetSelectedButton(button);
            ShowDrawingContent();
        }

        // ============================================================
        // SEÇİLİ BUTON YÖNETİMİ
        // ============================================================

        private void SetSelectedButton(Button newButton)
        {
            // Önceki seçili butonu normal renge döndür
            if (_selectedButton != null)
            {
                _selectedButton.Background = Brushes.White;
                _selectedButton.Foreground = new SolidColorBrush(Color.FromRgb(59, 130, 246)); // #3B82F6
            }

            // Yeni butonu seçili yap (Mavi arka plan, beyaz yazı)
            if (newButton != null)
            {
                newButton.Background = new SolidColorBrush(Color.FromRgb(59, 130, 246)); // #3B82F6
                newButton.Foreground = Brushes.White;
                _selectedButton = newButton;
            }
        }

        // ============================================================
        // İÇERİK PANELLERİ
        // ============================================================

        private void ShowAudioContent()
        {
            // Önceki içeriği temizle (ÖNEMLİ: Sesi durdurur)
            CleanupCurrentContent();

            // Yeni sesli notlar paneli oluştur
            _currentAudioPanel = new AudioNotesPanel(_jobId);
            ContentArea.Content = _currentAudioPanel;
        }

        private void ShowPhotosContent()
        {
            CleanupCurrentContent();

            // TODO: Fotoğraflar panelini ekle
            ContentArea.Content = new TextBlock
            {
                Text = "Fotoğraflar Paneli",
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void ShowRevisionContent()
        {
            CleanupCurrentContent();

            // TODO: Revize geçmişi panelini ekle
            ContentArea.Content = new TextBlock
            {
                Text = "Revize Geçmişi Paneli",
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void ShowDrawingContent()
        {
            CleanupCurrentContent();

            // TODO: Çizim yükleme panelini ekle
            ContentArea.Content = new TextBlock
            {
                Text = "Çizim Yükleme Paneli",
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        // ============================================================
        // TEMİZLİK (SES DURDURMA)
        // ============================================================

        private void CleanupCurrentContent()
        {
            // Eğer şu an ses paneli açıksa, sesi durdur
            if (_currentAudioPanel != null)
            {
                _currentAudioPanel.Cleanup(); // Sesi durdurur, timer'ları temizler
                _currentAudioPanel = null;
            }

            ContentArea.Content = null;
        }

        // Window kapanırken (ÖNEMLİ: Sesi durdurur)
        protected override void OnClosing(CancelEventArgs e)
        {
            CleanupCurrentContent();
            base.OnClosing(e);
        }
    }
}

// ============================================================
// NewJobWindow.xaml.cs - DETAY BUTONUNDAN ÇAĞIRMA ÖRNEĞİ
// ============================================================
/*
// DataGrid'deki Detay butonuna tıklanınca:
private void BtnDetay_Click(object sender, RoutedEventArgs e)
{
    var button = sender as Button;
    var job = button?.DataContext as JobViewModel; // Veya kendi modeliniz

    if (job != null)
    {
        // DetailJobWindow'u aç ve bilgileri gönder
        var detailWindow = new DetailJobWindow(
            jobId: job.ProjeId,      // 376
            hk: job.HK,              // "HK-N-01"
            nvt: job.NVT,            // 8001
            sm: job.SM               // "Quick-City"
        );

        detailWindow.ShowDialog(); // Modal olarak aç
        // VEYA
        // detailWindow.Show(); // Modal olmadan aç
    }
}
*/