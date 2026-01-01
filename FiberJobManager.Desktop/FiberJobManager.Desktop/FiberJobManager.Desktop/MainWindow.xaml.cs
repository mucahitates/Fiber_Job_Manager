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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FiberJobManager.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly Brush _placeholderBrush = new SolidColorBrush(Color.FromRgb(156, 163, 175));
        private readonly Brush _textBrush = new SolidColorBrush(Color.FromRgb(75, 85, 99));

        public MainWindow()
        {
            InitializeComponent();
            SetupPlaceholders();
        }

        private void SetupPlaceholders()
        {
            // Basit placeholder: Email address
            txtEmail.Text = "Email address";
            txtEmail.Foreground = _placeholderBrush;
        }

        private void Email_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Foreground == _placeholderBrush)
            {
                txtEmail.Text = string.Empty;
                txtEmail.Foreground = _textBrush;
            }
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Email address";
                txtEmail.Foreground = _placeholderBrush;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            lblInfo.Text = "Checking credentials...";

            var emailOrUser = txtEmail.Foreground == _placeholderBrush ? string.Empty : txtEmail.Text;
            var password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(emailOrUser) || string.IsNullOrWhiteSpace(password))
            {
                lblInfo.Text = "Lütfen Email ve Şifrenizi Giriniz.";
                return;
            }

            // Buraya daha sonra: API çağrısı gelecek
            // Şimdilik sadece demo mesajı:
            lblInfo.Text = $"Welcome, {emailOrUser} (demo login).";
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pwdPlaceholder.Visibility = Visibility.Hidden;
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Password))
                pwdPlaceholder.Visibility = Visibility.Visible;
        }

    }
}


