using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginFPT.Gestori;
using LoginFPT.Models;
using System.Threading;

namespace FusoEuro5Japan_Client
{
    public class LoginP : BaseP, ILoginP
    {
        #region CAMPI PRIVATI
        private readonly ILoginV _view;
        private readonly ILogin_FPT _loginFPT;
        private BindingSource _bs = new BindingSource();
        private readonly IGestoreConfigurazione _gestoreConfigurazioni;

        private string _username;
        private string _password;
        private string _messaggio;
        private bool _isLoginInCorso = false;
        private readonly IInserimentoDisegniP _inserimentoDisegniP;
        #endregion

        #region PROPRIETA'
        public override SynchronizationContext SynchronizeContext { get; set; }

        public string Username
        {
            get { return _username; }
            set { _username = value; Notify(); }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; Notify(); }
        }
        public string Messaggio
        {
            get { return _messaggio; }
            set { _messaggio = value; Notify(); }
        }

        public bool AbilitaPulsanteLogin => ! (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || _isLoginInCorso);

        #endregion

        #region CTOR
        public LoginP
            (
                ILoginV view,    
                ILogin_FPT loginFPT,
                IGestoreConfigurazione gestoreConfigurazioni,
                IInserimentoDisegniP inserimentoDisegniP
            )
        {
            _view = view;
            _loginFPT = loginFPT;
            _gestoreConfigurazioni = gestoreConfigurazioni;
            _inserimentoDisegniP = inserimentoDisegniP;

            SynchronizeContext = _view.SynchronizeContext;
            _bs.DataSource = this;
            _view.SetBindingSource(_bs);

            SottoscriviEventi();

        }

        #endregion

        #region SOTTOSCRIZIONE EVENTI
        private void SottoscriviEventi()
        {
            _view.LoggaUtenteEvent += OnLoggaUtenteEvent;
        }
        #endregion

        #region GESTORI EVENTI
        private void OnLoggaUtenteEvent(object sender, EventArgs e)
        {
            EseguiLogin();
        }

        #endregion

        #region METODI PUBBLICI
        public void ShowView()
        {
           
            Username = string.Empty;
            Password = string.Empty;
            Messaggio = string.Empty;
            _view.Resetta();
            _view.ShowDialog();
            
            
        }
        #endregion

        #region METODI PRIVATI
        private async void EseguiLogin()
        {
            _isLoginInCorso = true;
            Messaggio = "";

            string _ruoloUtente = string.Empty;

            await Task.Run(() =>
            {
                try
                {
                    _ruoloUtente = _loginFPT.GetRuoloUtente(Username, Password, _gestoreConfigurazioni.IdApp);
                }
                catch (Exception)
                {

                    Messaggio = "Nome utente o password errati!";
                    return;
                }

            });

            _isLoginInCorso = false;

            if (!string.IsNullOrEmpty(_ruoloUtente))
            {
                _view.Close();
                _inserimentoDisegniP.ShowView();

            }
            else
            {
                _view.SetPerLoginFallito();
            }


        }
        #endregion

    }
}
