using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public interface IStrategia
    {
        string Strategia_String { get; }
        string Produzione_String { get; }
        string AzioneDaCompiere { get; }

        event EventHandler<string> AzioneDaCompiereChanged;
        event EventHandler ObiettivoTurnoRaggiuntoEvent;

        void EseguiSuMotore(Motore motoreLetto);
    }
}
