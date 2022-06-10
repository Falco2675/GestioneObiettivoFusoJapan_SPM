using System;
using System.Configuration;
using System.Threading;

namespace FusoEuro5Japan_Client
{
    public class GestoreConfigurazione : IGestoreConfigurazione
    {
        #region CAMPI PRIVATI
        private Config _configurazione;
        private readonly Timer _timerGetConfig;
        private readonly IDataSource _dataSource;
        private readonly IGestoreContatoriObiettivi _gestoreContatoriObiettivi;
        private readonly IGestoreStrategiaDiProduzione _gestoreStrategiaDiProduzione;
        private readonly IGestoreTurni _gestoreTurni;
        #endregion

        #region PROPRIETA'
        public int IdApp => 2;
        public StrategiaEnum StrategiaAdottata { get; private set; }
        public TurnoEnum TurnoCorrente => _gestoreTurni.Turno_enum;
        public string ContatoreTurno_string { get; private set; }
        public string ContatoreGiorno_string { get; private set; }
        public Config Configurazione
        {
            get { return _configurazione; }
            private set
            {
                ControllaStrategia(value);
                _configurazione = value;
            }
        }

        #endregion

        #region EVENTI
        public event EventHandler StrategiaChanged;
        public event EventHandler ContatoriChanged;

        #endregion

        #region CTOR
        public GestoreConfigurazione
            (
                IDataSource dataSource,
                IGestoreContatoriObiettivi gestoreContatoriObiettivi,
                IGestoreStrategiaDiProduzione gestoreStrategiaDiProduzione,
                IGestoreTurni gestoreTurni
            )
        {
            _dataSource = dataSource;
            _gestoreContatoriObiettivi = gestoreContatoriObiettivi;
            _gestoreStrategiaDiProduzione = gestoreStrategiaDiProduzione;
            _gestoreTurni = gestoreTurni;
            _configurazione = new Config();

            _timerGetConfig = new Timer((o) => { AggiornaConfigurazione(); }, null, 500, 5000);
        }
        #endregion

        #region METODI PUBBLICI
        public void AggiornaConfigurazione()
        {
            _timerGetConfig?.Change(-1, -1);

            try
            {
                Configurazione = _dataSource.GetConfigurazione();
                AggiornaContatori(Configurazione);

                ControllaStrategia(Configurazione);
            }
            catch (System.Exception ex)
            {
                throw new Exception("Errore DB. Configurazione!");
            }
            finally
            {
                _timerGetConfig.Change(5000, 5000);
            }

        }


        public void ResettaTurno()
        {
            _dataSource.ResettaTurno(_configurazione.Ogni_N_Pezzi);
        } 
        #endregion

        #region METODI PRIVATI
        private void ControllaStrategia(Config newConfig)
        {
            
            _gestoreStrategiaDiProduzione.SetStrategia(this);

        }
        //private StrategiaEnum GetStrategia(Config config)
        //{
        //    if (config.Ogni_N_Pezzi > 0 && (config.Obiettivo_1T == 0 && config.Obiettivo_2T == 0 && config.Obiettivo_3T == 0))
        //        return StrategiaEnum.Ogni_N_pezzi;
        //    if (config.Ogni_N_Pezzi == 0 && (config.Obiettivo_1T > 0 || config.Obiettivo_2T > 0 || config.Obiettivo_3T > 0))
        //        return StrategiaEnum.ProduzioneTurni;

        //    return StrategiaEnum.Non_Definita;
        //}

        //private void ControllaContatori(Config newConfig)
        //{
        //    var contatoreTurnoAggiornato = newConfig.Contatore_del_turno;
        //    var contatoreGiornoAggiornato = newConfig.Contatore_del_giorno;

        //    if(contatoreTurnoAggiornato != _configurazione.Contatore_del_turno || contatoreGiornoAggiornato != _configurazione.Contatore_del_giorno)
        //    {
        //        ContatoreGiorno_string = contatoreGiornoAggiornato.ToString();
        //        ContatoreTurno_string = contatoreTurnoAggiornato.ToString();
        //        ContatoriChanged?.Invoke(this, null);
        //    }

        //}
        private void AggiornaContatori(Config config)
        {
            _gestoreContatoriObiettivi.Obiettivo_1T = config.Obiettivo_1T;
            _gestoreContatoriObiettivi.Obiettivo_2T = config.Obiettivo_2T;
            _gestoreContatoriObiettivi.Obiettivo_3T = config.Obiettivo_3T;
            _gestoreContatoriObiettivi.Prod_1T = config.Prod_1T;
            _gestoreContatoriObiettivi.Prod_2T = config.Prod_2T;
            _gestoreContatoriObiettivi.Prod_3T = config.Prod_3T;
            _gestoreContatoriObiettivi.Prod_Ieri = config.Prod_Ieri;
            _gestoreContatoriObiettivi.Contatore_del_giorno = config.Contatore_del_giorno;
        }



        #endregion
    }
}
