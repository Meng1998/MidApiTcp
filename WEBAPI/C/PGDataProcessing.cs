using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Npgsql;
using System;
using System.Data;
using System.IO;

namespace WEBAPI.C
{
    public class PGDataProcessing
    {
        public DataSet ExecuteQuery(string sqrstr, out Boolean error)
        {
            error = true;
            DataSet ds = new DataSet();
            try
            {
                try
                {
                    var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                    .AddJsonFile("appsettings.json");
                    //AppContext.BaseDirectory + "appsettings.json";
                    IConfiguration configuration = builder.Build();
                    String Sql = configuration["Configs:DBConnectionString"];
                    using (NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sqrstr, Sql))
                    {
                        sqldap.Fill(ds);
                    }
                    error = true;
                    return ds;
                }
                catch (Exception)
                {
                    using (NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sqrstr, "PORT=5432;DATABASE=linyi;HOST=localhost;PASSWORD=tyaimap;USER ID=postgres"))
                    {
                        sqldap.Fill(ds);
                    }
                    error = true;
                    return ds;
                }
                
            }
            catch (System.Exception ex)
            {
                error = false;

                try
                {
                    _Log.WriteLog("数据库报错", ex.Message + " 语句：" + sqrstr + System.Environment.NewLine);
                }
                catch (Exception)
                {

                }
                return ds;
            }
        }
    }
}
