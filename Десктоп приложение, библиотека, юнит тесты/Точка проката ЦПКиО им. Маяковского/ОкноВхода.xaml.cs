using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноВхода : Window
    {
        private ПрокатContext db;
        private int logins = 0; //Количество неудачных попыток входа
        private bool isOpen; //Открыто окно или нет

        //При создании проверяется подключение к базе
        public ОкноВхода()
        {
            isOpen = true;
            db = new ПрокатContext(Properties.Resources.connectionString);

            InitializeComponent();
        }

        //При загрузке проверяется было ли время прошлого сеанса исчерпано,
        //Если было, то вход блокируется
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!db.Database.CanConnect())
            {
                MessageBox.Show("Не удалось подключиться к базе данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                isOpen = false;
                Application.Current.Shutdown();
            }

            if (Properties.Settings.Default.blockDateTime > DateTime.Now)
            {
                войти.IsEnabled = false;

                int seconds = (int)Properties.Settings.Default.blockDateTime.Subtract(DateTime.Now).TotalSeconds;

                for (int i = seconds; i > 0; i--)
                {
                    if (!isOpen)
                        return;

                    if (i > 60)
                        this.Title = $"Вход заблокирован. Осталось минут: {i / 60} секунд: {i % 60}";
                    else
                        this.Title = $"Вход заблокирован. Осталось секунд: {i}";

                    await Task.Delay(1000);
                }

                this.Title = "Окно входа";
                войти.IsEnabled = true;
            }
        }

        //Закрытие окна
        private void Window_Closed(object sender, EventArgs e)
        {
            isOpen = false;
        }

        //Показ пароля
        private void ShowPassword(object sender, MouseButtonEventArgs e)
        {
            показПароля.Text = пароль.Password;
            пароль.PasswordChar = '\0';
            показПароля.Visibility = Visibility.Visible;
        }

        //Прекращение закрытия показа
        private void CollapsePassword(object sender, MouseButtonEventArgs e)
        {
            показПароля.Visibility = Visibility.Collapsed;
            пароль.PasswordChar = '●';
        }

        //Попытка входа с записью результата в базу данных
        //Если было 2 неудачных попытки, то просит ввести капчу
        //Если капча введена неправильно, то вход блокируется на 10 секунд
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            if (логин.Text == "" || пароль.Password == "")
            {
                MessageBox.Show("Необходимо ввести логин и пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (logins >= 2)
            {
                ОкноКапчи окноКапчи = new ОкноКапчи();
                окноКапчи.ShowDialog();

                if (окноКапчи.DialogResult == false)
                {
                    войти.IsEnabled = false;

                    for (int i = 10; i > 0; i--)
                    {
                        if (!isOpen)
                            return;

                        this.Title = "Вход заблокирован. Осталось секунд: " + i;
                        await Task.Delay(1000);
                    }

                    this.Title = "Окно входа";
                    войти.IsEnabled = true;

                    return;
                }
            }

            Пользователи? пользователь;
            try
            {
                пользователь = db.Пользователиs.FirstOrDefault(p =>
                    p.Логин == логин.Text
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Не удалось войти\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            logins++;

            if (пользователь != null)
            {
                if (пользователь.Пароль == пароль.Password)
                {
                    await AddLoginInHistory(пользователь.Код, true);

                    ОкноСотрудника окноСотрудника = new ОкноСотрудника(пользователь);
                    окноСотрудника.Show();
                    this.Close();
                }
                else
                {
                    await AddLoginInHistory(пользователь.Код, false);

                    MessageBox.Show("Неправильный логин или пароль", "Не удалось войти", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
                MessageBox.Show("Неправильный логин или пароль", "Не удалось войти", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        //Добавление попытки входа в базу данных
        private async Task AddLoginInHistory(int userId, bool loginType)
        {
            try
            {
                ИсторияВходов вход = new ИсторияВходов();

                вход.Пользователь = userId;
                вход.Дата = DateTime.Now.Date;
                вход.Время = DateTime.Now.TimeOfDay;
                вход.Тип = loginType ? 1 : 2;

                await db.ИсторияВходовs.AddAsync(вход);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Для удобства тестирования, при двойном клике заполняются данные для входа
        private int sotrudnik = 1;
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            switch (sotrudnik)
            {
                case 1:
                    логин.Text = "fedorov@namecomp.ru";
                    пароль.Password = "8ntwUp";
                    break;
                case 2:
                    логин.Text = "mironov@namecomp.ru";
                    пароль.Password = "YOyhfR";
                    break;
                case 3:
                    логин.Text = "Ivanov@namecomp.ru";
                    пароль.Password = "2L6KZG";
                    break;
            }

            sotrudnik = sotrudnik == 3 ? 1 : sotrudnik + 1;
        }
    }
}
