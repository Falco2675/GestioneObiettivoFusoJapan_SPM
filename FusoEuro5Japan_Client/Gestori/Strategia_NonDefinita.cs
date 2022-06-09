using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Strategia_NonDefinita : Strategia_abs
    {
        public Strategia_NonDefinita(IDataSource dataSource, IGestoreConfigurazione gestoreConfigurazione) : base(dataSource, gestoreConfigurazione)
        {
        }

        public override string Strategia_String => "N.D.";
        public override string Produzione_String => "";

        public override string NomeStrategia => "N.D.";

        public override StrategiaEnum TipoStrategia => StrategiaEnum.Non_Definita;

        public override event EventHandler ObiettivoTurnoRaggiuntoEvent;

        internal override void EseguiSuMotoreCandidato(Motore motoreLetto)
        {
            
        }
    }
}
