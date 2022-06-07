
namespace FusoEuro5Japan_Client
{
    public interface IDataSource_Liv2
    {
        Motore GetMotore(string datoRicevuto, TipoDatoRicevuto tipoDatoRicevuto);
        void InserisciDisegni(string disegno);
        bool IsConnessioneDS_Ok();
        bool IsMotoreTarget(Motore motoreLetto);
    }
}