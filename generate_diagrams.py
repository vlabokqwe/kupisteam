import os
import matplotlib
matplotlib.use('Agg')
import matplotlib.pyplot as plt
import matplotlib.patches as patches

# Directory for diagrams
out_dir = r"E:\ucheba\Kupisteam\Kupisteam\bin\Debug\Screenshots"
os.makedirs(out_dir, exist_ok=True)

plt.rcParams['font.sans-serif'] = ['DejaVu Sans', 'Arial', 'Segoe UI']
plt.rcParams['axes.unicode_minus'] = False

# -----------------------------------------------------------------------------
# 1. ARCHITECTURE DIAGRAM
# -----------------------------------------------------------------------------
def make_architecture_diagram():
    fig, ax = plt.subplots(figsize=(10, 6.5), dpi=300)
    ax.set_facecolor('#171A21')
    fig.patch.set_facecolor('#171A21')
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 7)
    ax.axis('off')

    # Title
    ax.text(5, 6.6, "Архитектура программного комплекса «KupiSteam»", 
            color='#66C0F4', fontsize=14, weight='bold', ha='center')

    # Layer 1: Presentation Layer
    p1 = patches.FancyBboxPatch((0.5, 4.3), 9, 1.9, boxstyle="round,pad=0.15", 
                                ec='#2D73A0', fc='#1B2838', lw=2)
    ax.add_patch(p1)
    ax.text(0.8, 5.9, "1. УРОВЕНЬ ПРЕДСТАВЛЕНИЯ (XAML / WPF Presentation Layer)", 
            color='#C7D5E0', fontsize=11, weight='bold')

    # Boxes inside Layer 1
    ui_boxes = [
        ("MainWindow\n(Навигация, Шапка,\nToast-нотификации)", 0.8, 4.5, 2.5),
        ("AuthWindow\n(Форма входа и\nрегистрация)", 3.6, 4.5, 2.5),
        ("Страницы (Pages)\n• CatalogPage • CartPage\n• TopUpPage • ProfilePage", 6.4, 4.5, 2.8)
    ]
    for text, x, y, w in ui_boxes:
        b = patches.FancyBboxPatch((x, y), w, 1.2, boxstyle="round,pad=0.1", ec='#4B617A', fc='#213247', lw=1.5)
        ax.add_patch(b)
        ax.text(x + w/2, y + 0.6, text, color='#E5E4DC', fontsize=8.5, ha='center', va='center')

    # Layer 2: Business Logic Layer
    p2 = patches.FancyBboxPatch((0.5, 2.2), 9, 1.7, boxstyle="round,pad=0.15", 
                                ec='#5C7E10', fc='#1B2838', lw=2)
    ax.add_patch(p2)
    ax.text(0.8, 3.6, "2. УРОВЕНЬ БИЗНЕС-ЛОГИКИ (Application Logic & Services)", 
            color='#A4D007', fontsize=11, weight='bold')

    logic_boxes = [
        ("Менеджер корзины\n(Добавление, пересчёт,\nпромокоды STUDENT/KUPI20)", 0.8, 2.4, 2.7),
        ("Сервис пользователей\n(Валидация, сессия,\nбаланс, хеш паролей)", 3.8, 2.4, 2.7),
        ("Платёжный калькулятор\n(Комиссия 5%, списание,\nгенерация заказов)", 6.8, 2.4, 2.4)
    ]
    for text, x, y, w in logic_boxes:
        b = patches.FancyBboxPatch((x, y), w, 1.0, boxstyle="round,pad=0.1", ec='#6B961B', fc='#213824', lw=1.5)
        ax.add_patch(b)
        ax.text(x + w/2, y + 0.5, text, color='#D2EFA9', fontsize=8.5, ha='center', va='center')

    # Layer 3: Data & Storage Layer
    p3 = patches.FancyBboxPatch((0.5, 0.4), 9, 1.4, boxstyle="round,pad=0.15", 
                                ec='#8F98A0', fc='#1B2838', lw=2)
    ax.add_patch(p3)
    ax.text(0.8, 1.5, "3. УРОВЕНЬ ДАННЫХ И ХРАНЕНИЯ (Data Persistence Layer)", 
            color='#C5C3C0', fontsize=11, weight='bold')

    data_boxes = [
        ("In-Memory Модели\nGame, CartItem, User", 0.8, 0.6, 2.5),
        ("Статический класс Data.cs\n(Централизованный доступ)", 3.6, 0.6, 2.8),
        ("Файлы данных (UTF-8)\nusers.txt | orders.txt", 6.7, 0.6, 2.5)
    ]
    for text, x, y, w in data_boxes:
        b = patches.FancyBboxPatch((x, y), w, 0.75, boxstyle="round,pad=0.08", ec='#636E7B', fc='#262C38', lw=1.5)
        ax.add_patch(b)
        ax.text(x + w/2, y + 0.37, text, color='#C7D5E0', fontsize=8.5, ha='center', va='center')

    # Connectors between layers
    ax.annotate("", xy=(5.0, 4.3), xytext=(5.0, 3.9), arrowprops=dict(arrowstyle="<->", color='#66C0F4', lw=2.5))
    ax.annotate("", xy=(5.0, 2.2), xytext=(5.0, 1.8), arrowprops=dict(arrowstyle="<->", color='#A4D007', lw=2.5))

    plt.tight_layout()
    path = os.path.join(out_dir, "diagram_architecture.png")
    plt.savefig(path, facecolor=fig.get_facecolor(), edgecolor='none')
    plt.close()
    print("Created:", path)

