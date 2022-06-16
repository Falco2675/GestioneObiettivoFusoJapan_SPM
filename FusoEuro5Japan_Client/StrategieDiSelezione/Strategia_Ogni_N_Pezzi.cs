
namespace FusoEuro5Japan_Client
{
    public class Strategia_Ogni_N_Pezzi : Strategia_abs
    {

        #region PROPRIETA'
        public override StrategiaEnum TipoStrategia => StrategiaEnum.Ogni_N_pezzi;
        public override string NomeStrategia => $"1 Ogni {_configurazione.Configurazione.Ogni_N_Pezzi.ToString()} motori";

        #endregion

        #region CTOR
        public Strategia_Ogni_N_Pezzi() 
        {
        }
        #endregion

        public override string GetProduzioneTurno_string(int prod, int targetProd) => $"{prod.ToString()}";

        public override bool IsMotoreTarget()
        {
            bool isTarget = _configurazione.Configurazione.Contatore_di_comodo == _configurazione.Configurazione.Ogni_N_Pezzi;

            AzioneDaCompiere = isTarget
                ? AZIONE_DA_SVOLGERE
                : NESSUNA_AZIONE;

            _configurazione.AggiornaContatoreDiComodo();

            return isTarget;

        }

    }
}
