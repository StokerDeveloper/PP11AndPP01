using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Точка_проката_ЦПКиО_им._Маяковского.Models;

namespace Точка_проката_ЦПКиО_им._Маяковского.Pages
{
    public partial class СтраницаПринятияЗаказа : Page
    {
        private ПрокатContext db;
        private List<Услуги> услуги = new List<Услуги>();
        private List<int> кодыУслуг = new List<int>();
        private int стоимостьУслуг = 0;
        private Клиенты? выбранныйКлиент = null;

        //При создании загружаются услуги
        public СтраницаПринятияЗаказа()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);

            try
            {
                услуги = db.Услугиs.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Услуги не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            InitializeComponent();

            ClearAll();
        }

        //Очистка данных страницы
        private void ClearAll()
        {
            номерЗаказа.Text = "";
            номерЗаказа.Background = Brushes.White;

            количествоЧасов.Text = "1";
            стоимость.Text = "0";
            стоимостьУслуг = 0;
            List<int> кодыУслуг = new List<int>();
            выбранныйКлиент = null;
            клиент.Text = "Не выбран";

            UpdateComboBox(услуги);

            добавленныеУслугиДокПанел.Children.Clear();

            try
            {
                string _номерЗаказа = Convert.ToString(db.Заказыs.OrderBy(p => p.Код).Last().КодЗаказа);

                int _номерЗаказаЧисло;
                if (int.TryParse(_номерЗаказа, out _номерЗаказаЧисло))
                {
                    номерЗаказа.Text = "" + (_номерЗаказаЧисло + 1);
                }
                else
                {
                    номерЗаказа.Text = _номерЗаказа + (new Random()).Next(1, 10000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Не удалось получить номер последнего заказа\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Заполнение комбобокса услугами
        private void UpdateComboBox(List<Услуги> услуги)
        {
            if (услуги == null || услуги.Count == 0)
                return;

            добавлениеУслугКомбоБокс.Items.Clear();

            foreach (Услуги услуга in услуги)
            {
                добавлениеУслугКомбоБокс.Items.Add(
                    new ЭлементКомбоБокса(
                        услуга.Наименование,
                        услуга
                    )
                );
            }

            добавлениеУслугКомбоБокс.SelectedIndex = 0;
        }

        //Изменение количества часов
        private void ButtonHourse_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int count = Convert.ToInt32(количествоЧасов.Text);

            switch (button.Content)
            {
                case "-":
                    if (count > 1)
                        количествоЧасов.Text = "" + (count - 1);
                    break;
                case "+":
                    if (count < 24)
                        количествоЧасов.Text = "" + (count + 1);
                    break;
            }

            SetPrice();
        }

        //Расчет итоговой стоимости
        private void SetPrice()
        {
            стоимость.Text = "" + (Convert.ToInt32(количествоЧасов.Text) * стоимостьУслуг);
        }

        //Проверка номера заказа на уникальность
        private void UniqueNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (номерЗаказа.Text == "")
            {
                MessageBox.Show("Заполните номер заказа", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            UniqueNumber(номерЗаказа.Text);
        }

        //Метод проверки номера заказа на уникальность
        private bool UniqueNumber(string number)
        {
            bool result = true;

            try
            {
                Заказы? заказ = db.Заказыs.FirstOrDefault(p =>
                    p.КодЗаказа == number
                );

                if (заказ != null)
                    result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Не удалось проверить уникальнось номера заказа\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (result)
                номерЗаказа.Background = Brushes.Green;
            else
                номерЗаказа.Background = Brushes.Red;

            return result;
        }

        //Изменение цвета номера заказа
        private void номерЗаказа_TextChanged(object sender, TextChangedEventArgs e)
        {
            номерЗаказа.Background = Brushes.White;
        }

        //Добавление услуги
        private void AddService_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (добавлениеУслугКомбоБокс.Items.Count == 0)
                return;

            Услуги услуга = (Услуги)((ЭлементКомбоБокса)добавлениеУслугКомбоБокс.SelectedItem).значение;
            добавлениеУслугКомбоБокс.Items.Remove(добавлениеУслугКомбоБокс.SelectedItem);
            добавлениеУслугКомбоБокс.SelectedIndex = 0;

            Label label = new Label();
            label.Content = услуга.Наименование;
            label.FontSize = 16;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Margin = new Thickness(10, 0, 0, 0);

            Button button = new Button();
            button.Name = "кнопка" + услуга.Код;
            button.Content = "Удалить";
            button.Height = 25;
            button.Width = 100;
            button.Margin = new Thickness(0, 0, 30, 0);
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.Click += new RoutedEventHandler(DeleteService_Click);

            DockPanel dockPanel = new DockPanel();
            dockPanel.Name = "докПанел" + услуга.Код;
            dockPanel.Height = 30;
            dockPanel.SetValue(DockPanel.DockProperty, Dock.Top);
            dockPanel.Margin = new Thickness(0, 0, 0, 1);
            dockPanel.HorizontalAlignment = HorizontalAlignment.Center;
            dockPanel.VerticalAlignment = VerticalAlignment.Top;
            dockPanel.Background = new SolidColorBrush(Color.FromArgb(255, (byte)73, (byte)140, (byte)81));
            dockPanel.MinWidth = 800;

            dockPanel.Children.Add(label);
            dockPanel.Children.Add(button);

            добавленныеУслугиДокПанел.Children.Add(dockPanel);
            кодыУслуг.Add(услуга.Код);
            стоимостьУслуг += услуга.СтоимостьРублейЗаЧас;

            SetPrice();
        }

        //Удаление услуги
        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            int кодУслуги = Convert.ToInt32(button.Name.Replace("кнопка", ""));
            Услуги услуга = new Услуги();

            foreach (Услуги _услуга in услуги)
            {
                if (кодУслуги == _услуга.Код)
                {
                    услуга = _услуга;
                    break;
                }
            }

            добавлениеУслугКомбоБокс.Items.Add(
                new ЭлементКомбоБокса(
                    услуга.Наименование,
                    услуга
                )
            );

            добавлениеУслугКомбоБокс.SelectedIndex = 0;

            DockPanel dockPanel = (DockPanel)добавленныеУслугиДокПанел.FindName("докПанел" + кодУслуги);

            foreach (var элемент in добавленныеУслугиДокПанел.Children)
            {
                if (элемент is DockPanel)
                {
                    dockPanel = (DockPanel)элемент;

                    if (dockPanel.Name == "докПанел" + кодУслуги)
                        break;
                }
            }

            добавленныеУслугиДокПанел.Children.Remove(dockPanel);
            кодыУслуг.Remove(кодУслуги);
            стоимостьУслуг -= услуга.СтоимостьРублейЗаЧас;

            SetPrice();
        }

        //Создание заказа(принятие)
        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (номерЗаказа.Text == "")
            {
                MessageBox.Show("Заполните номер заказа", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (!UniqueNumber(номерЗаказа.Text))
            {
                MessageBox.Show("Номер заказа неунекален", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (клиент.Text == "Не выбран" || выбранныйКлиент == null)
            {
                MessageBox.Show("Клиент не выбран", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (кодыУслуг.Count == 0)
            {
                MessageBox.Show("Ни одной услуги не выбрано", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Заказы заказ = new Заказы();
            заказ.КодЗаказа = номерЗаказа.Text;
            заказ.Клиент = выбранныйКлиент.Код;
            заказ.ВремяПрокатаЧасов = Convert.ToInt32(количествоЧасов.Text);

            try
            {
                db.Заказыs.Add(заказ);
                db.SaveChanges();

                ДвиженияЗаказов движенияЗаказов = new ДвиженияЗаказов();
                движенияЗаказов.Заказ = заказ.Код;
                движенияЗаказов.Пользователь = ОкноСотрудника.пользователь.Код;
                движенияЗаказов.Дата = DateTime.Now;
                движенияЗаказов.Время = DateTime.Now.TimeOfDay;
                движенияЗаказов.Статус = 1;

                db.ДвиженияЗаказовs.Add(движенияЗаказов);

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Не удалось создать заказ\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<ПредоставлениеУслуг> предоставлениеУслуг = new List<ПредоставлениеУслуг>();
            foreach (int кодУслуги in кодыУслуг)
            {
                ПредоставлениеУслуг предоставлениеУслуги = new ПредоставлениеУслуг();
                предоставлениеУслуги.Заказ = заказ.Код;
                предоставлениеУслуги.Услуга = кодУслуги;

                предоставлениеУслуг.Add(предоставлениеУслуги);
            }

            try
            {
                db.ПредоставлениеУслугs.AddRange(предоставлениеУслуг);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                try
                {
                    db.Заказыs.Remove(заказ);
                    db.ПредоставлениеУслугs.RemoveRange(предоставлениеУслуг);
                    db.SaveChanges();
                }
                catch { }

                MessageBox.Show("Ошибка работы с базой данных. Не удалось создать заказ\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ClearAll();
            MessageBox.Show("Заказ успешно принят", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Выбор клиента
        private void GetClient_Click(object sender, RoutedEventArgs e)
        {
            клиент.Text = "Не выбран";

            ОкноВыбораКлиента окноВыбораКлиента = new ОкноВыбораКлиента();
            окноВыбораКлиента.ShowDialog();

            if (окноВыбораКлиента.DialogResult == true)
            {
                выбранныйКлиент = окноВыбораКлиента.клиент;
                
                if (выбранныйКлиент != null)
                {
                    клиент.Text = выбранныйКлиент.Фамилия + " " + выбранныйКлиент.Имя + " " + выбранныйКлиент.Отчество;
                }
            }
        }

        //Создание клиента с последующим его выбором
        private void CreateClient_Click(object sender, RoutedEventArgs e)
        {
            клиент.Text = "Не выбран";

            ОкноСозданияКлиента окноСозданияКлиента = new ОкноСозданияКлиента();
            окноСозданияКлиента.ShowDialog();

            if (окноСозданияКлиента.DialogResult == true)
            {
                выбранныйКлиент = окноСозданияКлиента.клиент;

                if (выбранныйКлиент != null)
                {
                    клиент.Text = выбранныйКлиент.Фамилия + " " + выбранныйКлиент.Имя + " " + выбранныйКлиент.Отчество;
                }
            }
        }
    }
}