# -----------------------------------------------------------------------------
# 2. USE CASE DIAGRAM
# -----------------------------------------------------------------------------
def make_usecase_diagram():
    fig, ax = plt.subplots(figsize=(10, 7), dpi=300)
    ax.set_facecolor('#171A21')
    fig.patch.set_facecolor('#171A21')
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 8)
    ax.axis('off')

    ax.text(5, 7.6, "Диаграмма прецедентов (Use Case Diagram)", 
            color='#66C0F4', fontsize=14, weight='bold', ha='center')

    # Actors
    # Actor 1: Guest
    ax.plot([1.2, 1.2], [5.9, 5.4], color='#C5C3C0', lw=2.5) # body
    ax.plot([0.9, 1.5], [5.7, 5.7], color='#C5C3C0', lw=2.5) # arms
    ax.plot([0.9, 1.2, 1.5], [4.9, 5.4, 4.9], color='#C5C3C0', lw=2.5) # legs
    head1 = patches.Circle((1.2, 6.2), 0.25, color='#66C0F4')
    ax.add_patch(head1)
    ax.text(1.2, 4.6, "Гость\n(Неавторизованный)", color='#C7D5E0', fontsize=9, ha='center', weight='bold')

    # Actor 2: Registered User
    ax.plot([1.2, 1.2], [2.7, 2.2], color='#C5C3C0', lw=2.5) # body
    ax.plot([0.9, 1.5], [2.5, 2.5], color='#C5C3C0', lw=2.5) # arms
    ax.plot([0.9, 1.2, 1.5], [1.7, 2.2, 1.7], color='#C5C3C0', lw=2.5) # legs
    head2 = patches.Circle((1.2, 3.0), 0.25, color='#A4D007')
    ax.add_patch(head2)
    ax.text(1.2, 1.4, "Авторизованный\nпользователь", color='#D2EFA9', fontsize=9, ha='center', weight='bold')

    # System boundary box
    sys_box = patches.FancyBboxPatch((3.0, 0.4), 6.5, 7.0, boxstyle="round,pad=0.15", 
                                     ec='#2D73A0', fc='#1B2838', lw=2)
    ax.add_patch(sys_box)
    ax.text(6.25, 7.1, "Подсистема приложения «KupiSteam»", color='#66C0F4', fontsize=11, weight='bold', ha='center')

    cases = [
        ("Просмотр каталога и слайдера", 6.25, 6.5, ['guest', 'user']),
        ("Поиск и фильтрация по жанрам", 6.25, 5.7, ['guest', 'user']),
        ("Добавление игр в корзину", 6.25, 4.9, ['guest', 'user']),
        ("Регистрация и вход в систему", 6.25, 4.1, ['guest']),
        ("Применение промокода скидки", 6.25, 3.3, ['user']),
        ("Оформление заказа (покупка игр)", 6.25, 2.5, ['user']),
        ("Пополнение кошелька Steam", 6.25, 1.7, ['user']),
        ("Пополнение баланса картой и история", 6.25, 0.9, ['user']),
    ]

    for label, cx, cy, roles in cases:
        ellipse = patches.Ellipse((cx, cy), 3.8, 0.6, ec='#66C0F4', fc='#243E56', lw=1.5)
        ax.add_patch(ellipse)
        ax.text(cx, cy, label, color='white', fontsize=8.5, ha='center', va='center')

        if 'guest' in roles:
            ax.annotate("", xy=(cx - 1.9, cy), xytext=(1.6, 5.5),
                        arrowprops=dict(arrowstyle="->", color='#8F98A0', lw=1.2, ls='--'))
        if 'user' in roles:
            ax.annotate("", xy=(cx - 1.9, cy), xytext=(1.6, 2.3),
                        arrowprops=dict(arrowstyle="->", color='#A4D007', lw=1.3))

    plt.tight_layout()
    path = os.path.join(out_dir, "diagram_usecase.png")
    plt.savefig(path, facecolor=fig.get_facecolor(), edgecolor='none')
    plt.close()
    print("Created:", path)

