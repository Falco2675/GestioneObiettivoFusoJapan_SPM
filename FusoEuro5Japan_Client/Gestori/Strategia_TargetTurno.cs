using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Strategia_TargetTurno : Strategia_abs
    {

        public override StrategiaEnum TipoStrategia => StrategiaEnum.ProduzioneTurni;
        public override string NomeStrategia => "Produzione Turno";
            
        //public override string ProduzioneTurno_String => $"{_configurazione.Configurazione.Contatore_del_turno}/{_configurazione.Configurazione.N_pezzi_definito}";


        #region CTOR
        public Strategia_TargetTurno
           (
               IDataSource dataSource
           ) : base(dataSource)
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

        public override string GetProduzioneTurno_string(int prod, int targetProd)
        {
            return $"{prod}/{targetProd}";
        }

        internal override void EseguiSuMotoreCandidato(Motore motoreLetto, TurnoEnum turno)
        {
            int prodT = 0;
            int obiettivoTurno = 0;
            switch (turno)
            {
                case TurnoEnum.PrimoTurno:
                    prodT = _configurazione.Configurazione.Prod_1T;
                    obiettivoTurno = _configurazione.Configurazione.Obiettivo_1T;
                    break;
                case TurnoEnum.SecondoTurno:
                    prodT = _configurazione.Configurazione.Prod_2T;
                    obiettivoTurno = _configurazione.Configurazione.Obiettivo_2T;
                    break;
                case TurnoEnum.TerzoTurno:
                    prodT = _configurazione.Configurazione.Prod_3T;
                    obiettivoTurno = _configurazione.Configurazione.Obiettivo_3T;
                    break;
                default:
                    break;
            }

            //var config = _dataSource.GetConfigurazione();
            var contGiorno = _configurazione.Configurazione.Contatore_del_giorno;
            //var obiettivoTurno = config.N_pezzi_definito;

            if (prodT > obiettivoTurno)
            {
                //Si è raggiunto l'obiettivo del turno
                //_dataSource.SettaPerMotoreTarget(contTurno - 1, config.Contatore_del_turno + 1, config.Contatore_del_giorno + 1);

                AzioneDaCompiere = NESSUNA_AZIONE;
                return;
            }
            
            if (prodT <= obiettivoTurno)
            {
                if(prodT == obiettivoTurno)
                {
                    ObiettivoTurnoRaggiuntoEvent?.Invoke(this, null);
                }

                _dataSource.AggiornaContatori(prodT + 1, contGiorno + 1);  // Aggiorno i contatori
                AzioneDaCompiere = AZIONE_DA_SVOLGERE;
                return;
            }
        }

        public override bool IsMotoreTarget()
        {
            switch (_configurazione.TurnoCorrente)
            {
                case TurnoEnum.PrimoTurno:
                    return _configurazione.Configurazione.Prod_1T <= _configurazione.Configurazione.Obiettivo_1T;

                case TurnoEnum.SecondoTurno:
                    return _configurazione.Configurazione.Prod_2T <= _configurazione.Configurazione.Obiettivo_2T;

                case TurnoEnum.TerzoTurno:
                    return _configurazione.Configurazione.Prod_3T <= _configurazione.Configurazione.Obiettivo_3T;
            }

            return false;
        }
    }
}
