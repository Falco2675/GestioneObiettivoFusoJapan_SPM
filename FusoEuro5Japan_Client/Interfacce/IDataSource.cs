
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
        void SettaPerMotoreTarget(int cont_di_comodo, int cont_turno, int cont_giorno);
        void ResettaTurno(int contat_di_comodo);
        void AggiornaContatori(int contTurno, int ContGiorno);
    }
}