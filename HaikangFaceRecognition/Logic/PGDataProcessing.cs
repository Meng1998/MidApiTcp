using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    class PGDataProcessing
    {
        public DataSet ExecuteQuery(string sqrstr,ref Boolean error)
        {
            DataSet ds = new DataSet();
            try
            {
                //DbType = DbType.PostgreSQL,
                //IsAutoCloseConnection = true
                Log.WriteLog("数据库", sqrstr);
                using (NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sqrstr, ConfigurationManager.ConnectionStrings["postgre"].ToString()))
                {
                    sqldap.Fill(ds);
                }
                error = true;
                return ds;
            }
            catch (System.Exception ex)
            {
                error = false;
                Log.WriteLog("数据库", ex.Message);
                return ds;
            }

            
        }
        public DataSet ExecuteQuerys(string sqrstr,String postgre, out Boolean error)
        {
            error = true;
            DataSet ds = new DataSet();
            try
            {
                using (NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sqrstr, postgre))
                {
                    sqldap.Fill(ds);
                }
                error = true;
                return ds;
            }
            catch (System.Exception ex)
            {
                error = false;
                Log.WriteLog("数据库", ex.Message);
                return ds;
            }
        }
    }

}
