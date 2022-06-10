using System;

namespace FusoEuro5Japan_Client
{
    public interface IGestoreStrategiaDiProduzione
    {
        string NomeStrategia { get; }
        IStrategia Strategia { get; }
        StrategiaEnum StrategiaEnum { get; }
        string AzioneDaCompiere { get; }
        bool IsMotoreTarget { get; }

        event EventHandler StrategiaDiProduzioneChanged;
        event EventHandler<string> AzioneDaCompiereChanged;
        //event EventHandler ObiettivoTurnoRaggiuntoEvent;

        void SetStrategia(IGestoreConfigurazione config);
        string GetProduzioneTurno_string(int prod_1T, int obiettivo_1T);
        //void EseguiSuMotore(Motore motoreLetto, TurnoEnum turno_enum);
        void ResettaAzione();
        void EseguiNessunaAzione();
    }
}