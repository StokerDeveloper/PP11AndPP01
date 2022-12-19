using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Точка_проката_ЦПКиО_им._Маяковского.Pages;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноСотрудника : Window
    {
        private ПрокатContext db;
        public static Пользователи пользователь = null!;
        private Сотрудники? сотрудник;
        private bool isOpen; //Открыто окно или нет

        //При создании окна из базы данных загружается сотрудник и в зависимости
        //от его должности определяются доступные ему вкладки
        public ОкноСотрудника(Пользователи пользователь)
        {
            ОкноСотрудника.пользователь = пользователь;
            db = new ПрокатContext(Properties.Resources.connectionString);
            isOpen = true;
            
            InitializeComponent();

            try
            {
                сотрудник = db.Сотрудникиs.FirstOrDefault(p =>
                    p.Код == пользователь.Сотрудник
                );

                if (сотрудник != null)
                {
                    db.Entry(сотрудник).Reference("ДолжностьNavigation").Load();

                    сотрудникДолжность.Content = сотрудник.ДолжностьNavigation.Наименование;
                    сотрудникФИ.Content = сотрудник.Фамилия + " " + сотрудник.Имя;

                    BitmapImage? image = LoadImage(пользователь.Фото);

                    if (image != null)
                    {
                        пользовательКартинка.Source = image;
                    }

                    switch (сотрудник.ДолжностьNavigation.Наименование)
                    {
                        case "Администратор":
                            принятьЗаказ.Visibility = Visibility.Collapsed;
                            оформитьЗаказ.Visibility = Visibility.Collapsed;
                            принятьОборудование.Visibility = Visibility.Collapsed;
                            break;
                        case "Старший смены":
                            историяВходов.Visibility = Visibility.Collapsed;
                            заказы.Visibility = Visibility.Collapsed;
                            оборудование.Visibility = Visibility.Collapsed;
                            отчет.Visibility = Visibility.Collapsed;
                            break;
                        case "Продавец":
                            принятьОборудование.Visibility = Visibility.Collapsed;
                            историяВходов.Visibility = Visibility.Collapsed;
                            заказы.Visibility = Visibility.Collapsed;
                            оборудование.Visibility = Visibility.Collapsed;
                            отчет.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка работы с базой данных. Сотрудник не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Не удалось получить данные сотрудника\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //При загрузке окна запускается таймер сеанса
        //Если время сеанса истекло окно закрывается открывается окно входа
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 150; i++)
            {
                if (!isOpen)
                    return;

                if (i < 135)
                    this.Title = $"Время сеанса - часов: {i / 60} минут: {i % 60}";
                else
                    this.Title = $"Время сеанса - часов: {i / 60} минут: {i % 60} !минут до окончания сеанса: {150 - i}";

                await Task.Delay(60000);
            }

            MessageBox.Show("Сеанс завершен", "Завершение сеанса", MessageBoxButton.OK, MessageBoxImage.Information);

            Properties.Settings.Default.blockDateTime = DateTime.Now.AddMinutes(15);
            Properties.Settings.Default.Save();

            ОкноВхода окноВхода = new ОкноВхода();
            окноВхода.Show();
            this.Close();
        }

        //Закрытие окна
        private void Window_Closed(object sender, EventArgs e)
        {
            isOpen = false;
        }

        //Получение картинки из массива байтов
        private BitmapImage? LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        //Кнопка выхода
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ОкноВхода окноВхода = new ОкноВхода();
            окноВхода.Show();
            this.Close();
        }

        //При наведении на вкладку ее цвет меняется
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Background = new SolidColorBrush(Color.FromArgb(255, (byte)73, (byte)140, (byte)81));
        }

        //При выходе курсора из вкладки ее цвет возвращается обратно
        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Background = Brushes.Transparent;
        }

        //При нажатии на вкладку определяется какая вкладка была выбрана
        //и в соответствии с этим открывается страница вкладки
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;

            фрейм.NavigationService.RemoveBackEntry();

            switch (label.Name)
            {
                case "принятьЗаказ":
                    фрейм.Navigate(new СтраницаПринятияЗаказа());
                    break;
                case "оформитьЗаказ":
                    фрейм.Navigate(new СтраницаОформленияЗаказа());
                    break;
                case "принятьОборудование":
                    фрейм.Navigate(new СтраницаПринятияОборудования());
                    break;
                case "историяВходов":
                    фрейм.Navigate(new СтраницаИсторииВходов());
                    break;
                case "заказы":
                    фрейм.Navigate(new СтраницаЗаказов());
                    break;
                case "оборудование":
                    фрейм.Navigate(new СтраницаОборудования());
                    break;
                case "отчет":
                    фрейм.Navigate(new СтраницаОтчета());
                    break;
            }
        }
    }
}
