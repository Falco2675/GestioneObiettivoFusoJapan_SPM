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
        private int _obiettivo_1T;
        private int _obiettivo_2T;
        private int _obiettivo_3T;
        private int _obiettivo_All_Turni;
        private int _prod_1T;
        private int _prod_2T;
        private int _prod_3T;
        private int _prod_All_Turni;
        private string _prod_Ieri;

        private bool _isTarget_1T_Raggiunto;
        private bool _isTarget_2T_Raggiunto;
        private bool _isTarget_3T_Raggiunto;
        private bool _isTarget_All_Turni_raggiunto;

        private readonly IGestoreStrategiaDiProduzione _gestoreStrategiaDiProduzione;
        #endregion

        #region PROPRIETA'
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
        public int Obiettivo_All_Turni
        {
            get { return _obiettivo_All_Turni; }
            set
            {
                if (_obiettivo_All_Turni == value) return;
                _obiettivo_All_Turni = value;
                Obiettivo_All_Turni_Changed?.Invoke(this, null);
            }
        }

        public int Prod_1T
        {
            get { return _prod_1T; }
            set
            {
                if (_prod_1T == value) return;
                _prod_1T = value;
                Prod_1TChanged?.Invoke(this, null);
                ControllaTargetGiorno();
                if(_gestoreStrategiaDiProduzione.Strategia.TipoStrategia == StrategiaEnum.ProduzioneTurni)
                    IsTarget_1T_Raggiunto = _prod_1T >= Obiettivo_1T;

            }
        }
        public int Prod_2T
        {
            get { return _prod_2T; }
            set
            {
                if (_prod_2T == value) return;
                _prod_2T = value;
                Prod_2TChanged?.Invoke(this, null);
                ControllaTargetGiorno();
                if (_gestoreStrategiaDiProduzione.Strategia.TipoStrategia == StrategiaEnum.ProduzioneTurni)
                    IsTarget_2T_Raggiunto = _prod_2T >= Obiettivo_2T;
            }
        }
        public int Prod_3T
        {
            get { return _prod_3T; }
            set
            {
                if (_prod_3T == value) return;
                _prod_3T = value;
                Prod_3TChanged?.Invoke(this, null);
                ControllaTargetGiorno();
                if (_gestoreStrategiaDiProduzione.Strategia.TipoStrategia == StrategiaEnum.ProduzioneTurni)
                    IsTarget_3T_Raggiunto = _prod_3T >= Obiettivo_3T;
            }
        }

        public bool IsTarget_1T_Raggiunto
        {
            get { return _isTarget_1T_Raggiunto; }
            set
            {
                if (_isTarget_1T_Raggiunto == value) return;
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
        public bool IsTarget_All_Turni_Raggiunto
        {
            get { return _isTarget_All_Turni_raggiunto; }
            set
            {
                if (_isTarget_All_Turni_raggiunto == value) return;
                _isTarget_All_Turni_raggiunto = value;
                TargetProduzGiornalieraRaggiuntoChanged?.Invoke(this, null);
            }
        }

        public int Prod_All_Turni
        {
            get { return _prod_All_Turni; }
            set
            {
                if (_prod_All_Turni == value) return;
                _prod_All_Turni = value;
                ProduzioneGiornalieraChanged?.Invoke(this, null);
            }
        }
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

        #region Eventi
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

        public GestoreContatoriObiettivi
            (
                IGestoreStrategiaDiProduzione gestoreStrategiaDiProduzione
            )
        {
            _gestoreStrategiaDiProduzione = gestoreStrategiaDiProduzione;
        }


        #region METODI PRIVATI
        private void ControllaTargetGiorno()
        {
            if (_gestoreStrategiaDiProduzione.Strategia.TipoStrategia == StrategiaEnum.Non_Definita) return;

            if (Obiettivo_All_Turni == Prod_1T + Prod_2T + Prod_3T)
                TargetProduzGiornalieraRaggiuntoChanged?.Invoke(this, null);
        }

        #endregion


    }
}
