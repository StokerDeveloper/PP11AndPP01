using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class Оборудование
    {
        public Оборудование()
        {
            ПрокатОборудованияs = new HashSet<ПрокатОборудования>();
        }

        public int Код { get; set; }
        public string КодОборудования { get; set; } = null!;
        public int Тип { get; set; }

        public virtual ТипыОборудования ТипNavigation { get; set; } = null!;
        public virtual ICollection<ПрокатОборудования> ПрокатОборудованияs { get; set; }
    }
}