# -----------------------------------------------------------------------------
# 3. DATA MODEL / CLASS DIAGRAM
# -----------------------------------------------------------------------------
def make_datamodel_diagram():
    fig, ax = plt.subplots(figsize=(10, 6.5), dpi=300)
    ax.set_facecolor('#171A21')
    fig.patch.set_facecolor('#171A21')
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 7)
    ax.axis('off')

    ax.text(5, 6.6, "Диаграмма классов и моделей данных (Data Model)", 
            color='#66C0F4', fontsize=14, weight='bold', ha='center')

    # Class 1: Game
    b1 = patches.FancyBboxPatch((0.6, 3.4), 3.8, 2.8, boxstyle="round,pad=0.1", ec='#4B617A', fc='#1B2838', lw=2)
    ax.add_patch(b1)
    header1 = patches.Rectangle((0.6, 5.6), 3.8, 0.6, color='#2A475E')
    ax.add_patch(header1)
    ax.text(2.5, 5.9, "Game", color='white', fontsize=11, weight='bold', ha='center')
    game_fields = (
        "+ Name : string\n"
        "+ Genre : string\n"
        "+ Price : int\n"
        "+ OldPrice : int\n"
        "+ Image : string\n"
        "-------------------------------\n"
        "+ HasDiscount : bool\n"
        "+ DiscountText : string\n"
        "+ PriceText : string"
    )
    ax.text(0.8, 4.4, game_fields, color='#C7D5E0', fontsize=8.5, family='monospace')

    # Class 2: CartItem
    b2 = patches.FancyBboxPatch((5.6, 3.7), 3.8, 2.5, boxstyle="round,pad=0.1", ec='#5C7E10', fc='#1B2838', lw=2)
    ax.add_patch(b2)
    header2 = patches.Rectangle((5.6, 5.6), 3.8, 0.6, color='#4C6B22')
    ax.add_patch(header2)
    ax.text(7.5, 5.9, "CartItem", color='white', fontsize=11, weight='bold', ha='center')
    cart_fields = (
        "+ Game : Game\n"
        "+ Count : int\n"
        "-------------------------------\n"
        "+ Sum : int\n"
        "+ SumText : string"
    )
    ax.text(5.8, 4.6, cart_fields, color='#D2EFA9', fontsize=8.5, family='monospace')

    # Association Game -> CartItem
    ax.annotate("", xy=(5.6, 4.9), xytext=(4.4, 4.9), 
                arrowprops=dict(arrowstyle="<|-", color='#66C0F4', lw=2))
    ax.text(5.0, 5.1, "1..*", color='#66C0F4', fontsize=9, weight='bold', ha='center')

    # Class 3: User
    b3 = patches.FancyBboxPatch((0.6, 0.5), 3.8, 2.3, boxstyle="round,pad=0.1", ec='#8F98A0', fc='#1B2838', lw=2)
    ax.add_patch(b3)
    header3 = patches.Rectangle((0.6, 2.2), 3.8, 0.6, color='#344654')
    ax.add_patch(header3)
    ax.text(2.5, 2.5, "User", color='white', fontsize=11, weight='bold', ha='center')
    user_fields = (
        "+ Login : string\n"
        "+ Email : string\n"
        "+ Password : string\n"
        "+ Balance : int"
    )
    ax.text(0.8, 1.3, user_fields, color='#C5C3C0', fontsize=8.5, family='monospace')

    # Class 4: Order Record
    b4 = patches.FancyBboxPatch((5.6, 0.5), 3.8, 2.3, boxstyle="round,pad=0.1", ec='#E5A93C', fc='#1B2838', lw=2)
    ax.add_patch(b4)
    header4 = patches.Rectangle((5.6, 2.2), 3.8, 0.6, color='#7A5A1B')
    ax.add_patch(header4)
    ax.text(7.5, 2.5, "Order (Заказ)", color='white', fontsize=11, weight='bold', ha='center')
    order_fields = (
        "+ UserLogin : string\n"
        "+ Date : string (dd.MM.yyyy)\n"
        "+ Description : string\n"
        "+ TotalSum : int"
    )
    ax.text(5.8, 1.3, order_fields, color='#F3D79B', fontsize=8.5, family='monospace')

    # Association User -> Order
    ax.annotate("", xy=(5.6, 1.6), xytext=(4.4, 1.6), 
                arrowprops=dict(arrowstyle="->", color='#E5A93C', lw=2))
    ax.text(5.0, 1.8, "1..*", color='#E5A93C', fontsize=9, weight='bold', ha='center')

    plt.tight_layout()
    path = os.path.join(out_dir, "diagram_datamodel.png")
    plt.savefig(path, facecolor=fig.get_facecolor(), edgecolor='none')
    plt.close()
    print("Created:", path)

