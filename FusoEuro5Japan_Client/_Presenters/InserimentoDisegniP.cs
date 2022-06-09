using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FusoEuro5Japan_Client
{
    public class InserimentoDisegniP : BaseP, IInserimentoDisegniP
    {
        #region CAMPI PRIVATI
        private readonly IInserimentoDisegniV _view;
        private BindingSource _bs = new BindingSource();

        private string _disegno;

        private string _messaggio;
        private readonly IGestoreDisegni _gestoreDisegni;
        private readonly IGestoreConvalidaDatoRicevuto _validatoreDisegni;
        private readonly IDataSource _dataSource;

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

        private List<string> _elencoDisegniInseriti;

        public List<string> ElencoDisegniInseriti
        {
            get { return _elencoDisegniInseriti; }
            set { _elencoDisegniInseriti = value; Notify(); }
        }


        public bool AbilitaPulsanteAggiungi => !(string.IsNullOrEmpty(Disegno));

        #endregion

        #region CTOR
        public InserimentoDisegniP
            (
                IInserimentoDisegniV view,
                IDataSource dataSource
            )
        {
            _view = view;
            _dataSource = dataSource;

            SynchronizeContext = _view.SynchronizeContext;
            _bs.DataSource = this;
            _view.SetBindingSource(_bs);

            SottoscriviEventi();

        }
        #endregion

        #region SOTTOSCRIZIONE EVENTI
        private void SottoscriviEventi()
        {
            _view.AggiungiDisegnoEvent += OnAggiungiDisegnoEvent;
        }

        #endregion

        #region GESTORI EVENTI
        private void OnAggiungiDisegnoEvent(object sender, EventArgs e) => AggiungiDisegni();
        #endregion

        #region METODI PUBBLICI
        public void ShowView()
        {
            ResettaCampi();
            _view.ShowDialog();
        }
        #endregion

        #region METODI PRIVATI
        private void AggiungiDisegni()
        {
            try
            {
                ConvalidaDisegni();
                AggiungiDisegniSuDB();
                ResettaCampi();

            }
            catch (Exception ex)
            {
                Messaggio = ex.Message;
            }
        }

        private void ConvalidaDisegni()
        {
            _validatoreDisegni.ConvalidaDato(Disegno.Trim());
        }
        private void AggiungiDisegniSuDB()
        {
            
            _dataSource.InserisciDisegni(Disegno);

            ElencoDisegniInseriti.Add(Disegno);

        }
        private void ResettaCampi()
        {
            Disegno = string.Empty;

            Messaggio = string.Empty;
            Notify(nameof(ElencoDisegniInseriti));
        }

        #endregion

    }
}
