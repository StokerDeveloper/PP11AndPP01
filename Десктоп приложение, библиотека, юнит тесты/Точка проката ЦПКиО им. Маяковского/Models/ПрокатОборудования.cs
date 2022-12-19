using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ПрокатОборудования
    {
        public int Код { get; set; }
        public int Заказ { get; set; }
        public int Оборудование { get; set; }
        public int? Статус { get; set; }

        public virtual Заказы ЗаказNavigation { get; set; } = null!;
        public virtual Оборудование ОборудованиеNavigation { get; set; } = null!;
        public virtual СтатусыПроката? СтатусNavigation { get; set; }
    }
}
