using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KosmicznaPodroz
{
    /// <summary>
    /// Klasa przechowująca dane celu którym oznaczamy planetę na którą chcemy polecieć
    /// </summary>
    public class Cel
    {
        public Image Obrazek { get; private set; }
        public Planeta OznaczonaPlaneta { get; private set; }

        private EventHandler DelegatAktualizuj;
        private EventHandler DelegatUsun;

        public Cel(Image obrazek)
        {
            Obrazek = obrazek;
            DelegatAktualizuj = (s, args) => Aktualizuj();
            DelegatUsun = (s, args) => Odznacz();
        }

        public void Aktualizuj()
        {
            Canvas.SetLeft(Obrazek, OznaczonaPlaneta.Pozycja.X + OznaczonaPlaneta.Obrazek.Width / 2 - Obrazek.Width / 2);
            Canvas.SetTop(Obrazek, OznaczonaPlaneta.Pozycja.Y + OznaczonaPlaneta.Obrazek.Height / 2 - Obrazek.Height / 2);
        }

        public void Zaznacz(Planeta planeta)
        {
            OznaczonaPlaneta = planeta;
            OznaczonaPlaneta.PoruszenieEvent += DelegatAktualizuj;
            OznaczonaPlaneta.UsowanieEvent += DelegatUsun;
            Obrazek.Visibility = System.Windows.Visibility.Visible;
            Aktualizuj();
        }

        public void Odznacz()
        {
            OznaczonaPlaneta.PoruszenieEvent -= DelegatAktualizuj;
            OznaczonaPlaneta.UsowanieEvent -= DelegatUsun;
            OznaczonaPlaneta = null;
            Obrazek.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
