using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Точка_проката_ЦПКиО_им._Маяковского.Models;

namespace Точка_проката_ЦПКиО_им._Маяковского.Pages
{
    public partial class СтраницаОборудования : Page
    {
        private ПрокатContext db;
        private List<ОборудованиеСоСтатусомИТипом> оборудованиеСписок;

        //При создании загружается оборудование
        public СтраницаОборудования()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);
            оборудованиеСписок = db.Оборудованиеs.
            Include(u => u.ПрокатОборудованияs).
            Select(c =>
                new ОборудованиеСоСтатусомИТипом
                {
                    Код = c.Код,
                    Номер = c.КодОборудования,
                    Тип = c.ТипNavigation.Наименование,
                    Статус = (new Random()).Next(0, 100) > 40 ? "В прокате" : "Свободно"
                }
            ).ToList();

            InitializeComponent();
        }

        //При загрузке выводится оборудование
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableElems();
            try
            {
                await db.Оборудованиеs.
                Include(u => u.ТипNavigation).
                LoadAsync();

                оборудование.ItemsSource = оборудованиеСписок;
                оборудование.Columns[0].Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Оборудование не получено\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            EnableElems();
        }

        //Поиск оборудования
        private async void выбрать_Click(object sender, RoutedEventArgs e)
        {
            EnableElems();

            if (поиск.Text == "")
            {
                try
                {
                    await db.Оборудованиеs.
                    Include(u => u.ТипNavigation).
                    LoadAsync();

                    оборудованиеСписок = db.Оборудованиеs.
                    Include(u => u.ПрокатОборудованияs).
                    Select(c =>
                        new ОборудованиеСоСтатусомИТипом
                        {
                            Код = c.Код,
                            Номер = c.КодОборудования,
                            Тип = c.ТипNavigation.Наименование,
                            Статус = (new Random()).Next(0, 100) > 40 ? "В прокате" : "Свободно"
                        }
                    ).ToList();

                    оборудование.ItemsSource = оборудованиеСписок;
                    оборудование.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Оборудование не получено\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    await db.Оборудованиеs.
                    Include(u => u.ТипNavigation).
                    LoadAsync();

                    оборудованиеСписок = db.Оборудованиеs.
                    Include(u => u.ПрокатОборудованияs).
                    Select(c =>
                        new ОборудованиеСоСтатусомИТипом
                        {
                            Код = c.Код,
                            Номер = c.КодОборудования,
                            Тип = c.ТипNavigation.Наименование,
                            Статус = (new Random()).Next(0, 100) > 40 ? "В прокате" : "Свободно"
                        }
                    ).ToList();

                    оборудование.ItemsSource = оборудованиеСписок.
                    Where(p =>
                        p.Номер.Contains(поиск.Text) ||
                        p.Тип.Contains(поиск.Text) ||
                        p.Статус.Contains(поиск.Text)
                    ).
                    ToList();
                    оборудование.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Оборудование не получено\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            EnableElems();
        }

        //Блокировка элементов
        private void EnableElems()
        {
            поиск.IsEnabled = !поиск.IsEnabled;
            выбрать.IsEnabled = !выбрать.IsEnabled;
            создать.IsEnabled = !создать.IsEnabled;
            оборудование.IsEnabled = !оборудование.IsEnabled;
        }

        //Создание оборудования
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            EnableElems();

            ОкноСозданияОборудования осо = new ОкноСозданияОборудования();
            осо.ShowDialog();

            if (осо.DialogResult == true)
            {
                try
                {
                    await db.Оборудованиеs.
                    Include(u => u.ТипNavigation).
                    LoadAsync();

                    оборудованиеСписок = db.Оборудованиеs.
                    Include(u => u.ПрокатОборудованияs).
                    Select(c =>
                        new ОборудованиеСоСтатусомИТипом
                        {
                            Код = c.Код,
                            Номер = c.КодОборудования,
                            Тип = c.ТипNavigation.Наименование,
                            Статус = (new Random()).Next(0, 100) > 40 ? "В прокате" : "Свободно"
                        }
                    ).ToList();

                    оборудование.ItemsSource = оборудованиеСписок;
                    оборудование.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Оборудование не получено\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            EnableElems();
        }
    }
}
