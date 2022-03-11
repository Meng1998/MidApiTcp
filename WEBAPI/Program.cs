using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LMWEBAPI.Encryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WEBAPI.C;
using LMWEBAPI.ClearCache;
using LMWEBAPI.C.Hik;

namespace WEBAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //new Initevery();
            //new Encryption().InitRestart();
           // new SerilogClass().Slog();


            TCPoperation Tcpinit = new TCPoperation();
            Tcpinit.TcpInit();
            //ClearCache.init();
             var host = new WebHostBuilder()
                .UseUrls("http://localhost:5000")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();
            CreateHostBuilder(args).Build().Run();
        }
    
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
