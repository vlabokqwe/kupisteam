using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Kupisteam
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
            ShowLogin();
        }

        // ---------- Переключение вкладок ----------
        void ShowLogin()
        {
            LoginForm.Visibility = Visibility.Visible;
            RegisterForm.Visibility = Visibility.Collapsed;
            SetTab(TabLogin, true);
            SetTab(TabRegister, false);
        }

        void ShowRegister()
        {
            LoginForm.Visibility = Visibility.Collapsed;
            RegisterForm.Visibility = Visibility.Visible;
            SetTab(TabRegister, true);
            SetTab(TabLogin, false);
        }

        // активная вкладка — подсвеченный фон и белый текст
        void SetTab(System.Windows.Controls.Button tab, bool active)
        {
            tab.Background = active ? (Brush)FindResource("TabActiveBrush") : Brushes.Transparent;
            tab.Foreground = active ? Brushes.White : (Brush)FindResource("GrayText");
        }

        private void TabLogin_Click(object sender, RoutedEventArgs e) { ShowLogin(); }
        private void TabRegister_Click(object sender, RoutedEventArgs e) { ShowRegister(); }

        // ---------- Вход ----------
        private void DoLogin_Click(object sender, RoutedEventArgs e)
        {
            // Enter на форме регистрации — не обрабатываем как вход
            if (LoginForm.Visibility != Visibility.Visible) return;

            string login = LoginBox.Text.Trim();
            string password = LoginPassword.Password;

            if (login == "" || password == "")
            {
                LoginError.Text = "Заполните все поля";
                return;
            }

            User user = Data.FindUser(login);
            if (user == null || user.Password != password)
            {
                LoginError.Text = "Неверный логин или пароль";
                return;
            }

            Data.CurrentUser = user;
            Close();
        }

        // ---------- Регистрация ----------
        private void DoRegister_Click(object sender, RoutedEventArgs e)
        {
            string login = RegLogin.Text.Trim();
            string email = RegEmail.Text.Trim();
            string pass = RegPassword.Password;
            string pass2 = RegPassword2.Password;

            if (login == "" || email == "" || pass == "")
            {
                RegError.Text = "Заполните все поля";
                return;
            }
            if (login.Length < 3 || login.Contains(";"))
            {
                RegError.Text = "Логин должен быть от 3 символов (без «;»)";
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                RegError.Text = "Введите корректную почту";
                return;
            }
            if (pass.Length < 4 || pass.Contains(";"))
            {
                RegError.Text = "Пароль должен быть не короче 4 символов";
                return;
            }
            if (pass != pass2)
            {
                RegError.Text = "Пароли не совпадают";
                return;
            }
            if (Data.FindUser(login) != null)
            {
                RegError.Text = "Такой логин уже занят";
                return;
            }

            // новый пользователь получает 500 ₽ бонуса
            User user = new User { Login = login, Email = email, Password = pass, Balance = 500 };
            Data.AddUser(user);
            Data.CurrentUser = user;

            MessageBox.Show("Аккаунт создан! На ваш баланс начислено 500 ₽.", "KupiSteam",
                            MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        // ---------- Окно ----------
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // перетаскивание окна мышкой
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
