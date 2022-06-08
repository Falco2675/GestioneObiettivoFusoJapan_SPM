﻿using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;

namespace FusoEuro5Japan_Client
{
    public class DataSource_Liv2 : IDataSource
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

        public void SetContatoreDiComoodo(int contdiComodo)
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

        public void SettaPerMotoreTarget(int cont_di_comodo, int cont_turno, int cont_giorno)
        {
            try
            {
                using (OdbcConnection conn = new OdbcConnection(connString))
                {

                    string query = @"UPDATE OBIETTIVO_JAPAN_SPM_CONFIG
                                    SET 
                                    (CONTATORE_DI_COMODO = :cont_di_Comodo,
                                        CONTATORE_TURNO = :contTurno,
                                        CONTATORE_GIORNO = :contGiorno) ";

                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(query, conn))
                    {
                        cmd.Parameters.Add("@cont_di_Comodo", OdbcType.Int).Value = cont_di_comodo;
                        cmd.Parameters.Add("@contTurno", OdbcType.Int).Value = cont_turno;
                        cmd.Parameters.Add("@contGiorno", OdbcType.Int).Value = cont_giorno;

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
    }
}
