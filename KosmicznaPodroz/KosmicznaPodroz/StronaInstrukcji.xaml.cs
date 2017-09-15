using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KosmicznaPodroz
{
    /// <summary>
    /// Interaction logic for StronaInstrukcji.xaml
    /// </summary>
    public partial class StronaInstrukcji : UserControl
    {
        public StronaInstrukcji()
        {
            InitializeComponent();
        }

        private void przyciskCofnij_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.OtworzOkno(MainWindow.mainWindow.stronaStartowa, this);
        }
    }
}
