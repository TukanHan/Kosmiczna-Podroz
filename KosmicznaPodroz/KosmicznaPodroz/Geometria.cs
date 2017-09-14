using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KosmicznaPodroz
{
    public struct Punkt
    {
        public double X { get; }
        public double Y { get; }

        public Punkt(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class Geometria
    {
        private Punkt obiektA;
        private Punkt obiektB;

        public Geometria(Punkt obiektA, Punkt obiektB)
        {
            this.obiektA = obiektA;
            this.obiektB = obiektB;
        }

        public Punkt ObliczPozycjePomiedzy()
        {
            return new Punkt((obiektA.X + obiektB.X) / 2, (obiektA.Y + obiektB.Y) / 2);
        }

        public double ObliczOdlegloscPomiedzy()
        {
            return Math.Sqrt(Math.Pow(obiektA.X - obiektB.X, 2) + Math.Pow(obiektA.Y - obiektB.Y, 2));
        }

        public double ObliczKatPomiedzy()
        {
            return Math.Atan2(obiektB.Y - obiektA.Y, obiektB.X - obiektA.X) * (180 / Math.PI) + 90;           
        }
    }
}
