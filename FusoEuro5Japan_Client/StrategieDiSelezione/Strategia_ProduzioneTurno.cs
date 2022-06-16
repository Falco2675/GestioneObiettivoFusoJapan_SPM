using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Strategia_ProduzioneTurno : Strategia_abs
    {

        public override StrategiaEnum TipoStrategia => StrategiaEnum.ProduzioneTurni;
        public override string NomeStrategia => "Produzione Turno";
            


        #region CTOR
        public Strategia_ProduzioneTurno() 
        {
        }

        #endregion

        public override string GetProduzioneTurno_string(int prod, int targetProd) => $"{prod}/{targetProd}";

        public override bool IsMotoreTarget()
        {
            bool result = false;

            if (_configurazione.IsObiettivoTurnoRaggiunto) return false;
            
            switch (_configurazione.TurnoCorrente)
            {
                case TurnoEnum.PrimoTurno:
                    AzioneDaCompiere = AZIONE_DA_SVOLGERE;
                    result = _configurazione.Configurazione.Prod_1T <= _configurazione.Configurazione.Obiettivo_1T;
                    break;

                case TurnoEnum.SecondoTurno:
                    AzioneDaCompiere = AZIONE_DA_SVOLGERE;
                    result= _configurazione.Configurazione.Prod_2T <= _configurazione.Configurazione.Obiettivo_2T;
                    break;

                case TurnoEnum.TerzoTurno:
                    AzioneDaCompiere = AZIONE_DA_SVOLGERE;
                    result= _configurazione.Configurazione.Prod_3T <= _configurazione.Configurazione.Obiettivo_3T;
                    break;
            }

            AzioneDaCompiere = result ? AZIONE_DA_SVOLGERE : NESSUNA_AZIONE;
            return result;
        }
    }
}
