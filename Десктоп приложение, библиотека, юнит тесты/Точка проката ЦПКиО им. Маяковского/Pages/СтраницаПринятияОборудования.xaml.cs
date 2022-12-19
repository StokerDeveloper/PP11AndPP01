using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace Точка_проката_ЦПКиО_им._Маяковского.Pages
{
    public partial class СтраницаПринятияОборудования : Page
    {
        private ПрокатContext db;
        private Заказы выбранныйЗаказ = null!;

        public СтраницаПринятияОборудования()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);
            InitializeComponent();
        }

        //Блокировка элементов
        private void EnableElems()
        {
            выбрать.IsEnabled = !выбрать.IsEnabled;
            принять.IsEnabled = !принять.IsEnabled;
        }

        //Выбора заказа
        private void GetOrder_Click(object sender, RoutedEventArgs e)
        {
            EnableElems();

            заказ.Text = "Не выбран";

            ОкноВыбораЗаказа окноВыбораЗаказа = new ОкноВыбораЗаказа();
            окноВыбораЗаказа.ShowDialog();

            if (окноВыбораЗаказа.DialogResult == true && окноВыбораЗаказа.заказ != null)
            {
                выбранныйЗаказ = окноВыбораЗаказа.заказ;
                заказ.Text = выбранныйЗаказ.КодЗаказа;
            }

            EnableElems();
        }

        //Принятие оборудования и закрытие заказа с записью в базу данных
        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (заказ.Text == "Не выбран")
            {
                MessageBox.Show("Заказ не выбран", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            EnableElems();

            try
            {
                List<ПрокатОборудования> прокатОборудования = await db.ПрокатОборудованияs.Where(p => p.Заказ == выбранныйЗаказ.Код).ToListAsync();
                List<ПрокатОборудования> НовыйПрокатОборудования = new List<ПрокатОборудования>();

                foreach (ПрокатОборудования по in прокатОборудования)
                {
                    ПрокатОборудования прокат = new ПрокатОборудования();
                    прокат.Заказ = выбранныйЗаказ.Код;
                    прокат.Оборудование = по.Оборудование;
                    прокат.Статус = 2;

                    НовыйПрокатОборудования.Add(прокат);
                }

                if (НовыйПрокатОборудования.Count != 0)
                {
                    db.ПрокатОборудованияs.AddRange(НовыйПрокатОборудования);
                }

                ДвиженияЗаказов движенияЗаказов = new ДвиженияЗаказов();
                движенияЗаказов.Заказ = выбранныйЗаказ.Код;
                движенияЗаказов.Пользователь = ОкноСотрудника.пользователь.Код;
                движенияЗаказов.Дата = DateTime.Now;
                движенияЗаказов.Время = DateTime.Now.TimeOfDay;
                движенияЗаказов.Статус = 3;

                db.ДвиженияЗаказовs.Add(движенияЗаказов);

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Не удалось принять оборудование\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            заказ.Text = "Не выбран";
            EnableElems();

            MessageBox.Show("Оборудование успешно принято. Заказ закрыт", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
