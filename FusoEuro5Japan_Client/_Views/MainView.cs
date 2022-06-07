using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FusoEuro5Japan_Client
{
    public partial class MainView : Form, IMainV
    {
        private IMainP _presenter;

        #region EVENTI
        public event EventHandler<string> StringaRicevutaEvent;
        public event EventHandler ResetEvent;
        public event EventHandler AvviaStrumentiEvent;
        #endregion

        #region CTOR
        public MainView()
        {
            InitializeComponent();
        }
        #endregion

        #region GESTORI EVENTI
        private void MainView_Load(object sender, EventArgs e)
        {
            txtDato.Focus();
            ResettaCampi();
        }
        private void txtDato_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                StringaRicevutaEvent?.Invoke(this, txtDato.Text);
                txtDato.Clear();
            }
        }

        private void txtDato_Leave(object sender, EventArgs e)
        {
            txtDato.Focus();
        }
        private void pBoxStrumenti_Click(object sender, EventArgs e)
        {
            AvviaStrumentiEvent?.Invoke(this, null);
        }


        #endregion


        #region METODI PUBBLICI
        public void SetPresenter(IMainP presenter)
        {
            _presenter = presenter;
            _presenter.SynchronizeContext = SynchronizationContext.Current;
            BindingData();
        }


        public void ResettaCampi()
        {
            lblMatricola.Text = "--";
            lblDisegno.Text = "--";
            lblCodBasamento.Text = "--";
            txtDato.Clear();
            txtDato.Focus();
            ResetEvent?.Invoke(this, null);
        }

        #endregion

        #region METODI PRIVATI
        private void BindingData()
        {
            lblOrario.DataBindings.Add("Text", _presenter, "Orario");
            lblMatricola.DataBindings.Add("Text", _presenter, "Matricola");
            lblDisegno.DataBindings.Add("Text", _presenter, "Disegno");
            lblCodBasamento.DataBindings.Add("Text", _presenter, "CodBasamento");
            lblAzione.DataBindings.Add("Text", _presenter, "AzioneDaCompiere_string");
            lblConnessioneDS.DataBindings.Add("BackColor", _presenter, "IsAliveColor");
            lblVersion.DataBindings.Add("Text", _presenter, "Versione");
        }
        #endregion

    }
}
