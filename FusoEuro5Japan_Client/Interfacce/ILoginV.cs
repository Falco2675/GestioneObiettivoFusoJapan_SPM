using LoginFPT.Models;
using System;
using System.Threading;
using System.Windows.Forms;

namespace FusoEuro5Japan_Client
{
    public interface ILoginV
    {
        SynchronizationContext SynchronizeContext { get; }

        event EventHandler LoggaUtenteEvent;

        void SetBindingSource(BindingSource bs);
        void Resetta();
        DialogResult ShowDialog();
        void Close();
        void SetPerLoginFallito();
    }
}