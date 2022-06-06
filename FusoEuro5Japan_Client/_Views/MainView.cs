using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FusoEuro5Japan_Client
{
    public partial class MainView : Form
    {

        #region EVENTI
        public event EventHandler<string> StringaRicevutaEvent;
        #endregion


        #region CTOR
        public MainView()
        {
            InitializeComponent();
        }
        #endregion

        #region GESTORI EVENTI
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
        #endregion
    }
}
