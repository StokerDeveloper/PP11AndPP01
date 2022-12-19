using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class Клиенты
    {
        public Клиенты()
        {
            Заказыs = new HashSet<Заказы>();
        }

        public int Код { get; set; }
        public string Фамилия { get; set; } = null!;
        public string Имя { get; set; } = null!;
        public string Отчество { get; set; } = null!;
        public string СерияПаспорта { get; set; } = null!;
        public string НомерПаспорта { get; set; } = null!;
        public DateTime ДатаРождения { get; set; }
        public string Адрес { get; set; } = null!;
        public string ЭлектроннаяПочта { get; set; } = null!;

        public virtual ICollection<Заказы> Заказыs { get; set; }
    }
}
