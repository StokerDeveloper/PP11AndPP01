using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using System.IO;
using Aspose.Pdf;

namespace Точка_проката_ЦПКиО_им._Маяковского.Models
{
    public class ШтрихКод
    {
        private ImageSource штрихКодКартинка = null!;
        private RenderTargetBitmap штрихКодРендер = null!;
        private int ширинаКартинки = 0;
        private int высотаКартинки = 2593;
        
        List<int> данныеШтрихКода;
        List<int> номерШтрихКода;

        public ImageSource GetImageSource
        {
            get { return штрихКодКартинка; }
        }

        public RenderTargetBitmap GetRender
        {
            get { return штрихКодРендер; }
        }

        public int GetWidth
        {
            get { return ширинаКартинки; }
        }

        public int GetHeigth
        {
            get { return высотаКартинки; }
        }

        private DrawingVisual drawingVisual;
        private DrawingContext drawingContext;
        private Rect штрих;

        public ШтрихКод(List<int> данныеШтрихКода, List<int> номерШтрихКода)
        {
            this.данныеШтрихКода = данныеШтрихКода;
            this.номерШтрихКода = номерШтрихКода;

            drawingVisual = new DrawingVisual();
            drawingContext = drawingVisual.RenderOpen();
            СоздатьШтрихКод();
        }

        public ШтрихКод(List<int> данныеШтрихКода)
        {
            this.данныеШтрихКода = данныеШтрихКода;
            this.номерШтрихКода = new List<int>() {4, 8, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            drawingVisual = new DrawingVisual();
            drawingContext = drawingVisual.RenderOpen();
            СоздатьШтрихКод();
        }

        public ШтрихКод()
        {
            this.данныеШтрихКода = new List<int>();
            this.номерШтрихКода = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                if (i < 13)
                    номерШтрихКода.Add((new Random()).Next(0, 10));
                данныеШтрихКода.Add((new Random()).Next(0, 10));
            }

            drawingVisual = new DrawingVisual();
            drawingContext = drawingVisual.RenderOpen();
            СоздатьШтрихКод();
        }

