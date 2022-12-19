using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ПредоставлениеУслуг
    {
        public int Код { get; set; }
        public int Заказ { get; set; }
        public int Услуга { get; set; }

        public virtual Заказы ЗаказNavigation { get; set; } = null!;
        public virtual Услуги УслугаNavigation { get; set; } = null!;
    }
}
