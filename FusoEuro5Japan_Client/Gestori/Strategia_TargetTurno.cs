using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Strategia_TargetTurno : Strategia_abs
    {

        public override string Strategia_String => "Produzione";
            
        public override string Produzione_String => $"{_gestoreConfigurazione.Configurazione.Contatore_del_turno}/{_gestoreConfigurazione.Configurazione.N_pezzi_definito}";

        public override string NomeStrategia => "Produzione turno";

        #region CTOR
        public Strategia_TargetTurno
           (
               IDataSource dataSource,
               IGestoreConfigurazione gestoreConfigurazione
           ) : base(dataSource, gestoreConfigurazione)
        {
            SottoscriviEventi();
        }

        public override event EventHandler ObiettivoTurnoRaggiuntoEvent;
        #endregion

        #region SOTTOSCRIZIONE EVENTI
        public void SottoscriviEventi()
        {
            //_gestoreConfigurazione.ContatoriChanged += OnContatoriChanged;
        }
        #endregion



        internal override void EseguiSuMotoreCandidato(Motore motoreLetto)
        {
            var config = _dataSource.GetConfigurazione();
            var contTurno = config.Contatore_del_turno;
            var contGiorno = config.Contatore_del_giorno;
            var obiettivoTurno = config.N_pezzi_definito;

            if (contTurno > obiettivoTurno)
            {
                //Si è raggiunto l'obiettivo del turno
                //_dataSource.SettaPerMotoreTarget(contTurno - 1, config.Contatore_del_turno + 1, config.Contatore_del_giorno + 1);

                AzioneDaCompiere = NESSUNA_AZIONE;
                return;
            }
            
            if (contTurno <= obiettivoTurno)
            {
                if(contTurno == obiettivoTurno)
                {
                    ObiettivoTurnoRaggiuntoEvent?.Invoke(this, null);
                }

                _dataSource.AggiornaContatori(contTurno + 1, contGiorno + 1);  // Aggiorno i contatori
                AzioneDaCompiere = AZIONE_DA_SVOLGERE;
                return;
            }
        }
    }
}
