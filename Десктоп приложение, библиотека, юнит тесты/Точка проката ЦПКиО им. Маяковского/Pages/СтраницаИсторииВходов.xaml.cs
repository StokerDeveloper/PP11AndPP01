using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Точка_проката_ЦПКиО_им._Маяковского.Models;

namespace Точка_проката_ЦПКиО_им._Маяковского.Pages
{
    public partial class СтраницаИсторииВходов : Page
    {
        private ПрокатContext db;

        public СтраницаИсторииВходов()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);

            InitializeComponent();
        }

        //При странице загружается история входов
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableElems();
            try
            {
                await db.ИсторияВходовs.LoadAsync();
                историяВходов.ItemsSource = db.ИсторияВходовs.
                    Include(u => u.ПользовательNavigation).
                    Include(u => u.ТипNavigation).
                    Select(c =>
                        new ИсторияВходовСПользователемИТипом
                        {
                            Код = c.Код,
                            Логин = c.ПользовательNavigation.Логин,
                            Дата = c.Дата,
                            Время = c.Время,
                            ТипВхода = c.ТипNavigation.Наименование
                        }
                    ).ToList();

                историяВходов.Columns[0].Visibility = Visibility.Collapsed;
                историяВходов.Columns[2].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
                историяВходов.Columns[3].ClipboardContentBinding.StringFormat = "hh\\:mm\\:ss";
                историяВходов.Columns[4].Header = "Тип входа";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. История входов не получена\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            EnableElems();
        }

        //Поиск по истории входов
        private async void выбрать_Click(object sender, RoutedEventArgs e)
        {
            EnableElems();

            if (поиск.Text == "")
            {
                try
                {
                    await db.ИсторияВходовs.LoadAsync();
                    историяВходов.ItemsSource = db.ИсторияВходовs.
                        Include(u => u.ПользовательNavigation).
                        Include(u => u.ТипNavigation).
                        Select(c => 
                            new ИсторияВходовСПользователемИТипом { 
                                Код = c.Код, 
                                Логин = c.ПользовательNavigation.Логин, 
                                Дата = c.Дата, 
                                Время = c.Время, 
                                ТипВхода = c.ТипNavigation.Наименование
                            }
                        ).ToList();

                    историяВходов.Columns[0].Visibility = Visibility.Collapsed;
                    историяВходов.Columns[2].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
                    историяВходов.Columns[3].ClipboardContentBinding.StringFormat = "hh\\:mm\\:ss";
                    историяВходов.Columns[4].Header = "Тип входа";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. История входов не получена\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    await db.ИсторияВходовs.LoadAsync();
                    историяВходов.ItemsSource = db.ИсторияВходовs.
                        Include(u => u.ПользовательNavigation).
                        Include(u => u.ТипNavigation).
                        Where(c => 
                            c.ПользовательNavigation.Логин.Contains(поиск.Text) ||
                            c.ТипNavigation.Наименование.Contains(поиск.Text)
                        ).
                        Select(c =>
                            new ИсторияВходовСПользователемИТипом
                            {
                                Код = c.Код,
                                Логин = c.ПользовательNavigation.Логин,
                                Дата = c.Дата,
                                Время = c.Время,
                                ТипВхода = c.ТипNavigation.Наименование
                            }
                        ).ToList();

                    историяВходов.Columns[0].Visibility = Visibility.Collapsed;
                    историяВходов.Columns[2].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
                    историяВходов.Columns[3].ClipboardContentBinding.StringFormat = "hh\\:mm\\:ss";
                    историяВходов.Columns[4].Header = "Тип входа";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. История входов не получена\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            EnableElems();
        }

        //Блокировка элементов
        private void EnableElems()
        {
            поиск.IsEnabled = !поиск.IsEnabled;
            выбрать.IsEnabled = !выбрать.IsEnabled;
            историяВходов.IsEnabled = !историяВходов.IsEnabled;
        }
    }
}
