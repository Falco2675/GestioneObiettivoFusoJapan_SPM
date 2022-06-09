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
                IDataSource dataSource
            )
        {
            _dataSource = dataSource;
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
            var strategiaInCorso = GetStrategia(_configurazione.Ogni_N_Pezzi, _configurazione.N_pezzi_definito);
            var strategiaDaAdottare = GetStrategia(newConfig.Ogni_N_Pezzi, newConfig.N_pezzi_definito);

            if (strategiaInCorso != strategiaDaAdottare)
            {
                StrategiaAdottata = strategiaDaAdottare;
                StrategiaChanged?.Invoke(this, null);
            }

        }
        private StrategiaEnum GetStrategia(int ogni_N_Pezzi, int n_pezzi_definito)
        {
            if (ogni_N_Pezzi > 0 && n_pezzi_definito == 0)
                return StrategiaEnum.Ogni_N_pezzi;
            if (ogni_N_Pezzi == 0 && n_pezzi_definito > 0)
                return StrategiaEnum.N_Pezzi_Definito;

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
