using System;
using System.IO;
using System.Text;
using Predac.Models;

namespace Predac.DataSource
{
    public class DataSourceFake_Liv2 : IDataSource_Liv2
    {
        string nomeFileTxt = "DISEGNI_FPT.txt";
        string pathFile;

        #region CTOR
        public DataSourceFake_Liv2()
        {
            pathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeFileTxt);
        }
        #endregion

        public MotoreFuso GetMotore(string matricola)
        {
            var motore = new MotoreFuso();
            foreach (string line in File.ReadLines(pathFile, Encoding.UTF8))
            {
                var splitLine = line.Split(';');
                if (splitLine[0] == matricola)
                {
                    motore.Matricola= matricola;
                    motore.DisegnoFPT = splitLine[1];
                    motore.DisegnoPredac = splitLine[2];
                    motore.IsPerRicambi = splitLine[3]=="p" ? false : true;
                    motore.DataServer = DateTime.Now;
                    motore.DisegnoTMC = "5809999999";
                    break;
                }
            }
            return motore;
        }

        public void InserisciDisegni(DisegnoFPT_TMC disegnoFPT_TMC)
        {
            
        }

        public bool IsConnessioneDS_Ok()
        {
            return true;
        }
    }
}
