using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace WEBAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private readonly string AllowSpecificOrigin = "AllowSpecificOrigin";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v2", new OpenApiInfo { Title = "图洋综合集成", Version = "v2" });
                //// 获取xml文件路径
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                //// 添加控制器层注释，true表示显示控制器注释
                //c.IncludeXmlComments(xmlPath, true);
            });

            {
                #region 跨域
                services.AddCors(options =>
                {
                    options.AddPolicy(AllowSpecificOrigin,
                        builder =>
                        {
                            builder.AllowAnyMethod()
                                .AllowAnyOrigin()
                                .AllowAnyHeader();
                        });
                });
                #endregion
                //配置返回Json
                services.AddControllersWithViews().AddNewtonsoftJson();
            }
           
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
       


            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {//ISC_API/
               // c.SwaggerEndpoint("/swagger/v2/swagger.json", "GolmudIntegratedPlatform API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

          
            app.UseHttpsRedirection();
            //设置远程
            app.UseRouting();
            //CORS 中间件必须配置为在对 UseRouting 和 UseEndpoints的调用之间执行。 配置不正确将导致中间件停止正常运行。
            app.UseCors(AllowSpecificOrigin);
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            _ = app.UseAuthorization();

        }
    }
}
