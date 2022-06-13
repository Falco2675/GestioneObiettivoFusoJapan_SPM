using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Strategia_NonDefinita : Strategia_abs
    {
        public Strategia_NonDefinita() 
        {
        }

        public override string NomeStrategia => "N.D.";

        public override string GetProduzioneTurno_string(int prod, int targetProd)
        {
            return "-";
        }
        public override StrategiaEnum TipoStrategia => StrategiaEnum.Non_Definita;

        public override event EventHandler ObiettivoTurnoRaggiuntoEvent;

        public override bool IsMotoreTarget()
        {
            return false;
        }
    }
}
