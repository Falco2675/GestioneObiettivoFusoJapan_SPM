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
    public partial class InserimentoDisegniV : Form, IInserimentoDisegniV
    {
        #region CAMPI PRIVATI
        private IMainP _presenter;
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
        #endregion

        #region CTOR
        public InserimentoDisegniV()
        {
            InitializeComponent();
            _bs = new BindingSource();
            InizializzaDataGrid_DisegniInseriti();
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
        private void btnChiudi_Click(object sender, EventArgs e) => ChiudiView();


        #endregion

        #region METODI PUBBLICI
        public void SetBindingSource(BindingSource bs)
        {
            _bs = bs;
            BindingData();
        }


        #endregion

        #region METODI PRIVATI
        private void BindingData()
        {
            txtDisegnoFPT.DataBindings.Add("Text", _bs, "DisegnoFPT");
            lblMessaggio.DataBindings.Add("Text", _bs, "Messaggio");
            btnAggiungi.DataBindings.Add("Enabled", _bs, "AbilitaPulsanteAggiungi", false, DataSourceUpdateMode.Never);
            dgvDisegniInseriti.DataBindings.Add("DataSource", _bs, "ElencoDisegniInseritiOrdinati");
        }
        private void InizializzaDataGrid_DisegniInseriti()
        {
            dgvDisegniInseriti.AutoGenerateColumns = false;
            dgvDisegniInseriti.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(128, 255, 255);
            dgvDisegniInseriti.EnableHeadersVisualStyles = false;
            dgvDisegniInseriti.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "DATA",
                    DataPropertyName = "DataIns",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                },
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Disegno",
                    DataPropertyName = "Disegno",
                    Width=100,
                    //AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                }
                );
            dgvDisegniInseriti.ClearSelection();
        }

        private void AggiungiDisegno()
        {
            AggiungiDisegnoEvent?.Invoke(this, null);
        }

        private void ChiudiView()
        {
            this.Close();
        }


        #endregion

    }
}
