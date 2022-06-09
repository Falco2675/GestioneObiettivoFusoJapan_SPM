using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class GestoreAzioniDaCompiere  : IGestoreAzioniDaCompiere
    {
        private readonly IDataSource _dataSource;

        #region CTOR
        public GestoreAzioniDaCompiere
            (
                IDataSource dataSource
            )
        {
            _dataSource = dataSource;
        }
        #endregion

        #region METODI
        //public string GetAzioniDaCompiere(Motore motoreLetto)
        //{
        //    if (_dataSource.IsMotoreTarget(motoreLetto))
        //    {
        //        return "Effettuare PROVA A CALDO.";
        //    }

        //    return "Nessuna azione da eseguire.";
        //}
        #endregion


    }
}
