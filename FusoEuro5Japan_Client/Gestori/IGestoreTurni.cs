using System;
namespace FusoEuro5Japan_Client
{
    public interface IGestoreTurni
    {
        DateTime Orario { get; }

        event EventHandler<string> OrarioChanged;
    }
}