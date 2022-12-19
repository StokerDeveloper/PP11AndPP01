using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ТипыОборудования
    {
        public ТипыОборудования()
        {
            Оборудованиеs = new HashSet<Оборудование>();
        }

        public int Код { get; set; }
        public string Наименование { get; set; } = null!;

        public virtual ICollection<Оборудование> Оборудованиеs { get; set; }
    }
}
