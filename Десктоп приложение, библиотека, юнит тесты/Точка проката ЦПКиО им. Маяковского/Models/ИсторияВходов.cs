using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ИсторияВходов
    {
        public int Код { get; set; }
        public int Пользователь { get; set; }
        public DateTime Дата { get; set; }
        public TimeSpan Время { get; set; }
        public int Тип { get; set; }

        public virtual Пользователи ПользовательNavigation { get; set; } = null!;
        public virtual ТипыВходов ТипNavigation { get; set; } = null!;
    }
}