# -----------------------------------------------------------------------------
# 4. NAVIGATION FLOWCHART
# -----------------------------------------------------------------------------
def make_navigation_diagram():
    fig, ax = plt.subplots(figsize=(10, 6.5), dpi=300)
    ax.set_facecolor('#171A21')
    fig.patch.set_facecolor('#171A21')
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 7)
    ax.axis('off')

    ax.text(5, 6.6, "Схема экранных форм и навигации (UI Navigation)", 
            color='#66C0F4', fontsize=14, weight='bold', ha='center')

    # MainWindow center top
    main_box = patches.FancyBboxPatch((3.0, 4.6), 4.0, 1.5, boxstyle="round,pad=0.15", 
                                      ec='#66C0F4', fc='#20374C', lw=2.5)
    ax.add_patch(main_box)
    ax.text(5.0, 5.5, "MainWindow", color='#FFFFFF', fontsize=12, weight='bold', ha='center')
    ax.text(5.0, 5.0, "Шапка: Логотип | Меню | Корзина | Профиль/Вход", 
            color='#C7D5E0', fontsize=8.5, ha='center')

    # AuthWindow modal dialog
    auth_box = patches.FancyBboxPatch((0.5, 4.8), 2.0, 1.1, boxstyle="round,pad=0.1", 
                                      ec='#E55653', fc='#2C2024', lw=2)
    ax.add_patch(auth_box)
    ax.text(1.5, 5.5, "AuthWindow", color='#FFFFFF', fontsize=10, weight='bold', ha='center')
    ax.text(1.5, 5.1, "Вход / Регистрация\n(ShowDialog)", color='#E5A2A2', fontsize=8, ha='center')

    # Navigation to AuthWindow
    ax.annotate("", xy=(2.5, 5.35), xytext=(3.0, 5.35), 
                arrowprops=dict(arrowstyle="<->", color='#E55653', lw=2))

    # Pages in Frame
    pages = [
        ("CatalogPage\n(Витрина игр, баннер,\nпоиск и категории)", 0.5, 1.8, 2.0, '#3E7EA7'),
        ("TopUpPage\n(Калькулятор пополнения\nSteam + комиссия 5%)", 2.8, 1.8, 2.0, '#4C6B22'),
        ("CartPage\n(Список товаров, счетчик,\nпромокоды, чек)", 5.1, 1.8, 2.0, '#5C7E10'),
        ("ProfilePage\n(Баланс, пополнение\nкартой, история)", 7.4, 1.8, 2.1, '#7A5A1B')
    ]

    for title, px, py, pw, clr in pages:
        p_box = patches.FancyBboxPatch((px, py), pw, 1.7, boxstyle="round,pad=0.1", 
                                      ec=clr, fc='#1B2838', lw=2)
        ax.add_patch(p_box)
        ax.text(px + pw/2, py + 0.85, title, color='#FFFFFF', fontsize=8.5, ha='center', va='center')

        # Arrow from MainWindow to Page
        ax.annotate("", xy=(px + pw/2, py + 1.7), xytext=(5.0, 4.6),
                    arrowprops=dict(arrowstyle="->", color='#66C0F4', lw=1.5, 
                                    connectionstyle="arc3,rad=0.05"))

    # Toast element
    toast_box = patches.FancyBboxPatch((7.6, 5.0), 1.9, 0.9, boxstyle="round,pad=0.08", 
                                       ec='#A4D007', fc='#223524', lw=1.5)
    ax.add_patch(toast_box)
    ax.text(8.55, 5.55, "Toast Notification", color='#D2EFA9', fontsize=9, weight='bold', ha='center')
    ax.text(8.55, 5.25, "Всплывающие уведомления\n(анимация в правом углу)", color='#C7D5E0', fontsize=7.5, ha='center')
    ax.annotate("", xy=(7.6, 5.45), xytext=(7.0, 5.45), 
                arrowprops=dict(arrowstyle="<-", color='#A4D007', lw=1.5))

    plt.tight_layout()
    path = os.path.join(out_dir, "diagram_navigation.png")
    plt.savefig(path, facecolor=fig.get_facecolor(), edgecolor='none')
    plt.close()
    print("Created:", path)

