using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using Точка_проката_ЦПКиО_им._Маяковского.Models;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.IO;
using System.Windows.Media.Imaging;

namespace Точка_проката_ЦПКиО_им._Маяковского.Pages
{
    public partial class СтраницаОтчета : Page
    {
        private ПрокатContext db;

        private DockPanel currentTable = null;
        private CartesianChart currentGrafic = null;

        private DockPanel currentTable1 = null;
        private CartesianChart currentGrafic1 = null;

        private DockPanel currentTable2 = null;
        private CartesianChart currentGrafic2 = null;

        public СтраницаОтчета()
        {
            db = new ПрокатContext(Properties.Resources.connectionString);
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateComboBox(
                await db.Услугиs.ToListAsync()
            );

            DataContext = this;
        }

        private async void Сформировать_Click(object sender, RoutedEventArgs e)
        {
            Enable();

            DateTime startDate = началоПериода.SelectedDate == null ?
                DateTime.MinValue :
                new DateTime(
                    ((DateTime)началоПериода.SelectedDate).Year,
                    ((DateTime)началоПериода.SelectedDate).Month,
                    ((DateTime)началоПериода.SelectedDate).Day
                );
            DateTime endDate = конецПериода.SelectedDate == null ?
                DateTime.MaxValue : 
                new DateTime(
                    ((DateTime)конецПериода.SelectedDate).Year,
                    ((DateTime)конецПериода.SelectedDate).Month,
                    ((DateTime)конецПериода.SelectedDate).Day
                );

            List<DateTime> даты = new List<DateTime>();
            List<double> значения = new List<double>();

            if (началоПериода.SelectedDate == null || конецПериода.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (startDate > endDate)
            {
                MessageBox.Show("Выберите правильные даты", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (Convert.ToString(((TabItem)табКонтрол.SelectedItem).Header) == "Количество оказанных услуг")
                {
                    dockPanel1.Children.Clear();

                    List<ДвиженияЗаказов> движенияЗаказов = new List<ДвиженияЗаказов>();
                    try
                    {
                        движенияЗаказов = await db.ДвиженияЗаказовs.
                            FromSqlRaw(@$"
                                select 
                                    * 
                                from 
                                    [Движения заказов] 
                                where 
                                    [Движения заказов].[Статус] = 2 and 
                                    [Движения заказов].[Дата] >= '{startDate}' and 
                                    [Движения заказов].[Дата] <= '{endDate}'
                            ").
                            Include(i => i.ЗаказNavigation).
                            Include(i => i.ЗаказNavigation.ПредоставлениеУслугs).
                            OrderBy(o => o.Дата).
                            ToListAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка работы с базой данных. Не удалось сформировать табличный отчет\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        Enable();
                        return;
                    }

                    int index = 0;
                    for (int i = 0; i < движенияЗаказов.Count; i++)
                    {
                        DateTime дата = new DateTime(
                            движенияЗаказов[i].Дата.Year,
                            движенияЗаказов[i].Дата.Month,
                            движенияЗаказов[i].Дата.Day
                        );

                        if (i == 0)
                        {
                            DateTime _дата = startDate;

                            while (_дата != дата)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }

                            даты.Add(дата);
                            значения.Add(движенияЗаказов[i].ЗаказNavigation.ПредоставлениеУслугs.Count);
                        }
                        else if (даты[index] != дата)
                        {
                            DateTime _дата = даты[index].AddDays(1);

                            while (_дата != дата)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }

                            даты.Add(дата);
                            значения.Add(движенияЗаказов[i].ЗаказNavigation.ПредоставлениеУслугs.Count);

                            index += 1;
                        }
                        else
                        {
                            значения[index] += движенияЗаказов[i].ЗаказNavigation.ПредоставлениеУслугs.Count;
                        }

                        if (i == движенияЗаказов.Count - 1)
                        {
                            DateTime _дата = даты[index].AddDays(1);

                            while (_дата <= endDate)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }
                        }
                    }

                    if (движенияЗаказов.Count == 0)
                    {
                        DateTime дата = startDate;

                        while (дата <= endDate)
                        {
                            даты.Add(дата);
                            значения.Add(0);

                            дата = дата.AddDays(1);
                        }
                    }


                    DockPanel table = new();
                    table.Children.Add(СформироватьЗаголовкиТаблицы("Количество оказанных услуг"));
                    table.Children.Add(СформироватьТаблицу(даты, значения));

                    Viewbox viewbox = new();
                    viewbox.Child = table;
                    viewbox.Measure(table.RenderSize);
                    viewbox.Arrange(new Rect(new Point(0, 0), table.RenderSize));
                    viewbox.UpdateLayout();

                    currentTable = table;

                    ScrollViewer scrollViewer = new();
                    scrollViewer.Content = viewbox;


                    CartesianChart grafic = СформироватьГрафик("Количество оказанных услуг:", даты, значения, 600, 200);

                    Viewbox viewbox1 = new();
                    viewbox1.Child = grafic;
                    viewbox1.Measure(grafic.RenderSize);
                    viewbox1.Arrange(new Rect(new Point(0, 0), grafic.RenderSize));
                    grafic.Update(true, true);
                    viewbox1.UpdateLayout();

                    currentGrafic = grafic;


                    if (формироватьТаблицу.IsChecked == true)
                    {
                        dockPanel1.Children.Add(scrollViewer);
                    }
                    else if (формироватьГрафик.IsChecked == true)
                    {
                        dockPanel1.Children.Add(viewbox1);
                    }
                }
                else if (Convert.ToString(((TabItem)табКонтрол.SelectedItem).Header) == "Количество заказов по услуге")
                {
                    dockPanel2.Children.Clear();

                    List<ДвиженияЗаказов> движенияЗаказов = new List<ДвиженияЗаказов>();
                    try
                    {
                        движенияЗаказов = await db.ДвиженияЗаказовs.
                            FromSqlRaw(@$"
                                select 
                                    [Движения заказов].* 
                                from 
                                    [Движения заказов], 
                                    [Предоставление услуг]
                                where 
                                    [Движения заказов].[Статус] = 2 and 
                                    [Движения заказов].[Дата] >= '{startDate}' and 
                                    [Движения заказов].[Дата] <= '{endDate}' and 
                                    [Движения заказов].[Заказ] = [Предоставление услуг].[Заказ] and 
                                    [Предоставление услуг].[Услуга] = {((Услуги)((ЭлементКомбоБокса)comboBoxTypes.SelectedItem).значение).Код}
                            ").
                            OrderBy(o => o.Дата).
                            ToListAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка работы с базой данных. Не удалось сформировать табличный отчет\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        Enable();
                        return;
                    }

                    движенияЗаказов = движенияЗаказов.Distinct().ToList();

                    int index = 0;
                    for (int i = 0; i < движенияЗаказов.Count; i++)
                    {
                        DateTime дата = new DateTime(
                            движенияЗаказов[i].Дата.Year,
                            движенияЗаказов[i].Дата.Month,
                            движенияЗаказов[i].Дата.Day
                        );

                        if (i == 0)
                        {
                            DateTime _дата = startDate;

                            while (_дата != дата)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }

                            даты.Add(дата);
                            значения.Add(1);
                        }
                        else if (даты[index] != дата)
                        {
                            DateTime _дата = даты[index].AddDays(1);

                            while (_дата != дата)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }

                            даты.Add(дата);
                            значения.Add(1);

                            index += 1;
                        }
                        else
                        {
                            значения[index] += 1;
                        }

                        if (i == движенияЗаказов.Count - 1)
                        {
                            DateTime _дата = даты[index].AddDays(1);

                            while (_дата <= endDate)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }
                        }
                    }

                    if (движенияЗаказов.Count == 0)
                    {
                        DateTime дата = startDate;

                        while (дата <= endDate)
                        {
                            даты.Add(дата);
                            значения.Add(0);

                            дата = дата.AddDays(1);
                        }
                    }


                    DockPanel table = new();
                    table.Children.Add(СформироватьЗаголовкиТаблицы("Количество заказов"));
                    table.Children.Add(СформироватьТаблицу(даты, значения));

                    Viewbox viewbox = new();
                    viewbox.Child = table;
                    viewbox.Measure(table.RenderSize);
                    viewbox.Arrange(new Rect(new Point(0, 0), table.RenderSize));
                    viewbox.UpdateLayout();

                    currentTable1 = table;

                    ScrollViewer scrollViewer = new();
                    scrollViewer.Content = viewbox;


                    CartesianChart grafic = СформироватьГрафик("Количество заказов:", даты, значения, 600, 200);

                    Viewbox viewbox1 = new();
                    viewbox1.Child = grafic;
                    viewbox1.Measure(grafic.RenderSize);
                    viewbox1.Arrange(new Rect(new Point(0, 0), grafic.RenderSize));
                    grafic.Update(true, true);
                    viewbox1.UpdateLayout();

                    currentGrafic1 = grafic;


                    if (формироватьТаблицу.IsChecked == true)
                    {
                        dockPanel2.Children.Add(scrollViewer);
                    }
                    else if (формироватьГрафик.IsChecked == true)
                    {
                        dockPanel2.Children.Add(viewbox1);
                    }
                }
                else if (Convert.ToString(((TabItem)табКонтрол.SelectedItem).Header) == "Количество заказов")
                {
                    dockPanel3.Children.Clear();

                    List<ДвиженияЗаказов> движенияЗаказов = new List<ДвиженияЗаказов>();
                    try
                    {
                        движенияЗаказов = await db.ДвиженияЗаказовs.
                            FromSqlRaw(@$"
                                select 
                                    * 
                                from 
                                    [Движения заказов] 
                                where 
                                    [Движения заказов].[Статус] = 2 and 
                                    [Движения заказов].[Дата] >= '{startDate}' and 
                                    [Движения заказов].[Дата] <= '{endDate}'
                            ").
                            OrderBy(o => o.Дата).
                            ToListAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка работы с базой данных. Не удалось сформировать табличный отчет\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        Enable();
                        return;
                    }

                    int index = 0;
                    for (int i = 0; i < движенияЗаказов.Count; i++)
                    {
                        DateTime дата = new DateTime(
                            движенияЗаказов[i].Дата.Year,
                            движенияЗаказов[i].Дата.Month,
                            движенияЗаказов[i].Дата.Day
                        );

                        if (i == 0)
                        {
                            DateTime _дата = startDate;

                            while (_дата != дата)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }

                            даты.Add(дата);
                            значения.Add(1);
                        }
                        else if (даты[index] != дата)
                        {
                            DateTime _дата = даты[index].AddDays(1);

                            while (_дата != дата)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }

                            даты.Add(дата);
                            значения.Add(1);

                            index += 1;
                        }
                        else
                        {
                            значения[index] += 1;
                        }

                        if (i == движенияЗаказов.Count - 1)
                        {
                            DateTime _дата = даты[index].AddDays(1);

                            while (_дата <= endDate)
                            {
                                даты.Add(_дата);
                                значения.Add(0);

                                index += 1;

                                _дата = _дата.AddDays(1);
                            }
                        }
                    }

