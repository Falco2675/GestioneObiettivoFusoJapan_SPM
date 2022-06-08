﻿using System;
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

        #region CTOR
        public Strategia_Ogni_N_Pezzi
           (
               IDataSource dataSource,
               IGestoreConfigurazione gestoreConfigurazione
           ) : base(dataSource, gestoreConfigurazione)
        {

        }

        internal override void EseguiSuMotoreCandidato(Motore motoreLetto)
        {
            var config = _dataSource.GetConfigurazione();
            var cont_di_Comodo = config.Contatore_di_comodo;
            if (cont_di_Comodo == config.Ogni_N_Pezzi)
            {
                //_dataSource.SetContatoreDiComoodo(cont_di_Comodo - 1);
                _dataSource.SettaPerMotoreTarget(cont_di_Comodo - 1, config.Contatore_del_turno + 1);
                AzioneDaCompiere = AZIONE_DA_SVOLGERE;
                return;
            }
            if (cont_di_Comodo == 1)
            {
                _dataSource.SetContatoreDiComoodo(config.Ogni_N_Pezzi);
                AzioneDaCompiere = DEFAULT_AZIONE;
                return;
            }
            if(cont_di_Comodo < config.Ogni_N_Pezzi)
            {
                _dataSource.SetContatoreDiComoodo(cont_di_Comodo - 1);
                AzioneDaCompiere = DEFAULT_AZIONE;
                return;
            }

        }
        #endregion


    }
}