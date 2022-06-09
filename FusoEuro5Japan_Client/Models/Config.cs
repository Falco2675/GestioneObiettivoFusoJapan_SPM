using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class Config
    {
        public int Ogni_N_Pezzi { get; set; }
        public int Obiettivo_1T { get; set; }
        public int Obiettivo_2T { get; set; }
        public int Obiettivo_3T { get; set; }

        public int Contatore_di_comodo { get; set; }

        public int Prod_1T { get; set; }
        public int Prod_2T { get; set; }
        public int Prod_3T { get; set; }

        public int Prod_1T_ieri { get; set; }
        public int Prod_2T_ieri { get; set; }
        public int Prod_3T_ieri { get; set; }

        public int Contatore_del_giorno { get; set; }

    }
}
