﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public abstract class Strategia_abs : IStrategia
    {
        protected readonly IDataSource _dataSource;
        protected readonly IGestoreConfigurazione _gestoreConfigurazione;
        protected string NESSUNA_AZIONE = "Nessuna azione.";
        protected string AZIONE_DA_SVOLGERE = "EFFETTUARE PROVA A CALDO.";
        private string _azioneDaCompiere;

        public abstract StrategiaEnum TipoStrategia { get; }
        public abstract string Strategia_String { get; }
        public abstract string Produzione_String { get; }
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
        public Strategia_abs
            (
                IDataSource dataSource,
                IGestoreConfigurazione gestoreConfigurazione
            )
        {
            _dataSource = dataSource;
            _gestoreConfigurazione = gestoreConfigurazione;
        }

        public void EseguiSuMotore(Motore motoreLetto)
        {
            if (motoreLetto.IsTargetCandidate)
                EseguiSuMotoreCandidato(motoreLetto);
            else
                AzioneDaCompiere = "Nessuna azione da eseguire.";

        }

        internal abstract void EseguiSuMotoreCandidato(Motore motoreLetto);
        #endregion



    }
}
