using System;
using System.Windows.Forms;
using System.Threading;

namespace FusoEuro5Japan_Client
{
    public partial class LoginV : Form, ILoginV
    {
        #region CAMPI PRIVATI
        private BindingSource _bs;
        #endregion

        #region PROPRIETA'
        public SynchronizationContext SynchronizeContext => SynchronizationContext.Current;
        #endregion

        #region EVENTI
        public event EventHandler LoggaUtenteEvent;
        #endregion

        #region CTOR
        public LoginV()
        {
            InitializeComponent();
            _bs = new BindingSource();
            txtUsername.Select();
        }
        #endregion

        #region GESTORI EVENTI
        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (!string.IsNullOrEmpty(txtPassw.Text))
                    Login();
                else
                    txtPassw.Select();
            }
        }
        private void txtPassw_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (!string.IsNullOrEmpty(txtUsername.Text))
                    Login();
                else
                    txtUsername.Select();
            }
        }
        private void txt_Enter(object sender, EventArgs e) => (sender as TextBox).SelectAll();
        private void btnLogin_Click(object sender, EventArgs e) => Login();
        private void btnAnnulla_Click(object sender, EventArgs e) => Chiudi();
        private void lblClose_Click(object sender, EventArgs e) => Chiudi();
        #endregion

        #region METODI PUBBLICI
        public void SetBindingSource(BindingSource bs)
        {
            _bs = bs;
            BindingData();
        }
        public void Resetta()
        {
            txtUsername.Clear();
            txtPassw.Clear();
            txtUsername.Enabled = true;
            txtPassw.Enabled = true;
            lblMessaggio.Text = string.Empty;
            txtUsername.Select();
        }
        public void SetPerLoginFallito()
        {
            txtUsername.Enabled = true;
            txtPassw.Enabled = true;
            btnLogin.Enabled = true;
            txtPassw.Focus();
            txtPassw.SelectAll();
        }

        #endregion

        #region METODI PRIVATI
        private void BindingData()
        {
            txtUsername.DataBindings.Add("Text", _bs, "Username", false, DataSourceUpdateMode.OnPropertyChanged);
            txtPassw.DataBindings.Add("Text", _bs, "Password", false, DataSourceUpdateMode.OnPropertyChanged);
            btnLogin.DataBindings.Add("Enabled", _bs, "AbilitaPulsanteLogin");
            lblMessaggio.DataBindings.Add("Text", _bs, "Messaggio", false, DataSourceUpdateMode.OnPropertyChanged);
        }
        private void Login()
        {
            SetPerStartLogin();
            LoggaUtenteEvent?.Invoke(this, null);
        }
        private void SetPerStartLogin()
        {
            txtUsername.Enabled = false;
            txtPassw.Enabled = false;
            btnLogin.Enabled = false;
        }
        private void Chiudi() => this.Close();

        #endregion

    }
}
