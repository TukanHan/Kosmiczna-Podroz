using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KosmicznaPodroz
{
    /// <summary>
    /// Interfejs zawierający cechy wspólne dla obiektów przesuwalnych
    /// </summary>
    interface IPrzesowalnyObiekt
    {
        Image Obrazek { get; }
        Punkt<double> Pozycja { get; set; }
        void Aktualizuj();
    }
}
