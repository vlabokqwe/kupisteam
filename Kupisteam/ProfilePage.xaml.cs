using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kupisteam
{
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            Refresh();
        }

        void Refresh()
        {
            User user = Data.CurrentUser;
            if (user == null)
            {
                GuestPanel.Visibility = Visibility.Visible;
                UserPanel.Visibility = Visibility.Collapsed;
                return;
            }

            GuestPanel.Visibility = Visibility.Collapsed;
            UserPanel.Visibility = Visibility.Visible;

            AvatarLetter.Text = user.Login.Substring(0, 1).ToUpper();
            LoginText.Text = user.Login;
            EmailText.Text = user.Email;
            BalanceText.Text = user.Balance + " ₽";

            var orders = Data.GetOrders();
            OrdersList.ItemsSource = orders;
            NoOrdersText.Visibility = orders.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

            MainWindow.Instance.UpdateHeader();
        }

        private void Digits_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
                if (!char.IsDigit(c)) e.Handled = true;
        }

        private void Quick_Click(object sender, RoutedEventArgs e)
        {
            AmountBox.Text = ((Button)sender).Tag.ToString();
        }

        private void AddMoney_Click(object sender, RoutedEventArgs e)
        {
            string card = CardBox.Text.Replace(" ", "");
            int amount;
            int.TryParse(AmountBox.Text, out amount);

            if (card.Length != 16)
            {
                ErrorText.Text = "Номер карты должен содержать 16 цифр";
                return;
            }
            if (amount < 10 || amount > 100000)
            {
                ErrorText.Text = "Введите сумму от 10 до 100 000 ₽";
                return;
            }

            ErrorText.Text = "";
            Data.CurrentUser.Balance += amount;
            Data.SaveCurrentUser();

            AmountBox.Text = "";
            MainWindow.Instance.ShowToast("Баланс пополнен на " + amount + " ₽");
            Refresh();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Data.CurrentUser = null;
            MainWindow.Instance.UpdateHeader();
            MainWindow.Instance.GoCatalog();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.OpenLogin())
                Refresh();
        }
    }
}
