using System;
using System.Configuration;
using System.Threading;

namespace FusoEuro5Japan_Client
{
    public class GestoreConfigurazione : IGestoreConfigurazione
    {
        private Config _configurazione;
        private readonly Timer _timerGetConfig;
        private readonly IDataSource _dataSource;

        public int IdApp => 2;

        public Config Configurazione
        {
            get { return _configurazione; }
            set { _configurazione = value; }
        }

        #region EVENTI
        public event EventHandler<StrategiaEnum> CambioStrategiaChanged;
        public event EventHandler<int> ContatoreDelTurnoChanged;

        #endregion


        #region CTOR
        public GestoreConfigurazione
            (
                IDataSource dataSource
            )
        {
            _dataSource = dataSource;
            _timerGetConfig = new Timer((o) => { GetConfigurazione(); }, null, 500, 5000);
        }
        #endregion

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


    }
}
