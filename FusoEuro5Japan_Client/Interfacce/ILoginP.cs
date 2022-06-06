namespace FusoEuro5Japan_Client
{
    public interface ILoginP
    {
        bool AbilitaPulsanteLogin { get; }
        string Password { get; set; }
        string Username { get; set; }

        void ShowView();
    }
}