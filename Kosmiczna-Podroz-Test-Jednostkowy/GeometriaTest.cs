using KosmicznaPodroz;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kosmiczna_Podroz_Test_Jednostkowy
{

    [TestFixture]
    public class GeometriaTest
    {
        [TestCase(2,4,2,4,2,4)]
        [TestCase(4, 0, -4, 0, 0, 0)]
        [TestCase(1, 1, 0, 0, 0.5, 0.5)]
        public void ObliczPozycjePomiedzyTest(double obiektA_X, double obiektA_Y, double obiektB_X, double obiektB_Y, double oczekiwany_X, double oczekiwany_Y)
        {
            var geometria = new Geometria(new Punkt(obiektA_X, obiektA_Y), new Punkt(obiektB_X, obiektB_Y));

            Assert.AreEqual(new Punkt(oczekiwany_X, oczekiwany_Y), geometria.ObliczPozycjePomiedzy());
        }

        [TestCase(2, 5, 5, 9, 5)]
        [TestCase(2, 5, 2, 5, 0)]
        [TestCase(1, 0, 0, 0, 1)]
        [TestCase(0.5, 0, 0, 0, 0.5)]
        public void OliczOdlegloscPomiedzyTest(double obiektA_X, double obiektA_Y, double obiektB_X, double obiektB_Y, double oczekiwany)
        {
            var geometria = new Geometria(new Punkt(obiektA_X, obiektA_Y), new Punkt(obiektB_X, obiektB_Y));

            Assert.AreEqual(oczekiwany, geometria.ObliczOdlegloscPomiedzy());
        }

        [TestCase(0, 0, 1, 0, 90)]
        [TestCase(0, 0, -1, 0, 270)]
        public void ObliczKatPomiedzy(double obiektA_X, double obiektA_Y, double obiektB_X, double obiektB_Y, double oczekiwany)
        {
            var geometria = new Geometria(new Punkt(obiektA_X, obiektA_Y), new Punkt(obiektB_X, obiektB_Y));

            Assert.AreEqual(oczekiwany, geometria.ObliczKatPomiedzy());
        }
    }
}
