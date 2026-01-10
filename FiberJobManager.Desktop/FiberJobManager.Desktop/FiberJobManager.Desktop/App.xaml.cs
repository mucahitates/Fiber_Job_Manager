using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace FiberJobManager.Desktop
{
    public partial class App : Application
    {
        public static HttpClient ApiClient { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ApiClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5210")
            };

            // FULL QUALIFIED yazıyoruz isim çakışması olmasın
            var token = FiberJobManager.Desktop.Properties.Settings.Default.Token;

            if (!string.IsNullOrWhiteSpace(token))
            {
                ApiClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

          /* Hata yakalamak için yazıldı
           * 
           * 
           * 
           * 
           * base.OnStartup(e);

            DispatcherUnhandledException += (s, ex) =>
            {
                MessageBox.Show(
                    ex.Exception.ToString(),
                    "WPF CRASH",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                ex.Handled = true;
            };*/
        }
    }
}
