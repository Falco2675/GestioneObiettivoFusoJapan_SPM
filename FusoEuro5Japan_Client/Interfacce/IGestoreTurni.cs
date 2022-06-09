using System;
namespace FusoEuro5Japan_Client
{
    public interface IGestoreTurni
    {
        string Turno_string { get; }

        void ControllaTurno();

        event EventHandler TurnoChanged;
    }
}