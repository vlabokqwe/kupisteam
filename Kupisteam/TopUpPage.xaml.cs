using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kupisteam
{
    public partial class TopUpPage : Page
    {
        const int MinSum = 100;
        const int MaxSum = 15000;
        const int FeePercent = 5;

        public TopUpPage()
        {
            InitializeComponent();
            Calculate();
        }

        // Пересчёт суммы
        void Calculate()
        {
            int amount;
            int.TryParse(AmountBox.Text, out amount);

            int fee = amount * FeePercent / 100;
            int total = amount + fee;

            ReceiveText.Text = amount + " ₽";
            FeeText.Text = fee + " ₽";
            TotalText.Text = total + " ₽";

            if (Data.CurrentUser != null)
                UserBalanceText.Text = Data.CurrentUser.Balance + " ₽";
            else
                UserBalanceText.Text = "войдите в аккаунт";

            ErrorText.Text = "";
        }

        private void AmountBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TotalText != null) Calculate();
        }

        // Разрешаем вводить только цифры
        private void AmountBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
                if (!char.IsDigit(c)) e.Handled = true;
        }

        private void Quick_Click(object sender, RoutedEventArgs e)
        {
            AmountBox.Text = ((Button)sender).Tag.ToString();
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            int amount;
            int.TryParse(AmountBox.Text, out amount);

            if (SteamLoginBox.Text.Trim() == "")
            {
                ErrorText.Text = "Введите логин Steam";
                return;
            }
            if (amount < MinSum || amount > MaxSum)
            {
                ErrorText.Text = "Сумма должна быть от " + MinSum + " до " + MaxSum + " ₽";
                return;
            }

            // нужно войти в аккаунт
            if (Data.CurrentUser == null)
            {
                if (!MainWindow.Instance.OpenLogin()) return;
                Calculate();
            }

            int total = amount + amount * FeePercent / 100;
            if (Data.CurrentUser.Balance < total)
            {
                ErrorText.Text = "Недостаточно средств. Пополните баланс в профиле (не хватает " +
                                 (total - Data.CurrentUser.Balance) + " ₽)";
                return;
            }

            // списываем деньги
            Data.CurrentUser.Balance -= total;
            Data.SaveCurrentUser();
            Data.AddOrder("Пополнение Steam (" + SteamLoginBox.Text.Trim() + ") на " + amount + " ₽", total);

            MainWindow.Instance.UpdateHeader();
            MainWindow.Instance.ShowToast("Steam пополнен на " + amount + " ₽!");

            AmountBox.Text = "";
            Calculate();
        }
    }
}
