using LoginFPT.Gestori;
using System;
using System.Threading;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace FusoEuro5Japan_Client
{
    static class Program
    {
        static UnityContainer _container;
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool instanceCountOne = false;
            using (Mutex mtex = new Mutex(true, "IstanzaMutex", out instanceCountOne))
            {
                if (instanceCountOne)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Bootstrap();

                    var presenter = _container.Resolve<IMainP>();

                    Application.Run((Form)presenter.GetView);
                    mtex.ReleaseMutex();
                }
                else
                {
                    MessageBox.Show("L'applicativo è gia in esecuzione.");
                }
            }
        }

        private static void Bootstrap()
        {
            _container = new UnityContainer();

            _container
                .RegisterType<IMainV, MainView>(new ContainerControlledLifetimeManager())
                .RegisterType<IMainP, MainP>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataSource, DataSource_Liv2>(new ContainerControlledLifetimeManager())
                //.RegisterType<IDataSource, DataSourceFake_Access>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreConfigurazione, GestoreConfigurazione>(new ContainerControlledLifetimeManager())
                .RegisterType<ILoginP, LoginP>(new ContainerControlledLifetimeManager())
                .RegisterType<ILoginV, LoginV>(new ContainerControlledLifetimeManager())
                .RegisterType<ILogin_FPT, Login_FPT>(new ContainerControlledLifetimeManager())
                .RegisterType<IStrumentiV, StrumentiV>(new ContainerControlledLifetimeManager())
                .RegisterType<IStrumentiP, StrumentiP>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreTurni, GestoreTurni>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreConvalidaDatoRicevuto, GestoreConvalidaDatoRicevuto>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreContatoriObiettivi, GestoreContatoriObiettivi>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreStrategiaDiSelezione, GestoreStrategiaDiSelezione>(new ContainerControlledLifetimeManager())
                ;
        }

    }
}
