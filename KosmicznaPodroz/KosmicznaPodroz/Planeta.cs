using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace KosmicznaPodroz
{
    /// <summary>
    /// Klasa przechowująca dane o planetach i ich połączeniach
    /// </summary>
    public class Planeta : IPrzesowalnyObiekt, IDisposable
    {
        public List<Polaczenie> Polaczenia { get; private set; } = new List<Polaczenie>();
        public Image Obrazek { get; set; }
        public Punkt<double> Pozycja
        {
            get { return new Punkt<double>(Canvas.GetLeft(Obrazek), Canvas.GetTop(Obrazek)); }
            set { Canvas.SetLeft(Obrazek, value.X); Canvas.SetTop(Obrazek, value.Y); }
        }

        public event EventHandler PoruszenieEvent;
        public event EventHandler UsowanieEvent;

        public Planeta()
        {
            MainWindow.mainWindow.stronaSymulacji.Planety.Add(this);
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
            MainWindow.mainWindow.stronaSymulacji.Planety.Remove(this);
        }
    }
}
