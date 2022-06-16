using System;

namespace FusoEuro5Japan_Client
{
    public class GestoreStrategiaDiSelezione : IGestoreStrategiaDiSelezione
    {
        #region CAMPI PRIVATI
        private IStrategia _strategia;
        private StrategiaEnum _strategiaEnum;
        private readonly IGestoreTurni _gestoreTurni; 
        #endregion

        #region PROPRIETA'
        public StrategiaEnum StrategiaEnum
        {
            get { return _strategiaEnum; }
            private set
            {
                if (_strategiaEnum == value) return;
                _strategiaEnum = value;
                switch (StrategiaEnum)
                {
                    case StrategiaEnum.Ogni_N_pezzi:
                        Strategia = new Strategia_Ogni_N_Pezzi();
                        break;
                    case StrategiaEnum.ProduzioneTurni:
                        Strategia = new Strategia_ProduzioneTurno();
                        break;
                    case StrategiaEnum.Non_Definita:
                        Strategia = new Strategia_NonDefinita();
                        break;
                    default:
                        break;
                }
                StrategiaDiSelezioneChanged?.Invoke(this, null);
            }
        }
        public IStrategia Strategia
        {
            get { return _strategia; }
            set
            {
                if (_strategia == value) return;
                _strategia = value;
                StrategiaDiSelezioneChanged?.Invoke(this, null);

            }
        }
        public bool IsMotoreTarget => Strategia.IsMotoreTarget();
        public string NomeStrategia => _strategia.NomeStrategia;
        public string AzioneDaCompiere => Strategia.AzioneDaCompiere;
        public bool Azione_Bool => Strategia.Azione_Bool; 
        #endregion

        #region EVENTI
        public event EventHandler StrategiaDiSelezioneChanged;
        #endregion

        #region CTOR
        public GestoreStrategiaDiSelezione
            (
                IGestoreTurni gestoreTurni
            )
        {
            _gestoreTurni = gestoreTurni;
            StrategiaEnum = StrategiaEnum.Non_Definita;
            
        }
        #endregion

        #region METODI PUBBLICI
        public void SetStrategia(IGestoreConfigurazione config)
        {
            if (config.Configurazione.Ogni_N_Pezzi > 0 && (config.Configurazione.Obiettivo_1T == 0 && config.Configurazione.Obiettivo_2T == 0 && config.Configurazione.Obiettivo_3T == 0))
            {
                StrategiaEnum = StrategiaEnum.Ogni_N_pezzi;
                Strategia.SetConfigurazione(config);
                return;
            }
            if (config.Configurazione.Ogni_N_Pezzi == 0 && (config.Configurazione.Obiettivo_1T > 0 || config.Configurazione.Obiettivo_2T > 0 || config.Configurazione.Obiettivo_3T > 0))
            {
                StrategiaEnum = StrategiaEnum.ProduzioneTurni;
                Strategia.SetConfigurazione(config);
                return;
            }

            StrategiaEnum = StrategiaEnum.Non_Definita;
            Strategia.SetConfigurazione(config);
        }

        public string GetProduzioneTurno_string(int prod, int obiettivo) 
            => Strategia.GetProduzioneTurno_string(prod, obiettivo);

        public void ResettaAzione()
        {
            Strategia.ResettaAzione();
        }

        public void EseguiNessunaAzione()
        {
            Strategia.EseguiNessunaAzione();
        }


        #endregion
    }
}
