using System;

namespace FusoEuro5Japan_Client
{
    public interface IGestoreConfigurazione
    {
        int IdApp { get; }
        Config Configurazione { get; }

        event EventHandler<StrategiaEnum> CambioStrategiaChanged;
        event EventHandler<int> ContatoreDelTurnoChanged;


        Config GetConfigurazione();
        void ResettaTurno();
    }
}