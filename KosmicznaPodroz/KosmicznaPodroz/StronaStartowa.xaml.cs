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
    /// Strona startowa
    /// </summary>
    public partial class StronaStartowa : UserControl
    {
        public StronaStartowa()
        {
            InitializeComponent();
        }

        private void przyciskNowaSymulacja_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.OtworzOkno(MainWindow.mainWindow.stronaSymulacji, this);
        }

        private void przyciskPomoc_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.OtworzOkno(MainWindow.mainWindow.stronaPomoc, this);
        }

        private void przyciskOProgramie_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.OtworzOkno(MainWindow.mainWindow.stronaOProgramie, this);
        }

        private void przyciskZamknij_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
