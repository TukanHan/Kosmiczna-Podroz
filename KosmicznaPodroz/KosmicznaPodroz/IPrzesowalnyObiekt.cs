using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KosmicznaPodroz
{
    interface IPrzesowalnyObiekt : IDisposable
    {
        Image Obrazek { get; }
        void Aktualizuj();
    }
}
