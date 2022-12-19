using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class Сотрудники
    {
        public Сотрудники()
        {
            Пользователиs = new HashSet<Пользователи>();
        }

        public int Код { get; set; }
        public string Фамилия { get; set; } = null!;
        public string Имя { get; set; } = null!;
        public string Отчество { get; set; } = null!;
        public int Должность { get; set; }

        public virtual Должности ДолжностьNavigation { get; set; } = null!;
        public virtual ICollection<Пользователи> Пользователиs { get; set; }
    }
}
