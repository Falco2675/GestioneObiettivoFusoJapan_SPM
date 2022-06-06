﻿using System;
using System.Threading;
using System.Windows.Forms;

namespace FusoEuro5Japan_Client
{
    public interface IInserimentoDisegniV
    {
        SynchronizationContext SynchronizeContext { get; }

        string DisegnoFPT { get; set; }
        string DisegnoTMC { get; set; }

        event EventHandler AggiungiDisegnoEvent;

        void SetBindingSource(BindingSource bs);
        DialogResult ShowDialog();
    }
}