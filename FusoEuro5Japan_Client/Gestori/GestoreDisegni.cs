using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusoEuro5Japan_Client
{
    public class GestoreDisegni : IGestoreDisegni
    {
        private readonly IDataSource _dataSource;

        public GestoreDisegni
            (
                IDataSource dataSource
            )
        {
            _dataSource = dataSource;
        }

        public void AggiungiDisegno(string disegno)
        {
            _dataSource.InserisciDisegni(disegno);
        }
    }
}
