using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace FusoEuro5Japan_Client
{
    public interface IStrumentiV
    {
        SynchronizationContext SynchronizeContext { get; }

        string DisegnoFPT { get; set; }
        //StrategiaEnum Strategia { get; set; }

        event EventHandler AggiungiDisegnoEvent;
        event EventHandler<StrategiaEnum> StrategiaChanged;
        event EventHandler SalvaStrategiaEvent;

        void SetBindingSource(BindingSource bs);
        void AggiornaElencoDisegni(List<string> disegni);
        DialogResult ShowDialog();
        void AggiungiDisegnoAElenco(string disegno);
    }
}