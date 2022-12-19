using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Точка_проката_ЦПКиО_им._Маяковского.Models
{
    public partial class ОборудованиеСоСтатусомИТипом
    {
        public int Код { get; set; }
        public string Номер { get; set; } = null!;
        public string Тип { get; set; } = null!;
        public string Статус { get; set; } = null!;
    }
}
