using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public abstract class Strategia_abs : IStrategia
    {
        protected IGestoreConfigurazione _configurazione;
        protected string NESSUNA_AZIONE = "Nessuna azione.";
        protected string AZIONE_DA_SVOLGERE = "EFFETTUARE PROVA A CALDO.";
        private string _azioneDaCompiere;

        public abstract StrategiaEnum TipoStrategia { get; }
        public abstract string NomeStrategia { get; }

        public string AzioneDaCompiere
        {
            get { return _azioneDaCompiere; }
            set
            {

                _azioneDaCompiere = value;
                AzioneDaCompiereChanged?.Invoke(this, _azioneDaCompiere);
            }
        }


        public event EventHandler<string> AzioneDaCompiereChanged;
        public abstract event EventHandler ObiettivoTurnoRaggiuntoEvent;

        #region CTOR
        public Strategia_abs()
        {
        }
        #endregion

        public abstract string GetProduzioneTurno_string(int prod, int targetProd);
        public void SetConfigurazione(IGestoreConfigurazione config)
        {
            _configurazione = config;
        }

        public abstract bool IsMotoreTarget();

        public void ResettaAzione()
        {
            AzioneDaCompiere = "";
        }

        public void EseguiNessunaAzione()
        {
            AzioneDaCompiere = NESSUNA_AZIONE;
        }
    }
}
