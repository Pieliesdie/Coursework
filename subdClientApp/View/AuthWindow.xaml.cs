using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace subdClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        Brush _default;

        public AuthWindow()
        {
            InitializeComponent();
            _default = GuestButton.Background;
        }

        private void Button_Click(object sender, RoutedEventArgs e) => AuthButtonHandlerAsync(LoginText.Text, PasswordText.Password);

        private void Chip_Click(object sender, RoutedEventArgs e) => AuthButtonHandlerAsync("GuestUser", "Guest");

        private async void AuthButtonHandlerAsync(string name, string pass)
        {
            GuestButton.Opacity = 0.5;
            MainContainer.IsEnabled = false;
            Progress.Visibility = Visibility.Visible;
            if (await AuthAsync(name, pass))
            {
                if (name != "GuestUser")
                {
                    if (RememberMe.IsChecked ?? false)
                    {
                        Properties.Settings.Default.Login = name;
                        Properties.Settings.Default.Password = pass;
                        Properties.Settings.Default.IsRemember = true;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.Login = null;
                        Properties.Settings.Default.Password = null;
                        Properties.Settings.Default.IsRemember = false;
                        Properties.Settings.Default.Save();
                    }
                }
                App.Name = name;
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                GuestButton.Opacity = 1;
                Progress.Visibility = Visibility.Hidden;
                _ = new MessageBlock("Wrong password or name") { Owner = this }.ShowDialog();
                MainContainer.IsEnabled = true;
            }
        }

        private bool Auth(string name, string password)
        {
            try
            {
                App.dataReader = new DataReader(App.dataSource, name, password);
                return App.dataReader.CanConnect;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async ValueTask<bool> AuthAsync(string name, string password) => await Task.Run(() => Auth(name, password));


        private void GuestButton_MouseEnter(object sender, MouseEventArgs e)
        {
            GuestButton.Background = Brushes.LightGray;
        }

        private void GuestButton_MouseLeave(object sender, MouseEventArgs e)
        {
            GuestButton.Background = _default;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.IsRemember)
            {
                LoginText.Text = Properties.Settings.Default.Login ?? string.Empty;
                PasswordText.Password = Properties.Settings.Default.Password ?? string.Empty;
                RememberMe.IsChecked = Properties.Settings.Default.IsRemember;
            }
        }
    }


    public class NotEmptyValidationRule : ValidationRule
    {
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(false, Message ?? "A value is required");
            }
            return ValidationResult.ValidResult;
        }
    }
}
