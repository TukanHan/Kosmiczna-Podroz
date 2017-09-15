﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KosmicznaPodroz
{
    public class PolaczenieDwustronne : Polaczenie
    {
        public PolaczenieDwustronne() { }

        public override bool DodajPlanete(Planeta planeta)
        {
            planeta.Polaczenia.Add(this);
            planeta.PoruszenieEvent += delegatDoPoruszaniaObiektu;
            planeta.UsowanieEvent += delegatDoUsunieciaObiektu;

            if (PlanetaStartowa == null)
            {
                PlanetaStartowa = planeta;
                return false;
            }
            else
            {
                PlanetaKoncowa = planeta;
                Obrazek.Visibility = System.Windows.Visibility.Visible;

                Aktualizuj();

                if (CzyNalerzyUsunac())
                    Dispose();

                return true;
            }
        }
    }
}
