using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноВыбораКлиента : Window
    {
        private ПрокатContext db;
        public Клиенты? клиент;

        public ОкноВыбораКлиента()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);
            клиент = null;

            InitializeComponent();
        }

        //Загрузка клиентов
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableElems();
            try
            {
                await db.Клиентыs.LoadAsync();
                клиенты.ItemsSource = db.Клиентыs.ToList();
                клиенты.Columns[0].Visibility = Visibility.Collapsed;
                клиенты.Columns[9].Visibility = Visibility.Collapsed;
                клиенты.Columns[4].Header = "Серия паспорта";
                клиенты.Columns[5].Header = "Номер паспорта";
                клиенты.Columns[6].Header = "Дата рождения";
                клиенты.Columns[6].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
                клиенты.Columns[8].Header = "Электронная почта";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Клиенты не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            EnableElems();
        }

        //Поиск клиента
        private async void выбрать_Click(object sender, RoutedEventArgs e)
        {
            EnableElems();

            if (поиск.Text == "")
            {
                try
                {
                    await db.Клиентыs.LoadAsync();
                    клиенты.ItemsSource = db.Клиентыs.ToList();

                    foreach (System.Data.DataRowView dr in клиенты.ItemsSource)
                    {
                        dr[6] = ((DateTime)dr[6]).ToShortDateString();
                    }

                    клиенты.Columns[0].Visibility = Visibility.Collapsed;
                    клиенты.Columns[9].Visibility = Visibility.Collapsed;
                    клиенты.Columns[4].Header = "Серия паспорта";
                    клиенты.Columns[5].Header = "Номер паспорта";
                    клиенты.Columns[6].Header = "Дата рождения";
                    клиенты.Columns[6].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
                    клиенты.Columns[8].Header = "Электронная почта";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Клиенты не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    клиенты.ItemsSource = db.Клиентыs.Where(p =>
                        p.Фамилия.Contains(поиск.Text) ||
                        p.Имя.Contains(поиск.Text) ||
                        p.Отчество.Contains(поиск.Text) ||
                        p.СерияПаспорта.Contains(поиск.Text) ||
                        p.НомерПаспорта.Contains(поиск.Text) ||
                        p.Адрес.Contains(поиск.Text) ||
                        p.ЭлектроннаяПочта.Contains(поиск.Text)
                    ).ToList();
                    клиенты.Columns[0].Visibility = Visibility.Collapsed;
                    клиенты.Columns[9].Visibility = Visibility.Collapsed;
                    клиенты.Columns[4].Header = "Серия паспорта";
                    клиенты.Columns[5].Header = "Номер паспорта";
                    клиенты.Columns[6].Header = "Дата рождения";
                    клиенты.Columns[6].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
                    клиенты.Columns[8].Header = "Электронная почта";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Клиенты не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            EnableElems();
        }

        //Выбор клиента
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dataGridRow = (DataGridRow)sender;
            клиент = (Клиенты)dataGridRow.Item;

            this.DialogResult = true;
        }

        //Блокировка элементов
        private void EnableElems()
        {
            поиск.IsEnabled = !поиск.IsEnabled;
            выбрать.IsEnabled = !выбрать.IsEnabled;
            клиенты.IsEnabled = !клиенты.IsEnabled;
        }
    }
}
