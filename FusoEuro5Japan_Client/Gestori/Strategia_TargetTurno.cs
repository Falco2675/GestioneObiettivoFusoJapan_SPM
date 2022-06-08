using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Strategia_TargetTurno : Strategia_abs
    {

        public override string Strategia_String => "Obiettivo";
            
        public override string Produzione_String => $"{_gestoreConfigurazione.Configurazione.Contatore_del_turno}/{_gestoreConfigurazione.Configurazione.N_pezzi_definito}";

        #region CTOR
        public Strategia_TargetTurno
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
            var contTurno = config.Contatore_del_turno;
            var obiettivo = config.N_pezzi_definito;



            if (contTurno == obiettivo)
            {
                //Si è raggiunto l'obiettivo del turno
                //_dataSource.SettaPerMotoreTarget(contTurno - 1, config.Contatore_del_turno + 1, config.Contatore_del_giorno + 1);

                AzioneDaCompiere = NESSUNA_AZIONE;
                return;
            }
            if (contTurno == 1)
            {
                _dataSource.SetContatoreDiComoodo(config.Ogni_N_Pezzi); // Porto il contatore di comodo al numero "Ogni_N_Pezzi"
                AzioneDaCompiere = NESSUNA_AZIONE;
                return;
            }
            if (contTurno < config.Ogni_N_Pezzi)
            {
                _dataSource.SetContatoreDiComoodo(contTurno - 1);  // Decremento du db il contatore di comodo
                AzioneDaCompiere = NESSUNA_AZIONE;
                return;
            }
        }
    }
}
