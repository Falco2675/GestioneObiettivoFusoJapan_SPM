using System;

namespace FusoEuro5Japan_Client
{
    public class Motore
    {
        public string Matricola { get; set; }
        public string Disegno { get; set; }
        public string CodBasamento { get; set; }
        public bool IsTargetCandidate { get; set; }

        public Motore()
        {
        }


        public void Set(Motore motoreFuso)
        {
            Matricola = motoreFuso.Matricola;
            Disegno = motoreFuso.Disegno;
            CodBasamento = motoreFuso.CodBasamento;
        }
    }
}
