using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Точка_проката_ЦПКиО_им._Маяковского.Models
{
    public partial class ИсторияВходовСПользователемИТипом
    {
        public int Код { get; set; }
        public string Логин { get; set; } = null!;
        public DateTime Дата { get; set; }
        public TimeSpan Время { get; set; }
        public string ТипВхода { get; set; } = null!;
    }
}
