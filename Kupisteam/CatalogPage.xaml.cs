using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Kupisteam
{
    public partial class CatalogPage : Page
    {
        // Данные для слайдера баннеров
        string[] bannerImages = { "banner_1091500.jpg", "banner_1245620.jpg", "banner_1086940.jpg", "banner_2358720.jpg", "banner_1174180.jpg" };
        string[] bannerGames = { "Cyberpunk 2077", "ELDEN RING", "Baldur's Gate 3", "Black Myth: Wukong", "Red Dead Redemption 2" };
        string[] bannerTags = { "СКИДКА -33%", "ИГРА ГОДА", "ХИТ ПРОДАЖ", "НОВИНКА", "СКИДКА -37%" };
        string[] bannerTexts =
        {
            "Найт-Сити ждёт тебя. Открытый мир будущего, импланты и неоновые улицы.",
            "Восстань, Погасший, и стань Повелителем Элдена в огромном фэнтези-мире.",
            "Собери отряд и вернись в Забытые Королевства. Лучшая RPG последних лет.",
            "Отправься в путь Сунь Укуна по мотивам китайской мифологии.",
            "Эпическая история о жизни на Диком Западе от создателей GTA."
        };

        int currentBanner = 0;
        string currentGenre = "Все";
        DispatcherTimer timer = new DispatcherTimer();

        public CatalogPage()
        {
            InitializeComponent();

            // точки под баннером
            for (int i = 0; i < bannerImages.Length; i++)
            {
                Border dot = new Border();
                dot.Height = 8;
                dot.CornerRadius = new CornerRadius(4);
                dot.Margin = new Thickness(4, 0, 4, 0);
                Dots.Children.Add(dot);
            }

            ShowBanner(0);

            // автопрокрутка баннера каждые 5 секунд
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) => ShowBanner(currentBanner + 1);
            timer.Start();

            SelectGenreButton("Все");
            ApplyFilter();
        }

        // ---------- Баннер ----------
        void ShowBanner(int index)
        {
            if (index >= bannerImages.Length) index = 0;
            if (index < 0) index = bannerImages.Length - 1;
            currentBanner = index;

            BannerBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/" + bannerImages[index]));
            BannerTitle.Text = bannerGames[index];
            BannerTag.Text = bannerTags[index];
            BannerText.Text = bannerTexts[index];

            Game game = Data.FindGame(bannerGames[index]);
            BannerPrice.Text = game.PriceText;
            BannerOldPrice.Text = game.OldPriceText;

            // плавное появление
            DoubleAnimation fade = new DoubleAnimation(0.3, 1, TimeSpan.FromSeconds(0.6));
            BannerImage.BeginAnimation(OpacityProperty, fade);
            BannerContent.BeginAnimation(OpacityProperty, fade);

            // активная точка длиннее
            for (int i = 0; i < Dots.Children.Count; i++)
            {
                Border dot = (Border)Dots.Children[i];
                if (i == index)
                {
                    dot.Width = 28;
                    dot.Background = (Brush)FindResource("AccentBrush");
                }
                else
                {
                    dot.Width = 8;
                    dot.Background = new SolidColorBrush(Color.FromArgb(120, 255, 255, 255));
                }
            }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop(); timer.Start(); // сбросить таймер
            ShowBanner(currentBanner - 1);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop(); timer.Start();
            ShowBanner(currentBanner + 1);
        }

        private void BannerBuy_Click(object sender, RoutedEventArgs e)
        {
            Game game = Data.FindGame(bannerGames[currentBanner]);
            AddGame(game);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        // ---------- Пополнение ----------
        private void TopUpBanner_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow.Instance.GoTopUp();
        }

        private void TopUpButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.GoTopUp();
        }

        // ---------- Каталог ----------
        void ApplyFilter()
        {
            string search = SearchBox.Text.Trim().ToLower();
            List<Game> result = new List<Game>();

            foreach (Game g in Data.Games)
            {
                bool genreOk = currentGenre == "Все"
                               || g.Genre == currentGenre
                               || (currentGenre == "Скидки" && g.HasDiscount);
                bool searchOk = g.Name.ToLower().Contains(search);

                if (genreOk && searchOk)
                    result.Add(g);
            }

            GamesList.ItemsSource = result;
            FoundText.Text = "найдено: " + result.Count;
            EmptyText.Visibility = result.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        void SelectGenreButton(string genre)
        {
            foreach (Button b in GenresPanel.Children)
            {
                if ((string)b.Tag == genre)
                    b.Background = (Brush)FindResource("AccentBrush");
                else
                    b.Background = (Brush)FindResource("CardBrush");
            }
        }

        private void Genre_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            currentGenre = (string)b.Tag;
            SelectGenreButton(currentGenre);
            ApplyFilter();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Game game = (Game)b.Tag;
            AddGame(game);
        }

        void AddGame(Game game)
        {
            Data.AddToCart(game);
            MainWindow.Instance.UpdateHeader();
            MainWindow.Instance.ShowToast(game.Name + " добавлена в корзину");
        }
    }
}
