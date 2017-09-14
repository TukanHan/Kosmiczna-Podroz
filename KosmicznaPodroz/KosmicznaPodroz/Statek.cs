using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KosmicznaPodroz
{
    public class Statek : IPrzesowalnyObiekt
    {
        public Planeta PlanetaDokowania { get; private set; }
        public Image Obrazek { get; }

        private EventHandler delegatDoUsunieciaObiektu;
        private EventHandler delegatDoPoruszaniaObiektu;

        public Statek()
        {
            Obrazek = MainWindow.mainWindow.stronaSymulacji.StworzObrazekStatku(this);
            delegatDoUsunieciaObiektu = (s, args) => Dispose();
            delegatDoPoruszaniaObiektu = (s, args) => UstawPozycje();
        }

        public void Aktualizuj()
        {
            //nic - wymuszenie zgodności
        }

        public void UstawPozycje()
        {
            Canvas.SetLeft(Obrazek, Canvas.GetLeft(PlanetaDokowania.Obrazek) + PlanetaDokowania.Obrazek.Width / 2 - Obrazek.Width / 2);
            Canvas.SetTop(Obrazek, Canvas.GetTop(PlanetaDokowania.Obrazek) + PlanetaDokowania.Obrazek.Height / 2 - Obrazek.Height / 2);
        }

        public void UstawPlanete(Planeta planeta)
        {
            if (PlanetaDokowania != null)
            {
                PlanetaDokowania.PoruszenieEvent -= delegatDoPoruszaniaObiektu;
                PlanetaDokowania.UsowanieEvent -= delegatDoUsunieciaObiektu;
            }

            PlanetaDokowania = planeta;
            PlanetaDokowania.PoruszenieEvent += delegatDoPoruszaniaObiektu;
            PlanetaDokowania.UsowanieEvent += delegatDoUsunieciaObiektu;
            UstawPozycje();
        }

        public void Dispose()
        {
            if (PlanetaDokowania != null)
            {
                PlanetaDokowania.PoruszenieEvent -= delegatDoPoruszaniaObiektu;
                PlanetaDokowania.UsowanieEvent -= delegatDoUsunieciaObiektu;
            }

            (Obrazek.Parent as Canvas).Children.Remove(Obrazek);
            MainWindow.mainWindow.stronaSymulacji.Statek = null;
        }
    }
}