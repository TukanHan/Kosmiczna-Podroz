using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KosmicznaPodroz
{
    public enum TrybPoruszania { Normalny, Usowanie, Trasa, Obliczenia};

    /// <summary>
    /// Miejsce gdzie odbywają się wszelkie interakcje z użytkownikiem
    /// </summary>
    public partial class StronaSymulacji : UserControl
    {
        public List<Planeta> Planety { get; } = new List<Planeta>();
        public Statek Statek { get; set; }

        private TrybPoruszania trybPoruszania= TrybPoruszania.Normalny;   
        private Polaczenie nowePolaczenie = null;       
        private IPrzesowalnyObiekt zlapanyObiekt = null;
        private Cel oznaczonaPlaneta;


        public StronaSymulacji()
        {
            InitializeComponent();
            oznaczonaPlaneta = new Cel(obrazekCel);
        }

        #region Interakcja
        private void ZlapObiekt(IPrzesowalnyObiekt obiekt)
        {
            if(zlapanyObiekt == null && trybPoruszania == TrybPoruszania.Normalny)
            {
                Mouse.OverrideCursor = Cursors.Hand;
                Mouse.Capture(obiekt.Obrazek);
                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);

                Canvas.SetZIndex(obiekt.Obrazek, 3);
                obiekt.Pozycja = new Punkt<double>(mousePoint.X - obiekt.Obrazek.Width / 2, mousePoint.Y - obiekt.Obrazek.Height / 2);

                zlapanyObiekt = obiekt;
            }          
        }

        private void PrzesowajObiekt(IPrzesowalnyObiekt obiekt)
        {
            if (zlapanyObiekt == obiekt && trybPoruszania == TrybPoruszania.Normalny)
            {
                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);              
                obiekt.Pozycja = new Punkt<double>(mousePoint.X - obiekt.Obrazek.Width / 2, mousePoint.Y - obiekt.Obrazek.Height / 2);
                obiekt.Aktualizuj();
            }
        }

        private void UsunObiektNaKlikniecie(IDisposable obiekt)
        {
            if (trybPoruszania == TrybPoruszania.Usowanie)
                obiekt.Dispose();
        }

        private void PuscPlanete(Planeta planeta)
        {
            if (zlapanyObiekt == planeta)
            {
                Mouse.Capture(null);
                Mouse.OverrideCursor = Cursors.Arrow;

                Canvas.SetZIndex(planeta.Obrazek, 2);
                zlapanyObiekt = null;

                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);
                double y = mousePoint.Y;
                double x = mousePoint.X;

                if (x >= 670 || y >= 470 || y <= 0 || x <= 0)
                    planeta.Dispose();
            }
        }

        private void PuscStatek(Statek statek)
        {
            if (zlapanyObiekt == statek)
            {
                Mouse.Capture(null);
                Mouse.OverrideCursor = Cursors.Arrow;

                Canvas.SetZIndex(statek.Obrazek, 4);
                zlapanyObiekt = null;

                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);
                double y = mousePoint.Y;
                double x = mousePoint.X;

                if (x >= 670 || y >= 470 || y <= 0 || x <= 0)
                {
                    statek.Dispose();
                }
                else
                {
                    Planeta najblizsza = null;
                    double  dystans = int.MaxValue;

                    foreach(Planeta planeta in Planety)
                    {
                        Geometria geometria = new Geometria
                            (
                                new Punkt<double>(statek.Pozycja.X + statek.Obrazek.Width /2, statek.Pozycja.Y + statek.Obrazek.Height / 2),
                                new Punkt<double>(planeta.Pozycja.X + planeta.Obrazek.Width / 2, planeta.Pozycja.Y + planeta.Obrazek.Height / 2)
                            );

                        if (geometria.ObliczOdlegloscPomiedzy()<dystans && geometria.ObliczOdlegloscPomiedzy()<40)
                        {
                            dystans = geometria.ObliczOdlegloscPomiedzy();
                            najblizsza = planeta;
                        }
                    }

                    if(najblizsza == null)                   
                        statek.Dispose();                   
                    else                    
                        statek.UstawPlanete(najblizsza);               
                }
            }
        }

        private void DodajTraseMiedzyPlanetamiNaKlikniecie(Planeta planeta)
        {
            if (trybPoruszania == TrybPoruszania.Trasa)
            {
                if (nowePolaczenie.DodajPlanete(planeta))                
                    ZmienTryb(TrybPoruszania.Normalny);      
            }
        }

        private void UruchomWyszukiwanie(Planeta planeta)
        {
            if(oznaczonaPlaneta.OznaczonaPlaneta == planeta && trybPoruszania == TrybPoruszania.Normalny)
            {
                ZmienTryb(TrybPoruszania.Obliczenia);
                AlgorytmDijkstry przeszukiwacz = new AlgorytmDijkstry(Planety);
                Statek.UstawKurs(przeszukiwacz.ZwrocScierzke(Statek.PlanetaDokowania, planeta));
            }
        }        
        #endregion

        #region Przyciski
        private void przyciskTworzPlanete_Click(object sender, RoutedEventArgs e)
        {
            if(trybPoruszania!= TrybPoruszania.Obliczenia)
            {
                ZmienTryb(TrybPoruszania.Normalny);

                Planeta planeta = new Planeta();
                planeta.Obrazek = StworzObrazekPlanety();

                planeta.Obrazek.MouseDown += (s, args) => UruchomWyszukiwanie(planeta);
                planeta.Obrazek.MouseUp += (s, args) => PuscPlanete(planeta);
                planeta.Obrazek.MouseDown += (s, args) => ZlapObiekt(planeta);
                planeta.Obrazek.MouseMove += (s, args) => PrzesowajObiekt(planeta);               
                planeta.Obrazek.MouseDown += (s, args) => DodajTraseMiedzyPlanetamiNaKlikniecie(planeta);
                planeta.Obrazek.MouseUp += (s, args) => UsunObiektNaKlikniecie(planeta);

                //oznaczanie na cel
                planeta.Obrazek.MouseEnter += (s, args) =>
                {
                    if (Statek != null && Statek.PlanetaDokowania != planeta && trybPoruszania == TrybPoruszania.Normalny && zlapanyObiekt == null)
                        oznaczonaPlaneta.Zaznacz(planeta);
                };
                planeta.Obrazek.MouseLeave += (s, args) =>
                {
                    if (oznaczonaPlaneta.OznaczonaPlaneta == planeta)
                        oznaczonaPlaneta.Odznacz();
                };

                ZlapObiekt(planeta);
            }         
        }

        private void przyciskTworzPolaczenieJednokierunkowe_Click(object sender, RoutedEventArgs e)
        {
            if (trybPoruszania != TrybPoruszania.Obliczenia)
            {
                ZmienTryb(TrybPoruszania.Trasa);

                if (trybPoruszania == TrybPoruszania.Trasa)
                {
                    Polaczenie polaczenie = nowePolaczenie = new PolaczenieJednostronne();
                    polaczenie.Obrazek = StworzObrazekPolaczenia("Obrazki/Inne/TrasaPojedyncza.png");

                    polaczenie.Obrazek.MouseDown += (s, args) => UsunObiektNaKlikniecie(polaczenie);
                }
            }   
        }

        private void przyciskTworzPolaczenieDwukierunkowe_Click(object sender, RoutedEventArgs e)
        {
            if (trybPoruszania != TrybPoruszania.Obliczenia)
            {
                ZmienTryb(TrybPoruszania.Trasa);

                if (trybPoruszania == TrybPoruszania.Trasa)
                {
                    Polaczenie polaczenie = nowePolaczenie = new PolaczenieDwustronne();
                    polaczenie.Obrazek = StworzObrazekPolaczenia("Obrazki/Inne/TrasaPodwojna.png");

                    polaczenie.Obrazek.MouseDown += (s, args) => UsunObiektNaKlikniecie(polaczenie);
                }
            }
        }

        private void przyciskTworzStatek_Click(object sender, RoutedEventArgs e)
        {
            if (trybPoruszania != TrybPoruszania.Obliczenia)
            {
                ZmienTryb(TrybPoruszania.Normalny);
                if (Statek == null)
                {
                    Statek statekKosmiczny = Statek = new Statek();
                    Statek.Obrazek = StworzObrazekStatku();

                    Statek.Obrazek.MouseDown += (s, args) => ZlapObiekt(statekKosmiczny);
                    Statek.Obrazek.MouseMove += (s, args) => PrzesowajObiekt(statekKosmiczny);
                    Statek.Obrazek.MouseUp += (s, args) => UsunObiektNaKlikniecie(statekKosmiczny);
                    Statek.Obrazek.MouseUp += (s, args) => PuscStatek(statekKosmiczny);

                    ZlapObiekt(Statek);
                }
            }
        }

        private void przyciskUsun_Click(object sender, RoutedEventArgs e)
        {
            if (trybPoruszania != TrybPoruszania.Obliczenia)
            {
                ZmienTryb(TrybPoruszania.Usowanie);
            }
        }

        private void przyciskCofnij_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.OtworzOkno(MainWindow.mainWindow.stronaStartowa, this);
        }
        #endregion

        #region Tworzenie obrazków
        public Grid StworzObrazekPolaczenia(string lokalizacja)
        {
            Grid obrazek = new Grid()
            {
                Width = 50, Height = 50,
                RenderTransformOrigin = new Point(0.5, 0.5),
                Visibility = Visibility.Hidden,
                Background = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/{lokalizacja}", UriKind.Absolute)),
                    Viewport = new System.Windows.Rect(0, 0, 50, 50),
                    TileMode = TileMode.Tile,
                    ViewportUnits = BrushMappingMode.Absolute
                }
            };
            Canvas.SetZIndex(obrazek, 0);

            przestrzen.Children.Add(obrazek);

            return obrazek;
        }

        public Image StworzObrazekPlanety()
        {
            Random generatorLosowosci = new Random();
            Image obrazek = new Image()
            {
                Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/Planety/planet_{generatorLosowosci.Next(18,28)}.png", UriKind.Absolute)),
                Height = 80, Width = 80
            };
            Canvas.SetZIndex(obrazek, 2);

            przestrzen.Children.Add(obrazek);

            return obrazek;
        }

        public Image StworzObrazekStatku()
        {
            Image obrazek = new Image()
            {
                Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/Statek/F5S4.png", UriKind.Absolute)),
                Height = 50, Width = 50,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };
            Canvas.SetZIndex(obrazek, 4);

            przestrzen.Children.Add(obrazek);

            return obrazek;
        }
        #endregion

        public void ZmienTryb(TrybPoruszania tryb)
        {
            if (trybPoruszania == TrybPoruszania.Trasa)
            {
                if (nowePolaczenie.PlanetaKoncowa == null)
                    nowePolaczenie.Dispose();
                nowePolaczenie = null;
            }              

            if (tryb == trybPoruszania)
                trybPoruszania = TrybPoruszania.Normalny;
            else
                trybPoruszania = tryb;

            stackTrybUsowania.Visibility = Visibility.Hidden;
            stackTrybTrasy.Visibility = Visibility.Hidden;

            switch (trybPoruszania)
            {
                case TrybPoruszania.Usowanie:
                    stackTrybUsowania.Visibility = Visibility.Visible;
                    break;
                case TrybPoruszania.Trasa:
                    stackTrybTrasy.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}