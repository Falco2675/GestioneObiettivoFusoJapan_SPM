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
        #endregion

        #region PROPRIETA'
        public int IdApp => 2;
        public StrategiaEnum StrategiaAdottata { get; private set; }
        public string ContatoreTurno_string { get; private set; }
        public string ContatoreGiorno_string { get; private set; }
        public Config Configurazione
        {
            get { return _configurazione; }
            private set
            {
                ControllaStrategia(value);
                ControllaContatori(value);
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
                IGestoreContatoriObiettivi gestoreContatoriObiettivi
            )
        {
            _dataSource = dataSource;
            _gestoreContatoriObiettivi = gestoreContatoriObiettivi;
            _configurazione = new Config();

            _timerGetConfig = new Timer((o) => { Configurazione = GetConfigurazione(); }, null, 500, 5000);
        }
        #endregion

        #region METODI PUBBLICI
        public Config GetConfigurazione()
        {
            _timerGetConfig?.Change(-1, -1);

            try
            {
                return _dataSource.GetConfigurazione();

            }
            catch (System.Exception)
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
            var strategiaInCorso = GetStrategia(_configurazione);
            var strategiaDaAdottare = GetStrategia(newConfig);

            if (strategiaInCorso != strategiaDaAdottare)
            {
                StrategiaAdottata = strategiaDaAdottare;
                StrategiaChanged?.Invoke(this, null);
            }

        }
        private StrategiaEnum GetStrategia(Config config)
        {
            if (config.Ogni_N_Pezzi > 0 && (config.Obiettivo_1T == 0 && config.Obiettivo_2T == 0 && config.Obiettivo_3T == 0))
                return StrategiaEnum.Ogni_N_pezzi;
            if (config.Ogni_N_Pezzi == 0 && (config.Obiettivo_1T > 0 || config.Obiettivo_2T > 0 || config.Obiettivo_3T > 0))
                return StrategiaEnum.ProduzioneTurni;

            return StrategiaEnum.Non_Definita;
        }

        private void ControllaContatori(Config newConfig)
        {
            var contatoreTurnoAggiornato = newConfig.Contatore_del_turno;
            var contatoreGiornoAggiornato = newConfig.Contatore_del_giorno;

            if(contatoreTurnoAggiornato != _configurazione.Contatore_del_turno || contatoreGiornoAggiornato != _configurazione.Contatore_del_giorno)
            {
                ContatoreGiorno_string = contatoreGiornoAggiornato.ToString();
                ContatoreTurno_string = contatoreTurnoAggiornato.ToString();
                ContatoriChanged?.Invoke(this, null);
            }

        }



        #endregion
    }
}
