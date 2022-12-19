using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ТипыУслуг
    {
        public ТипыУслуг()
        {
            Услугиs = new HashSet<Услуги>();
        }

        public int Код { get; set; }
        public string Наименование { get; set; } = null!;

        public virtual ICollection<Услуги> Услугиs { get; set; }
    }
}
