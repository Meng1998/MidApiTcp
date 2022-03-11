using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DeploymentTools.Logic
{
    public class AppConfig
    {
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>
        /// 获取指定节点的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                string value = config.AppSettings.Settings[key].Value;
                return value;
            }
            return null;
        }

        /// <summary>
        /// 增加一个appsetting节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSetting(string key, string value)
        {
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                //如果不 存在就增加一个节点
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// 更新appsetting节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void UpdateAppSettings(string key, string value)
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                //如果当前节点存在，则更新当前节点
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }


        /// <summary>
        /// 删除appsetting节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void DeleteAppSettings(string key, string value)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                //如果当前节点存在，则删除当前节点
                config.AppSettings.Settings.Remove(key);
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static void XmlSaveAppsetting(string AppKey, string AppValue)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                //获取App.config文件绝对路径
                String basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                basePath = basePath.Substring(0, basePath.Length - 10);
                String path = basePath + "App.config";
                xDoc.Load(path);

                XmlNode xNode;
                XmlElement xElem1;
                XmlElement xElem2;
                //修改完文件内容，还需要修改缓存里面的配置内容，使得刚修改完即可用
                //如果不修改缓存，需要等到关闭程序，在启动，才可使用修改后的配置信息
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
                if (xElem1 != null)
                {
                    xElem1.SetAttribute("value", AppValue);
                    cfa.AppSettings.Settings[$"{AppKey}"].Value = AppValue;
                }
                else
                {
                    xElem2 = xDoc.CreateElement("add");
                    xElem2.SetAttribute("key", AppKey);
                    xElem2.SetAttribute("value", AppValue);
                    xNode.AppendChild(xElem2);
                    cfa.AppSettings.Settings.Add(AppKey, AppValue);
                }
                // 保存对配置文件所作的更改 
                config.Save(ConfigurationSaveMode.Modified);
                // 强制重新载入配置文件的ConnectionStrings配置节  
                ConfigurationManager.RefreshSection("appSettings");

                xDoc.Save(path);

                //Properties.Settings.Default.Reload();
            }
            catch (Exception e)
            {
                string error = e.Message;

            }
        }

    }
}
