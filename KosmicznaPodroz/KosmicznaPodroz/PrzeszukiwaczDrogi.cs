using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KosmicznaPodroz
{  
    /// <summary>
    /// Klasa w której wyznaczana jest trasa z punktu A do B
    /// </summary>
    public class PrzeszukiwaczDrogi
    {
        /// <summary>
        /// Wewnętrzna klasa pomocnicza
        /// </summary>
        private class Wezel
        {
            public Planeta Klucz { get; }
            public Wezel Poprzednik { get; set; }
            public Double Dystans { get; set; } = Double.MaxValue;
            public bool CzyDodawany { get; set; } = false;

            public Wezel(Planeta klucz)
            {
                Klucz = klucz;
            }
        }

        private List<Planeta> infrastruktura;

        public PrzeszukiwaczDrogi(List<Planeta> infrastruktura)
        {
            this.infrastruktura = infrastruktura;
        }

        //metoda oznaczająca poprzednków każdego węzła
        public List<Planeta> ZwrocScierzke(Planeta start, Planeta koniec)
        {
            List<Wezel> wezly = new List<Wezel>();
            for (int i = 0; i < infrastruktura.Count; ++i)
            {
                wezly.Add(new Wezel(infrastruktura[i]));
            }

            wezly.Find(obiekt => obiekt.Klucz == start).Dystans = 0;

            List<Wezel> kolejka = new List<Wezel>();
            kolejka.Add(wezly.Find(obiekt => obiekt.Klucz == start));
            wezly.Find(obiekt => obiekt.Klucz == start).CzyDodawany = true;
            while (kolejka.Count > 0)
            {
                Wezel aktualnyWezel = kolejka.OrderBy(obiekt => obiekt.Dystans).First();
                if (aktualnyWezel.Klucz == koniec)
                    break;

                foreach(Polaczenie poloczenie in aktualnyWezel.Klucz.Polaczenia)
                {
                    Wezel sasiedniWezel = wezly.Find(obiekt => obiekt.Klucz == poloczenie.ZwrocPrzeciwnaPlanete(aktualnyWezel.Klucz));
                    if(sasiedniWezel.Dystans > aktualnyWezel.Dystans + poloczenie.Waga)
                    {
                        sasiedniWezel.Dystans = aktualnyWezel.Dystans + poloczenie.Waga;
                        sasiedniWezel.Poprzednik = aktualnyWezel;
                    }                   
                    if (!sasiedniWezel.CzyDodawany)
                    {
                        kolejka.Add(sasiedniWezel);
                        sasiedniWezel.CzyDodawany = true;
                    }
                }
                
                kolejka.Remove(aktualnyWezel);
            }
            return ZnajdzTraseZPoprzednikow(wezly, start, koniec);           
        }

        //metoda która z poprzedników węzła potrafi znaleźć trasę
        private List<Planeta> ZnajdzTraseZPoprzednikow(List<Wezel> poprzednicy, Planeta start, Planeta koniec)
        {
            List<Planeta> planety = new List<Planeta>();

            Wezel nastepny = poprzednicy.Find(obiekt => obiekt.Klucz == koniec);
            while(nastepny.Poprzednik != null)
            {
                planety.Add(nastepny.Klucz);
                nastepny = nastepny.Poprzednik;
            }

            return planety.Reverse<Planeta>().ToList();
        }
    }
}