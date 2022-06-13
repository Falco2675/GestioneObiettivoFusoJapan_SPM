using System;
using System.Threading;
using System.Windows.Forms;

namespace FusoEuro5Japan_Client
{
    public interface IInserimentoDisegniV
    {
        SynchronizationContext SynchronizeContext { get; }

        string DisegnoFPT { get; set; }
        //StrategiaEnum Strategia { get; set; }

        event EventHandler AggiungiDisegnoEvent;
        event EventHandler<StrategiaEnum> StrategiaChanged;
        event EventHandler SalvaStrategiaEvent;

        void SetBindingSource(BindingSource bs);
        DialogResult ShowDialog();
    }
}