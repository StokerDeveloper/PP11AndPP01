using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Точка_проката_ЦПКиО_им._Маяковского.Models;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноСозданияОборудования : Window
    {
        private ПрокатContext db;
        public Оборудование оборудование = null!;

        //При создании окна заполняется комбобокс
        public ОкноСозданияОборудования()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);
            InitializeComponent();

            UpdateComboBox(db.ТипыОборудованияs.ToList());
        }

        //Заполнение комбобокса типами оборудования
        private void UpdateComboBox(List<ТипыОборудования> типыОборудования)
        {
            if (типыОборудования == null || типыОборудования.Count == 0)
                return;

            тип.Items.Clear();

            foreach (ТипыОборудования типОборудования in типыОборудования)
            {
                тип.Items.Add(
                    new ЭлементКомбоБокса(
                        типОборудования.Наименование,
                        типОборудования
                    )
                );
            }

            тип.SelectedIndex = 0;
        }

        //Создание оборудования
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (номер.Text != "" && тип.Text != "")
            {
                оборудование = new Оборудование();
                оборудование.КодОборудования = номер.Text;
                оборудование.Тип = ((ТипыОборудования)((ЭлементКомбоБокса)тип.SelectedItem).значение).Код;
                оборудование.ТипNavigation = (ТипыОборудования)((ЭлементКомбоБокса)тип.SelectedItem).значение;

                try
                {
                    db.Оборудованиеs.Add(оборудование);
                    db.SaveChanges();

                    MessageBox.Show("Оборудование успешно добавлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Не удалось добавить оборудование\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.DialogResult = false;
                }
            }
        }
    }
}
