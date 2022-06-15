using System;
using System.Collections.Generic;
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
                                config.Contatore_di_comodo = tabella.Rows[0].Field<int>("CONTATORE_DI_COMODO");

                                config.Obiettivo_1T = tabella.Rows[0].Field<int>("OBIETTIVO_1T");
                                config.Obiettivo_2T = tabella.Rows[0].Field<int>("OBIETTIVO_2T");
                                config.Obiettivo_3T = tabella.Rows[0].Field<int>("OBIETTIVO_3T");
                                config.Obiettivo_Giornaliero = tabella.Rows[0].Field<int>("OBIETTIVO_GIORNALIERO");

                                config.Prod_1T = tabella.Rows[0].Field<int>("PROD_1T");
                                config.Prod_2T = tabella.Rows[0].Field<int>("PROD_2T");
                                config.Prod_3T = tabella.Rows[0].Field<int>("PROD_3T");
                                
                                config.Contatore_del_giorno = tabella.Rows[0].Field<int>("CONTATORE_GIORNO");

                                config.Prod_Ieri = tabella.Rows[0].Field<string>("PROD_IERI");

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

        public void AggiornaTabellaConfig(Config config)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                        Ogni_N_pezzi = [ogni_N_Pezzo],
                                        CONTATORE_DI_COMODO = [cont_di_Comodo],
                                        CONTATORE_GIORNO = [contGiorno], 
                                        OBIETTIVO_1T = [obiettivo_1T],
                                        OBIETTIVO_2T = [obiettivo_2T],
                                        OBIETTIVO_3T = [obiettivo_3T],
                                        PROD_1T = [prod_1T],
                                        PROD_2T = [prod_2T],
                                        PROD_3T = [prod_3T],
                                        PROD_IERI = [prod_ieri]
                                        ";


                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@ogni_N_Pezzo", OleDbType.Integer).Value = config.Ogni_N_Pezzi;
                        cmd.Parameters.Add("@cont_di_Comodo", OleDbType.Integer).Value = config.Contatore_di_comodo;
                        cmd.Parameters.Add("@contGiorno", OleDbType.Integer).Value = config.Contatore_del_giorno;
                        cmd.Parameters.Add("@obiettivo_1T", OleDbType.Integer).Value = config.Obiettivo_1T;
                        cmd.Parameters.Add("@obiettivo_2T", OleDbType.Integer).Value = config.Obiettivo_2T;
                        cmd.Parameters.Add("@obiettivo_3T", OleDbType.Integer).Value = config.Obiettivo_3T;
                        cmd.Parameters.Add("@prod_1T", OleDbType.Integer).Value = config.Prod_1T;
                        cmd.Parameters.Add("@prod_2T", OleDbType.Integer).Value = config.Prod_2T;
                        cmd.Parameters.Add("@prod_3T", OleDbType.Integer).Value = config.Prod_3T;
                        cmd.Parameters.Add("@prod_ieri", OleDbType.VarChar).Value = config.Prod_Ieri;

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
                                        CONTATORE_DI_COMODO = [cont_di_Comodo],
                                        CONTATORE_TURNO = 0,
                                        CONTATORE_GIORNO = 0
                                    ";

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

        public void AggiornaContatori(Config config)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                        PROD_1T = [prod_1T],
                                        PROD_2T = [prod_2T],
                                        PROD_3T = [prod_3T],
                                        CONTATORE_GIORNO = [contGiorno]
                                     ";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@prod_1T", OleDbType.Integer).Value = config.Prod_1T;
                        cmd.Parameters.Add("@prod_2T", OleDbType.Integer).Value = config.Prod_2T;
                        cmd.Parameters.Add("@prod_3T", OleDbType.Integer).Value = config.Prod_3T;
                        cmd.Parameters.Add("@contGiorno", OleDbType.Integer).Value = config.Contatore_del_giorno;

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");
            }
        }

        public void SetConfig_ProduzioneFissa(int prod_1T, int prod_2T, int prod_3T)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                        OBIETTIVO_1T = :prod_1T,
                                        OBIETTIVO_2T = :prod_2T,
                                        OBIETTIVO_3T = :prod_3T,
                                        Ogni_N_pezzi = 0,
                                        Contatore_di_comodo = 0
                                     ";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@prod_1T", OleDbType.Integer).Value = prod_1T;
                        cmd.Parameters.Add("@prod_2T", OleDbType.Integer).Value = prod_2T;
                        cmd.Parameters.Add("@prod_3T", OleDbType.Integer).Value = prod_3T;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");
            }
        }
        public void SetConfig_1_Ogni_N_Pezzi(int N_Pezzi)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                        OBIETTIVO_1T = 0,
                                        OBIETTIVO_2T = 0,
                                        OBIETTIVO_3T = 0,
                                        Ogni_N_pezzi = :n_pezzi,
                                        Contatore_di_comodo = :n_pezzi
                                     ";

                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("@n_pezzi", OleDbType.Integer).Value = N_Pezzi;

                        cmd.ExecuteNonQuery();
                    }
                }
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

        public void ResettaProduzioneDelGiorno(string prodDiIeri)
        {
            throw new NotImplementedException();
        }

        public List<string> GetElencoDisegni()
        {
            throw new NotImplementedException();
        }
    }
}
