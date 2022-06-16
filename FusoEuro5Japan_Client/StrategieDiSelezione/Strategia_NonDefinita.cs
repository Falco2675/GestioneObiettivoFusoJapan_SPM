namespace FusoEuro5Japan_Client
{
    public class Strategia_NonDefinita : Strategia_abs
    {
        public Strategia_NonDefinita() 
        {
        }

        public override string NomeStrategia => "N.D.";

        public override string GetProduzioneTurno_string(int prod, int targetProd) => "-";
        public override StrategiaEnum TipoStrategia => StrategiaEnum.Non_Definita;

        public override bool IsMotoreTarget() => false;

    }
}
