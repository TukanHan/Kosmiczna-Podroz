using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KosmicznaPodroz
{
    public abstract class Polaczenie : IDisposable
    {
        protected Grid obrazekPolaczenia;
        public Planeta PlanetaStartowa { get; protected set; }
        public Planeta PlanetaKoncowa { get; protected set; }
        public double Waga { get; protected set; }

        protected EventHandler delegatDoUsunieciaObiektu;
        protected EventHandler delegatDoPoruszaniaObiektu;

        public Polaczenie()
        {
            delegatDoUsunieciaObiektu = (s, args) => Dispose();
            delegatDoPoruszaniaObiektu = (s, args) => Aktualizuj();
        }

        public abstract bool DodajPlanete(Planeta planeta);

        public void Aktualizuj()
        {
            Geometria obliczanie = new Geometria
                (
                    new Punkt(Canvas.GetLeft(PlanetaStartowa.Obrazek), Canvas.GetTop(PlanetaStartowa.Obrazek)),
                    new Punkt(Canvas.GetLeft(PlanetaKoncowa.Obrazek), Canvas.GetTop(PlanetaKoncowa.Obrazek))
                );
            Waga = obrazekPolaczenia.Height = obliczanie.ObliczOdlegloscPomiedzy();

            Canvas.SetLeft(obrazekPolaczenia, obliczanie.ObliczPozycjePomiedzy().X + (PlanetaStartowa.Obrazek.Width/2 - obrazekPolaczenia.Width/2) );
            Canvas.SetTop(obrazekPolaczenia, obliczanie.ObliczPozycjePomiedzy().Y + (PlanetaStartowa.Obrazek.Height / 2 - obrazekPolaczenia.Height / 2));

            obrazekPolaczenia.RenderTransform = new RotateTransform(obliczanie.ObliczKatPomiedzy());
        }

        public void Dispose()
        {
            if (PlanetaStartowa != null)
            {
                PlanetaStartowa.PoruszenieEvent -= delegatDoPoruszaniaObiektu;
                PlanetaStartowa.UsowanieEvent -= delegatDoUsunieciaObiektu;
                PlanetaStartowa.Polaczenia.Remove(this);
            }
            if (PlanetaKoncowa != null)
            {
                PlanetaKoncowa.PoruszenieEvent -= delegatDoPoruszaniaObiektu;
                PlanetaKoncowa.UsowanieEvent -= delegatDoUsunieciaObiektu;
                PlanetaKoncowa.Polaczenia.Remove(this);
            }

            (obrazekPolaczenia.Parent as Canvas).Children.Remove(obrazekPolaczenia);
        }

        protected bool CzyNalerzyUsunac()
        {
            if (PlanetaKoncowa == PlanetaStartowa)
                return true;

            if (PlanetaStartowa.Polaczenia.Find(polaczenie => polaczenie.Equals(this) && polaczenie != this) != null)
                return true;

            else if (PlanetaStartowa.Polaczenia.Find(polaczenie => polaczenie.Equals(this) && polaczenie != this) != null)
                return true;

            return false;
        }

        public override bool Equals(object obj)
        {
            Polaczenie drugi = obj as Polaczenie;
            if (drugi == null)
                return false;
            else
            {
                if (drugi.PlanetaStartowa == PlanetaStartowa && drugi.PlanetaKoncowa == PlanetaKoncowa)
                    return true;
                else if (drugi.PlanetaStartowa == PlanetaKoncowa && drugi.PlanetaKoncowa == PlanetaStartowa)
                    return true;
                else
                    return false;
            }
        }
    }
}