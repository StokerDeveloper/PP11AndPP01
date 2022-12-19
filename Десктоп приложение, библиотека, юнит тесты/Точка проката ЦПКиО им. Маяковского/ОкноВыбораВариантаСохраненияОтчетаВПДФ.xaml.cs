using System.Windows;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноВыбораВариантаСохраненияОтчетаВПДФ : Window
    {
        public int value = 1;

        public ОкноВыбораВариантаСохраненияОтчетаВПДФ(string title)
        {
            this.Title = title;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (вариант1.IsChecked == true)
            {
                value = 1;
            }
            else if (вариант2.IsChecked == true)
            {
                value = 2;
            }
            else if (вариант3.IsChecked == true)
            {
                value = 3;
            }

            this.DialogResult = true;
        }
    }
}
