using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace FusoEuro5Japan_Client
{
    public class DataSourceFake_Access : IDataSource
    {
        static OleDbConnection conn = new OleDbConnection();
        static string connString = ConfigurationManager.ConnectionStrings["AccessConnectionString"].ConnectionString;
        private readonly IGestoreConfigurazione _gestoreConfigurazione;

        #region CTOR
        public DataSourceFake_Access
            (
                IGestoreConfigurazione gestoreConfigurazione
            )
        {
            _gestoreConfigurazione = gestoreConfigurazione;
        }

        #endregion

        public Motore GetMotore(string datoRicevuto, TipoDatoRicevuto tipoDatoRicevuto)
        {
            string query =
                tipoDatoRicevuto == TipoDatoRicevuto.Matricola
                ? @"SELECT * from pmotori p left join obiettivoJapan_disegni j 
                                On (p.disegno = j.disegno)
                                where p.matricola = [datoRicev]"
                : @"SELECT * from pmotori p left join obiettivoJapan_disegni j 
                                On (p.disegno = j.disegno)
                                where p.cod_basamento = [datoRicev]";

            System.Data.DataTable tabella = new System.Data.DataTable();
            var motore = new Motore();
            try
            {

                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@datoRicev", OleDbType.Char).Value = datoRicevuto.Trim();
                        using (OleDbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                tabella.Load(dr);
                                motore.Matricola = tabella.Rows[0].Field<string>("Matricola")?.Trim();
                                motore.Disegno = tabella.Rows[0].Field<string>("p.Disegno")?.Trim();
                                motore.CodBasamento = tabella.Rows[0].Field<string>("Cod_Basamento")?.Trim();

                                if (string.IsNullOrEmpty(tabella.Rows[0].Field<string>("Cod_Basamento")?.Trim()))
                                    motore.IsTargetCandidate = false;
                                else
                                    motore.IsTargetCandidate = true;

                                
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

        public void InserisciDisegni(string disegnoMotore)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"insert into ObiettivoJapan_Disegni (disegno) 
                                    values
                                    ([disegno])";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@disegno", OleDbType.Char, 15).Value = disegnoMotore.Trim();

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
                using (OleDbConnection conn = new OleDbConnection(connString))
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
        public Config GetConfigurazione()
        {
            Config config = new FusoEuro5Japan_Client.Config();
            System.Data.DataTable tabella = new System.Data.DataTable();

            try
            {

                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"SELECT * from ObiettivoJapan_Config";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        using (OleDbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                tabella.Load(dr);
                                config.Ogni_N_Pezzi = tabella.Rows[0].Field<int>("p.Disegno");
                                config.N_pezzi_definito = tabella.Rows[0].Field<int>("N_pezzi_Definito");
                                config.Contatore_di_comodo = tabella.Rows[0].Field<int>("Contatore_di_comodo");
                                config.Contatore_del_turno = tabella.Rows[0].Field<int>("Contatore_del_turno");

                            }
                        }
                    }
                }
                return config;

            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");

            }
        }


    }
}
