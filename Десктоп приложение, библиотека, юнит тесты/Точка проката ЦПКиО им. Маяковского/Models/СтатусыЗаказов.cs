using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class СтатусыЗаказов
    {
        public СтатусыЗаказов()
        {
            ДвиженияЗаказовs = new HashSet<ДвиженияЗаказов>();
        }

        public int Код { get; set; }
        public string Наименование { get; set; } = null!;

        public virtual ICollection<ДвиженияЗаказов> ДвиженияЗаказовs { get; set; }
    }
}
