using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class Услуги
    {
        public Услуги()
        {
            ПредоставлениеУслугs = new HashSet<ПредоставлениеУслуг>();
        }

        public int Код { get; set; }
        public string КодУслуги { get; set; } = null!;
        public string Наименование { get; set; } = null!;
        public int СтоимостьРублейЗаЧас { get; set; }
        public int Тип { get; set; }

        public virtual ТипыУслуг ТипNavigation { get; set; } = null!;
        public virtual ICollection<ПредоставлениеУслуг> ПредоставлениеУслугs { get; set; }
    }
}
