using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.DirectoryServices;
using System.Dynamic;
using System.Collections.ObjectModel;
using static DeploymentTools.Mod.ListDataMOD;
using Microsoft.Web.Administration;
using System.Net;

namespace ISCwebApi.Controllers
{
    /// <summary>
    ///ConfigIIS 的摘要说明
    /// </summary>
    public class ConfigIIS
    {

        public int CreateWebSite(string webSiteName, string pathToRoot, bool createDir, string website,String port)
        {
            DirectoryEntry root = new DirectoryEntry("IIS://localhost/W3SVC");
            // Find unused ID value for new web site
            int siteID = 1;

            foreach (DirectoryEntry e in root.Children)
            {
                if (e.SchemaClassName == "IIsWebServer")
                {
                    int ID = Convert.ToInt32(e.Name);
                    if (ID >= siteID)
                    {
                        siteID = ID + 1;
                    }
                    if ((String)e.Properties["ServerComment"].Value == webSiteName)///判断iis是否拥有过站点
                    {
                        return -1;
                    }

                }

            }
            string AppPoolName = webSiteName;
            {//生成应用池

                if (!IsAppPoolName(AppPoolName))
                {
                    DirectoryEntry newpool;
                    DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                    newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
                    newpool.CommitChanges();
                    //MessageBox.Show(AppPoolName + "程序池增加成功");
                }


                ServerManager sm = new ServerManager();
                sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion = "";//v4.0 null无托管代码
                sm.ApplicationPools[AppPoolName].ManagedPipelineMode = ManagedPipelineMode.Integrated; //托管模式Integrated为集成 Classic为经典
                sm.CommitChanges();
                //MessageBox.Show(AppPoolName + "程序池托管管道模式：" + sm.ApplicationPools[AppPoolName].ManagedPipelineMode.ToString() + "运行的NET版本为:" + sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion);
                //IsAppPoolName(webSiteName);
            }
            // Create web site

            //将一个应用程序（Application）添加到一个站点 
            //iisManager.Sites[siteName].Applications.Add("/" + siteName, physicalPath);

            DirectoryEntry site = (DirectoryEntry)root.Invoke("Create", "IIsWebServer", siteID);
            site.Invoke("Put", "ServerComment", webSiteName);
            site.Invoke("Put", "KeyType", "IIsWebServer");
            //site.Invoke("Put", "ServerBindings", ":8080:");
            site.Invoke("Put", "ServerState", 2);
            site.Invoke("Put", "FrontPageWeb", 1);
            site.Invoke("Put", "DefaultDoc", "Default.aspx");
            //site.Invoke("Put", "SecureBindings", ":443:");//https
            site.Invoke("Put", "ServerAutoStart", 1);
            site.Invoke("Put", "ServerSize", 1);
            site.Invoke("SetInfo");
            // Create application virtual directory
            DirectoryEntry siteVDir = site.Children.Add("Root", "IISWebVirtualDir");
            siteVDir.Properties["AppIsolated"][0] = 2;
            siteVDir.Properties["Path"][0] = pathToRoot;
            siteVDir.Properties["AccessFlags"][0] = 513;
            siteVDir.Properties["FrontPageWeb"][0] = 1;
            siteVDir.Properties["AppRoot"][0] = "LM/W3SVC/" + siteID + "/Root";
            siteVDir.Properties["AppFriendlyName"][0] = "Root";

            AssignAppPool(siteVDir, AppPoolName);//设置站点应用池
            //增加主机头（站点编号.ip.端口.域名） 
            PropertyValueCollection serverBindings = site.Properties["ServerBindings"];
            string headerStr = string.Format("{0}:{1}:{2}", "*", port, website);//{0}iis站点的ip地址 //{2}网站端口
            if (!serverBindings.Contains(headerStr))
            {
                serverBindings.Add(headerStr);
            }

            siteVDir.CommitChanges();
            site.CommitChanges();
            return siteID;
        }
        /// <summary>
        /// 修改站点的应用池
        /// </summary>
        /// <param name="rootEntry"></param>
        /// <param name="AppPoolName"></param>
        public static void AssignAppPool(DirectoryEntry rootEntry, string AppPoolName)
        {
            object[] param = { 0, AppPoolName, true };
            rootEntry.Invoke("AppCreate3", param);
            rootEntry.Properties["AppFriendlyName"].Value = AppPoolName;
            rootEntry.CommitChanges();
        }
        /// <summary>
        /// 判断程序池是否存在
        /// </summary>
        /// <param name="AppPoolName">程序池名称</param>
        /// <returns>true存在 false不存在</returns>
        private bool IsAppPoolName(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
