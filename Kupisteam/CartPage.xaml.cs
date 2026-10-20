using System;
using System.Windows;
using System.Windows.Controls;

namespace Kupisteam
{
    public partial class CartPage : Page
    {
        int promoPercent = 0; // скидка по промокоду в процентах

        public CartPage()
        {
            InitializeComponent();
            Refresh();
        }

        // Обновить список и пересчитать сумму
        void Refresh()
        {
            CartList.ItemsSource = null;
            CartList.ItemsSource = Data.Cart;

            bool empty = Data.Cart.Count == 0;
            EmptyPanel.Visibility = empty ? Visibility.Visible : Visibility.Collapsed;
            CartPanel.Visibility = empty ? Visibility.Collapsed : Visibility.Visible;

            int subtotal = Data.CartTotal();
            int discount = subtotal * promoPercent / 100;

            CountText.Text = "Товары (" + Data.CartCount() + ")";
            SubtotalText.Text = subtotal + " ₽";
            DiscountText.Text = "-" + discount + " ₽";
            TotalText.Text = (subtotal - discount) + " ₽";

            MainWindow.Instance.UpdateHeader();
        }

        int GetTotal()
        {
            int subtotal = Data.CartTotal();
            return subtotal - subtotal * promoPercent / 100;
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            CartItem item = (CartItem)((Button)sender).Tag;
            item.Count++;
            Refresh();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            CartItem item = (CartItem)((Button)sender).Tag;
            item.Count--;
            if (item.Count <= 0) Data.Cart.Remove(item);
            Refresh();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            CartItem item = (CartItem)((Button)sender).Tag;
            Data.Cart.Remove(item);
            Refresh();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Data.Cart.Clear();
            Refresh();
        }

        private void Promo_Click(object sender, RoutedEventArgs e)
        {
            string code = PromoBox.Text.Trim().ToUpper();

            if (code == "STUDENT") promoPercent = 10;
            else if (code == "KUPI20") promoPercent = 20;
            else promoPercent = 0;

            if (promoPercent > 0)
            {
                PromoInfo.Text = "Промокод применён: -" + promoPercent + "%";
                PromoInfo.Foreground = (System.Windows.Media.Brush)FindResource("GreenBrush");
            }
            else
            {
                PromoInfo.Text = "Такого промокода нет";
                PromoInfo.Foreground = System.Windows.Media.Brushes.IndianRed;
            }
            Refresh();
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            // сначала нужно войти
            if (Data.CurrentUser == null)
            {
                if (!MainWindow.Instance.OpenLogin()) return;
            }

            int total = GetTotal();
            if (Data.CurrentUser.Balance < total)
            {
                ErrorText.Text = "На балансе " + Data.CurrentUser.Balance + " ₽. Не хватает " +
                                 (total - Data.CurrentUser.Balance) + " ₽ — пополните баланс в профиле.";
                return;
            }

            // описание заказа: список игр
            string text = "";
            foreach (CartItem item in Data.Cart)
            {
                if (text != "") text += ", ";
                text += item.Game.Name + (item.Count > 1 ? " x" + item.Count : "");
            }

            Data.CurrentUser.Balance -= total;
            Data.SaveCurrentUser();
            Data.AddOrder(text, total);
            Data.Cart.Clear();

            MessageBox.Show("Заказ оформлен!\n\nКлючи отправлены на почту " + Data.CurrentUser.Email +
                            "\nСписано: " + total + " ₽", "KupiSteam", MessageBoxButton.OK, MessageBoxImage.Information);

            MainWindow.Instance.UpdateHeader();
            MainWindow.Instance.GoProfile();
        }

        private void GoCatalog_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.GoCatalog();
        }
    }
}