# -----------------------------------------------------------------------------
# 5. ALGORITHM FLOWCHART (CHECKOUT)
# -----------------------------------------------------------------------------
def make_algorithm_diagram():
    fig, ax = plt.subplots(figsize=(9, 7), dpi=300)
    ax.set_facecolor('#171A21')
    fig.patch.set_facecolor('#171A21')
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 9)
    ax.axis('off')

    ax.text(5, 8.6, "Блок-схема алгоритма оформления заказа (Checkout)", 
            color='#66C0F4', fontsize=13, weight='bold', ha='center')

    # Step 1: Start
    s1 = patches.FancyBboxPatch((3.5, 7.8), 3.0, 0.55, boxstyle="round,pad=0.15", ec='#66C0F4', fc='#2A475E', lw=2)
    ax.add_patch(s1)
    ax.text(5.0, 8.07, "Нажатие кнопки «Оформить заказ»", color='white', fontsize=9, ha='center', weight='bold')

    # Step 2: Auth check decision
    d1 = patches.Polygon([[5.0, 7.4], [6.8, 6.7], [5.0, 6.0], [3.2, 6.7]], closed=True, 
                         ec='#E5A93C', fc='#423116', lw=2)
    ax.add_patch(d1)
    ax.text(5.0, 6.7, "Пользователь\nавторизован?", color='white', fontsize=8.5, ha='center', va='center')

    # Step 2a: Login Dialog
    b_auth = patches.FancyBboxPatch((7.5, 6.35), 2.0, 0.7, boxstyle="round,pad=0.08", ec='#C15755', fc='#2D1B1E', lw=1.5)
    ax.add_patch(b_auth)
    ax.text(8.5, 6.7, "Вызов AuthWindow\n(ShowDialog)", color='#F5B7B1', fontsize=8, ha='center', va='center')

    # Step 3: Balance check decision
    d2 = patches.Polygon([[5.0, 5.4], [6.8, 4.7], [5.0, 4.0], [3.2, 4.7]], closed=True, 
                         ec='#E5A93C', fc='#423116', lw=2)
    ax.add_patch(d2)
    ax.text(5.0, 4.7, "Баланс кошелька\n>= Сумме заказа?", color='white', fontsize=8.5, ha='center', va='center')

    # Step 3a: Error insufficient balance
    b_err = patches.FancyBboxPatch((7.4, 4.35), 2.2, 0.7, boxstyle="round,pad=0.08", ec='#C15755', fc='#2D1B1E', lw=1.5)
    ax.add_patch(b_err)
    ax.text(8.5, 4.7, "Ошибка: не хватает средств\n(переход в пополнение)", color='#F5B7B1', fontsize=7.5, ha='center', va='center')

    # Step 4: Deduct balance & Save
    b_pay = patches.FancyBboxPatch((3.0, 2.7), 4.0, 0.8, boxstyle="round,pad=0.1", ec='#5C7E10', fc='#213824', lw=2)
    ax.add_patch(b_pay)
    ax.text(5.0, 3.1, "Списание средств с баланса:\nUser.Balance -= Total; SaveCurrentUser()", 
            color='#D2EFA9', fontsize=8.5, ha='center', va='center')

    # Step 5: Add order record
    b_order = patches.FancyBboxPatch((3.0, 1.5), 4.0, 0.7, boxstyle="round,pad=0.1", ec='#4B617A', fc='#1B2838', lw=2)
    ax.add_patch(b_order)
    ax.text(5.0, 1.85, "Запись в orders.txt, очистка корзины,\nгенерация ключей активации", 
            color='#C7D5E0', fontsize=8.5, ha='center', va='center')

    # Step 6: Finish
    s_end = patches.FancyBboxPatch((3.5, 0.4), 3.0, 0.6, boxstyle="round,pad=0.15", ec='#66C0F4', fc='#2A475E', lw=2)
    ax.add_patch(s_end)
    ax.text(5.0, 0.7, "MessageBox: «Заказ оформлен!»\nПереход в профиль", color='white', fontsize=9, ha='center', weight='bold')

    # Arrows
    # s1 -> d1
    ax.annotate("", xy=(5.0, 7.4), xytext=(5.0, 7.8), arrowprops=dict(arrowstyle="->", color='#66C0F4', lw=2))
    # d1 -> b_auth (No)
    ax.annotate("", xy=(7.5, 6.7), xytext=(6.8, 6.7), arrowprops=dict(arrowstyle="->", color='#E5A93C', lw=1.5))
    ax.text(7.15, 6.85, "Нет", color='#E5A93C', fontsize=8, weight='bold')
    # b_auth -> d1 (After login)
    ax.annotate("", xy=(5.9, 7.05), xytext=(8.5, 7.05), arrowprops=dict(arrowstyle="->", color='#A4D007', lw=1.5))
    ax.text(7.2, 7.2, "Успех", color='#A4D007', fontsize=8)
    # d1 -> d2 (Yes)
    ax.annotate("", xy=(5.0, 5.4), xytext=(5.0, 6.0), arrowprops=dict(arrowstyle="->", color='#66C0F4', lw=2))
    ax.text(5.15, 5.7, "Да", color='#66C0F4', fontsize=8, weight='bold')
    # d2 -> b_err (No)
    ax.annotate("", xy=(7.4, 4.7), xytext=(6.8, 4.7), arrowprops=dict(arrowstyle="->", color='#C15755', lw=1.5))
    ax.text(7.1, 4.85, "Нет", color='#C15755', fontsize=8, weight='bold')
    # d2 -> b_pay (Yes)
    ax.annotate("", xy=(5.0, 3.5), xytext=(5.0, 4.0), arrowprops=dict(arrowstyle="->", color='#A4D007', lw=2))
    ax.text(5.15, 3.75, "Да", color='#A4D007', fontsize=8, weight='bold')
    # b_pay -> b_order
    ax.annotate("", xy=(5.0, 2.2), xytext=(5.0, 2.7), arrowprops=dict(arrowstyle="->", color='#66C0F4', lw=2))
    # b_order -> s_end
    ax.annotate("", xy=(5.0, 1.0), xytext=(5.0, 1.5), arrowprops=dict(arrowstyle="->", color='#66C0F4', lw=2))

    plt.tight_layout()
    path = os.path.join(out_dir, "diagram_algorithm_checkout.png")
    plt.savefig(path, facecolor=fig.get_facecolor(), edgecolor='none')
    plt.close()
    print("Created:", path)

if __name__ == '__main__':
    make_architecture_diagram()
    make_usecase_diagram()
    make_datamodel_diagram()
    make_navigation_diagram()
    make_algorithm_diagram()
    print("ALL_DIAGRAMS_CREATED")
