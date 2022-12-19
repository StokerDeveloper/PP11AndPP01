using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class Заказы
    {
        public Заказы()
        {
            ДвиженияЗаказовs = new HashSet<ДвиженияЗаказов>();
            ПредоставлениеУслугs = new HashSet<ПредоставлениеУслуг>();
            ПрокатОборудованияs = new HashSet<ПрокатОборудования>();
        }

        public int Код { get; set; }
        public string КодЗаказа { get; set; } = null!;
        public int Клиент { get; set; }
        public int ВремяПрокатаЧасов { get; set; }

        public virtual Клиенты КлиентNavigation { get; set; } = null!;
        public virtual ICollection<ДвиженияЗаказов> ДвиженияЗаказовs { get; set; }
        public virtual ICollection<ПредоставлениеУслуг> ПредоставлениеУслугs { get; set; }
        public virtual ICollection<ПрокатОборудования> ПрокатОборудованияs { get; set; }
    }
}
