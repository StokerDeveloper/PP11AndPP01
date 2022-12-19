using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class СтатусыПроката
    {
        public СтатусыПроката()
        {
            ПрокатОборудованияs = new HashSet<ПрокатОборудования>();
        }

        public int Код { get; set; }
        public string Наименование { get; set; } = null!;

        public virtual ICollection<ПрокатОборудования> ПрокатОборудованияs { get; set; }
    }
}
