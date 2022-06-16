using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace FusoEuro5Japan_Client
{
    public class StrumentiP : BaseP, IStrumentiP
    {
        #region CAMPI PRIVATI
        private readonly IStrumentiV _view;
        private readonly IGestoreConfigurazione _gestoreConfigurazione;
        private readonly IGestoreDisegni _gestoreDisegni;
        private readonly IGestoreConvalidaDatoRicevuto _gestoreConvalidaDato;
        private readonly IDataSource _dataSource;

        private BindingSource _bs = new BindingSource();

        private List<string> _elencoDisegniInseriti = new List<string>();

        private string _disegno;
        private int _obiettivo_1T;
        private int _obiettivo_2T;
        private int _obiettivo_3T;
        private int _ogni_N_Pezzi;
        private StrategiaEnum _strategia;

        private string _messaggio;
        #endregion

        #region PROPRIETA'
        public override SynchronizationContext SynchronizeContext { get; set; }

        public string Disegno
        {
            get { return _disegno; }
            set { _disegno = value; Notify(); }
        }
        public string Messaggio
        {
            get { return _messaggio; }
            set { _messaggio = value; Notify(); }
        }

        public int Obiettivo_1T
        {
            get { return _obiettivo_1T; }
            set { _obiettivo_1T = value; }
        }
        public int Obiettivo_2T
        {
            get { return _obiettivo_2T; }
            set { _obiettivo_2T = value; }
        }
        public int Obiettivo_3T
        {
            get { return _obiettivo_3T; }
            set { _obiettivo_3T = value; }
        }
        public int Ogni_N_Pezzi
        {
            get { return _ogni_N_Pezzi; }
            set { _ogni_N_Pezzi = value; }
        }

        public StrategiaEnum Strategia
        {
            get { return _strategia; }
            set
            {
                _strategia = value;
                Notify(nameof(IsStartegiaFrequenza));
                Notify(nameof(IsStartegiaProduzione));
            }
        }

        public bool IsStartegiaProduzione => _strategia == StrategiaEnum.ProduzioneTurni;
        public bool IsStartegiaFrequenza => _strategia == StrategiaEnum.Ogni_N_pezzi;

        public bool AbilitaPulsanteAggiungi => !(string.IsNullOrEmpty(Disegno));

        #endregion

        #region CTOR
        public StrumentiP
            (
                IStrumentiV view,
                IDataSource dataSource,
                IGestoreConfigurazione gestoreConfigurazione,
                IGestoreConvalidaDatoRicevuto gestoreConvalidaDato
            )
        {
            _view = view;
            _dataSource = dataSource;
            _gestoreConfigurazione = gestoreConfigurazione;
            _gestoreConvalidaDato = gestoreConvalidaDato;

            SynchronizeContext = _view.SynchronizeContext;
            _bs.DataSource = this;
            _view.SetBindingSource(_bs);

            Strategia = StrategiaEnum.Non_Definita;

            SottoscriviEventi();

        }
        #endregion

        #region SOTTOSCRIZIONE EVENTI
        private void SottoscriviEventi()
        {
            _view.AggiungiDisegnoEvent += OnAggiungiDisegnoEvent;
            _view.SalvaStrategiaEvent += OnSalvaStrategiaEvent;
            _view.StrategiaChanged += OnStrategiaChanged;

        }
        #endregion

        #region GESTORI EVENTI
        private void OnAggiungiDisegnoEvent(object sender, EventArgs e) => AggiungiDisegno();
        private void OnStrategiaChanged(object sender, StrategiaEnum strategia) => Strategia = strategia;
        private void OnSalvaStrategiaEvent(object sender, EventArgs e)
        {
            try
            {
                switch (_strategia)
                {
                    case StrategiaEnum.Ogni_N_pezzi:
                        _dataSource.SetConfig_1_Ogni_N_Pezzi(Ogni_N_Pezzi);
                        break;
                    case StrategiaEnum.ProduzioneTurni:
                        _dataSource.SetConfig_ProduzioneFissa(Obiettivo_1T, Obiettivo_2T, Obiettivo_3T);
                        break;
                    case StrategiaEnum.Non_Definita:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #endregion

        #region METODI PUBBLICI
        public void ShowView()
        {
            ResettaCampi();

            _elencoDisegniInseriti = _dataSource.GetElencoDisegni();
            _view.AggiornaElencoDisegni(_elencoDisegniInseriti);

            Obiettivo_1T = _gestoreConfigurazione.Configurazione.Obiettivo_1T;
            Obiettivo_2T = _gestoreConfigurazione.Configurazione.Obiettivo_2T;
            Obiettivo_3T = _gestoreConfigurazione.Configurazione.Obiettivo_3T;

            Ogni_N_Pezzi = _gestoreConfigurazione.Configurazione.Ogni_N_Pezzi;
            Strategia = _gestoreConfigurazione.StrategiaAdottata;
            Notify(nameof(IsStartegiaFrequenza));
            Notify(nameof(IsStartegiaProduzione));

            _view.ShowDialog();
        }
        #endregion

        #region METODI PRIVATI
        private void AggiungiDisegno()
        {
            try
            {
                Messaggio = "";
                ConvalidaDisegni();
                AggiungiDisegniSuDB();
                Messaggio = "Disegno inserito correttamente.";
                _view.AggiungiDisegnoAElenco(Disegno);

            }
            catch (Exception ex)
            {
                Messaggio = ex.Message;
            }
        }
        private void AggiungiDisegniSuDB()
        {
            _dataSource.InserisciDisegni(Disegno);

            _elencoDisegniInseriti.Add(Disegno);
        }

        private void ConvalidaDisegni()
        {
            _gestoreConvalidaDato.ConvalidaDisegno(Disegno.Trim());
        }
        private void ResettaCampi()
        {
            Disegno = string.Empty;

            Obiettivo_1T = 0;
            Obiettivo_1T = 0;
            Obiettivo_1T = 0;
            Ogni_N_Pezzi = 0;

            Messaggio = string.Empty;
        }
        #endregion

    }
}
