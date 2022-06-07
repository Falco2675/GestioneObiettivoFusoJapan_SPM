using System;
using System.Linq;

namespace FusoEuro5Japan_Client
{
    public class GestoreConvalidaDatoRicevuto : IGestoreConvalidaDatoRicevuto
    {

        private Motore _motoreCorrente = new Motore();

        public GestoreConvalidaDatoRicevuto ()
        {
        }


        public void ConvalidaDato(string stringaRicevuta)
        {
            if (!stringaRicevuta.All(Char.IsNumber) && !(stringaRicevuta.Length == 7 || stringaRicevuta.Length == 15))
                throw new Exception($"Dato non conforme! \nLEGGERE MATRICOLA MOTORE o COD. BASAMENTO.");
        }

        public TipoDatoRicevuto GetTipoDatoRicevuto(string datoRicevuto)
        {
            if (datoRicevuto.Length == 7)
                return TipoDatoRicevuto.Matricola;
            if (datoRicevuto.Length == 15)
                return TipoDatoRicevuto.CodBasamento;
            return TipoDatoRicevuto.Sconosciuto;
        }

        
    }
}
