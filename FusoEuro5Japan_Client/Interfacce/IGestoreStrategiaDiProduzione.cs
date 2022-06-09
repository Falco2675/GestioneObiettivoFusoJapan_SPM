using System;

namespace FusoEuro5Japan_Client
{
    public interface IGestoreStrategiaDiProduzione
    {
        IStrategia Strategia { get; }

        event EventHandler StrategiaDiProduzioneChanged;
    }
}