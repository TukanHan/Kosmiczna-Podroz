using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KosmicznaPodroz
{
    public class Cel
    {
        public Image Obrazek { get; private set; }
        public Planeta OznaczonaPlaneta { get; private set; }

        private EventHandler DelegatAktualizuj;

        public Cel(Image obrazek)
        {
            Obrazek = obrazek;
            DelegatAktualizuj = (s, args) => Aktualizuj();
        }

        public void Aktualizuj()
        {
            Canvas.SetLeft(Obrazek, Canvas.GetLeft(OznaczonaPlaneta.Obrazek) + OznaczonaPlaneta.Obrazek.Width / 2 - Obrazek.Width / 2);
            Canvas.SetTop(Obrazek, Canvas.GetTop(OznaczonaPlaneta.Obrazek) + OznaczonaPlaneta.Obrazek.Height / 2 - Obrazek.Height / 2);
        }

        public void Zaznacz(Planeta planeta)
        {
            OznaczonaPlaneta = planeta;
            OznaczonaPlaneta.PoruszenieEvent += DelegatAktualizuj;
            Obrazek.Visibility = System.Windows.Visibility.Visible;
            Aktualizuj();          
        }

        public void Odznacz()
        {
            OznaczonaPlaneta.PoruszenieEvent -= DelegatAktualizuj;
            OznaczonaPlaneta = null;
            Obrazek.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
