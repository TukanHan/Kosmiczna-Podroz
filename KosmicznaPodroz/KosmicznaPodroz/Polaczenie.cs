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

    /// <summary>
    /// Klasa zawierająca informacje o pojedyńczej trasie między planetarnej
    /// </summary>
    public abstract class Polaczenie : IDisposable
    {
        public Grid Obrazek { get; set; }
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
            Waga = Obrazek.Height = obliczanie.ObliczOdlegloscPomiedzy();

            Canvas.SetLeft(Obrazek, obliczanie.ObliczPozycjePomiedzy().X + (PlanetaStartowa.Obrazek.Width/2 - Obrazek.Width/2) );
            Canvas.SetTop(Obrazek, obliczanie.ObliczPozycjePomiedzy().Y + (PlanetaStartowa.Obrazek.Height / 2 - Obrazek.Height / 2));

            Obrazek.RenderTransform = new RotateTransform(obliczanie.ObliczKatPomiedzy());
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

            (Obrazek.Parent as Canvas).Children.Remove(Obrazek);
        }

        public Planeta ZwrocPrzeciwnaPlanete(Planeta planeta)
        {
            if (planeta == PlanetaStartowa)
                return PlanetaKoncowa;
            else
                return PlanetaStartowa;
        }

        protected bool CzyNalerzyUsunac()
        {
            if (PlanetaKoncowa == PlanetaStartowa)
                return true;

            foreach(Polaczenie polaczenie in PlanetaStartowa.Polaczenia)
            {
                if (polaczenie.PlanetaStartowa == PlanetaStartowa && polaczenie.PlanetaKoncowa == PlanetaKoncowa && polaczenie != this)
                    return true;
                else if (polaczenie.PlanetaKoncowa == PlanetaStartowa && polaczenie.PlanetaStartowa == PlanetaKoncowa && polaczenie != this)
                    return true;
            }

            foreach (Polaczenie polaczenie in PlanetaKoncowa.Polaczenia)
            {
                if (polaczenie.PlanetaStartowa == PlanetaStartowa && polaczenie.PlanetaKoncowa == PlanetaKoncowa && polaczenie != this)
                    return true;
                else if (polaczenie.PlanetaKoncowa == PlanetaStartowa && polaczenie.PlanetaStartowa == PlanetaKoncowa && polaczenie != this)
                    return true;
            }

            return false;
        }
    }
}