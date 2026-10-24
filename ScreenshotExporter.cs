using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Kupisteam;

namespace KupisteamScreenshot
{
    class Program
    {
        static void RenderToPng(FrameworkElement element, int width, int height, string filename)
        {
            element.Measure(new Size(width, height));
            element.Arrange(new Rect(0, 0, width, height));
            element.UpdateLayout();

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(element);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using (FileStream fs = File.Create(filename))
            {
                encoder.Save(fs);
            }
            Console.WriteLine("Saved: " + filename + " (" + width + "x" + height + ")");
        }

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var app = new Kupisteam.App();
                app.InitializeComponent();

                string outDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(outDir);

                // 1. AuthWindow - Login
                var auth = new AuthWindow();
                RenderToPng(auth, 480, 560, Path.Combine(outDir, "01_auth_login.png"));

                // 2. AuthWindow - Register
                var tabRegister = (Button)auth.FindName("TabRegister");
                if (tabRegister != null)
                {
                    tabRegister.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    RenderToPng(auth, 480, 560, Path.Combine(outDir, "02_auth_register.png"));
                }

                // Setup current user
                Data.CurrentUser = new User { Login = "Vladislav_ISP08", Email = "solodukhin@mail.ru", Password = "password", Balance = 5400 };

                // 3. MainWindow with Catalog
                var mainWin = new MainWindow();
                mainWin.Width = 1280;
                mainWin.Height = 820;
                mainWin.UpdateHeader();
                RenderToPng(mainWin, 1280, 820, Path.Combine(outDir, "03_catalog_page.png"));

                // 4. TopUpPage
                mainWin.GoTopUp();
                RenderToPng(mainWin, 1280, 820, Path.Combine(outDir, "04_topup_page.png"));

                // 5. CartPage (add some items first)
                Data.Cart.Clear();
                Data.AddToCart(Data.Games[0]); // Cyberpunk 2077
                Data.AddToCart(Data.Games[1]); // ELDEN RING
                mainWin.GoCart();
                RenderToPng(mainWin, 1280, 820, Path.Combine(outDir, "05_cart_page.png"));

                // 6. ProfilePage
                Data.AddOrder("Cyberpunk 2077, ELDEN RING", 4498);
                Data.AddOrder("Пополнение Steam (solodukhin_steam) на 1000 ₽", 1050);
                mainWin.GoProfile();
                RenderToPng(mainWin, 1280, 820, Path.Combine(outDir, "06_profile_page.png"));

                Console.WriteLine("ALL_SCREENSHOTS_DONE");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }
        }
    }
}
