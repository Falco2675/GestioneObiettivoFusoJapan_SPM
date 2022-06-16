namespace FusoEuro5Japan_Client
{
    public class Config
    {
        private int _contatore_di_comodo;

        public int Ogni_N_Pezzi { get; set; }
        public int Obiettivo_1T { get; set; }
        public int Obiettivo_2T { get; set; }
        public int Obiettivo_3T { get; set; }
        public int Obiettivo_Giornaliero { get; set; }

        public int Contatore_di_comodo
        {
            get { return _contatore_di_comodo; }
            set
            {
                if (value == 0)
                {
                    _contatore_di_comodo = Ogni_N_Pezzi;
                    return;
                }
                _contatore_di_comodo = value;
            }
        }

        public int Prod_1T { get; set; }
        public int Prod_2T { get; set; }
        public int Prod_3T { get; set; }

        public string Prod_Ieri { get; set; }

        public int Contatore_del_giorno { get; set; }

    }
}
