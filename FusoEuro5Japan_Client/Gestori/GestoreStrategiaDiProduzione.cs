using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class GestoreStrategiaDiProduzione : IGestoreStrategiaDiProduzione
    {
        private IStrategia _strategia;
        private readonly IDataSource _dataSource;

        private StrategiaEnum _strategiaEnum;
        private readonly IGestoreTurni _gestoreTurni;

        public StrategiaEnum StrategiaEnum
        {
            get { return _strategiaEnum; }
            set
            {
                if (_strategiaEnum == value) return;
                _strategiaEnum = value;
                switch (StrategiaEnum)
                {
                    case StrategiaEnum.Ogni_N_pezzi:
                        Strategia = new Strategia_Ogni_N_Pezzi(_dataSource);
                        break;
                    case StrategiaEnum.ProduzioneTurni:
                        Strategia = new Strategia_TargetTurno(_dataSource);
                        break;
                    case StrategiaEnum.Non_Definita:
                        Strategia = new Strategia_NonDefinita(_dataSource);
                        break;
                    default:
                        break;
                }
                StrategiaDiProduzioneChanged?.Invoke(this, null);
            }
        }


        public IStrategia Strategia
        {
            get { return _strategia; }
            set
            {
                if (_strategia == value) return;
                _strategia = value;
                StrategiaDiProduzioneChanged?.Invoke(this, null);

            }
        }

        public bool IsMotoreTarget => Strategia.IsMotoreTarget();


        public string NomeStrategia => _strategia.NomeStrategia;

        public string AzioneDaCompiere => Strategia.AzioneDaCompiere;

        public event EventHandler StrategiaDiProduzioneChanged;
        public event EventHandler<string> AzioneDaCompiereChanged;
        //public event EventHandler ObiettivoTurnoRaggiuntoEvent;

        #region CTOR
        public GestoreStrategiaDiProduzione
            (
                IDataSource dataSource,
                IGestoreTurni gestoreTurni
            )
        {
            _dataSource = dataSource;
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

        public string GetProduzioneTurno_string(int prod_1T, int obiettivo_1T)
        {
            return Strategia.GetProduzioneTurno_string(prod_1T, obiettivo_1T);
        }

        public void EseguiSuMotore(Motore motoreLetto, TurnoEnum turno_enum)
        {
            Strategia.EseguiSuMotore(motoreLetto, turno_enum);
        }


        #endregion
    }
}
