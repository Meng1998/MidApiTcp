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

            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v2", new OpenApiInfo { Title = "ͼ���ۺϼ���", Version = "v2" });
                //// ��ȡxml�ļ�·��
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                //// ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
                //c.IncludeXmlComments(xmlPath, true);
            });

            {
                #region ����
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
                //���÷���Json
                services.AddControllersWithViews().AddNewtonsoftJson();
            }
           
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
       


            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
            app.UseSwaggerUI(c =>
            {//ISC_API/
               // c.SwaggerEndpoint("/swagger/v2/swagger.json", "GolmudIntegratedPlatform API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

          
            app.UseHttpsRedirection();
            //����Զ��
            app.UseRouting();
            //CORS �м����������Ϊ�ڶ� UseRouting �� UseEndpoints�ĵ���֮��ִ�С� ���ò���ȷ�������м��ֹͣ�������С�
            app.UseCors(AllowSpecificOrigin);
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            _ = app.UseAuthorization();

        }
    }
}
