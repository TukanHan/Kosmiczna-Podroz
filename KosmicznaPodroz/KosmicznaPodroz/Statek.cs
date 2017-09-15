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
                    new Punkt(Canvas.GetLeft(trasaPlanetarna[0].Obrazek) + trasaPlanetarna[0].Obrazek.Width/2, Canvas.GetTop(trasaPlanetarna[0].Obrazek )+ trasaPlanetarna[0].Obrazek.Height / 2),
                    new Punkt(Canvas.GetLeft(Obrazek) + Obrazek.Width/2, Canvas.GetTop(Obrazek) + Obrazek.Height /2));

                if(geometriaStatekCel.ObliczOdlegloscPomiedzy() >= 15)
                {
                    Geometria geometriaStartCel = new Geometria(
                        new Punkt(Canvas.GetLeft(PlanetaDokowania.Obrazek), Canvas.GetTop(PlanetaDokowania.Obrazek)),
                        new Punkt(Canvas.GetLeft(trasaPlanetarna[0].Obrazek), Canvas.GetTop(trasaPlanetarna[0].Obrazek)));

                    Canvas.SetLeft(Obrazek, Canvas.GetLeft(Obrazek) + geometriaStartCel.ObliczWektorPrzesuniecia(3).X);
                    Canvas.SetTop(Obrazek, Canvas.GetTop(Obrazek) + geometriaStartCel.ObliczWektorPrzesuniecia(3).Y);

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