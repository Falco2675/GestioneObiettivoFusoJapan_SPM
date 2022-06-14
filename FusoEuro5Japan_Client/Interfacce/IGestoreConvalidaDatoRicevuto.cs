namespace FusoEuro5Japan_Client
{
    public interface IGestoreConvalidaDatoRicevuto
    {
        void ConvalidaMatricola_CodBasamento(string datoRicevuto);
        TipoDatoRicevuto GetTipoDatoRicevuto( string datoRicevuto);
        void ConvalidaDisegno(string v);
        //void ConvalidaMatricola(string matricola);
    }
}