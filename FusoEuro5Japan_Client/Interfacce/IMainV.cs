using System;
using System.Drawing;

namespace FusoEuro5Japan_Client
{
    public interface IMainV
    {

        event EventHandler<string> StringaRicevutaEvent;
        event EventHandler ResetEvent;
        event EventHandler AvviaStrumentiEvent;

        void SetPresenter(IMainP presenter);
        void ResettaCampi();
    }
}