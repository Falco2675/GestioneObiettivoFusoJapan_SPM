namespace FusoEuro5Japan_Client
{
    public class Motore
    {
        public string Matricola { get; set; }
        public string Disegno { get; set; }
        public string CodBasamento { get; set; }
        public bool IsTargetCandidate { get; set; }

        #region CTOR
        public Motore()
        {
            Matricola = "--";
            Disegno = "--";
            CodBasamento = "--";
            IsTargetCandidate = false;
        } 
        #endregion

    }
}
