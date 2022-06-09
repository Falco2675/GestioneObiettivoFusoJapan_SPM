using System;

namespace FusoEuro5Japan_Client
{
    public interface IGestoreConfigurazione
    {
        int IdApp { get; }
        Config Configurazione { get; }
        StrategiaEnum StrategiaAdottata { get; }
        string ContatoreTurno_string { get; }
        string ContatoreGiorno_string { get; }

        event EventHandler StrategiaChanged;
        event EventHandler ContatoriChanged;


        Config GetConfigurazione();
        void ResettaTurno();
    }
}