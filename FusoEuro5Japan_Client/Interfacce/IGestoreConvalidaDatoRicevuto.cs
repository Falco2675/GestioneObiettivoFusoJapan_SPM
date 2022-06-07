namespace FusoEuro5Japan_Client
{
    public interface IGestoreConvalidaDatoRicevuto
    {
        void ConvalidaDato(string datoRicevuto);
        TipoDatoRicevuto GetTipoDatoRicevuto( string datoRicevuto);
        //void ConvalidaMatricola(string matricola);
    }
}