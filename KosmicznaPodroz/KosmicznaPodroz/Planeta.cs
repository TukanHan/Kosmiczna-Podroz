using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KosmicznaPodroz
{
    public class Planeta : IPrzesowalnyObiekt
    {
        public List<Polaczenie> Polaczenia { get; private set; } = new List<Polaczenie>();
        public Image Obrazek { get; private set; }

        public event EventHandler PoruszenieEvent;
        public event EventHandler UsowanieEvent;

        public Planeta()
        {
            Obrazek = MainWindow.mainWindow.stronaSymulacji.StworzObrazekPlanety(this);
        }

        public void Aktualizuj()
        {
            if (PoruszenieEvent != null)
                PoruszenieEvent(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (UsowanieEvent != null)
                UsowanieEvent(this, EventArgs.Empty);

            (Obrazek.Parent as Canvas).Children.Remove(Obrazek);
        }
    }
}
