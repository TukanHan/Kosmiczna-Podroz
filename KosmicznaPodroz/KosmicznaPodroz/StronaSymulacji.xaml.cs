using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public enum TrybPoruszania { Normalny, Usowanie, Trasa, Statek};

    /// <summary>
    /// Miejsce gdzie odbywają się wszelkie interakcje z użytkownikiem
    /// </summary>
    public partial class StronaSymulacji : UserControl
    {
        private List<Planeta> Planety { get; } = new List<Planeta>();
        public Statek Statek { get; set; }
        public TrybPoruszania TrybPoruszania { get; private set; } = TrybPoruszania.Normalny;
           
        private Polaczenie nowePolaczenie = null;       
        private object zlapanyObiekt = null;
        private Cel oznaczonaPlaneta;


        public StronaSymulacji()
        {
            InitializeComponent();
            oznaczonaPlaneta = new Cel(obrazekCel);
        }

        #region Interakcja
        private void ZlapObiekt(IPrzesowalnyObiekt obiekt)
        {
            if(zlapanyObiekt == null && TrybPoruszania == TrybPoruszania.Normalny)
            {
                Mouse.OverrideCursor = Cursors.Hand;
                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);

                Canvas.SetZIndex(obiekt.Obrazek, 2);
                Canvas.SetTop(obiekt.Obrazek, mousePoint.Y - obiekt.Obrazek.Height / 2);
                Canvas.SetLeft(obiekt.Obrazek, mousePoint.X - obiekt.Obrazek.Width / 2);

                zlapanyObiekt = obiekt.Obrazek;
            }          
        }

        private void PrzesowajObiekt(IPrzesowalnyObiekt obiekt)
        {
            if (zlapanyObiekt == obiekt.Obrazek && TrybPoruszania == TrybPoruszania.Normalny)
            {
                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);
                Mouse.Capture(obiekt.Obrazek);
                Canvas.SetTop(obiekt.Obrazek, mousePoint.Y - obiekt.Obrazek.Height / 2);
                Canvas.SetLeft(obiekt.Obrazek, mousePoint.X - obiekt.Obrazek.Width / 2);
                obiekt.Aktualizuj();
            }
        }

        private void PuscPlanete(Planeta planeta)
        {
            if (zlapanyObiekt == planeta.Obrazek)
            {
                Mouse.Capture(null);
                Mouse.OverrideCursor = Cursors.Arrow;

                Canvas.SetZIndex(planeta.Obrazek, 1);
                zlapanyObiekt = null;

                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);
                double y = mousePoint.Y;
                double x = mousePoint.X;

                if (x >= 670 || y >= 470 || y <= 0 || x <= 0)
                {
                    planeta.Dispose();
                }
            }
        }

        private void PuscStatek(Statek statek)
        {
            if (zlapanyObiekt == statek.Obrazek)
            {
                Mouse.Capture(null);
                Mouse.OverrideCursor = Cursors.Arrow;

                Canvas.SetZIndex(statek.Obrazek, 2);
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
                                new Punkt(Canvas.GetLeft(statek.Obrazek) + statek.Obrazek.Width /2, Canvas.GetTop(statek.Obrazek) + statek.Obrazek.Height / 2),
                                new Punkt(Canvas.GetLeft(planeta.Obrazek) + planeta.Obrazek.Width / 2, Canvas.GetTop(planeta.Obrazek) + planeta.Obrazek.Height / 2)
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
        #endregion

        #region Przyciski
        private void przyciskTworzPlanete_Click(object sender, RoutedEventArgs e)
        {
            ZmienTryb(TrybPoruszania.Normalny);

            Planeta nowaPlaneta = new Planeta();
            ZlapObiekt(nowaPlaneta);
            Planety.Add(nowaPlaneta);
        }

        private void przyciskTworzPolaczenieJedno_Click(object sender, RoutedEventArgs e)
        {
            ZmienTryb(TrybPoruszania.Trasa);

            if (TrybPoruszania == TrybPoruszania.Trasa)
                nowePolaczenie = new PolaczenieJednostronne();
        }

        private void przyciskTworzPolaczenieWdu_Click(object sender, RoutedEventArgs e)
        {
            ZmienTryb(TrybPoruszania.Trasa);

            if (TrybPoruszania == TrybPoruszania.Trasa)
                nowePolaczenie = new PolaczenieDwustronne();
        }

        private void przyciskTworzStatek_Click(object sender, RoutedEventArgs e)
        {
            ZmienTryb(TrybPoruszania.Normalny);
            if (Statek == null)
            {
                Statek = new Statek();
                ZlapObiekt(Statek);
            }
        }

        private void przyciskUsun_Click(object sender, RoutedEventArgs e)
        {
            ZmienTryb(TrybPoruszania.Usowanie);
        }
        #endregion

        #region Tworzenie obrazków
        public Grid StworzObrazekPolaczenia(string lokalizacja, Polaczenie polaczenie)
        {
            Grid obrazek = new Grid()
            {
                Width = 50,
                Height = 50,
                RenderTransformOrigin = new Point(0.5, 0.5),
                Background = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/{lokalizacja}", UriKind.Absolute)),
                    Viewport = new System.Windows.Rect(0, 0, 50, 50),
                    TileMode = TileMode.Tile,
                    ViewportUnits = BrushMappingMode.Absolute
                }
            };

            obrazek.MouseDown += (s, args) =>
            {
                if (TrybPoruszania == TrybPoruszania.Usowanie)
                    polaczenie.Dispose();
            };

            przestrzen.Children.Add(obrazek);

            return obrazek;
        }

        public Image StworzObrazekPlanety(Planeta planeta)
        {
            Random generatorLosowosci = new Random();
            Image obrazek = new Image()
            {
                Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/Planety/planet_{generatorLosowosci.Next(18,33)}.png", UriKind.Absolute)),
                Height = 80,
                Width = 80
            };

            obrazek.MouseDown += (s, args) => ZlapObiekt(planeta);
            obrazek.MouseDown += (s, args) =>
            {
                if (TrybPoruszania == TrybPoruszania.Trasa)
                {
                    if (nowePolaczenie.DodajPlanete(planeta))
                    {
                        ZmienTryb(TrybPoruszania.Normalny);
                    }
                }
            };
            obrazek.MouseMove += (s, args) => PrzesowajObiekt(planeta);
            obrazek.MouseMove += (s, args) =>
            {
                if (Statek != null && Statek.PlanetaDokowania != planeta && TrybPoruszania == TrybPoruszania.Normalny && zlapanyObiekt == null)
                {
                    oznaczonaPlaneta.Zaznacz(planeta);
                }
            };
            obrazek.MouseLeave += (s, args) =>
            {
                if (oznaczonaPlaneta.OznaczonaPlaneta == planeta)
                    oznaczonaPlaneta.Odznacz();
            };
            obrazek.MouseUp += (s, args) => PuscPlanete(planeta);
            obrazek.MouseUp += (s, args) =>
            {
                if (TrybPoruszania == TrybPoruszania.Usowanie)
                {
                    Planety.Remove(planeta);
                    planeta.Dispose();
                }
            };

            przestrzen.Children.Add(obrazek);

            return obrazek;
        }

        public Image StworzObrazekStatku(Statek statek)
        {
            Image obrazek = new Image()
            {
                Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/Statek/F5S4.png", UriKind.Absolute)),
                Height = 50,
                Width = 50
            };

            obrazek.MouseDown += (s, args) => ZlapObiekt(statek);
            obrazek.MouseMove += (s, args) => PrzesowajObiekt(statek);
            obrazek.MouseUp += (s, args) =>
            {
                if (TrybPoruszania == TrybPoruszania.Usowanie)                
                    statek.Dispose();
            };
            obrazek.MouseUp += (s, args) => PuscStatek(statek);

            przestrzen.Children.Add(obrazek);

            return obrazek;
        }
        #endregion

        private void ZmienTryb(TrybPoruszania tryb)
        {
            if (TrybPoruszania == TrybPoruszania.Trasa)
            {
                if (nowePolaczenie.PlanetaKoncowa == null)
                    nowePolaczenie.Dispose();
                nowePolaczenie = null;
            }              

            if (tryb == TrybPoruszania)
                TrybPoruszania = TrybPoruszania.Normalny;
            else
                TrybPoruszania = tryb;

            stackTrybUsowania.Visibility = Visibility.Hidden;
            stackTrybTrasy.Visibility = Visibility.Hidden;

            switch (TrybPoruszania)
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
