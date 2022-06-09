using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class GestoreStrategiaDiProduzione : IGestoreStrategiaDiProduzione
    {
        private IStrategia _strategia;

        public IStrategia Strategia
        {
            get { return _strategia; }
            set
            {
                if (_strategia == value) return;
                _strategia = value;

            }
        }

        public event EventHandler StrategiaDiProduzioneChanged;


    }
}
