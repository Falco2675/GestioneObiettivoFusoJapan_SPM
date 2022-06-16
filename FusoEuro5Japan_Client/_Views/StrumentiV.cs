using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace FusoEuro5Japan_Client
{
    public partial class StrumentiV : Form, IStrumentiV
    {
        #region CAMPI PRIVATI
        private BindingSource _bs;
        #endregion

        #region PROPRIETA' PUBBLICHE
        public SynchronizationContext SynchronizeContext => SynchronizationContext.Current;
        public string DisegnoFPT
        {
            get { return txtDisegnoFPT.Text; }
            set { txtDisegnoFPT.Text = value; }
        }

        #endregion

        #region EVENTI
        public event EventHandler AggiungiDisegnoEvent;
        public event EventHandler<StrategiaEnum> StrategiaChanged;
        public event EventHandler SalvaStrategiaEvent;
        #endregion

        #region CTOR
        public StrumentiV()
        {
            InitializeComponent();
            _bs = new BindingSource();
            chkFrequenza.Checked = false;
            chkProdFissa.Checked = false;
        }
        #endregion

        #region SOTTOSCRIZIONE EVENTI
        public void SottoscriviEventi()
        {
        }
        #endregion

        #region GESTIONE EVENTI
        private void InserimentoDisegniV_Load(object sender, EventArgs e)
        {
            txtDisegnoFPT.Focus();
            txtDisegnoFPT.Select();
        }
        private void btnAggiungi_Click(object sender, EventArgs e) => AggiungiDisegno();
        private void lblClose_Click(object sender, EventArgs e) => ChiudiView();
        private void chkProdFissa_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProdFissa.Checked)
            {
                StrategiaChanged?.Invoke(this, StrategiaEnum.ProduzioneTurni);
                pnlFrequenza.Enabled = false;
                pnlProdFissa.Enabled = true;
            }
        }
        private void chkFrequenza_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFrequenza.Checked)
            {
                StrategiaChanged?.Invoke(this, StrategiaEnum.Ogni_N_pezzi);
                pnlProdFissa.Enabled = false;
                pnlFrequenza.Enabled = true;
            }
        }
        private void btnSalvaStrateg_Click(object sender, EventArgs e) => SalvaStrategia();

        #endregion

        #region METODI PUBBLICI
        public void SetBindingSource(BindingSource bs)
        {
            _bs = bs;
            BindingData();
        }
        public void AggiungiDisegnoAElenco(string disegno) => lBoxDisegniInseriti.Items.Insert(0, disegno); 
        public void AggiornaElencoDisegni(List<string> disegni)
        {
            lBoxDisegniInseriti.Items.Clear();
            foreach (var item in disegni)
            {
                lBoxDisegniInseriti.Items.Add(item);
            }
        }

        #endregion

        #region METODI PRIVATI
        private void BindingData()
        {
            txtDisegnoFPT.DataBindings.Add("Text", _bs, "Disegno", false, DataSourceUpdateMode.OnPropertyChanged);
            lblMessaggioDisegni.DataBindings.Add("Text", _bs, "Messaggio");
            btnAggiungiDis.DataBindings.Add("Enabled", _bs, "AbilitaPulsanteAggiungi", false, DataSourceUpdateMode.Never);
            num_Prod1Turno.DataBindings.Add("Text", _bs, "Obiettivo_1T");
            num_Prod2Turno.DataBindings.Add("Text", _bs, "Obiettivo_2T");
            num_Prod3Turno.DataBindings.Add("Text", _bs, "Obiettivo_3T");
            numFrequenza.DataBindings.Add("Text", _bs, "Ogni_N_Pezzi");
            chkProdFissa.DataBindings.Add("Checked", _bs, "IsStartegiaProduzione");
            chkFrequenza.DataBindings.Add("Checked", _bs, "IsStartegiaFrequenza");
        }
        private void AggiungiDisegno() => AggiungiDisegnoEvent?.Invoke(this, null);
        private void SalvaStrategia() => SalvaStrategiaEvent?.Invoke(this, null);
        private void ChiudiView() => this.Close();
        #endregion


    }
}
