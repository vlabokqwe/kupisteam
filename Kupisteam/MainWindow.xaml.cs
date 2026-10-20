using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Kupisteam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Чтобы страницы могли обращаться к главному окну
        public static MainWindow Instance;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            MainFrame.Navigate(new CatalogPage());
            UpdateHeader();
        }

        // Обновить шапку: корзина, баланс, кнопка входа
        public void UpdateHeader()
        {
            int count = Data.CartCount();
            CartCountText.Text = count.ToString();
            CartBadge.Visibility = count > 0 ? Visibility.Visible : Visibility.Collapsed;

            if (Data.CurrentUser != null)
            {
                LoginButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Visible;
                BalancePanel.Visibility = Visibility.Visible;
                ProfileName.Text = Data.CurrentUser.Login;
                BalanceText.Text = Data.CurrentUser.Balance + " ₽";
            }
            else
            {
                LoginButton.Visibility = Visibility.Visible;
                ProfileButton.Visibility = Visibility.Collapsed;
                BalancePanel.Visibility = Visibility.Collapsed;
            }
        }

        // Показать уведомление в правом нижнем углу
        public void ShowToast(string text)
        {
            ToastText.Text = text;
            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2))));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2.2))));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2.6))));
            Toast.BeginAnimation(OpacityProperty, anim);
        }

        // Открыть окно входа. Возвращает true, если пользователь вошёл
        public bool OpenLogin()
        {
            AuthWindow auth = new AuthWindow();
            auth.Owner = this;
            auth.ShowDialog();
            UpdateHeader();
            return Data.CurrentUser != null;
        }

        public void GoCatalog() { MainFrame.Navigate(new CatalogPage()); }
        public void GoTopUp() { MainFrame.Navigate(new TopUpPage()); }
        public void GoCart() { MainFrame.Navigate(new CartPage()); }
        public void GoProfile() { MainFrame.Navigate(new ProfilePage()); }

        private void Logo_Click(object sender, System.Windows.Input.MouseButtonEventArgs e) { GoCatalog(); }
        private void NavCatalog_Click(object sender, RoutedEventArgs e) { GoCatalog(); }
        private void NavTopUp_Click(object sender, RoutedEventArgs e) { GoTopUp(); }
        private void NavCart_Click(object sender, RoutedEventArgs e) { GoCart(); }
        private void ProfileButton_Click(object sender, RoutedEventArgs e) { GoProfile(); }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpenLogin())
                ShowToast("Добро пожаловать, " + Data.CurrentUser.Login + "!");
        }
    }
}
