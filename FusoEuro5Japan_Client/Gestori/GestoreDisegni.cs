using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class GestoreDisegni : IGestoreDisegni
    {
        private readonly IDataSource_Liv2 _dataSource;

        public GestoreDisegni
            (
                IDataSource_Liv2 dataSource
            )
        {
            _dataSource = dataSource;
        }

        public void AggiungiDisegno(DisegnoFPT_TMC disegni)
        {
            _dataSource.InserisciDisegni(disegni);
        }
    }
}
