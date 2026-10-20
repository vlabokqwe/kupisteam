using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kupisteam
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "crash.txt", e.ExceptionObject.ToString());
            Startup += (s, e) =>
            {
                if (e.Args.Length == 0) return;
                string dir = e.Args[0];
                var t = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                int step = 0;
                t.Tick += (s2, e2) =>
                {
                    var w = Kupisteam.MainWindow.Instance;
                    Save(w, dir + "\\shot" + step + ".png");
                    step++;
                    if (step == 1) { Data.CurrentUser = new User { Login = "Vlad", Email = "vlad@mail.ru", Balance = 3500 }; Data.AddToCart(Data.Games[0]); Data.AddToCart(Data.Games[3]); w.UpdateHeader(); w.GoCart(); }
                    else if (step == 2) w.GoTopUp();
                    else if (step == 3) w.GoProfile();
                    else if (step == 4) { var a = new AuthWindow(); a.Show(); t.Tag = a; }
                    else if (step == 5) { Save((Window)t.Tag, dir + "\\auth.png"); Shutdown(); }
                };
                t.Start();
            };
        }

        static void Save(Window w, string path)
        {
            var el = (System.Windows.FrameworkElement)w.Content;
            var rtb = new System.Windows.Media.Imaging.RenderTargetBitmap((int)el.ActualWidth, (int)el.ActualHeight, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            rtb.Render(el);
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rtb));
            using (var f = System.IO.File.Create(path)) enc.Save(f);
        }
    }
}
