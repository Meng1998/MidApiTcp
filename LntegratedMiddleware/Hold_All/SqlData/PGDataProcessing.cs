using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;
using System;
using System.Data;
using System.IO;

namespace LntegratedMiddleware.SqlData
{
    class PGDataProcessing
    {
        public void TestExecuteQuery()
        {

            ExecuteQuery($"SELECT * FROM \"public\".\"event_type\";", out Boolean error);
            if (error)
            {
                Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                Console.BackgroundColor = ConsoleColor.Green; //设置背景色

                Log.Debug($"PGSQL OK. ");//正在启动 WebSocket和终端通信

                Console.ResetColor(); //将控制台的前景色和背景色设为默认值
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                Console.BackgroundColor = ConsoleColor.Red; //设置背景色

                Log.Debug($"PGSQL ERROR! May be appsettings.json Cofnig DBConnectionString Problem with field:{errormsg}");//正在启动 WebSocket和终端通信


                Console.ResetColor(); //将控制台的前景色和背景色设为默认值
            }


        }
        String errormsg = null;
        public DataSet ExecuteQuery(string sqrstr, out Boolean error)
        {
            error = true; String Sql = null;
            DataSet ds = new DataSet();
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                .AddJsonFile("appsettings.json");
                //AppContext.BaseDirectory + "appsettings.json";
                IConfiguration configuration = builder.Build();
                //"PORT=5432;DATABASE=aimap;HOST=192.168.0.69;PASSWORD=;USER ID=postgres",
                Sql = $"PORT={configuration["Configs:PORT"]};DATABASE={configuration["Configs:DATABASE"]};HOST={configuration["Configs:HOST"]};PASSWORD={configuration["Configs:PASSWORD"]};USER ID=postgres";// 
                using (NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sqrstr, Sql))
                {
                    sqldap.Fill(ds);
                }
                error = true;
                return ds;
            }
            catch (System.Exception ex)
            {
                error = false;
                Log.Debug($"({Sql})MQTT数据库存储信息报错." + ex.Message);
                errormsg = ex.Message;
                return ds;
            }
        }
    }
}