        public bool СохранитьКакПДФ(string fileName)
        {
            try
            {
                using (var fileStream = new FileStream(fileName + @".png", FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(штрихКодРендер));
                    encoder.Save(fileStream);
                }

                using (Document doc = new Document())
                {
                    Aspose.Pdf.Page page = doc.Pages.Add();
                    Aspose.Pdf.Image картинка = new Aspose.Pdf.Image();
                    картинка.File = (fileName + @".png");

                    картинка.FixHeight = высотаКартинки / 10;
                    картинка.FixWidth = ширинаКартинки / 10;
                    page.Paragraphs.Add(картинка);

                    doc.Save(fileName + @".pdf");
                }

                FileInfo fileInfo = new FileInfo(fileName + @".png");
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void СоздатьШтрихКод()
        {
            #region Переменные
            int _x; //Переменная пустышка для отрисовки фона
            int x = 0;
            int y = 0;
            int x1 = 423;
            int y1 = 2300;
            int ширинаКартинки = РасчитатьШиринуКартинки(данныеШтрихКода);
            int центрКартинки = РасчитатьЦентрКартинки(данныеШтрихКода); //Примерный центр картинки (а точнее координата правого края центрального ограничивающего штриха)
            int ширинаЛевойОбласти = ширинаКартинки - (ширинаКартинки - центрКартинки) - 363 - 60 * 2; //Ширина левой области между ограничивающими штрихами
            int ширинаПравойОбласти = ширинаКартинки - центрКартинки - 231; //Ширина правой области между ограничивающими штрихами
            int размерТекста = (ширинаПравойОбласти / 6) > 275 ? 275 : ширинаПравойОбласти / 6;
            RenderTargetBitmap bmp;
            #endregion Переменные

            #region Отрисовка штрихов
            НарисоватьПрямоугольник(Brushes.White, 0, 0, ширинаКартинки, 2593, out _x); //Фон картинки заполняется белым цветом

            x += 363; //Левая пустая область

            НарисоватьОграничивающийШтрих(x, y, out x); //Левый ограничивающий штрих

            x += 20; //Все увеличения х на 20 означают пустую область между штрихами

            for (int i = 0; i < данныеШтрихКода.Count; i++)
            {
                if (данныеШтрихКода[i] != 0)
                    НарисоватьПрямоугольник(Brushes.Black, x, y, данныеШтрихКода[i] * 15, 2285, out x); //Если цифра не 0, то рисуется черный штрих
                else
                    x += 135; //Если цифра 0, то пустая область

                if (i == данныеШтрихКода.Count / 2)
                {
                    x += 20;
                    НарисоватьОграничивающийШтрих(x, y, out x); // Центральный ограничивающий штрих
                    центрКартинки = x;                          // и фиксация его координаты х
                }

                x += 20;
            }

            НарисоватьОграничивающийШтрих(x, y, out x); //Правый ограничивающий штрих

            x += 231; //Правая пустая область
            #endregion Отрисовка Штрихов

            #region Отрисовка нижнего номера
            НарисоватьТекст(номерШтрихКода[0].ToString(), размерТекста, Brushes.Black, drawingContext, x1 - размерТекста / 2 - 60, y1); //Левая внешняя цифра

            for (int i = 1; i < 6; i++)
            {
                НарисоватьТекст(номерШтрихКода[i].ToString(), размерТекста, Brushes.Black, drawingContext, x1, y1);
                x1 += (ширинаЛевойОбласти - размерТекста * 2) / 4;
            }

            НарисоватьТекст(номерШтрихКода[6].ToString(), размерТекста, Brushes.Black, drawingContext, центрКартинки - 60 - размерТекста / 2, y1); //Правая цифра левой области

            x1 = центрКартинки;
            for (int i = 7; i < 12; i++)
            {
                НарисоватьТекст(номерШтрихКода[i].ToString(), размерТекста, Brushes.Black, drawingContext, x1, y1);
                x1 += (ширинаПравойОбласти - размерТекста * 2) / 4;
            }

            НарисоватьТекст(номерШтрихКода[12].ToString(), размерТекста, Brushes.Black, drawingContext, ширинаКартинки - 231 - 60 - размерТекста / 2, y1); //Последняя цифра
            #endregion Отрисовка нижнего номера

            #region Сохранение результата
            drawingContext.Close();

            ширинаКартинки = x;

            bmp = new RenderTargetBitmap(x, 2593, 96, 96, PixelFormats.Default);
            bmp.Render(drawingVisual);
            штрихКодРендер = bmp;

            Image image = new Image();
            image.Source = bmp;
            штрихКодКартинка = image.Source;
            #endregion Сохранение результата
        }

        private int РасчитатьШиринуКартинки(List<int> данныеШтрихКода)
        {
            //Симулируется отрисовка штрихов с последующим возратом ширины
            int result = 363 + 60 + 20;

            for (int i = 0; i < данныеШтрихКода.Count; i++)
            {
                if (данныеШтрихКода[i] != 0)
                    result += данныеШтрихКода[i] * 15;
                else
                    result += 135;

                if (i == данныеШтрихКода.Count / 2)
                {
                    result += 20 + 60;
                }

                result += 20;
            }

            result += 60 + 231;

            return result;
        }

        private int РасчитатьЦентрКартинки(List<int> данныеШтрихКода)
        {
            //Симулируется отрисовка штрихов с возвратом координаты центра
            int result = 363 + 60 + 20;

            for (int i = 0; i < данныеШтрихКода.Count; i++)
            {
                if (данныеШтрихКода[i] != 0)
                    result += данныеШтрихКода[i] * 15;
                else
                    result += 135;

                if (i == данныеШтрихКода.Count / 2)
                {
                    result += 20 + 60;
                    return result;
                }

                result += 20;
            }

            result += 60 + 231;

            return result;
        }

        private void НарисоватьОграничивающийШтрих(int x, int y, out int result)
        {
            result = x;
            НарисоватьПрямоугольник(Brushes.Black, result, y, 20, 2450, out result);
            НарисоватьПрямоугольник(Brushes.White, result, y, 20, 2450, out result);
            НарисоватьПрямоугольник(Brushes.Black, result, y, 20, 2450, out result);
        }

        private void НарисоватьПрямоугольник(Brush brush, int x, int y, int ширинаПрямоугольника, int высотаПрямоугольника, out int result)
        {
            штрих = new Rect(x, y, ширинаПрямоугольника, высотаПрямоугольника);
            drawingContext.DrawRectangle(brush, null, штрих);
            result = x + ширинаПрямоугольника;
        }

        private void НарисоватьТекст(string text, int размерТекста, Brush brush, DrawingContext drawingContext, int x, int y)
        {
            FormattedText formattedText = new FormattedText(
                    text,
                    new CultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(
                        new FontFamily("OCR B"),
                        new FontStyle(),
                        FontWeight.FromOpenTypeWeight(8),
                        new FontStretch()
                    ),
                    размерТекста,
                    brush,
                    1
                );

            drawingContext.DrawText(formattedText, new System.Windows.Point(x, y));
        }
    }
}
