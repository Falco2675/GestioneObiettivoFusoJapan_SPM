
namespace FusoEuro5Japan_Client
{
    public interface IDataSource_Liv2
    {
        Motore GetMotore(string matricola);
        bool Is_FareTestACaldo();
        void InserisciDisegni(string disegno);
        bool IsConnessioneDS_Ok();
    }
}