using LntegratedMiddleware.HIK.M;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LntegratedMiddleware.CQLJ
{
    class DHDataSet
    {
        private static List<DHSecretKey> DHusermodelList = new List<DHSecretKey>();
        public void InitParameters()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
            .AddJsonFile("appsettings.json");
            try
            {
                List<DHSecretKey> usermodelListKey = new List<DHSecretKey>();
                SecretKey Key = new SecretKey();
                {
                    IConfiguration configuration = builder.Build();


                    for (int i = 1; i < 1000; i++)
                    {
                        if (configuration[$"DH:{i}KeyAPI"] == "STOP")
                        {
                            break;
                        }

                        usermodelListKey.Add(new DHSecretKey()
                        {
                            API = configuration[$"DH:{i}KeyAPI"],
                            Port = Int32.Parse(configuration[$"DH:nPort"]),
                            Host = configuration[$"DH:szIp"],
                            user = configuration[$"DH:szUsername"],
                            pwd = configuration[$"DH:szPassword"]
                        });
                    }

                }//初始化ISCkey
                DHusermodelList = new List<DHSecretKey>(usermodelListKey);
            }
            catch (Exception ex)
            {

            }
        }

        public static DHSecretKey GetDHkey(Int32 index, Int32 type)
        {

            DHSecretKey key = new DHSecretKey();


          
                    return new DHSecretKey()
                    {

                        API = DHusermodelList[index].API,
                        Port = DHusermodelList[index].Port,
                        Host = DHusermodelList[index].Host,
                        user = DHusermodelList[index].user,
                        pwd = DHusermodelList[index].pwd,
                    };

          
        }
    }
}
