using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Точка_проката_ЦПКиО_им._Маяковского.Models;

namespace Точка_проката_ЦПКиО_им._Маяковского.Pages
{
    public partial class СтраницаЗаказов : Page
    {
        private ПрокатContext db;
        private Заказы заказ = null!;

        public СтраницаЗаказов()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);

            InitializeComponent();
        }

        //Загрузка заказов
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableElems();
            try
            {
                await db.Заказыs.LoadAsync();
                заказы.ItemsSource = db.Заказыs.
                    Include(u => u.КлиентNavigation).
                    Select(c =>
                        new ЗаказыСКлиентамиИСтатусом
                        {
                            Код = c.Код,
                            Номер = c.КодЗаказа,
                            Клиент = c.КлиентNavigation.Фамилия + " " + c.КлиентNavigation.Имя + " " + c.КлиентNavigation.Отчество,
                            ВремяПрокатаЧасов = c.ВремяПрокатаЧасов
                        }
                    ).ToList();
                заказы.Columns[0].Visibility = Visibility.Collapsed;
                заказы.Columns[3].Header = "Время проката (часов)";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Заказы не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            EnableElems();
        }

        //Поиск по заказам
        private async void выбрать_Click(object sender, RoutedEventArgs e)
        {
            EnableElems();

            if (поиск.Text == "")
            {
                try
                {
                    await db.Заказыs.LoadAsync();
                    заказы.ItemsSource = db.Заказыs.
                        Include(u => u.КлиентNavigation).
                        Select(c =>
                            new ЗаказыСКлиентамиИСтатусом
                            {
                                Код = c.Код,
                                Номер = c.КодЗаказа,
                                Клиент = c.КлиентNavigation.Фамилия + " " + c.КлиентNavigation.Имя + " " + c.КлиентNavigation.Отчество,
                                ВремяПрокатаЧасов = c.ВремяПрокатаЧасов
                            }
                        ).ToList();
                    заказы.Columns[0].Visibility = Visibility.Collapsed;
                    заказы.Columns[3].Header = "Время проката (часов)";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Заказы не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    await db.Заказыs.LoadAsync();
                    заказы.ItemsSource = db.Заказыs.
                        Include(u => u.КлиентNavigation).
                        Select(c =>
                            new ЗаказыСКлиентамиИСтатусом
                            {
                                Код = c.Код,
                                Номер = c.КодЗаказа,
                                Клиент = c.КлиентNavigation.Фамилия + " " + c.КлиентNavigation.Имя + " " + c.КлиентNavigation.Отчество,
                                ВремяПрокатаЧасов = c.ВремяПрокатаЧасов
                            }
                        ).
                        Where(c =>
                            c.Номер.Contains(поиск.Text) ||
                            c.Клиент.Contains(поиск.Text) ||
                            c.ВремяПрокатаЧасов == Convert.ToInt32(поиск.Text)
                        ).ToList();
                    заказы.Columns[0].Visibility = Visibility.Collapsed;
                    заказы.Columns[3].Header = "Время проката (часов)";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Заказы не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            EnableElems();
        }

        //Выбор заказа
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dataGridRow = (DataGridRow)sender;
            ЗаказыСКлиентамиИСтатусом элемент = (ЗаказыСКлиентамиИСтатусом)dataGridRow.Item;

            заказ = db.Заказыs.Where(p => p.Код == элемент.Код).First();

            ОкноПросмотраИнформацииОЗаказе инфо = new ОкноПросмотраИнформацииОЗаказе(заказ);
            инфо.ShowDialog();
        }

        //Блокировка элементов
        private void EnableElems()
        {
            поиск.IsEnabled = !поиск.IsEnabled;
            выбрать.IsEnabled = !выбрать.IsEnabled;
            заказы.IsEnabled = !заказы.IsEnabled;
        }
    }
}
