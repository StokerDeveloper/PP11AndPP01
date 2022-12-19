using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using Точка_проката_ЦПКиО_им._Маяковского.Models;
using System.IO;

namespace Точка_проката_ЦПКиО_им._Маяковского.Pages
{
    public partial class СтраницаОформленияЗаказа : Page
    {
        private ПрокатContext db;
        private Заказы выбранныйЗаказ = null!;
        private ШтрихКод штрихКод = new ШтрихКод();
        private List<ПредоставлениеУслуг> предоставлениеУслуг = new List<ПредоставлениеУслуг>();
        private List<Услуги> услугиЗаказа = new List<Услуги>();
        private List<ComboBox> выдачаОборудования = new List<ComboBox>();
        private List<string> выдачаОборудованияИмена = new List<string>();

        public СтраницаОформленияЗаказа()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);

            InitializeComponent();

            ClearAll();
        }

        //Очистка данных страницы
        private void ClearAll()
        {
            заказ.Text = "Не выбран";
            выбранныйЗаказ = null!;
            штрихКод = new ШтрихКод();
            предоставлениеУслуг = new List<ПредоставлениеУслуг>();
            выдачаОборудования = new List<ComboBox>();
            услугиЗаказа = new List<Услуги>();
            добавленноеОборудованиеДокПанел.Children.Clear();
            штрихКодКартинка.ClearValue(Image.SourceProperty);
        }

        //Блокировка элементов
        private void EnableElems()
        {
            выбрать.IsEnabled = !выбрать.IsEnabled;
            добавленноеОборудованиеДокПанел.IsEnabled = !добавленноеОборудованиеДокПанел.IsEnabled;
            оформить.IsEnabled = !оформить.IsEnabled;
        }

        //Поиск элемента по имени
        public static T FindChild<T>(DependencyObject parent, string childName)
        where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        //Выбор заказа с генерацией штрих кода
        private async void GetOrder_Click(object sender, RoutedEventArgs e)
        {
            EnableElems();

            заказ.Text = "Не выбран";

            ОкноВыбораЗаказа окноВыбораЗаказа = new ОкноВыбораЗаказа();
            окноВыбораЗаказа.ShowDialog();

            if (окноВыбораЗаказа.DialogResult == true)
            {
                if (окноВыбораЗаказа.заказ == null)
                    return;

                try
                {
                    предоставлениеУслуг = new List<ПредоставлениеУслуг>();
                    услугиЗаказа = new List<Услуги>();
                    List<ComboBox> выдачаОборудования = new List<ComboBox>();

                    выбранныйЗаказ = окноВыбораЗаказа.заказ;

                    заказ.Text = выбранныйЗаказ.КодЗаказа;

                    List<int> numbers = new List<int>();
                    ДвиженияЗаказов движение = await db.ДвиженияЗаказовs.Where(p => p.Заказ == выбранныйЗаказ.Код).FirstAsync();
                    string dataivrema = движение.Дата.ToString("ddMMyyyy") + движение.Время.ToString("hh\\mm");

                    string cod = Convert.ToString(выбранныйЗаказ.Код);

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
                    numbers.Add(выбранныйЗаказ.ВремяПрокатаЧасов);
                    numbers.Add((new Random()).Next(1, 10)); numbers.Add((new Random()).Next(1, 10));
                    numbers.Add((new Random()).Next(1, 10)); numbers.Add((new Random()).Next(1, 10));
                    numbers.Add((new Random()).Next(1, 10)); numbers.Add((new Random()).Next(1, 10));

                    ШтрихКод штрихКод = new ШтрихКод(numbers);
                    this.штрихКод = штрихКод;
                    штрихКодКартинка.Source = штрихКод.GetImageSource;

                    предоставлениеУслуг = await db.ПредоставлениеУслугs.Where(p =>
                        p.Заказ == выбранныйЗаказ.Код
                    ).Include(u =>
                        u.УслугаNavigation
                    ).ToListAsync();

                    foreach (ПредоставлениеУслуг пу in предоставлениеУслуг)
                    {
                        Услуги услуга = пу.УслугаNavigation;
                        услугиЗаказа.Add(услуга);

                        Label label = new Label();
                        label.Content = услуга.Наименование;
                        label.FontSize = 14;

                        DockPanel dockPanel = new DockPanel();
                        dockPanel.Name = "докПанел" + услуга.Код;
                        dockPanel.Height = 30;
                        dockPanel.SetValue(DockPanel.DockProperty, Dock.Top);
                        dockPanel.VerticalAlignment = VerticalAlignment.Top;
                        dockPanel.Background = new SolidColorBrush(Color.FromArgb(255, (byte)73, (byte)140, (byte)81));

                        dockPanel.Children.Add(label);

                        if (услуга.Тип == 2)
                        {
                            ComboBox comboBox = new ComboBox();
                            comboBox.Name = "комбоБокс" + услуга.Код;
                            comboBox.FontSize = 14;
                            comboBox.HorizontalAlignment = HorizontalAlignment.Right;
                            comboBox.Width = 150;

                            List<Оборудование> обор = new List<Оборудование>();
                            int typeOb = 0;

                            switch (услуга.Наименование)
                            {
                                case "Прокат коньков":
                                    typeOb = 1;
                                    break;
                                case "Прокат опорного оборудования для катка":
                                    typeOb = 2;
                                    break;
                                case "Прокат шлема":
                                    typeOb = 3;
                                    break;
                                case "Прокат набора защитного оборудования":
                                    typeOb = 4;
                                    break;
                                case "Прокат детских коньков":
                                    typeOb = 5;
                                    break;
                                case "Прокат вартушки":
                                    typeOb = 6;
                                    break;
                                case "Прокат санок":
                                    typeOb = 7;
                                    break;
                                case "Прокат салазок":
                                    typeOb = 8;
                                    break;
                                case "Прокат ледянки":
                                    typeOb = 9;
                                    break;
                            }

                            if (typeOb != 0)
                            {
                                List<Оборудование> списокОбор = await db.Оборудованиеs.
                                    Where(p => p.Тип == typeOb).ToListAsync();

                                if (списокОбор.Count != 0)
                                {
                                    foreach (Оборудование _оборудование in списокОбор)
                                    {
                                        comboBox.Items.Add(
                                            new ЭлементКомбоБокса(
                                                _оборудование.КодОборудования,
                                                _оборудование
                                            )
                                        );
                                    }

                                    comboBox.SelectedIndex = 0;
                                }
                                else
                                {
                                    comboBox.Text = "Нет доступного оборудования";
                                }

                                dockPanel.Children.Add(comboBox);
                                выдачаОборудования.Add(comboBox);
                                выдачаОборудованияИмена.Add(comboBox.Name);
                            }
                        }

                        добавленноеОборудованиеДокПанел.Children.Add(dockPanel);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка работы с базой данных. Услуги не получены\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            EnableElems();
        }

        //Оформление заказа с записью в базу данных, созданием пдф файла штрих кода
        //и записью ссылки в текстовый документ
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
                List<ПрокатОборудования> прокатОборудования = new List<ПрокатОборудования>();

                foreach (string name in выдачаОборудованияИмена)
                {
                    ComboBox комбоБокс = FindChild<ComboBox>(this, name);

                    Оборудование оборудование = (Оборудование)((ЭлементКомбоБокса)комбоБокс.SelectedItem).значение;

                    ПрокатОборудования по = new ПрокатОборудования();
                    по.Заказ = выбранныйЗаказ.Код;
                    по.Оборудование = оборудование.Код;
                    по.Статус = 1;

                    прокатОборудования.Add(по);
                }

                if (прокатОборудования.Count != 0)
                {
                    db.ПрокатОборудованияs.AddRange(прокатОборудования);
                }

                ДвиженияЗаказов движенияЗаказов = new ДвиженияЗаказов();
                движенияЗаказов.Заказ = выбранныйЗаказ.Код;
                движенияЗаказов.Пользователь = ОкноСотрудника.пользователь.Код;
                движенияЗаказов.Дата = DateTime.Now;
                движенияЗаказов.Время = DateTime.Now.TimeOfDay;
                движенияЗаказов.Статус = 2;

                db.ДвиженияЗаказовs.Add(движенияЗаказов);

                await db.SaveChangesAsync();

                try
                {
                    this.штрихКод.СохранитьКакПДФ("Заказ-" + выбранныйЗаказ.Код);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось сохранить штрих код в пдф\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    string ссылка = "https://wsrussia.ru/?data=" + Base64Encode(
                        "дата_заказа=" +
                        движенияЗаказов.Дата.ToString("yyyy-dd-MM") + 
                        "T" + 
                        движенияЗаказов.Время.ToString("hh\\:mm\\:ss") +
                        "номер_заказа=" +
                        выбранныйЗаказ.КодЗаказа    
                    );


                    using (StreamWriter writer = new StreamWriter("Заказ-" + выбранныйЗаказ.Код + ".txt"))
                    {
                        await writer.WriteLineAsync(ссылка);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось сохранить ссылку\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка работы с базой данных. Не удалось оформить заказ\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ClearAll();
            EnableElems();

            MessageBox.Show("Заказ успешно оформлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //Закодировать строку в base64
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
