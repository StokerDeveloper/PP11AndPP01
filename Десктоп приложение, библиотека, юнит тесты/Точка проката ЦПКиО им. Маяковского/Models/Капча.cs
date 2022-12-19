using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Globalization;
using Image = System.Windows.Controls.Image;
using System.Reflection;

namespace Точка_проката_ЦПКиО_им._Маяковского.Models
{
    public class Капча
    {

        private List<char> символы = new List<char>();
        private string текстКапчи = "";
        private int количествоСимволов;
        private int ширинаКартинки;
        private int высотаКартинки = 120;

        public Капча()
        {
            количествоСимволов = (new Random()).Next(3, 7);
            ширинаКартинки = 64 * количествоСимволов;

            for (int i = 65; i < 91; i++)
            {
                символы.Add((char)i);
            }

            for (int i = 97; i < 123; i++)
            {
                символы.Add((char)i);
            }

            for (int i = 48; i < 58; i++)
            {
                символы.Add((char)i);
            }
        }

        public ImageSource ПолучитьКапчу()
        {
            ширинаКартинки = 64 * количествоСимволов;
            высотаКартинки = 120;

            текстКапчи = "";
            for (int i = 0; i < количествоСимволов; i++)
            {
                текстКапчи += символы[(new Random()).Next(0, символы.Count)];
            }

            Image картинка = new Image();
            картинка.Stretch = Stretch.Fill;
            картинка.Width = ширинаКартинки;
            картинка.Height = высотаКартинки;

            RenderTargetBitmap bmp = СгенерироватьКапчу();
            картинка.Source = bmp;

            return картинка.Source;
        }

        private RenderTargetBitmap СгенерироватьКапчу()
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            int x = 5;
            int y = (int)Math.Ceiling(Convert.ToDouble(высотаКартинки) / 15);

            drawingContext = СгенерироватьФон(drawingVisual);

            for (int i = 0; i < количествоСимволов; i++)
            {
                PropertyInfo[] properties = typeof(Brushes).GetProperties();
                Brush кисть = (Brush)properties[(new Random()).Next(properties.Length)].GetValue(null, null);

                FormattedText text1 = new FormattedText(
                    текстКапчи[i].ToString().ToLower(),
                    new CultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(
                        new FontFamily("Comic Sans MS"),
                        new FontStyle(),
                        FontWeight.FromOpenTypeWeight(8),
                        new FontStretch()
                    ),
                    90,
                    кисть,
                    1
                );

                drawingContext.DrawText(
                    text1,
                    new Point(
                        (new Random()).Next(x - 15, x + 15),
                        (new Random()).Next(y - 25, y + 25)
                    )
                );

                x += 45;
            }

            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(ширинаКартинки, высотаКартинки, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }

        private DrawingContext СгенерироватьФон(DrawingVisual drawingVisual)
        {
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            for (int i = 0; i < ширинаКартинки; i += 5)
            {
                for (int j = 0; j < высотаКартинки; j += 5)
                {
                    Rect rect1 = new Rect(
                        i,
                        j,
                        (new Random()).Next(4, 10),
                        (new Random()).Next(4, 10)
                    );

                    PropertyInfo[] properties = typeof(Brushes).GetProperties();
                    Brush кисть = (Brush)properties[(new Random()).Next(properties.Length)].GetValue(null, null);

                    drawingContext.DrawRectangle(кисть, null, rect1);
                }
            }

            return drawingContext;
        }

        public bool ПроверитьКапчу(string текстКапчи)
        {
            return this.текстКапчи.ToUpper() == текстКапчи.ToUpper();
        }
    }
}
