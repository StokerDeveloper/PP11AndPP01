using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using LiveCharts.Wpf;

namespace Точка_проката_ЦПКиО_им._Маяковского.Pages
{
    public class График
    {
        public Func<double, string> Formatter { get; set; }

        public CartesianChart СформироватьГрафик(string заголовок, List<DateTime> даты, List<double> значения, double width, double heigth)
        {
            Formatter = value => new DateTime((long)value).ToString("dd.MM.yyyy");

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
                        Step = даты.Count / 10 < 1 ? DateTime.MinValue.AddDays(1).Ticks : Convert.ToInt32(даты.Count / 10) * DateTime.MinValue.AddDays(1).Ticks
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
    }
}
