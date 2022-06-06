﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                .RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager())
                .RegisterType<IMainV, MainV>(new ContainerControlledLifetimeManager())
                .RegisterType<IMainP, MainP>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataSource_Liv2, DataSource_Liv2>(new ContainerControlledLifetimeManager())
                //.RegisterType<IDataSource_Liv2, DataSourceFake_Liv2>(new ContainerControlledLifetimeManager())
                .RegisterType<IDataSource_ServicePMS, DataSourceFake_ServicePMS>(new ContainerControlledLifetimeManager())
                //.RegisterType<IDataSource_ServicePMS, DataSource_ServicePMS>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreStampa, GestoreStampa>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestorePedana, GestorePedana>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreCronologiaPedaneCompletate, GestoreCronologiaPedaneCompletate>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreConfigurazione, GestoreConfigurazione>(new ContainerControlledLifetimeManager())
                .RegisterType<ILoginP, LoginP>(new ContainerControlledLifetimeManager())
                .RegisterType<ILoginV, LoginV>(new ContainerControlledLifetimeManager())
                .RegisterType<ILogin_FPT, Login_FPT>(new ContainerControlledLifetimeManager())
                .RegisterType<IInserimentoDisegniV, InserimentoDisegniV>(new ContainerControlledLifetimeManager())
                .RegisterType<IInserimentoDisegniP, InserimentoDisegniP>(new ContainerControlledLifetimeManager())
                .RegisterType<IGestoreDisegni, GestoreDisegni>(new ContainerControlledLifetimeManager())
                .RegisterType<IValidatoreDisegni, ValidatoreDisegni>(new ContainerControlledLifetimeManager())

                ;
        }

    }
}