using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeploymentTools.View
{
    /// <summary>
    /// ServiceRunningStatus.xaml 的交互逻辑
    /// </summary>
    public partial class ServiceRunningStatus : Window
    {
        System.Timers.Timer ServerStatrT = new System.Timers.Timer();
        public ServiceRunningStatus()
        {
            InitializeComponent();

            ServerStatrT.Interval = 2000;
            ServerStatrT.Start();
            ServerStatrT.Elapsed += ServerStatrTC;
        }
        Boolean WEBAPIStatr = false;
        Boolean NGINXStatr = false;
        Boolean LntegratedMiddlewareStatr = false;
        Boolean DatabaseAPIStatr = false;
        /// <summary>
        /// 刷新服务状态UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerStatrTC(object sender, ElapsedEventArgs e)
        {
            Stack.Dispatcher.Invoke(new Action(() =>
            {
                Process[] LntegratedMiddleware = Process.GetProcessesByName("LntegratedMiddleware");
                if (LntegratedMiddleware.Length == 0)
                {
                    LntegratedMiddlewareBUT.Content = "当前运行状态(未启用)";
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFF3333");
                    LntegratedMiddlewareBUT.Background = new SolidColorBrush(color);
                    LntegratedMiddlewareStatr = false;
                }
                else
                {
                    LntegratedMiddlewareBUT.Content = "当前运行状态(正常)";
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CC00FF8B");
                    LntegratedMiddlewareBUT.Background = new SolidColorBrush(color);
                    LntegratedMiddlewareStatr = true;
                }

                Process[] nginx = Process.GetProcessesByName("MapNginx");
                if (nginx.Length == 0)
                {
                    NGINXBUT.Content = "当前运行状态(未启用)";
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFF3333");
                    NGINXBUT.Background = new SolidColorBrush(color);
                    NGINXStatr = false;
                }
                else
                {
                    NGINXBUT.Content = "当前运行状态(正常)";
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CC00FF8B");
                    NGINXBUT.Background = new SolidColorBrush(color);
                    NGINXStatr = true;
                }

                Process[] WEBAPI = Process.GetProcessesByName("LMWEBAPI");
                if (WEBAPI.Length == 0)
                {
                    WEBAPIBUT.Content = "当前运行状态(未启用)";
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFF3333");
                    WEBAPIBUT.Background = new SolidColorBrush(color);
                    WEBAPIStatr = false;
                }
                else { 
                    WEBAPIBUT.Content = "当前运行状态(正常)";
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CC00FF8B");
                    WEBAPIBUT.Background = new SolidColorBrush(color);
                    WEBAPIStatr = true;
                }

                Process[] DatabaseAPIList = Process.GetProcessesByName("WEBAPI");
                if (DatabaseAPIList.Length == 0)
                {
                    DatabaseAPI.Content = "当前运行状态(未启用)";
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFF3333");
                    DatabaseAPI.Background = new SolidColorBrush(color);
                    DatabaseAPIStatr = false;
                }
                else {
                    DatabaseAPI.Content = "当前运行状态(正常)";
                    System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CC00FF8B");
                    DatabaseAPI.Background = new SolidColorBrush(color);
                    DatabaseAPIStatr = true;
                }
            }));

        }

        private void LntegratedMiddlewareBUT_Click(object sender, RoutedEventArgs e)
        {
            Boolean statr = false;
            Process[] LntegratedMiddleware = Process.GetProcessesByName("LntegratedMiddleware");
            if (LntegratedMiddleware.Length != 0)
                foreach (Process process in LntegratedMiddleware) process.Kill();
            if (!LntegratedMiddlewareStatr)
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                foreach (var item in files)
                {
                    if (item.Name == "3D地图中间件集成.lnk")
                    {
                        statr = true;
                        System.Diagnostics.Process.Start(item.FullName);
                    }
                }
            }
            else statr = true;
            if (!statr)
                MessageBox.Show("服务未安装！");

        }

        private void NGINXBUT_Click(object sender, RoutedEventArgs e)
        {
            Boolean statr = false;
            Process[] nginx = Process.GetProcessesByName("MapNginx");
            if (nginx.Length != 0)
                foreach (Process process in nginx) process.Kill();
            if (!NGINXStatr)
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                foreach (var item in files)
                {
                    if (item.Name == "3D地图WEB服务器.lnk")
                    {
                        statr = true;
                        try
                        {
                            System.Diagnostics.Process.Start(item.FullName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message,"错误");
                        }
                       
                    }
                }

            }
            else statr = true;
            if (!statr)
                MessageBox.Show("服务未安装！");
        }

        private void WEBAPIBUT_Click(object sender, RoutedEventArgs e)
        {
            Boolean statr = false;
            Process[] WEBAPI = Process.GetProcessesByName("LMWEBAPI");
            if (WEBAPI.Length != 0)
                foreach (Process process in WEBAPI) process.Kill();
            if (!WEBAPIStatr)
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                foreach (var item in files)
                {
                    if (item.Name == "3D地图WEB服务器.lnk")
                    {
                        statr = true;
                        System.Diagnostics.Process.Start(item.FullName);
                    }
                }
            }
            else statr = true;
            if (!statr)
                MessageBox.Show("服务未安装！");
        }

        private void DatabaseAPI_Click(object sender, RoutedEventArgs e)
        {
            Boolean statr = false;
            Process[] DatabaseAPIList = Process.GetProcessesByName("WEBAPI");
            if (DatabaseAPIList.Length != 0)
                foreach (Process process in DatabaseAPIList) process.Kill();
            if (!DatabaseAPIStatr)
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                foreach (var item in files)
                {
                    if (item.Name == "数据操作API.lnk")
                    {
                        statr = true;
                        System.Diagnostics.Process.Start(item.FullName);
                    }
                }
            }
            else statr = true;
            if (!statr)
                MessageBox.Show("服务未安装！");
            
        }
    }
}
