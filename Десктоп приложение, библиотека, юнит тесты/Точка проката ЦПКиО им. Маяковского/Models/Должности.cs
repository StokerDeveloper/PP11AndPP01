using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class Должности
    {
        public Должности()
        {
            Сотрудникиs = new HashSet<Сотрудники>();
        }

        public int Код { get; set; }
        public string Наименование { get; set; } = null!;

        public virtual ICollection<Сотрудники> Сотрудникиs { get; set; }
    }
}
