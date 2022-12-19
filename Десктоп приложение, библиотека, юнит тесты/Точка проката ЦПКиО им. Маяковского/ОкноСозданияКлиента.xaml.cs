using System;
using System.Windows;


namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноСозданияКлиента : Window
    {
        private ПрокатContext db;
        public Клиенты? клиент;

        public ОкноСозданияКлиента()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);
            клиент = null;

            InitializeComponent();
        }

        //Создание клиента
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (фамилия.Text != "" && имя.Text != "" && отчество.Text != "" &&
                датаРождения.Text != "" && адрес.Text != "" && электроннаяПочта.Text != "")
            {
                if (серияПаспорта.Text.Length == 4 && номерПаспорта.Text.Length == 6)
                {
                    клиент = new Клиенты();

                    клиент.Фамилия = фамилия.Text;
                    клиент.Имя = имя.Text;
                    клиент.Отчество = отчество.Text;
                    клиент.СерияПаспорта = серияПаспорта.Text;
                    клиент.НомерПаспорта = номерПаспорта.Text;
                    клиент.ДатаРождения = датаРождения.SelectedDate == null ? датаРождения.DisplayDate : (DateTime)датаРождения.SelectedDate;
                    клиент.Адрес = адрес.Text;
                    клиент.ЭлектроннаяПочта = электроннаяПочта.Text;

                    try
                    {
                        db.Клиентыs.Add(клиент);
                        db.SaveChanges();

                        MessageBox.Show("Клиент успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        клиент = null;
                        MessageBox.Show("Ошибка работы с базой данных. Не удалось добавить клиента\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.DialogResult = false;
                    }
                }
                else
                {
                    MessageBox.Show("Заполните номер и серию паспорта", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Заполните данные", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
