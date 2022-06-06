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



        private bool _isAliveDataSource;
        private string _warningMessage;
        private readonly Timer _timerOrario, _timerIsAliveDS;
        private bool _isMultiDisegnoDaConfigurazione;

        private string _orario;
        private System.Timers.Timer _timeShowMessage;
        private readonly IDataSource_Liv2 _dataSourceLIV2;
        private readonly IGestoreConfigurazione _gestoreConfigurazione;
        private readonly ILoginP _loginP;
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
        public Color IsAliveColor => IsAliveDataSource ? Color.Green : Color.Red;
        public Color Color_SfondoMatricoleDisegni { get; private set; }

        public string Orario
        {
            get { return _orario; }
            set
            {
                _orario = value;
                Notify();
            }
        }
        public string WarningMessage
        {
            get { return _warningMessage; }
            set 
            {
                _warningMessage = value;
                if (!string.IsNullOrEmpty(_warningMessage))
                    AvviaTimerShowMessage();

                Notify();
            }
        }
        public string Versione => $"v. {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        #endregion

        #region CTOR
        public MainP
            (
               IMainV view,
               IDataSource_Liv2 dataSourceLIV2,
               IGestoreConfigurazione gestoreConfigurazione

            )
        {
            _view = view;
            //_validatoreDisegni = validatoreDisegni;
            _dataSourceLIV2 = dataSourceLIV2;
            _gestoreConfigurazione = gestoreConfigurazione;

            _view.SetPresenter(this);

            SottoscriviEventi();

            _timerOrario = new Timer((o) => { Orario = DateTime.Now.ToString("HH:mm:ss"); }, null, 500, 1000);
            _timerIsAliveDS = new Timer((o) => { IsAliveDataSource = _dataSourceLIV2.IsConnessioneDS_Ok(); }, null, 500, 15000);
            _timeShowMessage = new System.Timers.Timer();
            _timeShowMessage.Elapsed += _timeShowMessage_Elapsed;
            _timeShowMessage.Interval = 5000;

            Color_SfondoMatricoleDisegni = DEFAULT_COLOR;
            _view.SettaPerDisegnoMisti(_isMultiDisegnoDaConfigurazione);

        }


        #endregion

        #region SOTTOSCRIZIONE EVENTI
        public void SottoscriviEventi()
        {

            _view.StringaRicevutaEvent += OnStringaRicevutaEvent;
            _view.ResetEvent += OnResetEvent;
            _view.AvviaStrumentiEvent += OnAvviaStrumentiEvent;


            //_GestoreCronologiaPedaneCompletate.CronologiaPedaneChanged += OnCronologiaPedaneChanged;
        }


        #endregion

        #region GESTORI EVENTI
        private void _timeShowMessage_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            WarningMessage = "";
            _timeShowMessage.Stop();
        }

        private void OnStringaRicevutaEvent(object sender, string stringaRicevuta)
        {
            WarningMessage = "";
            _timeShowMessage.Stop();
            ElaboraStringaRicevuta(stringaRicevuta);
        }



        private void OnResetEvent(object sender, EventArgs e)
        {
            _view.SettaPerDisegnoMisti(_isMultiDisegnoDaConfigurazione);
            Color_SfondoMatricoleDisegni = DEFAULT_COLOR;
            Notify(nameof(Color_SfondoMatricoleDisegni));
        }

        private void OnAvviaStrumentiEvent(object sender, EventArgs e)
        {
            _loginP.ShowView();
        }


        #endregion

        #region METODI PRIVATI
        private void ElaboraStringaRicevuta(string stringaRicevuta)
        {
            if (string.IsNullOrEmpty(stringaRicevuta)) return;
            if (stringaRicevuta.Trim().ToUpper() == "RESET")
            {
                _view.ResettaCampi();
                return;
            }

            //try
            //{
            //    _validatoreDisegni.ConvalidaMatricola(stringaRicevuta);
            //    _gestorePedana.AggiungiMotore(stringaRicevuta);
            //}
            //catch (Exception ex)
            //{
            //    WarningMessage = ex.Message;
            //}


                
            if (!IsMatricolaValida(stringaRicevuta.Trim()))
            {
                WarningMessage = "Matricola non conforme! \nLEGGERE MATRICOLA MOTORE.";
                return;
            }
            try
            {
                _gestorePedana.ElaboraMatricola(stringaRicevuta);
            }
            catch (Exception ex)
            {
                WarningMessage = ex.Message;
            }
        }
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
