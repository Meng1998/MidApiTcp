using Npgsql;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    public class Sql
    {
        public static ConnectionConfig ConnectSql()
        {
            string ConnString = "Server=[Server];Port=[Port];Database=[Database];Uid=[Uid];Pwd=[Pwd];";
            ConnString = ConnString.Replace("[Server]", AppConfig.GetAppSetting("Server"));
            ConnString = ConnString.Replace("[Port]", AppConfig.GetAppSetting("Port"));
            ConnString = ConnString.Replace("[Database]", AppConfig.GetAppSetting("Database"));
            ConnString = ConnString.Replace("[Uid]", AppConfig.GetAppSetting("Uid"));
            ConnString = ConnString.Replace("[Pwd]", AppConfig.GetAppSetting("Pwd"));
            //建立连接
            ConnectionConfig ConnConfig = new ConnectionConfig()
            {
                ConnectionString = ConnString,
                DbType = DbType.PostgreSQL,
                IsAutoCloseConnection = true
            };
            return ConnConfig;
        }

        /// <summary>
        /// 数据库中插入sql
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static int Createdate(string sql)
        {
            string ConnString = "Server=[Server];Port=[Port];Database=[Database];Uid=[Uid];Pwd=[Pwd];";
            ConnString = ConnString.Replace("[Server]", AppConfig.GetAppSetting("Server"));
            ConnString = ConnString.Replace("[Port]", AppConfig.GetAppSetting("Port"));
            ConnString = ConnString.Replace("[Database]", AppConfig.GetAppSetting("Database"));
            ConnString = ConnString.Replace("[Uid]", AppConfig.GetAppSetting("Uid"));
            ConnString = ConnString.Replace("[Pwd]", AppConfig.GetAppSetting("Pwd"));
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            // 打开一个数据库连接，在执行相关SQL之前调用   
            conn.Open();
            NpgsqlCommand objCommand = new NpgsqlCommand(sql, conn);
            int count = Convert.ToInt32(objCommand.ExecuteScalar());
            //关闭一个数据库连接，在执行完相关SQL之后调用   
            conn.Close();
            return count;
        }

        /// <summary>
        /// 数据库中插入sql
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dataname">数据库名</param>
        /// <returns></returns>
        public static int Createdate(string sql, string dataname)
        {
            string ConnString = "Server=[Server];Port=[Port];Database=[Database];Uid=[Uid];Pwd=[Pwd];";
            ConnString = ConnString.Replace("[Server]", AppConfig.GetAppSetting("Server"));
            ConnString = ConnString.Replace("[Port]", AppConfig.GetAppSetting("Port"));
            ConnString = ConnString.Replace("[Database]", dataname);
            ConnString = ConnString.Replace("[Uid]", AppConfig.GetAppSetting("Uid"));
            ConnString = ConnString.Replace("[Pwd]", AppConfig.GetAppSetting("Pwd"));
            NpgsqlConnection conn = new NpgsqlConnection(ConnString);
            // 打开一个数据库连接，在执行相关SQL之前调用   
            conn.Open();
            NpgsqlCommand objCommand = new NpgsqlCommand(sql, conn);
            int count = Convert.ToInt32(objCommand.ExecuteScalar());
            //关闭一个数据库连接，在执行完相关SQL之后调用   
            conn.Close();
            return count;
        }

        /// <summary>
        /// 检查该数据库是否存在指定表
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public static bool Existstable(string tablename)
        {
            string sql = string.Format("select count(*) from pg_class where relname='{0}'", tablename);
            if (Sql.Createdate(sql) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Existsconnection()
        {

            using (SqlSugarClient client = new SqlSugarClient(ConnectSql()))
            {
                try
                {
                    client.Open();
                    client.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
