using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ТипыВходов
    {
        public ТипыВходов()
        {
            ИсторияВходовs = new HashSet<ИсторияВходов>();
        }

        public int Код { get; set; }
        public string Наименование { get; set; } = null!;

        public virtual ICollection<ИсторияВходов> ИсторияВходовs { get; set; }
    }
}
