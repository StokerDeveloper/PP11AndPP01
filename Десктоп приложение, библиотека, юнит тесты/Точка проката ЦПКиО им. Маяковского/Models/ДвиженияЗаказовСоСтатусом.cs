using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Точка_проката_ЦПКиО_им._Маяковского.Models
{
    public partial class ДвиженияЗаказовСоСтатусом
    {
        public int Код { get; set; }
        public DateTime Дата { get; set; }
        public TimeSpan Время { get; set; }
        public string Статус { get; set; } = null!;
    }
}
