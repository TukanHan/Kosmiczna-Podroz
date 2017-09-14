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
    /// Główny węzeł programu, przechodzenie między warstwami
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;

        public MainWindow()
        {
            mainWindow = this;
            InitializeComponent();
        }

        public void OtworzOkno(UserControl aktualnaStrona, UserControl poprzedniaStrona)
        {
            aktualnaStrona.Visibility = Visibility.Visible;
            poprzedniaStrona.Visibility = Visibility.Hidden;
        }
    }
}
