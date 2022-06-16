using System;

namespace FusoEuro5Japan_Client
{
    public interface IGestoreTurni
    {
        TurnoEnum Turno_enum { get; }
        string Turno_string { get; }

        event EventHandler TurnoChanged;

        bool IsTurno(TurnoEnum primoTurno);
        void ControllaTurno();
    }
}