                    if (движенияЗаказов.Count == 0)
                    {
                        DateTime дата = startDate;

                        while (дата <= endDate)
                        {
                            даты.Add(дата);
                            значения.Add(0);

                            дата = дата.AddDays(1);
                        }
                    }


                    DockPanel table = new();
                    table.Children.Add(СформироватьЗаголовкиТаблицы("Количество заказов"));
                    table.Children.Add(СформироватьТаблицу(даты, значения));

                    Viewbox viewbox = new();
                    viewbox.Child = table;
                    viewbox.Measure(table.RenderSize);
                    viewbox.Arrange(new Rect(new Point(0, 0), table.RenderSize));
                    viewbox.UpdateLayout();

                    currentTable2 = table;

                    ScrollViewer scrollViewer = new();
                    scrollViewer.Content = viewbox;


                    CartesianChart grafic = СформироватьГрафик("Количество заказов:", даты, значения, 600, 200);

                    Viewbox viewbox1 = new();
                    viewbox1.Child = grafic;
                    viewbox1.Measure(grafic.RenderSize);
                    viewbox1.Arrange(new Rect(new Point(0, 0), grafic.RenderSize));
                    grafic.Update(true, true);
                    viewbox1.UpdateLayout();

                    currentGrafic2 = grafic;


                    if (формироватьТаблицу.IsChecked == true)
                    {
                        dockPanel3.Children.Add(scrollViewer);
                    }
                    else if (формироватьГрафик.IsChecked == true)
                    {
                        dockPanel3.Children.Add(viewbox1);
                    }
                }
            }

            Enable();
        }

        private async void СохранитьВПДФ_Click(object sender, RoutedEventArgs e)
        {
            Enable();

            DateTime startDate = началоПериода.SelectedDate == null ?
                DateTime.MinValue :
                new DateTime(
                    ((DateTime)началоПериода.SelectedDate).Year,
                    ((DateTime)началоПериода.SelectedDate).Month,
                    ((DateTime)началоПериода.SelectedDate).Day
                );
            DateTime endDate = конецПериода.SelectedDate == null ?
                DateTime.MaxValue :
                new DateTime(
                    ((DateTime)конецПериода.SelectedDate).Year,
                    ((DateTime)конецПериода.SelectedDate).Month,
                    ((DateTime)конецПериода.SelectedDate).Day
                );

            if (началоПериода.SelectedDate == null || конецПериода.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (endDate.Ticks - startDate.Ticks > DateTime.MinValue.AddDays(1).Ticks * 31)
            {
                MessageBox.Show("Для сохранения отчета в пдф его период не должен превышать 31 день", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (Convert.ToString(((TabItem)табКонтрол.SelectedItem).Header) == "Количество оказанных услуг")
                {
                    if (currentTable == null || currentGrafic == null)
                    {
                        MessageBox.Show("Отчет не сформирован", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        Enable();
                        return;
                    }

                    ОкноВыбораВариантаСохраненияОтчетаВПДФ выбор = new("Выбор варианта сохранения отчета по количеству оказанных услуг");
                    выбор.ShowDialog();
                    
                    if (выбор.DialogResult == true)
                        if (
                                SaveToPDF(
                                    1,
                                    выбор.value,
                                    $"Количество оказанных услуг {startDate.ToShortDateString()}-{endDate.ToShortDateString()}")
                            )
                            MessageBox.Show("Отчет сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show("Неудалось сформировать отчет", "Неудача", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (Convert.ToString(((TabItem)табКонтрол.SelectedItem).Header) == "Количество заказов по услуге")
                {
                    if (currentTable1 == null || currentGrafic1 == null)
                    {
                        MessageBox.Show("Отчет не сформирован", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        Enable();
                        return;
                    }

                    ОкноВыбораВариантаСохраненияОтчетаВПДФ выбор = new("Выбор варианта сохранения отчета по количеству заказов по услуге");
                    выбор.ShowDialog();

                    if (выбор.DialogResult == true)
                        if (
                                SaveToPDF(
                                    1,
                                    выбор.value,
                                    $"Количество заказов по услуге {startDate.ToShortDateString()}-{endDate.ToShortDateString()}")
                            )
                            MessageBox.Show("Отчет сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show("Неудалось сформировать отчет", "Неудача", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (Convert.ToString(((TabItem)табКонтрол.SelectedItem).Header) == "Количество заказов")
                {
                    if (currentTable2 == null || currentGrafic2 == null)
                    {
                        MessageBox.Show("Отчет не сформирован", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        Enable();
                        return;
                    }

                    ОкноВыбораВариантаСохраненияОтчетаВПДФ выбор = new("Выбор варианта сохранения отчета по количеству заказов");
                    выбор.ShowDialog();

                    if (выбор.DialogResult == true)
                        if (
                                SaveToPDF(
                                    1,
                                    выбор.value,
                                    $"Количество заказов {startDate.ToShortDateString()}-{endDate.ToShortDateString()}")
                            )
                            MessageBox.Show("Отчет сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show("Неудалось сформировать отчет", "Неудача", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            Enable();
        }

        private void UpdateComboBox(List<Услуги> услуги)
        {
            if (услуги == null || услуги.Count == 0)
                return;

            comboBoxTypes.Items.Clear();

            foreach (Услуги услуга in услуги)
            {
                comboBoxTypes.Items.Add(
                    new ЭлементКомбоБокса(
                        услуга.Наименование,
                        услуга
                    )
                );
            }

            comboBoxTypes.SelectedIndex = 0;
        }

        private void Enable()
        {
            началоПериода.IsEnabled = !началоПериода.IsEnabled;
            конецПериода.IsEnabled = !конецПериода.IsEnabled;

            формироватьТаблицу.IsEnabled = !формироватьТаблицу.IsEnabled;
            формироватьГрафик.IsEnabled = !формироватьГрафик.IsEnabled;

            сформировать.IsEnabled = !сформировать.IsEnabled;
            сохранить.IsEnabled = !сохранить.IsEnabled;

            табКонтрол.IsEnabled = !табКонтрол.IsEnabled;
        }

        private DockPanel СформироватьЗаголовкиТаблицы(string имяВторогоЗаголовка)
        {
            Label label1 = new Label();
            label1.Content = "День (номер)";
            label1.HorizontalAlignment = HorizontalAlignment.Center;

            Border border1 = new Border();
            border1.BorderBrush = Brushes.Black;
            border1.BorderThickness = new Thickness(1);
            border1.Background = Brushes.White;
            border1.Width = 400;

            border1.Child = label1;


            Label label2 = new Label();
            label2.Content = имяВторогоЗаголовка;
            label2.HorizontalAlignment = HorizontalAlignment.Center;

            Border border2 = new Border();
            border2.BorderBrush = Brushes.Black;
            border2.BorderThickness = new Thickness(1);
            border2.Background = Brushes.White;
            border2.Height = 35;

            border2.Child = label2;


            DockPanel dockPanel = new DockPanel();
            dockPanel.Height = 35;
            dockPanel.VerticalAlignment = VerticalAlignment.Top;
            dockPanel.SetValue(DockPanel.DockProperty, Dock.Top);

            dockPanel.Children.Add(border1);
            dockPanel.Children.Add(border2);

            return dockPanel;
        }

        private DockPanel СформироватьТаблицу(List<DateTime> даты, List<double> значения)
        {
            DockPanel tableDockPanel = new();
            tableDockPanel.Background = new SolidColorBrush(Color.FromArgb(255, (byte)118, (byte)227, (byte)131));

            int year = 0;
            int month = 0;

            for (int i = 0; i < даты.Count(); i++)
            {
                DateTime дата = даты[i];

                if (year != дата.Year)
                {
                    year = дата.Year;

                    Label labelYear = new();
                    labelYear.Content = "" + year;
                    labelYear.HorizontalAlignment = HorizontalAlignment.Center;

                    Border borderYear = new();
                    borderYear.BorderBrush = Brushes.Black;
                    borderYear.BorderThickness = new Thickness(1); ;
                    borderYear.Background = new SolidColorBrush(Color.FromArgb(255, (byte)73, (byte)140, (byte)81));
                    borderYear.Height = 35;
                    borderYear.SetValue(DockPanel.DockProperty, Dock.Top);
                    borderYear.Child = labelYear;

                    tableDockPanel.Children.Add(borderYear);
                }

                if (month != дата.Month)
                {
                    month = дата.Month;

                    Label labelMonth = new();

                    switch (month)
                    {
                        case 1:
                            labelMonth.Content = "Январь";
                            break;
                        case 2:
                            labelMonth.Content = "Февраль";
                            break;
                        case 3:
                            labelMonth.Content = "Март";
                            break;
                        case 4:
                            labelMonth.Content = "Апрель";
                            break;
                        case 5:
                            labelMonth.Content = "Май";
                            break;
                        case 6:
                            labelMonth.Content = "Июнь";
                            break;
                        case 7:
                            labelMonth.Content = "Июль";
                            break;
                        case 8:
                            labelMonth.Content = "Август";
                            break;
                        case 9:
                            labelMonth.Content = "Сентябрь";
                            break;
                        case 10:
                            labelMonth.Content = "Октябрь";
                            break;
                        case 11:
                            labelMonth.Content = "Ноябрь";
                            break;
                        case 12:
                            labelMonth.Content = "Декабрь";
                            break;

                    }

                    Border borderMonth = new();
                    borderMonth.BorderBrush = Brushes.Black;
                    borderMonth.BorderThickness = new Thickness(1); ;
                    borderMonth.Background = new SolidColorBrush(Color.FromArgb(255, (byte)73, (byte)140, (byte)81));
                    borderMonth.Height = 35;
                    borderMonth.SetValue(DockPanel.DockProperty, Dock.Top);
                    borderMonth.Child = labelMonth;

                    tableDockPanel.Children.Add(borderMonth);
                }

                Label labelDay = new();
                labelDay.Content = "" + дата.Day;

                Border borderDay = new();
                borderDay.BorderBrush = Brushes.Black;
                borderDay.BorderThickness = new Thickness(1);
                borderDay.Width = 400;
                borderDay.SetValue(DockPanel.DockProperty, Dock.Left);
                borderDay.Child = labelDay;


                Label labelCount = new Label();
                labelCount.Content = "" + значения[i];

                Border borderCount = new Border();
                borderCount.BorderBrush = Brushes.Black;
                borderCount.BorderThickness = new Thickness(1);
                borderCount.SetValue(DockPanel.DockProperty, Dock.Right);
                borderCount.Child = labelCount;

                DockPanel dockPanel = new DockPanel();
                dockPanel.Height = 35;
                dockPanel.VerticalAlignment = VerticalAlignment.Top;
                dockPanel.SetValue(DockPanel.DockProperty, Dock.Top);
                dockPanel.Children.Add(borderDay);
                dockPanel.Children.Add(borderCount);
                if (значения[i] == 0)
                    dockPanel.Background = Brushes.Gray;

                tableDockPanel.Children.Add(dockPanel);
            }

            return tableDockPanel;
        }

        public Func<double, string> Formatter { get; set; } = value => new DateTime((long)value).ToString("dd.MM.yyyy");

        private CartesianChart СформироватьГрафик(string заголовок, List<DateTime> даты, List<double> значения, double width, double heigth)
        {
            ChartValues<DateTimePoint> values = new ChartValues<DateTimePoint>();

            for (int i = 0; i < даты.Count; i++)
                values.Add(
                    new DateTimePoint
                    {
                        DateTime = даты[i],
                        Value = значения[i]
                    }
                );

            SeriesCollection series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = заголовок,
                        Stroke = new SolidColorBrush(Color.FromArgb(255, (byte)73, (byte)140, (byte)81)),
                        PointGeometrySize = 10,
                        PointForeground = new SolidColorBrush(Color.FromArgb(255, (byte)73, (byte)140, (byte)81)),
                        Values = values,
                        Foreground = Brushes.Black,
                        //LabelPoint = point => point.X + "\n" + point.SeriesView.Title + " " + point.Y,
                        DataLabels = false
                    }
                };

            AxesCollection axisX = new AxesCollection
            {
                new Axis
                {
                    Foreground = Brushes.Black,
                    LabelFormatter = Formatter,
                    Separator = new LiveCharts.Wpf.Separator()
                    {
                        Step = даты.Count < 5 ? DateTime.MinValue.AddDays(1).Ticks : даты.Count / 5 * DateTime.MinValue.AddDays(1).Ticks
                    },
                    MinValue = даты.Min().Ticks,
                    MaxValue = даты.Max().Ticks
                }
            };

            AxesCollection axisY = new AxesCollection
            {
                new Axis
                {
                    Foreground = Brushes.Black,
                    Separator = new LiveCharts.Wpf.Separator()
                    {
                        Step = значения.Max() / 10 < 1 ? 1 : Convert.ToInt32(значения.Max() / 10)
                    },
                    MinValue = 0,
                    MaxValue = значения.Max() + 10 - значения.Max() % 10
                }
            };

            CartesianChart chart = new CartesianChart
            {
                Background = new SolidColorBrush(Color.FromArgb(255, (byte)118, (byte)227, (byte)131)),
                DisableAnimations = true,
                Zoom = ZoomingOptions.Xy,
                Width = width,
                Height = heigth,
                Series = series,
                AxisX = axisX,
                AxisY = axisY,
            };

            return chart;
        }

        private bool SaveToPDF(int report, int variant, string fileName)
        {
            DockPanel table = new();
            CartesianChart grafic = new();

            switch (report)
            {
                case 1:
                    table = currentTable;
                    grafic = currentGrafic;
                    break;
                case 2:
                    table = currentTable1;
                    grafic = currentGrafic1;
                    break;
                case 3:
                    table = currentTable2;
                    grafic = currentGrafic2;
                    break;
                default:
                    return false;
            }

            try
            {
                using (Aspose.Pdf.Document doc = new Aspose.Pdf.Document())
                {
                    Aspose.Pdf.Page page = doc.Pages.Add();


                    if (variant == 1 || variant == 3)
                    {
                        if (!SaveToPng(table, fileName + @"-таблица.png"))
                            return false;
                        
                        
                        Aspose.Pdf.Image картинка = new Aspose.Pdf.Image();
                        картинка.File = (fileName + @"-таблица.png");

                        //картинка.FixHeight = высотаКартинки / 10;
                        //картинка.FixWidth = ширинаКартинки / 10;

                        page.Paragraphs.Add(картинка);
                    }
                    if (variant == 2 || variant == 3)
                    {
                        if (!SaveToPng(grafic, fileName + @"-график.png"))
                            return false;

                        Aspose.Pdf.Image картинка = new Aspose.Pdf.Image();
                        картинка.File = (fileName + @"-график.png");

                        //картинка.FixHeight = высотаКартинки / 10;
                        //картинка.FixWidth = ширинаКартинки / 10;

                        page.Paragraphs.Add(картинка);
                    }

                    doc.Save(fileName + @".pdf");
                }

                if (variant == 1 || variant == 3)
                {
                    FileInfo fileInfo = new FileInfo(fileName + @"-таблица.png");
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
                if (variant == 2 || variant == 3)
                {
                    FileInfo fileInfo = new FileInfo(fileName + @"-график.png");
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private bool SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();

            try
            {
                var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(visual);
                var frame = BitmapFrame.Create(bitmap);
                encoder.Frames.Add(frame);
                using (var stream = File.Create(fileName)) encoder.Save(stream);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
