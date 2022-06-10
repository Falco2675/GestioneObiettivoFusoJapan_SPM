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
        private readonly Color GREEN_COLOR = Color.FromArgb(192, 0, 0);
        private readonly Color RED_COLOR = Color.Green;

        private readonly IMainV _view;
        private readonly IDataSource _dataSource;
        private readonly IGestoreConfigurazione _gestoreConfigurazione;
        private readonly IGestoreAzioniDaCompiere _gestoreAzioniDaCompiere;
        private readonly IGestoreTurni _gestoreTurni;
        private readonly IGestoreContatoriObiettivi _gestoreContatoreObiettivi;
        private readonly IGestoreStrategiaDiProduzione _gestoreStrategiaDiProduzione;
        private readonly ILoginP _loginP;
        private readonly IGestoreConvalidaDatoRicevuto _gestoreConvalidaDatoRicevuto;

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

        private Color _backColor_Prod_1T;
        private Color _backColor_Prod_2T;
        private Color _backColor_Prod_3T;

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

        //Binding con la View
        public string Strategia_string => $"Strategia: {_gestoreStrategiaDiProduzione.NomeStrategia}";
        public string TitoloProduzioneGiornaliera => $"{DateTime.Now.Date.ToString("dd/MM/yyyy")}";

        public string Prod_1T => _gestoreStrategiaDiProduzione.GetProduzioneTurno_string(_gestoreContatoreObiettivi.Prod_1T, _gestoreContatoreObiettivi.Obiettivo_1T);
        public Color ForeColor_Prod_1T => _gestoreContatoreObiettivi.IsTarget_1T_Raggiunto ? GREEN_COLOR : RED_COLOR;
        public string Prod_2T => _gestoreStrategiaDiProduzione.GetProduzioneTurno_string(_gestoreContatoreObiettivi.Prod_2T, _gestoreContatoreObiettivi.Obiettivo_2T);
        public Color ForeColor_Prod_2T => _gestoreContatoreObiettivi.IsTarget_2T_Raggiunto ? GREEN_COLOR : RED_COLOR;
        public string Prod_3T => _gestoreStrategiaDiProduzione.GetProduzioneTurno_string(_gestoreContatoreObiettivi.Prod_3T, _gestoreContatoreObiettivi.Obiettivo_3T);
        public Color ForeColor_Prod_3T => _gestoreContatoreObiettivi.IsTarget_3T_Raggiunto ? GREEN_COLOR : RED_COLOR;

        public string ProduzioneGiornaliera => _gestoreContatoreObiettivi.Contatore_del_giorno.ToString();
        public Color ForeColor_Prod_All_Turni => _gestoreContatoreObiettivi.IsTarget_All_Turni_Raggiunto ? GREEN_COLOR : RED_COLOR;

        public string AzioneDaCompiere_string => _gestoreStrategiaDiProduzione.AzioneDaCompiere;

        public string Errore_string
        {
            get { return _errore_string; }
            set { _errore_string = value; Notify(); }
        }
        
        public Color IsAliveColor => IsAliveDataSource ? Color.Green : Color.Red;
        public string Versione => $"v. {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        // Fine Binding con la View

        #endregion

        #region CTOR
        public MainP
            (
               IMainV view,
               IDataSource dataSource,
               IGestoreConfigurazione gestoreConfigurazione,
               IGestoreConvalidaDatoRicevuto gestoreConvalidaDatoRicevuto,
               IGestoreContatoriObiettivi gestoreContatoreObiettivi,
               IGestoreStrategiaDiProduzione gestoreStrategiaDiProduzione,
               //IGestoreAzioniDaCompiere gestoreAzioniDaCompiere,
               ILoginP loginP,
               IGestoreTurni gestoreTurni
            )
        {
            _view = view;
            _dataSource = dataSource;
            _gestoreConfigurazione = gestoreConfigurazione;
            _gestoreConvalidaDatoRicevuto = gestoreConvalidaDatoRicevuto;
            _gestoreContatoreObiettivi = gestoreContatoreObiettivi;
            _gestoreStrategiaDiProduzione = gestoreStrategiaDiProduzione;
            _loginP = loginP;
            _gestoreTurni = gestoreTurni;
            //_strategia = new Strategia_NonDefinita(_dataSource, _gestoreConfigurazione);
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
            //_gestoreConfigurazione.ContatoriChanged += OnContatoreDelTurnoChanged;
            _gestoreStrategiaDiProduzione.AzioneDaCompiereChanged += OnAzioneDaCompiereChanged;
            //_strategia.ObiettivoTurnoRaggiuntoEvent += OnObiettivoTurnoRaggiuntoEvent;

            _gestoreTurni.TurnoChanged += OnTurnoChanged;

            _gestoreStrategiaDiProduzione.StrategiaDiProduzioneChanged += OnStrategiaDiProduzioneChanged;

            _gestoreContatoreObiettivi.Obiettivo_1T_Changed += OnObiettivo_1T_Changed;
            _gestoreContatoreObiettivi.Obiettivo_2T_Changed += OnObiettivo_2T_Changed;
            _gestoreContatoreObiettivi.Obiettivo_3T_Changed += OnObiettivo_3T_Changed;
            _gestoreContatoreObiettivi.Obiettivo_All_Turni_Changed += OnObiettivo_All_Turni_Changed;
            _gestoreContatoreObiettivi.ProduzioneGiornalieraChanged += OnProduzioneGiornalieraChanged;
            _gestoreContatoreObiettivi.ProduzioniIeriChanged += OnProduzioniIeriChanged;
            _gestoreContatoreObiettivi.Prod_1TChanged += OnProd_1TChanged;
            _gestoreContatoreObiettivi.Prod_2TChanged += OnProd_2TChanged;
            _gestoreContatoreObiettivi.Prod_3TChanged += OnProd_3TChanged;
            _gestoreContatoreObiettivi.Target_Prod_1T_RaggiuntoChanged += OnTarget_Prod_1T_RaggiuntoChanged;
            _gestoreContatoreObiettivi.Target_Prod_2T_RaggiuntoChanged += OnTarget_Prod_2T_RaggiuntoChanged;
            _gestoreContatoreObiettivi.Target_Prod_3T_RaggiuntoChanged += OnTarget_Prod_3T_RaggiuntoChanged;
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

                //if(!MotoreLetto.IsTargetCandidate)

                _gestoreConfigurazione.AggiornaConfigurazione();
                
                //_gestoreStrategiaDiProduzione.EseguiSuMotore(MotoreLetto, _gestoreTurni.Turno_enum);
                if(_gestoreStrategiaDiProduzione.IsMotoreTarget)

                
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
        private void OnTurnoChanged(object sender, EventArgs e)
        {
            Notify(nameof(Strategia_string));
        }
        private void OnAzioneDaCompiereChanged(object sender, string e)
        {
            Notify(nameof(AzioneDaCompiere_string));
        }

        private void OnTarget_Prod_3T_RaggiuntoChanged(object sender, EventArgs e)
        {
            Notify(nameof(ForeColor_Prod_3T));
        }
        private void OnTarget_Prod_2T_RaggiuntoChanged(object sender, EventArgs e)
        {
            Notify(nameof(ForeColor_Prod_2T));
        }
        private void OnTarget_Prod_1T_RaggiuntoChanged(object sender, EventArgs e)
        {
            Notify(nameof(ForeColor_Prod_1T));
        }
        private void OnProd_3TChanged(object sender, EventArgs e)
        {
            Notify(nameof(Prod_3T));
        }
        private void OnProd_2TChanged(object sender, EventArgs e)
        {
            Notify(nameof(Prod_2T));
        }
        private void OnProd_1TChanged(object sender, EventArgs e)
        {
            Notify(nameof(Prod_1T));
        }
        private void OnProduzioniIeriChanged(object sender, EventArgs e)
        {
            
        }
        private void OnProduzioneGiornalieraChanged(object sender, EventArgs e)
        {
            Notify(nameof(ProduzioneGiornaliera));
        }
        private void OnObiettivo_All_Turni_Changed(object sender, EventArgs e)
        {
            Notify(nameof(ForeColor_Prod_All_Turni));
        }
        private void OnObiettivo_3T_Changed(object sender, EventArgs e)
        {
            Notify(nameof(ForeColor_Prod_3T));
        }
        private void OnObiettivo_2T_Changed(object sender, EventArgs e)
        {
            Notify(nameof(ForeColor_Prod_2T));
        }
        private void OnObiettivo_1T_Changed(object sender, EventArgs e)
        {
            Notify(nameof(ForeColor_Prod_1T));
        }

        private void OnStrategiaDiProduzioneChanged(object sender, EventArgs e)
        {
            Notify(nameof(Strategia_string));
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
