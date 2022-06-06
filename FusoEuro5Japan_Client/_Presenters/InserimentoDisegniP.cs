using Predac._Views;
using Predac.Gestori;
using Predac.Models;
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

        private string _disegnoFPT;
        private string _disegnoTMC;
        private bool _isPerRicambi;

        private string _messaggio;
        private BindingList<DisegnoFPT_TMC> _elencoDisegniInseriti = new BindingList<DisegnoFPT_TMC>();
        private readonly IGestoreDisegni _gestoreDisegni;
        private readonly IValidatoreDisegni _validatoreDisegni;

        #endregion

        #region PROPRIETA'
        public override SynchronizationContext SynchronizeContext { get; set; }

        public string DisegnoFPT
        {
            get { return _disegnoFPT; }
            set { _disegnoFPT = value; Notify(); }
        }
        public string DisegnoTMC
        {
            get { return _disegnoTMC; }
            set { _disegnoTMC = value; Notify(); }
        }
        public bool IsPerRicambi
        {
            get { return _isPerRicambi; }
            set { _isPerRicambi = value; Notify(); }
        }

        public string Messaggio
        {
            get { return _messaggio; }
            set { _messaggio = value; Notify(); }
        }

        public BindingList<DisegnoFPT_TMC> ElencoDisegniInseriti
        {
            get { return _elencoDisegniInseriti; }
            set { _elencoDisegniInseriti = value; }
        }

        public List<DisegnoFPT_TMC> ElencoDisegniInseritiOrdinati => ElencoDisegniInseriti.OrderByDescending(x => x.DataIns).ToList();
        public bool AbilitaPulsanteAggiungi => !(string.IsNullOrEmpty(DisegnoFPT) && string.IsNullOrEmpty(DisegnoTMC));

        #endregion

        #region CTOR
        public InserimentoDisegniP
            (
                IInserimentoDisegniV view,
                IGestoreDisegni gestoreDisegni,
                IValidatoreDisegni validatoreDisegni
            )
        {
            _view = view;
            _gestoreDisegni = gestoreDisegni;
            _validatoreDisegni = validatoreDisegni;

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
            _validatoreDisegni.ConvalidaDisegni(DisegnoFPT.Trim(), DisegnoTMC.Trim());
        }
        private void AggiungiDisegniSuDB()
        {
            var disegnoFPT_TMC = new DisegnoFPT_TMC
            {
                DisegnoFPT = DisegnoFPT.Trim(),
                DisegnoTMC = DisegnoTMC.Trim(),
                IsPerRicambi = IsPerRicambi,
                DataIns = DateTime.Now
            };

            _gestoreDisegni.AggiungiDisegno(disegnoFPT_TMC);

            ElencoDisegniInseriti.Add(disegnoFPT_TMC);

        }
        private void ResettaCampi()
        {
            DisegnoFPT = string.Empty;
            DisegnoTMC = string.Empty;
            IsPerRicambi = false;

            Messaggio = string.Empty;
            Notify(nameof(ElencoDisegniInseritiOrdinati));
        }

        #endregion

    }
}
