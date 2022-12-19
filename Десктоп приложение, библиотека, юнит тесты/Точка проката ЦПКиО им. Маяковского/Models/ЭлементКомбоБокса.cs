using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Точка_проката_ЦПКиО_им._Маяковского.Models
{
    internal class ЭлементКомбоБокса
    {
        public string текст { get; set; }
        public object значение { get; set; }

        public ЭлементКомбоБокса(string текст, object значение)
        {
            this.текст = текст;
            this.значение = значение;
        }

        public override string ToString()
        {
            return текст;
        }
    }
}
