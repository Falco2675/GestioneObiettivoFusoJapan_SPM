using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public interface IStrategia
    {
        StrategiaEnum TipoStrategia { get; }
        string NomeStrategia { get; }
        string AzioneDaCompiere { get; }
        bool Azione_Bool { get; }

        event EventHandler<string> AzioneDaCompiereChanged;

        string GetProduzioneTurno_string(int prod, int targetProd);
        void SetConfigurazione(IGestoreConfigurazione config);
        bool IsMotoreTarget();
        void ResettaAzione();
        void EseguiNessunaAzione();
    }
}
