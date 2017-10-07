using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace KosmicznaPodroz
{
    /// <summary>
    /// Klasa która zawiera metody związane z przelotem statku
    /// </summary>
    public class Statek : IPrzesowalnyObiekt, IDisposable
    {
        public Planeta PlanetaDokowania { get; private set; }
        public Image Obrazek { get; set; }
        public Punkt<double> Pozycja
        {
            get { return new Punkt<double>(Canvas.GetLeft(Obrazek), Canvas.GetTop(Obrazek)); }
            set { Canvas.SetLeft(Obrazek, value.X); Canvas.SetTop(Obrazek, value.Y); }
        }

        private EventHandler delegatDoUsunieciaObiektu;
        private EventHandler delegatDoPoruszaniaObiektu;

        private DispatcherTimer zdarzeniePoruszania;
        private List<Planeta> trasaPlanetarna;

        public Statek()
        {
            delegatDoUsunieciaObiektu = (s, args) => Dispose();
            delegatDoPoruszaniaObiektu = (s, args) => UstawPozycje();

            zdarzeniePoruszania = new DispatcherTimer();
            zdarzeniePoruszania.Tick += (s, args) => PoruszaniePoMapie();
            zdarzeniePoruszania.Interval = new TimeSpan(0, 0, 0, 0, 5);     
        }

        public void Aktualizuj()
        {
            //nic - wymuszenie zgodności
        }

        public void UstawPozycje()
        {
            Pozycja = new Punkt<double>(PlanetaDokowania.Pozycja.X + PlanetaDokowania.Obrazek.Width / 2 - Obrazek.Width / 2,
                                        PlanetaDokowania.Pozycja.Y + PlanetaDokowania.Obrazek.Height / 2 - Obrazek.Height / 2);
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

        public void UstawKurs(List<Planeta> planety)
        {
            trasaPlanetarna = planety;
            zdarzeniePoruszania.Start();
        }

        public void PoruszaniePoMapie()
        {
            if(trasaPlanetarna.Count>0)
            {
                Geometria geometriaStatekCel = new Geometria(
                    new Punkt<double>(trasaPlanetarna[0].Pozycja.X + trasaPlanetarna[0].Obrazek.Width/2, trasaPlanetarna[0].Pozycja.Y + trasaPlanetarna[0].Obrazek.Height / 2),
                    new Punkt<double>(Pozycja.X + Obrazek.Width/2, Pozycja.Y + Obrazek.Height /2));

                if(geometriaStatekCel.ObliczOdlegloscPomiedzy() >= 15)
                {
                    Geometria geometriaStartCel = new Geometria( PlanetaDokowania.Pozycja, trasaPlanetarna[0].Pozycja);

                    Pozycja = new Punkt<double>(Pozycja.X + geometriaStartCel.ObliczWektorPrzesuniecia(3).X,
                                                Pozycja.Y + geometriaStartCel.ObliczWektorPrzesuniecia(3).Y);

                    Obrazek.RenderTransform = new RotateTransform(geometriaStartCel.ObliczKatPomiedzy());
                }
                else
                {
                    UstawPlanete(trasaPlanetarna[0]);
                    Aktualizuj();
                    trasaPlanetarna.RemoveAt(0);
                }               
            }
            else
            {
                trasaPlanetarna = null;
                zdarzeniePoruszania.Stop();
                MainWindow.mainWindow.stronaSymulacji.ZmienTryb(TrybPoruszania.Normalny);
            }
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