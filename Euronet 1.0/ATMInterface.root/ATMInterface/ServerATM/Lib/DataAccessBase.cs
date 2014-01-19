using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using Common;

namespace ATMTransaction
{
    public class DataAccessBase
    {
        protected IDbTransaction _Transaction = null;
        protected IDbConnection con1 = null;
        protected string cbsConString;
        protected string intConString;
        protected string activeConString;
        protected bool cbsConnected;
        protected int atmTrId;
        protected int offlineException;        
        protected int acquirer;
        protected string CW_ProcessCode;
        protected string BQ_ProcessCode;
        protected string MS_ProcessCode;
        protected string FT_ProcessCode;

        public DataAccessBase()
        {
            cbsConString = ConfigManager.OracleAbabilConnectionString;
            intConString = ConfigManager.OracleATMConnectionString;
        }

        protected bool CheckRecordExists(string query)
        {
            bool recordFound = false;

            using (OracleConnection con = GetOracleConnection(intConString))
            {
                con.Open();
                OracleCommand oCmd = new OracleCommand(query, con);

                using (OracleDataReader reader = oCmd.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        recordFound = true;
                    }                    
                }                
            }

            return recordFound;
        }

        protected OracleConnection GetOracleConnection(string connectionString)
        {
            return  new OracleConnection(connectionString);             
        }      

        protected OracleDataReader GetOracleDataReader(IDbTransaction transaction, string query)
        {
            OracleDataReader rdr = null;

            using (OracleCommand cmd = new OracleCommand(query, (OracleConnection)transaction.Connection, (OracleTransaction)transaction))
            {
                rdr = cmd.ExecuteReader(CommandBehavior.Default);
            }

            return rdr;
        }        

        protected object GetFirstRowFirstColumnValue(IDbTransaction trans, string query)
        {
            object result = null;

            using (OracleCommand cmd = new OracleCommand(query, (OracleConnection)trans.Connection,(OracleTransaction) trans))
            {                
                result = cmd.ExecuteScalar();                
            }
            
            return result;
        }
    }
}
