using System;
using System.Collections.Generic;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class Пользователи
    {
        public Пользователи()
        {
            ДвиженияЗаказовs = new HashSet<ДвиженияЗаказов>();
            ИсторияВходовs = new HashSet<ИсторияВходов>();
        }

        public int Код { get; set; }
        public int Сотрудник { get; set; }
        public string Логин { get; set; } = null!;
        public string Пароль { get; set; } = null!;
        public byte[] Фото { get; set; } = null!;

        public virtual Сотрудники СотрудникNavigation { get; set; } = null!;
        public virtual ICollection<ДвиженияЗаказов> ДвиженияЗаказовs { get; set; }
        public virtual ICollection<ИсторияВходов> ИсторияВходовs { get; set; }
    }
}
