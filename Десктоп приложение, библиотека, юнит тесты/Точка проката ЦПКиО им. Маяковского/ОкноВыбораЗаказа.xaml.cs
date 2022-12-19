using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноВыбораЗаказа : Window
    {
        private ПрокатContext db;
        public Заказы? заказ;

        public ОкноВыбораЗаказа()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);
            заказ = null;

            InitializeComponent();
        }

        //Загрузка заказов
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableElems();
            try
            {
                await db.Заказыs.LoadAsync();
                заказы.ItemsSource = db.Заказыs.ToList();
                заказы.Columns[0].Visibility = Visibility.Collapsed;
                заказы.Columns[2].Visibility = Visibility.Collapsed;
                заказы.Columns[3].Visibility = Visibility.Collapsed;
                заказы.Columns[4].Visibility = Visibility.Collapsed;
                заказы.Columns[5].Visibility = Visibility.Collapsed;
                заказы.Columns[6].Visibility = Visibility.Collapsed;
                заказы.Columns[7].Visibility = Visibility.Collapsed;

                заказы.Columns[1].Header = "Номер заказа";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Заказы не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            EnableElems();
        }

        //Поиск заказа
        private async void выбрать_Click(object sender, RoutedEventArgs e)
        {
            EnableElems();

            if (поиск.Text == "")
            {
                try
                {
                    await db.Заказыs.LoadAsync();
                    заказы.ItemsSource = db.Заказыs.ToList();
                    заказы.Columns[0].Visibility = Visibility.Collapsed;
                    заказы.Columns[2].Visibility = Visibility.Collapsed;
                    заказы.Columns[3].Visibility = Visibility.Collapsed;
                    заказы.Columns[4].Visibility = Visibility.Collapsed;
                    заказы.Columns[5].Visibility = Visibility.Collapsed;
                    заказы.Columns[6].Visibility = Visibility.Collapsed;
                    заказы.Columns[7].Visibility = Visibility.Collapsed;

                    заказы.Columns[1].Header = "Номер заказа";
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
                    заказы.ItemsSource = db.Заказыs.Where(p =>
                        p.КодЗаказа.Contains(поиск.Text)
                    ).ToList();
                    заказы.Columns[0].Visibility = Visibility.Collapsed;
                    заказы.Columns[2].Visibility = Visibility.Collapsed;
                    заказы.Columns[3].Visibility = Visibility.Collapsed;
                    заказы.Columns[4].Visibility = Visibility.Collapsed;
                    заказы.Columns[5].Visibility = Visibility.Collapsed;
                    заказы.Columns[6].Visibility = Visibility.Collapsed;
                    заказы.Columns[7].Visibility = Visibility.Collapsed;

                    заказы.Columns[1].Header = "Номер заказа";
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
            заказ = (Заказы)dataGridRow.Item;

            this.DialogResult = true;
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
