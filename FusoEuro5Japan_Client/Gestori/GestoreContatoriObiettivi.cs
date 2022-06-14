using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class GestoreContatoriObiettivi : IGestoreContatoriObiettivi 
    {

        #region CAMPI PRIVATI
        private readonly IGestoreStrategiaDiSelezione _gestoreStrategiaDiProduzione;
        private readonly IGestoreTurni _gestoreTurni;


        private int _obiettivo_1T;
        private int _obiettivo_2T;
        private int _obiettivo_3T;
        private int _obiettivo_Giornaliero;
        private int _prod_1T;
        private int _prod_2T;
        private int _prod_3T;
        private int _contatore_del_giorno;
        private string _prod_Ieri;

        private bool _isTarget_1T_Raggiunto;
        private bool _isTarget_2T_Raggiunto;
        private bool _isTarget_3T_Raggiunto;
        private bool _isTarget_All_Turni_raggiunto;


        #endregion

        #region PROPRIETA'
        public int Prod_1T
        {
            get { return _prod_1T; }
            set
            {
                //if (_prod_1T == value) return;
                if (IsTarget_GiornalieroRaggiunto || IsTarget_1T_Raggiunto) return;
                _prod_1T = value;
                Prod_1TChanged?.Invoke(this, null);
                //ControllaTargetGiorno();
                AggiornaObiettivi();
                //if(_gestoreStrategiaDiProduzione.StrategiaEnum == StrategiaEnum.ProduzioneTurni)
                //    IsTarget_1T_Raggiunto = _prod_1T >= Obiettivo_1T;


            }
        }
        public int Prod_2T
        {
            get { return _prod_2T; }
            set
            {
                //if (_prod_2T == value) return;
                if (IsTarget_GiornalieroRaggiunto || IsTarget_2T_Raggiunto) return;
                _prod_2T = value;
                Prod_2TChanged?.Invoke(this, null);
                //ControllaTargetGiorno();
                AggiornaObiettivi();
                //if (_gestoreStrategiaDiProduzione.StrategiaEnum == StrategiaEnum.ProduzioneTurni)
                //    IsTarget_2T_Raggiunto = _prod_2T >= Obiettivo_2T;
            }
        }
        public int Prod_3T
        {
            get { return _prod_3T; }
            set
            {
                if (_prod_3T == value) return;
                if (IsTarget_GiornalieroRaggiunto || IsTarget_1T_Raggiunto) return;
                _prod_3T = value;
                Prod_3TChanged?.Invoke(this, null);
                //ControllaTargetGiorno();
                AggiornaObiettivi();
                //if (_gestoreStrategiaDiProduzione.Strategia.TipoStrategia == StrategiaEnum.ProduzioneTurni)
                //    IsTarget_3T_Raggiunto = _prod_3T >= Obiettivo_3T;
            }
        }

        public int Obiettivo_1T
        {
            get { return _obiettivo_1T; }
            set
            {
                if (_obiettivo_1T == value) return;
                _obiettivo_1T = value;
                
                Obiettivo_1T_Changed?.Invoke(this, null);
            }
        }
        public int Obiettivo_2T
        {
            get { return _obiettivo_2T; }
            set
            {
                if (_obiettivo_2T == value) return;
                _obiettivo_2T = value;
                Obiettivo_2T_Changed?.Invoke(this, null);

            }
        }
        public int Obiettivo_3T
        {
            get { return _obiettivo_3T; }
            set
            {
                if (_obiettivo_3T == value) return;
                _obiettivo_3T = value;
                Obiettivo_3T_Changed?.Invoke(this, null);
            }
        }
        public int Obiettivo_Giornaliero
        {
            get { return _obiettivo_Giornaliero; }
            set
            {
                if (_obiettivo_Giornaliero == value) return;
                _obiettivo_Giornaliero = value;
                Obiettivo_All_Turni_Changed?.Invoke(this, null);
            }
        }

        public bool IsObiettivoTurnoRaggiunto
        {
            get
            {
                bool result = false;
                switch (_gestoreTurni.Turno_enum)
                {
                    case TurnoEnum.PrimoTurno:
                        result = IsTarget_1T_Raggiunto;
                        break;
                    case TurnoEnum.SecondoTurno:
                        result = IsTarget_2T_Raggiunto;
                        break;
                    case TurnoEnum.TerzoTurno:
                        result = IsTarget_3T_Raggiunto;
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public bool IsTarget_1T_Raggiunto
        {
            get { return _isTarget_1T_Raggiunto; }
            set
            {
                if (IsTarget_1T_Raggiunto == value) return;
                _isTarget_1T_Raggiunto = value;
                Target_Prod_1T_RaggiuntoChanged?.Invoke(this, null);
            }
        }
        public bool IsTarget_2T_Raggiunto
        {
            get { return _isTarget_2T_Raggiunto; }
            set
            {
                if (_isTarget_2T_Raggiunto == value) return;
                _isTarget_2T_Raggiunto = value;
                Target_Prod_2T_RaggiuntoChanged?.Invoke(this, null);
            }
        }
        public bool IsTarget_3T_Raggiunto
        {
            get { return _isTarget_3T_Raggiunto; }
            set
            {
                if (_isTarget_3T_Raggiunto == value) return;
                _isTarget_3T_Raggiunto = value;
                Target_Prod_3T_RaggiuntoChanged?.Invoke(this, null);
            }
        }

        public bool IsTarget_GiornalieroRaggiunto
        {
            get { return _isTarget_All_Turni_raggiunto; }
            set
            {
                if (_isTarget_All_Turni_raggiunto == value) return;
                _isTarget_All_Turni_raggiunto = value;
                TargetProduzGiornalieraRaggiuntoChanged?.Invoke(this, null);
            }
        }

        public int Contatore_del_giorno => Prod_1T + Prod_2T + Prod_3T;
        //{
        //    get { return _contatore_del_giorno; }
        //    set
        //    {
        //        if (_contatore_del_giorno == value) return;
        //        _contatore_del_giorno = value;
        //        ProduzioneGiornalieraChanged?.Invoke(this, null);
        //    }
        //}


        public string Prod_Ieri
        {
            get { return _prod_Ieri; }
            set
            {
                if (_prod_Ieri == value) return;
                _prod_Ieri = value;
                ProduzioniIeriChanged?.Invoke(this, null);
            }
        }

        #endregion

        #region EVENTI
        public event EventHandler Prod_1TChanged;
        public event EventHandler Prod_2TChanged;
        public event EventHandler Prod_3TChanged;

        public event EventHandler Obiettivo_1T_Changed;
        public event EventHandler Obiettivo_2T_Changed;
        public event EventHandler Obiettivo_3T_Changed;
        public event EventHandler Obiettivo_All_Turni_Changed;

        public event EventHandler Target_Prod_1T_RaggiuntoChanged;
        public event EventHandler Target_Prod_2T_RaggiuntoChanged;
        public event EventHandler Target_Prod_3T_RaggiuntoChanged;
        public event EventHandler TargetProduzGiornalieraRaggiuntoChanged;

        public event EventHandler ProduzioneGiornalieraChanged;
        public event EventHandler ProduzioniIeriChanged;
        #endregion

        #region CTOR
        public GestoreContatoriObiettivi
            (
                IGestoreStrategiaDiSelezione gestoreStrategiaDiProduzione,
                IGestoreTurni gestoreTurni
            )
        {
            _gestoreStrategiaDiProduzione = gestoreStrategiaDiProduzione;
            _gestoreTurni = gestoreTurni;

        }
        #endregion

        #region METODI PUBBLICI
        public void AggiungiAllaProduzione()
        {
            switch (_gestoreTurni.Turno_enum)
            {
                case TurnoEnum.PrimoTurno:
                    Prod_1T += 1;
                    break;
                case TurnoEnum.SecondoTurno:
                    Prod_2T += 1;
                    break;
                case TurnoEnum.TerzoTurno:
                    Prod_3T = Prod_3T + 1;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region METODI PRIVATI
        //private void ControllaTargetGiorno()
        //{
        //    if (_gestoreStrategiaDiProduzione.StrategiaEnum == StrategiaEnum.Non_Definita) return;
        //    Contatore_del_giorno = Prod_1T + Prod_2T + Prod_3T;

        //    IsTarget_GiornalieroRaggiunto = Obiettivo_Giornaliero > 0
        //        ? Obiettivo_Giornaliero == Contatore_del_giorno
        //        : false;
        //}

        public void AggiornaObiettivi()
        {
            Obiettivo_Giornaliero = Obiettivo_1T + Obiettivo_2T + Obiettivo_3T;

            if (Obiettivo_Giornaliero == 0)
            {
                IsTarget_GiornalieroRaggiunto = IsTarget_1T_Raggiunto = IsTarget_2T_Raggiunto = IsTarget_3T_Raggiunto = false;
            }else
            {
                IsTarget_GiornalieroRaggiunto = IsTarget_1T_Raggiunto && IsTarget_2T_Raggiunto && IsTarget_3T_Raggiunto;
                IsTarget_1T_Raggiunto = Prod_1T >= Obiettivo_1T;
                IsTarget_2T_Raggiunto = Prod_2T >= Obiettivo_2T;
                IsTarget_3T_Raggiunto = Prod_3T >= Obiettivo_1T;
            }

        }


        #endregion


    }
}
