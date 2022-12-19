using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ДвиженияЗаказов
    {
        public int Код { get; set; }
        public int Заказ { get; set; }
        public int? Пользователь { get; set; }
        public DateTime Дата { get; set; }
        public TimeSpan Время { get; set; }
        public int Статус { get; set; }

        public virtual Заказы ЗаказNavigation { get; set; } = null!;
        public virtual Пользователи? ПользовательNavigation { get; set; }
        public virtual СтатусыЗаказов СтатусNavigation { get; set; } = null!;
    }
}
