using System;
using System.Drawing;

namespace FusoEuro5Japan_Client
{
    public interface IMainV
    {
        string DisegnoSingolo { get; set; }
        string MultiDis1 { get; set; }
        string MultiDis2 { get; set; }
        string MultiDis3 { get; set; }
        string Matricola1 { get; set; }
        string Matricola2 { get; set; }
        string Matricola3 { get; set; }
        Color ForeColorMatr1 { get; set; }
        Color ForeColorMatr2 { get; set; }
        Color ForeColorMatr3 { get; set; }
        Color BackColorMatr1 { get; set; }
        Color BackColorMatr2 { get; set; }
        Color BackColorMatr3 { get; set; }


        event EventHandler<string> StringaRicevutaEvent;
        event EventHandler StampaEvent;
        event EventHandler ResetEvent;
        event EventHandler ForzaChiusuraPedanaEvent;
        event EventHandler AvviaStrumentiEvent;

        void SetPresenter(IMainP presenter);
        void ResettaCampi();
        void PedanaChiusaCorrettamente();
        void SettaPerDisegnoMisti(bool isMultiDisegno);
    }
}