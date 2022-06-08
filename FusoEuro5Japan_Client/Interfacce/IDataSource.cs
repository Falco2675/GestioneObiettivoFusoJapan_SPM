
namespace FusoEuro5Japan_Client
{
    public interface IDataSource
    {
        Motore GetMotore(string datoRicevuto, TipoDatoRicevuto tipoDatoRicevuto);
        void InserisciDisegni(string disegno);
        bool IsConnessioneDS_Ok();
        Config GetConfigurazione();
        int GetContatoreDiComodo();
        void SetContatoreDiComoodo(int ogni_N_Pezzi);
        void SettaPerMotoreTarget(int v1, int v2);
    }
}