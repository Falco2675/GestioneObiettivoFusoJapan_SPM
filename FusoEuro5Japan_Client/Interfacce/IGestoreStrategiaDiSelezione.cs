using System;

namespace FusoEuro5Japan_Client
{
    public interface IGestoreStrategiaDiSelezione
    {
        string NomeStrategia { get; }
        IStrategia Strategia { get; }
        StrategiaEnum StrategiaEnum { get; }
        string AzioneDaCompiere { get; }
        bool Azione_Bool { get; }
        bool IsMotoreTarget { get; }

        event EventHandler StrategiaDiSelezioneChanged;

        void SetStrategia(IGestoreConfigurazione config);
        string GetProduzioneTurno_string(int prod_1T, int obiettivo_1T);
        void ResettaAzione();
        void EseguiNessunaAzione();
    }
}