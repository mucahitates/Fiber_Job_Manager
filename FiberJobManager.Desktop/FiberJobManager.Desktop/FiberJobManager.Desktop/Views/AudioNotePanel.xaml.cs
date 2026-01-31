using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FiberJobManager.Desktop.Views
{
    public partial class AudioNotesPanel : UserControl
    {
        private ObservableCollection<AudioNoteViewModel> _audioNotes;
        private MediaPlayer _mediaPlayer;
        private DispatcherTimer _progressTimer;
        private DispatcherTimer _saveTimer;
        private AudioNoteViewModel _currentlyPlaying;
        private int _currentJobId;

        // Constructor with jobId parameter
        public AudioNotesPanel(int jobId)
        {
            InitializeComponent();
            _currentJobId = jobId;
            InitializeMediaPlayer();
            InitializeAudioNotes();
        }

        // Parametre olmadan da çalışsın (tasarım modunda hata vermesin)
        public AudioNotesPanel() : this(0)
        {
        }

        private void InitializeMediaPlayer()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            _mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            _mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;

            // İlerleme timer'ı (her 100ms güncellenecek - WhatsApp gibi)
            _progressTimer = new DispatcherTimer();
            _progressTimer.Interval = TimeSpan.FromMilliseconds(100);
            _progressTimer.Tick += ProgressTimer_Tick;
        }

        private void InitializeAudioNotes()
        {
            _audioNotes = new ObservableCollection<AudioNoteViewModel>();
            AudioList.ItemsSource = _audioNotes;

            // İlgili iş için ses kayıtlarını yükle
            LoadAudioNotesForJob();
        }

        // API İÇİN HAZIR METOD
        private void LoadAudioNotesForJob()
        {
            _audioNotes.Clear();

            // TODO: API'den çek
            // var audioFiles = await _apiService.GetAudioNotesAsync(_currentJobId);
            // foreach (var audio in audioFiles)
            // {
            //     _audioNotes.Add(new AudioNoteViewModel(this)
            //     {
            //         Id = audio.Id,
            //         DisplayName = $"Sesli Not #{audio.Id}",
            //         FilePath = audio.LocalFilePath,
            //         CreatedDateTime = audio.CreatedDate,
            //         DurationSeconds = audio.DurationSeconds,
            //         CanvasWidth = 300
            //     });
            // }

            // TODO: Notları da API'den çek
            // TxtNotes.Text = await _apiService.GetJobNotesAsync(_currentJobId);

            // Şimdilik test dosyaları (İş ID'si fark etmeksizin)
            LoadTestAudioFiles();
        }

        // SİZİN DOSYALARINIZLA TEST İÇİN
        private void LoadTestAudioFiles()
        {
            string basePath = @"C:\Users\mucoa\Desktop\backend\FiberJobManager";

            var testFiles = new[]
            {
                new { Name = "Sesli Not #1", File = $@"{basePath}\ses_deneme_1.mp3" },
                new { Name = "Sesli Not #2", File = $@"{basePath}\ses_deneme_2.mp3" },
                new { Name = "Sesli Not #3", File = $@"{basePath}\ses_deneme_3.mp3" }
            };

            int id = 1;
            foreach (var file in testFiles)
            {
                if (System.IO.File.Exists(file.File))
                {
                    _audioNotes.Add(new AudioNoteViewModel(this)
                    {
                        Id = id++,
                        DisplayName = file.Name,
                        FilePath = file.File,
                        CreatedDateTime = DateTime.Now.AddMinutes(-id * 15),
                        CanvasWidth = 300 // Default genişlik, runtime'da güncellenecek
                    });
                }
                else
                {
                    MessageBox.Show($"Dosya bulunamadı: {file.File}", "Uyarı",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // API İÇİN HAZIR METOD (İLERİDE KULLANILACAK)
        public void LoadAudioNotesForJob(int jobId)
        {
            _currentJobId = jobId;
            _audioNotes.Clear();

            // TODO: API'den çek
            // var audioFiles = await _apiService.GetAudioNotesAsync(jobId);
            // foreach (var audio in audioFiles)
            // {
            //     _audioNotes.Add(new AudioNoteViewModel(this)
            //     {
            //         Id = audio.Id,
            //         DisplayName = $"Sesli Not #{audio.Id}",
            //         FilePath = audio.LocalFilePath,
            //         CreatedDateTime = audio.CreatedDate,
            //         DurationSeconds = audio.DurationSeconds,
            //         CanvasWidth = 300
            //     });
            // }

            // Şimdilik test dosyaları
            LoadTestAudioFiles();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var audioNote = button?.Tag as AudioNoteViewModel;

            if (audioNote == null) return;

            // Aynı ses kaydı mı?
            if (_currentlyPlaying == audioNote)
            {
                if (audioNote.IsPlaying)
                {
                    PauseAudio(audioNote);
                }
                else
                {
                    ResumeAudio(audioNote);
                }
            }
            else
            {
                // Başka bir ses çalıyorsa durdur
                if (_currentlyPlaying != null)
                {
                    StopAudio(_currentlyPlaying);
                }

                PlayAudio(audioNote);
            }
        }

        private void PlayAudio(AudioNoteViewModel audioNote)
        {
            try
            {
                _mediaPlayer.Open(new Uri(audioNote.FilePath));
                _mediaPlayer.Play();

                audioNote.IsPlaying = true;
                _currentlyPlaying = audioNote;
                _progressTimer.Start();

                UpdateButtonIcon(audioNote, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ses dosyası oynatılamadı: {ex.Message}", "Hata",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PauseAudio(AudioNoteViewModel audioNote)
        {
            _mediaPlayer.Pause();
            audioNote.IsPlaying = false;
            _progressTimer.Stop();

            UpdateButtonIcon(audioNote, false);
        }

        private void ResumeAudio(AudioNoteViewModel audioNote)
        {
            _mediaPlayer.Play();
            audioNote.IsPlaying = true;
            _progressTimer.Start();

            UpdateButtonIcon(audioNote, true);
        }

        private void StopAudio(AudioNoteViewModel audioNote)
        {
            _mediaPlayer.Stop();
            audioNote.IsPlaying = false;
            audioNote.CurrentPosition = 0;
            _progressTimer.Stop();

            UpdateButtonIcon(audioNote, false);
        }

        private void UpdateButtonIcon(AudioNoteViewModel audioNote, bool isPlaying)
        {
            // ItemContainer'ı bul
            var container = AudioList.ItemContainerGenerator.ContainerFromItem(audioNote) as FrameworkElement;
            if (container == null) return;

            // Button'u bul
            var button = FindVisualChild<Button>(container, "PlayPauseBtn");
            if (button == null) return;

            // Template içindeki Canvas'ı bul
            var template = button.Template;
            if (template == null) return;

            var playIcon = template.FindName("PlayIcon", button) as Canvas;
            if (playIcon == null) return;

            // İkon değiştir
            playIcon.Children.Clear();

            if (isPlaying)
            {
                // PAUSE İKONU (II)
                var pausePath1 = new System.Windows.Shapes.Rectangle
                {
                    Width = 3,
                    Height = 14,
                    Fill = Brushes.White
                };
                Canvas.SetLeft(pausePath1, 8);
                Canvas.SetTop(pausePath1, 5);

                var pausePath2 = new System.Windows.Shapes.Rectangle
                {
                    Width = 3,
                    Height = 14,
                    Fill = Brushes.White
                };
                Canvas.SetLeft(pausePath2, 13);
                Canvas.SetTop(pausePath2, 5);

                playIcon.Children.Add(pausePath1);
                playIcon.Children.Add(pausePath2);

                // Buton rengini kırmızı yap
                button.Background = new SolidColorBrush(Color.FromRgb(239, 68, 68)); // Kırmızı
            }
            else
            {
                // PLAY İKONU (▶)
                var playPath = new System.Windows.Shapes.Path
                {
                    Data = Geometry.Parse("M8 5 L8 19 L19 12 Z"),
                    Fill = Brushes.White
                };

                playIcon.Children.Add(playPath);

                // Buton rengini yeşil yap
                button.Background = new SolidColorBrush(Color.FromRgb(34, 197, 94)); // Yeşil
            }
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            if (_currentlyPlaying != null && _mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                _currentlyPlaying.DurationSeconds = (int)_mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;

                // Canvas genişliğini bul ve ayarla
                UpdateCanvasWidth(_currentlyPlaying);
            }
        }

        private void MediaPlayer_MediaFailed(object sender, ExceptionEventArgs e)
        {
            MessageBox.Show($"Medya hatası: {e.ErrorException?.Message}", "Hata",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            // Ses bitti - sonraki sese otomatik geç (WhatsApp gibi)
            if (_currentlyPlaying != null)
            {
                StopAudio(_currentlyPlaying);

                var currentIndex = _audioNotes.IndexOf(_currentlyPlaying);
                if (currentIndex < _audioNotes.Count - 1)
                {
                    // Sonraki sese geç
                    var nextAudio = _audioNotes[currentIndex + 1];

                    // Küçük bir gecikme ekle (daha doğal)
                    Task.Delay(300).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() => PlayAudio(nextAudio));
                    });
                }
            }
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (_currentlyPlaying != null)
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    _currentlyPlaying.CurrentPosition = (int)_mediaPlayer.Position.TotalSeconds;
                }
            }
        }

        // Progress bar'a tıklayınca o noktadan başlat
        private void ProgressCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentlyPlaying == null) return;

            var canvas = sender as Canvas;
            if (canvas == null) return;

            var clickPosition = e.GetPosition(canvas);
            var percentage = clickPosition.X / canvas.ActualWidth;
            var newPosition = (int)(_currentlyPlaying.DurationSeconds * percentage);

            _mediaPlayer.Position = TimeSpan.FromSeconds(newPosition);
            _currentlyPlaying.CurrentPosition = newPosition;
        }

        // Canvas genişliğini güncelle
        private void UpdateCanvasWidth(AudioNoteViewModel audioNote)
        {
            var container = AudioList.ItemContainerGenerator.ContainerFromItem(audioNote) as FrameworkElement;
            if (container == null) return;

            var canvas = FindVisualChild<Canvas>(container, "ProgressCanvas");
            if (canvas != null && canvas.ActualWidth > 0)
            {
                audioNote.CanvasWidth = canvas.ActualWidth;
            }
        }

        // Notlar değiştiğinde
        private void TxtNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Otomatik kaydetme simülasyonu
            TxtSaveStatus.Text = "Kaydediliyor...";
            TxtSaveStatus.Foreground = new SolidColorBrush(Color.FromRgb(234, 179, 8)); // Sarı

            // Debounce ile kaydetme (2 saniye sonra)
            _saveTimer?.Stop();
            _saveTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _saveTimer.Tick += (s, args) =>
            {
                _saveTimer.Stop();
                SaveNotes();
            };
            _saveTimer.Start();
        }

        private async void SaveNotes()
        {
            // TODO: API'ye kaydetme
            // await _apiService.SaveJobNotesAsync(_currentJobId, TxtNotes.Text);

            await Task.Delay(300); // Simülasyon

            TxtSaveStatus.Text = $"Otomatik kaydedildi (İş #{_currentJobId}) ✓";
            TxtSaveStatus.Foreground = new SolidColorBrush(Color.FromRgb(22, 163, 74)); // Yeşil
        }

        // Helper: Visual tree'de child bul
        private T FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild && (string.IsNullOrEmpty(childName) ||
                    (child as FrameworkElement)?.Name == childName))
                {
                    return typedChild;
                }

                var result = FindVisualChild<T>(child, childName);
                if (result != null) return result;
            }
            return null;
        }

        // Temizlik
        public void Cleanup()
        {
            _progressTimer?.Stop();
            _saveTimer?.Stop();
            _mediaPlayer?.Stop();
            _mediaPlayer?.Close();
        }
    }

    // ViewModel
    public class AudioNoteViewModel : INotifyPropertyChanged
    {
        private bool _isPlaying;
        private int _currentPosition;
        private int _durationSeconds;
        private double _canvasWidth = 300;
        private AudioNotesPanel _parent;

        public AudioNoteViewModel(AudioNotesPanel parent)
        {
            _parent = parent;
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public string CreatedDateTimeFormatted => CreatedDateTime.ToString("dd.MM.yyyy HH:mm");

        public int DurationSeconds
        {
            get => _durationSeconds;
            set
            {
                _durationSeconds = value;
                OnPropertyChanged(nameof(DurationSeconds));
                OnPropertyChanged(nameof(DurationFormatted));
                OnPropertyChanged(nameof(ProgressWidth));
            }
        }

        public string DurationFormatted => FormatTime(_durationSeconds);
        public string CurrentPositionFormatted => FormatTime(_currentPosition);

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        public int CurrentPosition
        {
            get => _currentPosition;
            set
            {
                _currentPosition = value;
                OnPropertyChanged(nameof(CurrentPosition));
                OnPropertyChanged(nameof(CurrentPositionFormatted));
                OnPropertyChanged(nameof(ProgressWidth));
            }
        }

        public double CanvasWidth
        {
            get => _canvasWidth;
            set
            {
                _canvasWidth = value;
                OnPropertyChanged(nameof(CanvasWidth));
                OnPropertyChanged(nameof(ProgressWidth));
            }
        }

        // Progress bar genişliği (WhatsApp gibi)
        public double ProgressWidth
        {
            get
            {
                if (_durationSeconds == 0) return 0;
                return (_currentPosition / (double)_durationSeconds) * _canvasWidth;
            }
        }

        private string FormatTime(int seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            return ts.TotalHours >= 1
                ? $"{(int)ts.TotalHours}:{ts:mm\\:ss}"
                : $"{ts:mm\\:ss}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}