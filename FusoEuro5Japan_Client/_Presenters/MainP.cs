using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Reflection;
using LoginFPT;

namespace FusoEuro5Japan_Client
{
    public class MainP : BaseP, IMainP
    {
        #region CAMPI PRIVATI
        private readonly Color DEFAULT_COLOR = SystemColors.ButtonShadow;
        private readonly Color COLORE_RICAMBI = Color.SandyBrown;

        private readonly IMainV _view;
        private readonly IDataSource _dataSource;
        private readonly IGestoreConfigurazione _gestoreConfigurazione;
        private readonly ILoginP _loginP;
        private readonly IGestoreConvalidaDatoRicevuto _gestoreConvalidaDatoRicevuto;
        private IStrategia _strategia;

        private readonly Timer _timerOrario, _timerIsAliveDS;
        private DateTime _orario;
        private bool _isAliveDataSource;
        //private string _strategia_string;
        private string _produzione_string;
        private string _dataProduzione;
        private string _produzioneGiornaliera;

        //private string _warningMessage;
        private Motore _motoreLetto;
        private Config _configurazione;
        private string _azioneDaCompiere;
        private string _errore_string;


        //private readonly Timer _timerOrario, _timerIsAliveDS;
        private TipoDatoRicevuto _tipoDatoRicevuto;

        private System.Timers.Timer _timeShowMessage;

        //private readonly IValidatoreDisegni _validatoreDisegni;

        //private string _datoLetto;
        #endregion

        #region PROPRIETA' PUBBLICHE

        public IMainV GetView => _view;
        
        public override SynchronizationContext SynchronizeContext { get; set; }
        public bool IsAliveDataSource
        {
            get { return _isAliveDataSource; }
            set { _isAliveDataSource = value; Notify(); }
        }
        public DateTime Orario
        {
            get { return _orario; }
            private set
            {
                _orario = value;
                _gestoreTurni.ControllaTurno();
                Notify(nameof(Orario_string));
            }
        }
        public string Orario_string => Orario.ToString("HH:mm:ss");
        public Motore MotoreLetto
        {
            get { return _motoreLetto; }
            set { _motoreLetto = value; Notify(); }
        }
        public string Strategia_string => $"{_strategia.Strategia_String}\n{_gestoreTurni.Turno_string}";
        public string ProduzioneTurno => _strategia.Produzione_String;
        public string ProduzioneGiornaliera => _gestoreConfigurazione.ContatoreGiorno_string;
        public string TitoloProduzioneGiornaliera => $"PRODUZIONE DEL\n{DateTime.Now.Date.ToString("dd/MM/yyyy")}";
        private Color _backColor_ProdTurno;

        public Color BackColor_ProdTurno
        {
            get { return _backColor_ProdTurno; }
            set { _backColor_ProdTurno = value; Notify(); }
        }


        public string Errore_string
        {
            get { return _errore_string; }
            set { _errore_string = value; Notify(); }
        }

        public string AzioneDaCompiere_string => _strategia.AzioneDaCompiere;
        //{
        //    get { return _azioneDaCompiere; }
        //    set
        //    {
        //        _azioneDaCompiere = value;
        //        if (!string.IsNullOrEmpty(_azioneDaCompiere))
        //            AvviaTimerShowMessage();
        //        Notify();
        //    }
        //}
        public Color IsAliveColor => IsAliveDataSource ? Color.Green : Color.Red;
        public string Versione => $"v. {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";


        public Config Configurazione
        {
            get { return _configurazione; }
            set
            {
                var old_Ogni_N_pezzi = _configurazione.Ogni_N_Pezzi;
                var old_Ogni_N_pezzi_definito = _configurazione.N_pezzi_definito;
                _configurazione = value;

                if ((_configurazione.Ogni_N_Pezzi != old_Ogni_N_pezzi) || (_configurazione.N_pezzi_definito != old_Ogni_N_pezzi_definito))
                {
                    Notify(nameof(Strategia_string));
                    Notify(nameof(ProduzioneTurno));
                }

            }
        }

        private readonly IGestoreAzioniDaCompiere _gestoreAzioniDaCompiere;
        private readonly IGestoreTurni _gestoreTurni;

        #endregion

        #region CTOR
        public MainP
            (
               IMainV view,
               IDataSource dataSource,
               IGestoreConfigurazione gestoreConfigurazione,
               IGestoreConvalidaDatoRicevuto gestoreConvalidaDatoRicevuto,
               //IGestoreAzioniDaCompiere gestoreAzioniDaCompiere,
               ILoginP loginP,
               IGestoreTurni gestoreTurni
            )
        {
            _view = view;
            _dataSource = dataSource;
            _gestoreConfigurazione = gestoreConfigurazione;
            _gestoreConvalidaDatoRicevuto = gestoreConvalidaDatoRicevuto;
            _loginP = loginP;
            _gestoreTurni = gestoreTurni;
            _strategia = new Strategia_NonDefinita(_dataSource, _gestoreConfigurazione);
            MotoreLetto = new Motore();

            _view.SetPresenter(this);

            
            SottoscriviEventi();

            _timerOrario = new Timer((o) => { Orario = DateTime.Now; }, null, 500, 1000);

            //_timerOrario = new Timer((o) => { Orario = DateTime.Now.ToString("HH:mm:ss"); }, null, 500, 1000);
            //_timerIsAliveDS = new Timer((o) => { IsAliveDataSource = _dataSource.IsConnessioneDS_Ok(); }, null, 500, 15000);
            _timeShowMessage = new System.Timers.Timer();
            _timeShowMessage.Elapsed += _timeShowMessage_Elapsed;
            _timeShowMessage.Interval = 5000;

        }


