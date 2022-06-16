using System;
using System.Drawing;
using System.Threading;
using System.Reflection;

namespace FusoEuro5Japan_Client
{
    public class MainP : BaseP, IMainP
    {
        #region CAMPI PRIVATI
        private readonly Color GREEN_COLOR = Color.Green;
        private readonly Color RED_COLOR = Color.FromArgb(192, 0, 0);

        private readonly IMainV _view;
        private readonly IDataSource _dataSource;
        private readonly IGestoreConfigurazione _gestoreConfigurazione;
        private readonly IGestoreTurni _gestoreTurni;
        private readonly IGestoreContatoriObiettivi _gestoreContatoreObiettivi;
        private readonly IGestoreStrategiaDiSelezione _gestoreStrategiaDiSelezione;
        private readonly ILoginP _loginP;
        private readonly IGestoreConvalidaDatoRicevuto _gestoreConvalidaDatoRicevuto;

        private readonly Timer _timerOrario;
        private DateTime _orario;
        private bool _isAliveDataSource;
        private Motore _motoreLetto;
        private string _errore_string;

        private TipoDatoRicevuto _tipoDatoRicevuto;

        private System.Timers.Timer _timeShowMessage;
        private readonly IStrumentiP _inserimentoDisegni;

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
        public Motore MotoreLetto
        {
            get { return _motoreLetto; }
            set { _motoreLetto = value; Notify(); }
        }

        //Binding con la View
        public string Orario_string => Orario.ToString("HH:mm:ss");
        public string Strategia_string => $"Strategia: {_gestoreStrategiaDiSelezione.NomeStrategia}";
        public string TitoloProduzioneGiornaliera => $"{DateTime.Now.Date.ToString("dd/MM/yyyy")}";

        public string Prod_1T => _gestoreStrategiaDiSelezione.GetProduzioneTurno_string(_gestoreContatoreObiettivi.Prod_1T, _gestoreContatoreObiettivi.Obiettivo_1T);
        public Color ForeColor_Prod_1T => _gestoreContatoreObiettivi.IsTarget_1T_Raggiunto ? GREEN_COLOR : RED_COLOR;
        public Color BackColor_Prod_1T => _gestoreTurni.Turno_enum == TurnoEnum.PrimoTurno ? Color.FromArgb(255, 255, 192) : SystemColors.Control;

        public string Prod_2T => _gestoreStrategiaDiSelezione.GetProduzioneTurno_string(_gestoreContatoreObiettivi.Prod_2T, _gestoreContatoreObiettivi.Obiettivo_2T);
        public Color ForeColor_Prod_2T => _gestoreContatoreObiettivi.IsTarget_2T_Raggiunto ? GREEN_COLOR : RED_COLOR;
        public Color BackColor_Prod_2T => _gestoreTurni.Turno_enum == TurnoEnum.SecondoTurno ? Color.FromArgb(255, 255, 192) : SystemColors.Control;

        public string Prod_3T => _gestoreStrategiaDiSelezione.GetProduzioneTurno_string(_gestoreContatoreObiettivi.Prod_3T, _gestoreContatoreObiettivi.Obiettivo_3T);
        public Color ForeColor_Prod_3T => _gestoreContatoreObiettivi.IsTarget_3T_Raggiunto ? GREEN_COLOR : RED_COLOR;
        public Color BackColor_Prod_3T => _gestoreTurni.Turno_enum == TurnoEnum.TerzoTurno ? Color.FromArgb(255, 255, 192) : SystemColors.Control;

        public string ProduzioneGiornaliera => _gestoreContatoreObiettivi.Contatore_del_giorno.ToString();
        public Color ForeColor_ProduzioneGiorn => _gestoreContatoreObiettivi.IsTarget_GiornalieroRaggiunto ? GREEN_COLOR : RED_COLOR;

        public string AzioneDaCompiere_string => _gestoreStrategiaDiSelezione.AzioneDaCompiere;
        public Color BackColor_Azione => _gestoreStrategiaDiSelezione.Azione_Bool ? GREEN_COLOR : Color.FromArgb(255, 224, 192);
        public Color ForeColor_Azione => _gestoreStrategiaDiSelezione.Azione_Bool ? Color.Yellow : Color.FromArgb(128, 64, 0);

        public string Errore_string
        {
            get { return _errore_string; }
            set { _errore_string = value; Notify(); }
        }

