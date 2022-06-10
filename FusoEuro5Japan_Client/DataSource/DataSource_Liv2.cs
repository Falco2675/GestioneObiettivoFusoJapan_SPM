using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;

namespace FusoEuro5Japan_Client
{
    public class DataSource_Liv2 : IDataSource
    {
        static string connString = "DSN=iveco; Uid=IVECO; Pwd=bedede;";

        #region CTOR
        public DataSource_Liv2
            (
            )
        {
        }
        #endregion

        public Motore GetMotore(string datoRicevuto, TipoDatoRicevuto tipoDatoRicevuto)
        {
            string query =
                 tipoDatoRicevuto == TipoDatoRicevuto.Matricola
                 ? @"SELECT * from pmotori p left join OBIETTIVO_JAPAN_SPM_DISEGNI j 
                                On (p.disegno = j.disegno)
                                where p.matricola = :datoRicev"
                 : @"SELECT * from pmotori p left join OBIETTIVO_JAPAN_SPM_DISEGNI j 
                                On (p.disegno = j.disegno)
                                where p.c_basamento = :datoRicev";

            System.Data.DataTable tabella = new System.Data.DataTable();
            var motore = new Motore();
            try
            {

                using (OdbcConnection conn = new OdbcConnection(connString))
                {
                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@datoRicev", OdbcType.Char).Value = datoRicevuto.Trim();
                        using (OdbcDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                tabella.Load(dr);
                                motore.Matricola = tabella.Rows[0].Field<string>("Matricola")?.Trim();
                                motore.Disegno = tabella.Rows[0].Field<string>("disegno")?.Trim();
                                motore.CodBasamento = tabella.Rows[0].Field<string>("C_Basamento")?.Trim();

                                //if (string.IsNullOrEmpty(tabella.Rows[0].Field<string>("j.disegno")?.Trim()))
                                //    motore.IsTargetCandidate = false;
                                //else
                                //    motore.IsTargetCandidate = true;

                                motore.IsTargetCandidate = ! string.IsNullOrEmpty(tabella.Rows[0].Field<string>("disegno1")?.Trim());

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
                using (OdbcConnection conn = new OdbcConnection(connString))
                {

                    string query = @"insert into OBIETTIVO_JAPAN_SPM_DISEGNI (disegno) 
                                    values
                                    (:disegno)";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@disegno", OdbcType.Char, 15).Value = disegnoMotore.Trim();

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
        public Config GetConfigurazione()
        {
            Config config = new FusoEuro5Japan_Client.Config();
            System.Data.DataTable tabella = new System.Data.DataTable();

            try
            {

                using (OdbcConnection conn = new OdbcConnection(connString))
                {

                    string query = @"SELECT * from OBIETTIVO_JAPAN_SPM_CONFIG";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        using (OdbcDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                tabella.Load(dr);
                                config.Ogni_N_Pezzi = Convert.ToInt16(tabella.Rows[0].Field<decimal>("OGNI_N_PEZZI"));
                                config.Contatore_di_comodo = Convert.ToInt16(tabella.Rows[0].Field<decimal>("CONTATORE_DI_COMODO"));


                                config.Obiettivo_1T = Convert.ToInt16(tabella.Rows[0].Field<decimal>("OBIETTIVO_1T"));
                                config.Obiettivo_2T = Convert.ToInt16(tabella.Rows[0].Field<decimal>("OBIETTIVO_2T"));
                                config.Obiettivo_3T = Convert.ToInt16(tabella.Rows[0].Field<decimal>("OBIETTIVO_3T"));
                                config.Obiettivo_Giornaliero = Convert.ToInt16(tabella.Rows[0].Field<decimal>("OBIETTIVO_GIORNALIERO"));

                                config.Prod_1T = Convert.ToInt16(tabella.Rows[0].Field<decimal>("PROD_1T"));
                                config.Prod_2T = Convert.ToInt16(tabella.Rows[0].Field<decimal>("PROD_2T"));
                                config.Prod_3T = Convert.ToInt16(tabella.Rows[0].Field<decimal>("PROD_3T"));

                                config.Contatore_del_giorno = Convert.ToInt16(tabella.Rows[0].Field<decimal>("CONTATORE_GIORNO"));

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

                using (OdbcConnection conn = new OdbcConnection(connString))
                {

                    string query = @"SELECT CONTATORE_DI_COMODO from OBIETTIVO_JAPAN_SPM_CONFIG";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        using (OdbcDataReader dr = cmd.ExecuteReader())
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
                using (OdbcConnection conn = new OdbcConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET (CONTATORE_DI_COMODO = :cont_di_Comodo) ";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@cont_di_Comodo", OdbcType.Int).Value = contdiComodo;

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
                using (OdbcConnection conn = new OdbcConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                        Ogni_N_pezzi = [ogni_N_Pezzo],
                                        CONTATORE_DI_COMODO = [cont_di_Comodo],
                                        CONTATORE_GIORNO = [contGiorno] 
                                        OBIETTIVO_1T = [obiettivo_1T]
                                        OBIETTIVO_2T = [obiettivo_2T]
                                        OBIETTIVO_3T = [obiettivo_3T]
                                        PROD_1T = [prod_1T]
                                        PROD_1T = [prod_2T]
                                        PROD_1T = [prod_3T]
                                        PROD_IERI = [prod_ieri],
";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@cont_di_Comodo", OdbcType.Int).Value = config.Ogni_N_Pezzi;
                        cmd.Parameters.Add("@cont_di_Comodo", OdbcType.Int).Value = config.Contatore_di_comodo;
                        cmd.Parameters.Add("@contGiorno", OdbcType.Int).Value = config.Contatore_del_giorno;
                        cmd.Parameters.Add("@obiettivo_1T", OdbcType.Int).Value = config.Obiettivo_1T;
                        cmd.Parameters.Add("@obiettivo_2T", OdbcType.Int).Value = config.Obiettivo_2T;
                        cmd.Parameters.Add("@obiettivo_3T", OdbcType.Int).Value = config.Obiettivo_3T;
                        cmd.Parameters.Add("@prod_1T", OdbcType.Int).Value = config.Prod_1T;
                        cmd.Parameters.Add("@prod_2T", OdbcType.Int).Value = config.Prod_2T;
                        cmd.Parameters.Add("@prod_3T", OdbcType.Int).Value = config.Prod_3T;
                        cmd.Parameters.Add("@prod_ieri", OdbcType.VarChar).Value = config.Prod_Ieri;

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
                using (OdbcConnection conn = new OdbcConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                    (CONTATORE_DI_COMODO = :cont_di_Comodo,
                                        CONTATORE_TURNO = 0,
                                        CONTATORE_GIORNO = 0) ";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@cont_di_Comodo", OdbcType.Int).Value = cont_di_comodo;

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
                using (OdbcConnection conn = new OdbcConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                        PROD_1T = :prod_1T,
                                        PROD_2T = :prod_2T,
                                        PROD_3T = :prod_3T,
                                        CONTATORE_GIORNO = :contGiorno
                                     ";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@prod_1T", OdbcType.Int).Value = config.Prod_1T;
                        cmd.Parameters.Add("@prod_2T", OdbcType.Int).Value = config.Prod_2T;
                        cmd.Parameters.Add("@prod_3T", OdbcType.Int).Value = config.Prod_3T;
                        cmd.Parameters.Add("@contGiorno", OdbcType.Int).Value = config.Contatore_del_giorno;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Errore DB!");
            }
        }

        //public void ScriviConfig(Config _configurazione)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
