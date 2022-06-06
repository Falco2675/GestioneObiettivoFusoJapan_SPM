using System;
using System.Configuration;

namespace FusoEuro5Japan_Client
{
    public class GestoreConfigurazione : IGestoreConfigurazione
    {
        public int IdApp => 2;

        //public bool Permetti_Chiusura_Pedana_Anticipata { get; private set; } = true;
        //public int Num_minimo_per_chiusura_pedana { get; private set; } = 2;
        //public bool Permetti_Predac_DisegnoMisto { get; private set; } = false;

        #region CTOR
        public GestoreConfigurazione()
        {
            LoadConfigurazione();
        }

        private void LoadConfigurazione()
        {
            try
            {
                //Num_minimo_per_chiusura_pedana = Convert.ToInt16(ConfigurationManager.AppSettings["Num_minimo_per_chiusura_pedana"]);
                //if (Num_minimo_per_chiusura_pedana < 1)
                //    Num_minimo_per_chiusura_pedana = 1;
                //if (Num_minimo_per_chiusura_pedana > 3)
                //    Num_minimo_per_chiusura_pedana = 3;

                //Permetti_Chiusura_Pedana_Anticipata = Convert.ToBoolean(ConfigurationManager.AppSettings["Permetti_Chiusura_Pedana_Anticipata"]);
                //if (Permetti_Chiusura_Pedana_Anticipata == false)
                //    Num_minimo_per_chiusura_pedana = 3;

                //Permetti_Predac_DisegnoMisto = Convert.ToBoolean(ConfigurationManager.AppSettings["Permetti_Predac_DisegnoMisto"]);
                
            }
            catch (System.Exception)
            {}

        }
        #endregion


    }
}
