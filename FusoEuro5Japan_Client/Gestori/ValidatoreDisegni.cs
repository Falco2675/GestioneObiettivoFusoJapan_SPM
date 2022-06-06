using System;
using System.Linq;

namespace FusoEuro5Japan_Client
{
    public class ValidatoreDisegni : IValidatoreDisegni
    {
        private readonly IGestorePedana _gestorePedana;
        private readonly IDataSource_Liv2 _dataSourceLIV2;

        private Motore _motoreCorrente = new Motore();

        public ValidatoreDisegni
            (
                IGestorePedana gestorePedana,
                IDataSource_Liv2 dataSourceLIV2

            )
        {
            _gestorePedana = gestorePedana;
            _dataSourceLIV2 = dataSourceLIV2;
        }


        public void ConvalidaDisegni(string disegnoFPT, string disegnoTMC)
        {
            ControlloFormatoDisegno(disegnoFPT);
            ControlloFormatoDisegno(disegnoTMC);
        }

        private void ControlloFormatoDisegno(string disegno)
        {
            if (!disegno.All(Char.IsNumber) || (disegno.Length < 9 || disegno.Length > 10))
                throw new Exception($"Disegno '{disegno}' non conforme! ");
        }
        
    }
}
