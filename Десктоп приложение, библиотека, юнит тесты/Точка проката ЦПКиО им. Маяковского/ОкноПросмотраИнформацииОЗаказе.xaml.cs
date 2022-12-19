using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Точка_проката_ЦПКиО_им._Маяковского.Models;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноПросмотраИнформацииОЗаказе : Window
    {
        private ПрокатContext db;
        private Заказы заказ;
        private ШтрихКод штрихКод = null!;

        public ОкноПросмотраИнформацииОЗаказе(Заказы заказ)
        {
            db = new ПрокатContext(Properties.Resources.connectionString);
            this.заказ = заказ;

            InitializeComponent();
        }

        //Вывод информации о заказе (в том числе штрих код, клиент, услуги, оборудование и движения заказа)
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await db.Заказыs.
                Include(u => u.ПредоставлениеУслугs).
                Include(u => u.ПрокатОборудованияs).
                Include(u => u.ДвиженияЗаказовs).
                LoadAsync();

                List<int> numbers = new List<int>();
                ДвиженияЗаказов движение = await db.ДвиженияЗаказовs.Where(p => p.Заказ == заказ.Код).FirstAsync();
                string dataivrema = движение.Дата.ToString("ddMMyyyy") + движение.Время.ToString("hh\\mm");
                string cod = Convert.ToString(заказ.Код);

                foreach (char ch in cod)
                {
                    try
                    {
                        numbers.Add(Int32.Parse("" + ch));
                    }
                    catch { }
                }
                foreach (char ch in dataivrema)
                {
                    try
                    {
                        numbers.Add(Int32.Parse("" + ch));
                    }
                    catch { }
                }
                numbers.Add(заказ.ВремяПрокатаЧасов);
                numbers.Add((new Random()).Next(1, 10)); numbers.Add((new Random()).Next(1, 10));
                numbers.Add((new Random()).Next(1, 10)); numbers.Add((new Random()).Next(1, 10));
                numbers.Add((new Random()).Next(1, 10)); numbers.Add((new Random()).Next(1, 10));

                ШтрихКод штрихКод = new ШтрихКод(numbers);
                this.штрихКод = штрихКод;
                штрихКодКартинка.Source = штрихКод.GetImageSource;

                Клиенты клиент = await db.Клиентыs.Where(p => p.Код == заказ.Клиент).FirstAsync();

                номерЗаказа.Content = "Номер заказа: " + заказ.КодЗаказа;
                часовПроката.Content = "Время проката (часов): " + заказ.ВремяПрокатаЧасов;
                фио.Content = "ФИО клиента: " + клиент.Фамилия + " " + клиент.Имя + " " + клиент.Отчество;
                электроннаяПочта.Content = "Электронная почта клиента: " + клиент.ЭлектроннаяПочта;

                List<ПредоставлениеУслуг> пу = await db.ПредоставлениеУслугs.
                Include(u => u.УслугаNavigation).
                Where(p => p.Заказ == заказ.Код).ToListAsync();

                List<Услуги> _услуги = new List<Услуги>();
                int summ = 0;
                foreach (ПредоставлениеУслуг усл in пу)
                {
                    _услуги.Add(усл.УслугаNavigation);
                    summ += усл.УслугаNavigation.СтоимостьРублейЗаЧас;
                }
                услуги.ItemsSource = _услуги;

                стоимостьЗаказа.Content = "Стоимость: " + summ * заказ.ВремяПрокатаЧасов;

                List<ПрокатОборудования> по = await db.ПрокатОборудованияs.
                Include(u => u.ОборудованиеNavigation).
                Where(p => p.Заказ == заказ.Код).ToListAsync();
                List<Оборудование> _оборудование = new List<Оборудование>();
                foreach (ПрокатОборудования обор in по)
                {
                    _оборудование.Add(обор.ОборудованиеNavigation);
                }
                оборудование.ItemsSource = _оборудование;
                
                история.ItemsSource = await db.ДвиженияЗаказовs.
                Include(u => u.СтатусNavigation).
                Where(p => p.Заказ == заказ.Код).
                Select(c =>
                    new ДвиженияЗаказовСоСтатусом
                    {
                        Код = c.Код,
                        Дата = c.Дата,
                        Время = c.Время,
                        Статус = c.СтатусNavigation.Наименование
                    }
                ).ToListAsync();

                услуги.Columns[0].Visibility = Visibility.Collapsed;
                услуги.Columns[4].Visibility = Visibility.Collapsed;
                услуги.Columns[5].Visibility = Visibility.Collapsed;
                услуги.Columns[6].Visibility = Visibility.Collapsed;

                услуги.Columns[1].Header = "Номер услуги";
                услуги.Columns[3].Header = "Стоимость (за час)";

                оборудование.Columns[0].Visibility = Visibility.Collapsed;
                оборудование.Columns[2].Visibility = Visibility.Collapsed;
                оборудование.Columns[3].Visibility = Visibility.Collapsed;
                оборудование.Columns[4].Visibility = Visibility.Collapsed;

                оборудование.Columns[1].Header = "Номер оборудования";

                история.Columns[0].Visibility = Visibility.Collapsed;
                история.Columns[1].ClipboardContentBinding.StringFormat = "dd.MM.yyyy";
                история.Columns[2].ClipboardContentBinding.StringFormat = "hh\\:mm\\:ss";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Заказ не получен\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
