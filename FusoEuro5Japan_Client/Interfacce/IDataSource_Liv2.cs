
namespace FusoEuro5Japan_Client
{
    public interface IDataSource_Liv2
    {
        MotoreFuso GetMotore(string matricola);
        void InserisciDisegni(DisegnoFPT_TMC disegnoFPT_TMC);
        bool IsConnessioneDS_Ok();
    }
}