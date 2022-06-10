using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Strategia_Ogni_N_Pezzi : Strategia_abs
    {

        #region PROPRIETA'
        public override StrategiaEnum TipoStrategia => StrategiaEnum.Ogni_N_pezzi;
        public override string NomeStrategia => $"1 Ogni {_configurazione.Configurazione.Ogni_N_Pezzi.ToString()} motori";
        //public override string ProduzioneTurno_String => $"{_configurazione.Configurazione.Contatore_del_turno}";

        //public override string NomeStrategia => $"1 Ogni {_gestoreConfigurazione.Configurazione.Ogni_N_Pezzi} motori";

        #endregion

        public override event EventHandler ObiettivoTurnoRaggiuntoEvent;

        #region CTOR
        public Strategia_Ogni_N_Pezzi
           (
               IDataSource dataSource
           ) : base(dataSource)
        {

        }
        #endregion

        public override string GetProduzioneTurno_string(int prod, int targetProd)
        {
            return $"{prod.ToString()}";
        }
        internal override void EseguiSuMotoreCandidato(Motore motoreLetto, TurnoEnum turno)
        {
            var config = _configurazione.Configurazione;

            var cont_di_Comodo = config.Contatore_di_comodo;
            if (cont_di_Comodo == config.Ogni_N_Pezzi)  
            {
                //Questo è un motore obiettivo, che dovrà quindi andare alle Prove a Caldo.
                _dataSource.AggiornaTabellaConfig(config);
                //switch (turno)
                //{
                    
                //    case TurnoEnum.PrimoTurno:
                //        _dataSource.AggiornaTabellaConfig(cont_di_Comodo - 1, config.Prod_1T + 1, config.Contatore_del_giorno + 1);
                //        break;
                //    case TurnoEnum.SecondoTurno:
                //        _dataSource.AggiornaTabellaConfig(cont_di_Comodo - 1, config.Prod_2T + 1, config.Contatore_del_giorno + 1);
                //        break;
                //    case TurnoEnum.TerzoTurno:
                //        _dataSource.AggiornaTabellaConfig(cont_di_Comodo - 1, config.Prod_3T + 1, config.Contatore_del_giorno + 1);
                //        break;
                //    default:
                //        break;
                //}
                AzioneDaCompiere = AZIONE_DA_SVOLGERE;
                return;
            }
            if (cont_di_Comodo == 1)
            {
                _dataSource.SetContatoreDiComodo(config.Ogni_N_Pezzi); // Porto il contatore di comodo al numero "Ogni_N_Pezzi"
                AzioneDaCompiere = NESSUNA_AZIONE;
                return;
            }
            if(cont_di_Comodo < config.Ogni_N_Pezzi)
            {
                _dataSource.SetContatoreDiComodo(cont_di_Comodo - 1);  // Decremento du db il contatore di comodo
                AzioneDaCompiere = NESSUNA_AZIONE;
                return;
            }

        }

        public override bool IsMotoreTarget()
        {
            if(_configurazione.Configurazione.Contatore_di_comodo == _configurazione.Configurazione.Ogni_N_Pezzi)
            {
                _configurazione.Configurazione.Contatore_di_comodo -= 1;
                return true;

            }
            return false;
        }

    }
}
