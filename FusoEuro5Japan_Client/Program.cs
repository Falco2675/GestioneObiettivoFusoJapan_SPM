using LoginFPT.Gestori;
using System;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Bootstrap();

            var presenter = _container.Resolve<IMainP>();

            Application.Run((Form)presenter.GetView);
        }

        private static void Bootstrap()
        {
            _container = new UnityContainer();

            _container
                .RegisterType<IMainV, MainView>(new ContainerControlledLifetimeManager())
                .RegisterType<IMainP, MainP>(new ContainerControlledLifetimeManager())
                //.RegisterType<IDataSource, DataSource_Liv2>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataSource, DataSourceFake_Access>(new ContainerControlledLifetimeManager())
                //.RegisterType<IDataSource_ServicePMS, DataSource_ServicePMS>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreConfigurazione, GestoreConfigurazione>(new ContainerControlledLifetimeManager())
                .RegisterType<ILoginP, LoginP>(new ContainerControlledLifetimeManager())
                .RegisterType<ILoginV, LoginV>(new ContainerControlledLifetimeManager())
                .RegisterType<ILogin_FPT, Login_FPT>(new ContainerControlledLifetimeManager())
                .RegisterType<IInserimentoDisegniV, InserimentoDisegniV>(new ContainerControlledLifetimeManager())
                .RegisterType<IInserimentoDisegniP, InserimentoDisegniP>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreTurni, GestoreTurni>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreConvalidaDatoRicevuto, GestoreConvalidaDatoRicevuto>(new ContainerControlledLifetimeManager())

                ;
        }

    }
}
