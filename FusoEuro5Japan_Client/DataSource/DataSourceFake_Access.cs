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

        #region CTOR
        public DataSourceFake_Access()
        {
        }

        #endregion

        public Motore GetMotore(string datoRicevuto, TipoDatoRicevuto tipoDatoRicevuto)
        {
            string query =
                tipoDatoRicevuto == TipoDatoRicevuto.Matricola
                ? @"SELECT * from pmotori p left join OBIETTIVO_JAPAN_SPM_DISEGNI j 
                                On (p.disegno = j.disegno)
                                where p.matricola = [datoRicev]"
                : @"SELECT * from pmotori p left join OBIETTIVO_JAPAN_SPM_DISEGNI j 
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
                                motore.CodBasamento = tabella.Rows[0].Field<string>("C_Basamento")?.Trim();

                                if (string.IsNullOrEmpty(tabella.Rows[0].Field<string>("j.disegno")?.Trim()))
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

                    string query = @"insert into OBIETTIVO_JAPAN_SPM_DISEGNI (disegno) 
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

                    string query = @"SELECT * from OBIETTIVO_JAPAN_SPM_CONFIG";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        using (OleDbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                tabella.Load(dr);
                                config.Ogni_N_Pezzi = tabella.Rows[0].Field<int>("OGNI_N_PEZZI");
                                config.N_pezzi_definito = tabella.Rows[0].Field<int>("N_PEZZI_A_TURNO");
                                config.Contatore_di_comodo = tabella.Rows[0].Field<int>("CONTATORE_DI_COMODO");
                                config.Contatore_del_turno = tabella.Rows[0].Field<int>("CONTATORE_TURNO");
                                config.Contatore_del_giorno = tabella.Rows[0].Field<int>("CONTATORE_GIORNO");

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

        public int GetContatoreDiComodo()
        {
            Config config = new FusoEuro5Japan_Client.Config();
            System.Data.DataTable tabella = new System.Data.DataTable();

            int result = 0;
            try
            {

                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"SELECT CONTATORE_DI_COMODO from OBIETTIVO_JAPAN_SPM_CONFIG";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        using (OleDbDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                tabella.Load(dr);
                                config.Contatore_di_comodo = tabella.Rows[0].Field<int>("CONTATORE_DI_COMODO");

                            }
                        }
                    }
                }
                return result;

            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");

            }
        }

        public void SetContatoreDiComodo(int contdiComodo)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET (CONTATORE_DI_COMODO = [cont_di_Comodo]) ";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@cont_di_Comodo", OleDbType.Integer).Value = contdiComodo;

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");
            }
        }

        public void SettaPerMotoreTarget(int cont_di_comodo, int cont_turno, int cont_giorno)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                    (CONTATORE_DI_COMODO = [cont_di_Comodo],
                                        CONTATORE_TURNO = [contTurno],
                                        CONTATORE_GIORNO = [contGiorno]) ";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@cont_di_Comodo", OleDbType.Integer).Value = cont_di_comodo;
                        cmd.Parameters.Add("@contTurno", OleDbType.Integer).Value = cont_turno;
                        cmd.Parameters.Add("@contGiorno", OleDbType.Integer).Value = cont_giorno;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");
            }
        }

        public void ResettaTurno(int cont_di_comodo)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                    (CONTATORE_DI_COMODO = [cont_di_Comodo],
                                        CONTATORE_TURNO = 0,
                                        CONTATORE_GIORNO = 0) ";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@cont_di_Comodo", OleDbType.Integer).Value = cont_di_comodo;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");
            }
        }

        public void AggiornaContatori(int contTurno, int ContGiorno)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                        CONTATORE_TURNO = [contTurno],
                                        CONTATORE_GIORNO = [contGiorno]
                                     ";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@contTurno", OleDbType.Integer).Value = contTurno;
                        cmd.Parameters.Add("@contGiorno", OleDbType.Integer).Value = ContGiorno;

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");
            }
        }
    }
}