        public Color IsAliveColor => string.IsNullOrEmpty(_gestoreConfigurazione.ExceptionDbConfigurazione) ? Color.Green : Color.Red;
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
               IGestoreStrategiaDiSelezione gestoreStrategiaDiProduzione,
               IStrumentiP inserimentoDisegni,
               ILoginP loginP,
               IGestoreTurni gestoreTurni
            )
        {
            _view = view;
            _dataSource = dataSource;
            _gestoreConfigurazione = gestoreConfigurazione;
            _gestoreConvalidaDatoRicevuto = gestoreConvalidaDatoRicevuto;
            _gestoreContatoreObiettivi = gestoreContatoreObiettivi;
            _gestoreStrategiaDiSelezione = gestoreStrategiaDiProduzione;
            _loginP = loginP;
            _gestoreTurni = gestoreTurni;
            _inserimentoDisegni = inserimentoDisegni;
            MotoreLetto = new Motore();

            _view.SetPresenter(this);


            SottoscriviEventi();

            _timerOrario = new Timer((o) => { Orario = DateTime.Now; }, null, 500, 1000);

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

            _gestoreTurni.TurnoChanged += OnTurnoChanged;

            //_gestoreStrategiaDiSelezione.AzioneDaCompiereChanged += OnAzioneDaCompiereChanged;
            _gestoreStrategiaDiSelezione.StrategiaDiSelezioneChanged += OnStrategiaDiSelezioneChanged;

            _gestoreContatoreObiettivi.Obiettivo_1T_Changed += OnObiettivo_1T_Changed;
            _gestoreContatoreObiettivi.Obiettivo_2T_Changed += OnObiettivo_2T_Changed;
            _gestoreContatoreObiettivi.Obiettivo_3T_Changed += OnObiettivo_3T_Changed;
            _gestoreContatoreObiettivi.Obiettivo_All_Turni_Changed += OnObiettivo_All_Turni_Changed;
            //_gestoreContatoreObiettivi.ProduzioneGiornalieraChanged += OnProduzioneGiornalieraChanged;
            _gestoreContatoreObiettivi.ProduzioniIeriChanged += OnProduzioniIeriChanged;
            _gestoreContatoreObiettivi.Prod_1TChanged += OnProd_1TChanged;
            _gestoreContatoreObiettivi.Prod_2TChanged += OnProd_2TChanged;
            _gestoreContatoreObiettivi.Prod_3TChanged += OnProd_3TChanged;
            _gestoreContatoreObiettivi.Target_Prod_1T_RaggiuntoChanged += OnTarget_Prod_1T_RaggiuntoChanged;
            _gestoreContatoreObiettivi.Target_Prod_2T_RaggiuntoChanged += OnTarget_Prod_2T_RaggiuntoChanged;
            _gestoreContatoreObiettivi.Target_Prod_3T_RaggiuntoChanged += OnTarget_Prod_3T_RaggiuntoChanged;

            _gestoreConfigurazione.ExceptionDBConfigurazioneEvent += OnExceptionDBConfigurazioneEvent;

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
                Reset();
                _gestoreConvalidaDatoRicevuto.ConvalidaMatricola_CodBasamento(stringaRicevuta);
                _tipoDatoRicevuto = _gestoreConvalidaDatoRicevuto.GetTipoDatoRicevuto(stringaRicevuta);
                MotoreLetto = _dataSource.GetMotore(stringaRicevuta, _tipoDatoRicevuto);
                if (MotoreLetto.Matricola == "--")
                    throw new Exception("Dato non conforme! \nLEGGERE MATRICOLA MOTORE o COD. BASAMENTO.");


                if (!MotoreLetto.IsTargetCandidate)
                {
                    _gestoreStrategiaDiSelezione.EseguiNessunaAzione();
                    return;
                }

                _gestoreConfigurazione.EseguiSuTargetCandidato();

            }
            catch (Exception ex)
            {
                Errore_string = ex.Message;
            }

        }
        private void OnResetEvent(object sender, EventArgs e)
        {
            Reset();
        }
        private void OnAvviaStrumentiEvent(object sender, EventArgs e)
        {
            _loginP.ShowView();
            //_inserimentoDisegni.ShowView();
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
            Notify(nameof(ForeColor_Prod_3T));
            Notify(nameof(ProduzioneGiornaliera));
            Notify(nameof(ForeColor_ProduzioneGiorn));
        }
        private void OnProd_2TChanged(object sender, EventArgs e)
        {
            Notify(nameof(Prod_2T));
            Notify(nameof(ForeColor_Prod_2T));
            Notify(nameof(BackColor_Prod_2T));
        }
        private void OnProd_1TChanged(object sender, EventArgs e)
        {
            Notify(nameof(Prod_1T));
            Notify(nameof(ForeColor_Prod_1T));
            Notify(nameof(ProduzioneGiornaliera));
            Notify(nameof(ForeColor_ProduzioneGiorn));
        }
        private void OnProduzioniIeriChanged(object sender, EventArgs e)
        {

        }
        private void OnObiettivo_All_Turni_Changed(object sender, EventArgs e)
        {
            Notify(nameof(ForeColor_ProduzioneGiorn));
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

        private void OnStrategiaDiSelezioneChanged(object sender, EventArgs e)
        {
            Notify(nameof(Strategia_string));
        }

        private void OnExceptionDBConfigurazioneEvent(object sender, EventArgs e)
        {
            Errore_string = _gestoreConfigurazione.ExceptionDbConfigurazione;
        }

        #endregion

        #region METODI PUBBLICI
        private void Reset()
        {
            MotoreLetto = new FusoEuro5Japan_Client.Motore();
            _gestoreStrategiaDiSelezione.ResettaAzione();
            Errore_string = "";

        }

        #endregion

        #region METODI PRIVATI
        #endregion
    }
}
