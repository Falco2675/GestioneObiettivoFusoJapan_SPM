using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;

namespace FusoEuro5Japan_Client
{
    public class DataSource_Liv2 : IDataSource_Liv2
    {
        static string connString = "DSN=iveco; Uid=IVECO; Pwd=bedede;";
        private readonly IGestoreConfigurazione _gestoreConfigurazione;

        #region CTOR
        public DataSource_Liv2
            (
                IGestoreConfigurazione gestoreConfigurazione
            )
        {
            _gestoreConfigurazione = gestoreConfigurazione;
        }
        #endregion

        public Motore GetMotore(string matricola)
        {
            System.Data.DataTable tabella = new System.Data.DataTable();
            var motore = new Motore();
            try
            {

                using (OdbcConnection conn = new OdbcConnection(connString))
                {
                    
                    string query = @"select sysdate, fpt.matricola AS MATRICOLA_FPT, fpt.disegno as DISEGNOFPT, p.disegnofpt as DISEGNO_PREDAC, DISEGNOTMC, RICAMBI from pmotori fpt
                    left join PREDAC_DIS_TMC p on (fpt.disegno = p.disegnofpt)
                    where fpt.matricola = ?";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@matricola", OdbcType.Char, 20).Value = matricola.Trim();
                        using (OdbcDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                tabella.Load(dr);
                                motore.Matricola = tabella.Rows[0].Field<string>("MATRICOLA_FPT")?.Trim();
                                motore.DisegnoFPT = tabella.Rows[0].Field<string>("DISEGNOFPT")?.Trim();
                                motore.DisegnoPredac = tabella.Rows[0].Field<string>("DISEGNO_PREDAC")?.Trim();

                                double? value = tabella.Rows[0].Field<double?>("RICAMBI");
                                motore.IsPerRicambi = value == 0 ? false : true;

                                motore.DataServer = tabella.Rows[0].Field<DateTime>("sysdate");
                                motore.DisegnoTMC = tabella.Rows[0].Field<string>("DISEGNOTMC")?.Trim();
                            }
                        }
                    }
                }
                return motore;

            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");

            }
        }

        public void InserisciDisegni(DisegnoFPT_TMC disegnoFPT_TMC)
        {
            try
            {
                using (OdbcConnection conn = new OdbcConnection(connString))
                {
                    
                    string query = @"insert into PREDAC_DIS_TMC (disegnofpt, disegnotmc, ricambi) 
                                    values
                                    (:disegnoFPT, :disegnoTMC, :ricambi)";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@disegnoFPT", OdbcType.Char, 15).Value = disegnoFPT_TMC.DisegnoFPT.Trim();
                        cmd.Parameters.Add("@disegnoTMC", OdbcType.Char, 15).Value = disegnoFPT_TMC.DisegnoTMC.Trim();
                        cmd.Parameters.Add("@ricambi", OdbcType.Numeric, 1).Value = disegnoFPT_TMC.IsPerRicambi ? 1 : 0;

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("ORA-00001"))
                {
                    throw new Exception("Disegno già registrato.");
                }
                throw new Exception();
            }
        }

        public bool IsConnessioneDS_Ok()
        {

            bool output = false;
            try
            {
                using (OdbcConnection conn = new OdbcConnection(connString))
                {
                    conn.Open();
                    output = true;
                }
            }
            catch (Exception)
            {

                output = false;
            }

            return output;
        }
    }
}
