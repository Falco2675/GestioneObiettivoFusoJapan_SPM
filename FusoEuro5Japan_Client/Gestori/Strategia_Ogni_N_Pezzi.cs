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
        public override string Strategia_String => $"1 Ogni {_gestoreConfigurazione.Configurazione.Ogni_N_Pezzi}";
        public override string Produzione_String => $"{_gestoreConfigurazione.Configurazione.Contatore_del_turno}";

        #endregion

        public override event EventHandler ObiettivoTurnoRaggiuntoEvent;

        #region CTOR
        public Strategia_Ogni_N_Pezzi
           (
               IDataSource dataSource,
               IGestoreConfigurazione gestoreConfigurazione
           ) : base(dataSource, gestoreConfigurazione)
        {

        }
        #endregion

        internal override void EseguiSuMotoreCandidato(Motore motoreLetto)
        {
            var config = _dataSource.GetConfigurazione();
            var cont_di_Comodo = config.Contatore_di_comodo;
            if (cont_di_Comodo == config.Ogni_N_Pezzi)  
            {
                //Questo è un motore obiettivo, che dovrà quindi andare alle Prove a Caldo.
                _dataSource.SettaPerMotoreTarget(cont_di_Comodo - 1, config.Contatore_del_turno + 1, config.Contatore_del_giorno + 1);
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

    }
}
