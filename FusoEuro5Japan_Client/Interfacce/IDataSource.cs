
namespace FusoEuro5Japan_Client
{
    public interface IDataSource
    {
        Motore GetMotore(string datoRicevuto, TipoDatoRicevuto tipoDatoRicevuto);
        void InserisciDisegni(string disegno);
        bool IsConnessioneDS_Ok();
        Config GetConfigurazione();
        int GetContatoreDiComodo();
        void SetContatoreDiComodo(int ogni_N_Pezzi);
        void AggiornaTabellaConfig(Config config);
        void ResettaTurno(int contat_di_comodo);
        void AggiornaContatori(Config config);
    }
}