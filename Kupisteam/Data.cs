using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kupisteam
{
    // Игра в каталоге
    public class Game
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public int Price { get; set; }
        public int OldPrice { get; set; }
        public string Image { get; set; }

        public Game(string name, string genre, int price, int oldPrice, int steamId)
        {
            Name = name;
            Genre = genre;
            Price = price;
            OldPrice = oldPrice;
            Image = "pack://application:,,,/Images/game_" + steamId + ".jpg";
        }

        public bool HasDiscount { get { return OldPrice > Price; } }

        public string DiscountText
        {
            get
            {
                if (!HasDiscount) return "";
                int percent = 100 - Price * 100 / OldPrice;
                return "-" + percent + "%";
            }
        }

        public string PriceText { get { return Price + " ₽"; } }
        public string OldPriceText { get { return HasDiscount ? OldPrice + " ₽" : ""; } }
    }

    // Товар в корзине
    public class CartItem
    {
        public Game Game { get; set; }
        public int Count { get; set; }

        public int Sum { get { return Game.Price * Count; } }
        public string SumText { get { return Sum + " ₽"; } }
    }

    // Пользователь
    public class User
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }
    }

    // Все данные приложения (храним в обычных текстовых файлах)
    public static class Data
    {
        public static User CurrentUser = null;
        public static List<CartItem> Cart = new List<CartItem>();

        static string usersFile = AppDomain.CurrentDomain.BaseDirectory + "users.txt";
        static string ordersFile = AppDomain.CurrentDomain.BaseDirectory + "orders.txt";

        public static List<Game> Games = new List<Game>
        {
            new Game("Cyberpunk 2077", "RPG", 1999, 2999, 1091500),
            new Game("ELDEN RING", "RPG", 2499, 3499, 1245620),
            new Game("Red Dead Redemption 2", "Экшен", 1899, 2999, 1174180),
            new Game("Baldur's Gate 3", "RPG", 2299, 2299, 1086940),
            new Game("Grand Theft Auto V", "Экшен", 999, 1599, 271590),
            new Game("Hogwarts Legacy", "RPG", 1799, 3299, 990080),
            new Game("Black Myth: Wukong", "Экшен", 2899, 2899, 2358720),
            new Game("The Witcher 3", "RPG", 399, 999, 292030),
            new Game("Sekiro: Shadows Die Twice", "Экшен", 1499, 2499, 814380),
            new Game("HELLDIVERS 2", "Кооператив", 1699, 1999, 553850),
            new Game("It Takes Two", "Кооператив", 1299, 1999, 1426210),
            new Game("Dying Light 2", "Кооператив", 999, 1999, 534380),
            new Game("Lethal Company", "Кооператив", 349, 349, 1966720),
            new Game("Stardew Valley", "Инди", 299, 299, 413150),
            new Game("Hades", "Инди", 449, 749, 1145360),
            new Game("Terraria", "Инди", 199, 299, 105600),
        };

        public static Game FindGame(string name)
        {
            foreach (Game g in Games)
                if (g.Name == name) return g;
            return null;
        }

        // ---------- Корзина ----------
        public static void AddToCart(Game game)
        {
            foreach (CartItem item in Cart)
            {
                if (item.Game == game)
                {
                    item.Count++;
                    return;
                }
            }
            Cart.Add(new CartItem { Game = game, Count = 1 });
        }

        public static int CartCount()
        {
            int count = 0;
            foreach (CartItem item in Cart) count += item.Count;
            return count;
        }

        public static int CartTotal()
        {
            int total = 0;
            foreach (CartItem item in Cart) total += item.Sum;
            return total;
        }

        // ---------- Пользователи ----------
        // Формат строки: логин;почта;пароль;баланс
        public static List<User> LoadUsers()
        {
            List<User> users = new List<User>();
            if (!File.Exists(usersFile)) return users;

            foreach (string line in File.ReadAllLines(usersFile, Encoding.UTF8))
            {
                string[] p = line.Split(';');
                if (p.Length < 4) continue;
                users.Add(new User { Login = p[0], Email = p[1], Password = p[2], Balance = int.Parse(p[3]) });
            }
            return users;
        }

        public static void SaveUsers(List<User> users)
        {
            List<string> lines = new List<string>();
            foreach (User u in users)
                lines.Add(u.Login + ";" + u.Email + ";" + u.Password + ";" + u.Balance);
            File.WriteAllLines(usersFile, lines, Encoding.UTF8);
        }

        public static User FindUser(string login)
        {
            foreach (User u in LoadUsers())
                if (u.Login.ToLower() == login.ToLower()) return u;
            return null;
        }

        public static void AddUser(User user)
        {
            List<User> users = LoadUsers();
            users.Add(user);
            SaveUsers(users);
        }

        // Сохранить изменения текущего пользователя (например, баланс)
        public static void SaveCurrentUser()
        {
            List<User> users = LoadUsers();
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login == CurrentUser.Login)
                    users[i] = CurrentUser;
            }
            SaveUsers(users);
        }

        // ---------- Заказы ----------
        // Формат строки: логин;дата;описание;сумма
        public static void AddOrder(string text, int sum)
        {
            string line = CurrentUser.Login + ";" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + ";" + text + ";" + sum;
            File.AppendAllText(ordersFile, line + Environment.NewLine, Encoding.UTF8);
        }

        public static List<string[]> GetOrders()
        {
            List<string[]> result = new List<string[]>();
            if (CurrentUser == null || !File.Exists(ordersFile)) return result;

            foreach (string line in File.ReadAllLines(ordersFile, Encoding.UTF8))
            {
                string[] p = line.Split(';');
                if (p.Length == 4 && p[0] == CurrentUser.Login)
                    result.Insert(0, p); // новые сверху
            }
            return result;
        }
    }
}