        #endregion

        #region SOTTOSCRIZIONE EVENTI
        public void SottoscriviEventi()
        {
            _view.StringaRicevutaEvent += OnStringaRicevutaEvent;
            _view.ResetEvent += OnResetEvent;
            _view.AvviaStrumentiEvent += OnAvviaStrumentiEvent;
            _gestoreConfigurazione.StrategiaChanged += OnCambioStrategiaChanged;
            _gestoreConfigurazione.ContatoriChanged += OnContatoreDelTurnoChanged;
            _strategia.AzioneDaCompiereChanged += OnAzioneDaCompiereChanged;
            _strategia.ObiettivoTurnoRaggiuntoEvent += OnObiettivoTurnoRaggiuntoEvent;

            _gestoreTurni.TurnoChanged += OnTurnoChanged;
        }

        #endregion

        #region GESTORI EVENTI
        private void _timeShowMessage_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timeShowMessage.Stop();
        }
        private void OnStringaRicevutaEvent(object sender, string stringaRicevuta)
        {
            _timeShowMessage.Stop();
            try
            {
                _gestoreConvalidaDatoRicevuto.ConvalidaDato(stringaRicevuta);
                _tipoDatoRicevuto = _gestoreConvalidaDatoRicevuto.GetTipoDatoRicevuto(stringaRicevuta);
                MotoreLetto = _dataSource.GetMotore(stringaRicevuta, _tipoDatoRicevuto);
                if (string.IsNullOrEmpty(MotoreLetto.Matricola))
                    throw new Exception("Dato non conforme! \nLEGGERE MATRICOLA MOTORE o COD. BASAMENTO.");


                _strategia.EseguiSuMotore(MotoreLetto);
                
            }
            catch (Exception ex)
            {
                Errore_string = ex.Message;
            }

        }
        private void OnResetEvent(object sender, EventArgs e)
        {
            //Color_SfondoMatricoleDisegni = DEFAULT_COLOR;
            //Notify(nameof(Color_SfondoMatricoleDisegni));
        }
        private void OnAvviaStrumentiEvent(object sender, EventArgs e)
        {
            _loginP.ShowView();
        }
        private void OnContatoreDelTurnoChanged(object sender, EventArgs e)
        {
            Notify(nameof(ProduzioneTurno));
            Notify(nameof(ProduzioneGiornaliera));
        }
        private void OnCambioStrategiaChanged(object sender, EventArgs e)
        {
            switch (_gestoreConfigurazione.StrategiaAdottata)
            {
                case StrategiaEnum.Ogni_N_pezzi:
                    _strategia = new Strategia_Ogni_N_Pezzi(_dataSource, _gestoreConfigurazione);
                    break;
                case StrategiaEnum.N_Pezzi_Definito:
                    _strategia = new Strategia_TargetTurno(_dataSource, _gestoreConfigurazione);
                    break;
                case StrategiaEnum.Non_Definita:
                    _strategia = new Strategia_NonDefinita(_dataSource, _gestoreConfigurazione);
                    break;
                default:
                    break;
            }
            Notify(nameof(Strategia_string));
        }
        private void OnTurnoChanged(object sender, EventArgs e)
        {
            Notify(nameof(Strategia_string));
        }
        private void OnAzioneDaCompiereChanged(object sender, string e)
        {
            Notify(nameof(AzioneDaCompiere_string));
        }
        private void OnObiettivoTurnoRaggiuntoEvent(object sender, EventArgs e)
        {
            BackColor_ProdTurno = Color.Green;
        }


        #endregion

        #region METODI PRIVATI
        //private void ElaboraStringaRicevuta(string stringaRicevuta)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //        WarningMessage = ex.Message;
        //    }



        //    if (string.IsNullOrEmpty(stringaRicevuta)) return;
        //    if (stringaRicevuta.Trim().ToUpper() == "RESET")
        //    {
        //        _view.ResettaCampi();
        //        return;
        //    }

        //    //try
        //    //{
        //    //    _validatoreDisegni.ConvalidaMatricola(stringaRicevuta);
        //    //    _gestorePedana.AggiungiMotore(stringaRicevuta);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    WarningMessage = ex.Message;
        //    //}



        //    if (!IsMatricolaValida(stringaRicevuta.Trim()))
        //    {
        //        WarningMessage = "Dato non conforme! \nLEGGERE MATRICOLA MOTORE o COD. BASAMENTO.";
        //        return;
        //    }
        //    try
        //    {
        //        _gestorePedana.ElaboraMatricola(stringaRicevuta);
        //    }
        //    catch (Exception ex)
        //    {
        //        WarningMessage = ex.Message;
        //    }
        //}
        private bool IsMatricolaValida(string matricola)
        {
            return
                matricola.All(Char.IsNumber) &&
                matricola.Length == 7 ;
        }
        private void AvviaTimerShowMessage()
        {
            _timeShowMessage.Stop();
            _timeShowMessage.Start();
        }



        #endregion
    }
}
