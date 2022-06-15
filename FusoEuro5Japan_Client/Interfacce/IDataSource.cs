
using System.Collections.Generic;

namespace FusoEuro5Japan_Client
{
    public interface IDataSource
    {
        Motore GetMotore(string datoRicevuto, TipoDatoRicevuto tipoDatoRicevuto);
        void InserisciDisegni(string disegno);
        void SetConfig_ProduzioneFissa(int prod_1T, int prod_2T, int prod_3T);
        void SetConfig_1_Ogni_N_Pezzi(int N_Pezzi);

        bool IsConnessioneDS_Ok();
        Config GetConfigurazione();
        int GetContatoreDiComodo();
        void SetContatoreDiComodo(int ogni_N_Pezzi);
        void AggiornaTabellaConfig(Config config);
        void ResettaTurno(int contat_di_comodo);
        void AggiornaContatori(Config config);
        void ResettaProduzioneDelGiorno(string prodDiIeri);
        List<string> GetElencoDisegni();
    }
}