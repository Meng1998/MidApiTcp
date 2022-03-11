using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WEBAPI.C
{
    public class SerilogClass
    {
        /// <summary>
        /// Serilog的初始化
        /// </summary>
        public void Slog() {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(DateTime.Now.ToString("yyyyMM") + "logs", $"log.txt"),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .CreateLogger();

            //.MinimumLevel.Information()
            //// 日志调用类命名空间如果以 Microsoft 开头，覆盖日志输出最小级别为 Information
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //.Enrich.FromLogContext()
            //// 配置日志输出到控制台
            //.WriteTo.Console()
            //// 配置日志输出到文件，文件输出到当前项目的 logs 目录下
            //// 日记的生成周期为每天
            //.WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day)
            //// 创建 logger
            //.CreateLogger();

            //这里因为设置了最低等级为Debug，
            //所以比Debug低的Verbose不会展示在控制台
       
        }

    }
}
