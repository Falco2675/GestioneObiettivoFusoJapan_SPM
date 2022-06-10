using System;

namespace FusoEuro5Japan_Client
{
    public interface IGestoreContatoriObiettivi
    {

        int Obiettivo_1T { get; set; }
        int Obiettivo_2T { get; set; }
        int Obiettivo_3T { get; set; }

        int Prod_1T { get; set; }
        int Prod_2T { get; set; }
        int Prod_3T { get; set; }

        int Contatore_del_giorno { get; set; }
        string Prod_Ieri { get; set; }

        bool IsTarget_1T_Raggiunto { get; }
        bool IsTarget_2T_Raggiunto { get; }
        bool IsTarget_3T_Raggiunto { get; }

        bool IsTarget_All_Turni_Raggiunto { get; }


        #region EVENTI
        event EventHandler ProduzioniIeriChanged;
        event EventHandler Prod_1TChanged;
        event EventHandler Prod_2TChanged;
        event EventHandler Prod_3TChanged;

        event EventHandler Obiettivo_1T_Changed;
        event EventHandler Obiettivo_2T_Changed;
        event EventHandler Obiettivo_3T_Changed;
        event EventHandler Obiettivo_All_Turni_Changed;

        event EventHandler ProduzioneGiornalieraChanged;

        event EventHandler Target_Prod_1T_RaggiuntoChanged;
        event EventHandler Target_Prod_2T_RaggiuntoChanged;
        event EventHandler Target_Prod_3T_RaggiuntoChanged; 
        #endregion

    }
}