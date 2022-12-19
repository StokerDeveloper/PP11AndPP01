using System.Windows;
using Точка_проката_ЦПКиО_им._Маяковского.Models;

namespace Точка_проката_ЦПКиО_им._Маяковского
{
    public partial class ОкноКапчи : Window
    {
        private Капча капча;

        public ОкноКапчи()
        {
            капча = new Капча();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            капчаКартинка.Source = капча.ПолучитьКапчу();
        }

        //Генерация новой капчи
        private void ReloadCaptcha_Click(object sender, RoutedEventArgs e)
        {
            капчаКартинка.Source = капча.ПолучитьКапчу();
        }

        //Проверка введеной капчи
        private void VerifCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (капчаВвод.Text != "")
            {
                if (капча.ПроверитьКапчу(капчаВвод.Text))
                    this.DialogResult = true;
                else
                    this.DialogResult = false;
            }
            else
                MessageBox.Show("Нужно ввести капчу", "Предупреждение");
        }
    }
}
