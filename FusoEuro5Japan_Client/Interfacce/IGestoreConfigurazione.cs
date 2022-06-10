using System;

namespace FusoEuro5Japan_Client
{
    public interface IGestoreConfigurazione
    {
        int IdApp { get; }
        string ExceptionDbConfigurazione { get; }
        Config Configurazione { get; }
        StrategiaEnum StrategiaAdottata { get; }
        TurnoEnum TurnoCorrente { get; }
        string ContatoreTurno_string { get; }
        string ContatoreGiorno_string { get; }
        bool IsObiettivoTurnoRaggiunto { get; }

        event EventHandler ExceptionDBConfigurazioneEvent;
        event EventHandler ConfigurazioneAggiornataEvent;

        void AggiornaConfigurazione();
        void ResettaTurno();
        void ScriviConfigurazione();
    }
}