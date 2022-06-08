using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Strategia_N_PezziDefinito : Strategia_abs
    {

        public override string Strategia_String => "Obiettivo";
            
        public override string Produzione_String => $"{_gestoreConfigurazione.Configurazione.Contatore_del_turno}/{_gestoreConfigurazione.Configurazione.N_pezzi_definito}";

        #region CTOR
        public Strategia_N_PezziDefinito
           (
               IDataSource dataSource,
               IGestoreConfigurazione gestoreConfigurazione
           ) : base(dataSource, gestoreConfigurazione)
        {

        }

        internal override void EseguiSuMotoreCandidato(Motore motoreLetto)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
