using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using static DeploymentTools.Mod.ListDataMOD;

namespace ISCwebApi.Controllers
{
    public class EstavlishIIS
    {
        ConfigIIS config = new ConfigIIS();
        public Int32 EstavlishISCAPI(String name,String Pathcss,String port) {
            if (!System.IO.Directory.Exists(Pathcss))//判断文件夹是否存在 
            {
                return -1;
            }

            
            Int32 IISID =
            config.CreateWebSite(name, Pathcss, true, "", port);///{3}主机名
            return IISID;

        }

        // 选择路径
        public static string SelectPath()
        {
            string path = string.Empty;
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹


            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                path = openFileDialog.SelectedPath;
            }
            return path;
        }

    }
}
