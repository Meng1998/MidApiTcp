using ICSharpCode.SharpZipLib.Zip;
using ISCwebApi.Controllers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static DeploymentTools.Mod.ComBoxMod;
using IWshRuntimeLibrary;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using DeploymentTools.Logic;
using System.Configuration;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Diagnostics;

namespace DeploymentTools.View
{
   
    /// <summary>
    /// LnstallLMWEBAPI.xaml 的交互逻辑
    /// </summary>
    public partial class LnstallWEBAPI : Window
    {
        Boolean ZipState = false;
        private ModelUnZipProgress modelUnZipProgress = new ModelUnZipProgress();
        Zip zip = new Zip();

        EstavlishIIS IIS = new EstavlishIIS();
        ObservableCollection<ItemContent> list = new ObservableCollection<ItemContent>();
        ObservableCollection<ItemContentIP> IPlist = new ObservableCollection<ItemContentIP>();
        public LnstallWEBAPI()
        {
            InitializeComponent();
            LabelPathcss.Content = Pathcss.Split('\\')[Pathcss.Split('\\').Length - 1];
            list = new ObservableCollection<ItemContent>();
            list.Add(new ItemContent() { Name = "选择API安装包", CameraId = null });
            //第一种方法
            if (!Directory.Exists("APIZip"))//判断是否存在
            {
                Directory.CreateDirectory("APIZip");//创建新路径
            }

            string[] files = Directory.GetFiles("APIZip", "*.zip");
            foreach (string file in files)
            {
                list.Add(new ItemContent() { Name = file.Split('\\')[file.Split('\\').Length - 1].Split('.')[0], CameraId = file });
                Console.WriteLine(file);
            }
            {
                this.comboBox1.ItemsSource = list;
                this.comboBox1.DisplayMemberPath = "Name";
                comboBox1.SelectedIndex = 0;
            }

            IPlist = new ObservableCollection<ItemContentIP>();
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            List<IPAddress> ipv4 = new List<IPAddress>();
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPlist.Add(new ItemContentIP() { Name = ipa.ToString(), IP = ipa });
                    

                }
            }
            try
            {
                if (AppConfig.GetAppSetting("Server") != "")
                {
                    IPlist.Add(new ItemContentIP() { Name = AppConfig.GetAppSetting("Server"), IP = IPAddress.Parse(AppConfig.GetAppSetting("Server")) });
                }
            }
            catch (Exception)
            {

            }
            
            {
                this.comboBox1_Copy.ItemsSource = IPlist;
                this.comboBox1_Copy.DisplayMemberPath = "Name";
                comboBox1_Copy.SelectedIndex = 0;
            }
            portTxt.Text= Config.GetConfigValue("Port");
            SQLTXTPATH.Text = Config.GetConfigValue("Database");
            SQLTXTPATH_Copy1.Text = Config.GetConfigValue("Pwd");

            ParasiticSettings.Visibility = Visibility.Hidden;
            progressBar.Visibility = Visibility.Hidden;
            //设置进度委托
            zip.unzipTotalReadProgress += UnzipTotalProgressChangelistener;
            zip.unzipFileReadProgress += UnzipFileProgressChangelistener;
            //设置数据上下文
            DataContext = modelUnZipProgress;
            //{
            //    EventListData.ItemsSource = new ConfigIIS().GetListIISname();
            //}
            
            try
            {
                var edition = Get.HttpGet("http://120.78.72.49:8009/");
                if (!String.IsNullOrEmpty(edition))
                {
                    long editionServer = long.Parse(edition);
                    long editionLocal = long.Parse(ConfigurationManager.ConnectionStrings["edition"].ToString());
                    if (editionServer > editionLocal)
                    {
                        MessageBox.Show($"当前地图版本过低 最新版本为:{editionServer.ToString()}  ,当前版本为{editionLocal.ToString()}", "更新");
                    }
                }

            }
            catch (Exception)
            {

            }
            
        }

        //public static FastZip fz = new FastZip();
        ///// <summary>
        ///// 解压Zip
        ///// </summary>
        ///// <param name="DirPath">解压后存放路径</param>
        ///// <param name="ZipPath">Zip的存放路径</param>
        ///// <param name="ZipPWD">解压密码（null代表无密码）</param>
        ///// <returns></returns>
        //public static string Compress(string DirPath, string ZipPath, string ZipPWD)
        //{
        //    string state = "Fail...";
        //    try
        //    {
        //        fz.Password = ZipPWD;
        //        fz.ExtractZip(ZipPath, DirPath, null);

        //        state = "Success !";
        //    }
        //    catch (Exception ex)
        //    {
        //        state += "," + ex.Message;
        //    }
        //    return state;
        //}
        //判断文件夹是否存在

        private void CreateDir(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

        }

        private void th(String Pathstr, String Replacestr1, String Replacestr2) {

            List<String> txt = new List<String>();
            using (StreamReader sr = new StreamReader(Pathstr, Encoding.UTF8))
            {
                int lineCount = 0;
                while (sr.Peek() > 0)
                {
                    lineCount++;
                    string temp = sr.ReadLine();
                    txt.Add(temp);
                }
            }

            for (int i = 0; i < txt.Count; i++)
            {
                txt[i] = txt[i].Replace(Replacestr1, Replacestr2);
            }

            using (FileStream fs = new FileStream(Pathstr, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    for (int i = 0; i < txt.Count; i++)
                    {
                        sw.WriteLine(txt[i]);
                    }
                }
            }
        }
        //解压进度回调 总进度
        public void UnzipTotalProgressChangelistener(int count, int index)
        {
            modelUnZipProgress.TotalCoutn = count;
            modelUnZipProgress.TotalIndex = index;
            if (count == index)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    control.Visibility = Visibility.Visible;
                    progressBar.Visibility = Visibility.Hidden;
                    SynchronizationButton.IsEnabled = true;
                    //设置解压完成后的操作
                    {
                        ItemContent path = this.comboBox1.SelectedItem as ItemContent;
                        String Apipath = Pathcss + @"\" + path.Name + @"\Core\LMWEBAPI.exe";
                        String Serverpath = Pathcss + @"\" + path.Name + @"\Server\LntegratedMiddleware.exe";
                        String Nginxpath = Pathcss + @"\" + path.Name + @"\Core\nginx-1.17.7\start.bat";
                        String WebApipath = Pathcss + @"\" + path.Name + @"\WebApi\webapi.exe";


                        //RegisterAutoStartUP("LMWEBAPI", Apipath);
                        //RegisterAutoStartUP("LntegratedMiddleware", Serverpath);
                        AddShortcut(Apipath, Pathcss + @"\" + path.Name + @"\Core\", "3D地图对外接口", true, System.AppDomain.CurrentDomain.BaseDirectory + "icon\\20200408111636574_easyicon_net_64.ico");
                        AddShortcut(Nginxpath, Pathcss + @"\" + path.Name + @"\Core\nginx-1.17.7\", "3D地图WEB服务器", true, System.AppDomain.CurrentDomain.BaseDirectory + "icon\\20200408111641868_easyicon_net_64.ico");
                        AddShortcut(Serverpath, Pathcss + @"\" + path.Name + @"\Server\", "3D地图中间件集成", true, System.AppDomain.CurrentDomain.BaseDirectory + "icon\\20200408111632712_easyicon_net_64.ico");
                        AddShortcut(WebApipath, Pathcss + @"\" + path.Name + @"\WebApi\", "数据操作API", true, "");


                        Config.UpdateConnectionStringsConfig("InstallationPath", Pathcss + @"\" + path.Name);

                        var sqlname = SQLTXTPATH.Text;
                        var PASSWORD = SQLTXTPATH_Copy1.Text;
                        var ip = comboBox1_Copy.Text;


                       


                        th(Pathcss + @"\" + path.Name + @"\Core\nginx-1.17.7\conf\nginx.conf", "[&root&]", Pathcss + @"\" + path.Name);


                        th(Pathcss + @"\" + path.Name + @"\Core\nginx-1.17.7\conf\nginx.conf", "[&PORT&]", portTxt.Text);
                        th(Pathcss + @"\" + path.Name + @"\Core\appsettings.json", "[&dataname&]", sqlname);
                        th(Pathcss + @"\" + path.Name + @"\Core\appsettings.json", "[&host&]", ip);
                        th(Pathcss + @"\" + path.Name + @"\Core\appsettings.json", "[&password&]", PASSWORD);

                        th(Pathcss + @"\" + path.Name + @"\Server\appsettings.json", "[&dataname&]", sqlname);
                        th(Pathcss + @"\" + path.Name + @"\Server\appsettings.json", "[&host&]", ip);
                        th(Pathcss + @"\" + path.Name + @"\Server\appsettings.json", "[&password&]", PASSWORD);

                        var ToggleState = (Boolean)Toggle.IsChecked;
                        th(Pathcss + @"\" + path.Name + @"\WebApi\config\default.json", "[&host&]", ToggleState ? ip :"localhost");
                        th(Pathcss + @"\" + path.Name + @"\WebApi\config\default.json", "[&password&]", PASSWORD);

                        th(Pathcss + @"\" + path.Name + @"\build\config.json", "[&IP&]", ip + ":" + portTxt.Text);
                        th(Pathcss + @"\" + path.Name + @"\build\config.json", "[&NOPORTIP&]", ip);
                        th(Pathcss + @"\" + path.Name + @"\build\config.json", "[&sqlname&]", sqlname);


                        th(Pathcss + @"\" + path.Name + @"\Server\appsettings.json", "[&IP&]", ip);
                        th(Pathcss + @"\" + path.Name + @"\Server\appsettings.json", "[&password&]", PASSWORD);
                        th(Pathcss + @"\" + path.Name + @"\Server\appsettings.json", "[&sqlname&]", sqlname);


                        //Config.GetConfigValue("PortTxt");
                        Boolean ell = true;
                        new PGDataProcessing().ExecuteQuery($"UPDATE \"public\".\"sys_config\" SET \"data_server_uri\" = 'http://{ip}:{portTxt.Text}/{sqlname}' WHERE \"id\" = '1'", ref ell);

                        InformationTips F2 = new InformationTips();
                        F2.Init("安装成功", $"API已经安装完成" + (ell ? "" : "但有点小问题数据库无法连接，请到平台手动设置IP及端口"));
                        F2.ShowDialog();
                        if (F2.needChangeUI)
                        {
                            //OK
                        }
                    }
                    ZipState = false;
                }));
            }

        }
        //解压进度回调 单文件进度
        public void UnzipFileProgressChangelistener(string fileName, long count, long index)
        {
            modelUnZipProgress.Filename = fileName;
            modelUnZipProgress.FileCoutn = count;
            modelUnZipProgress.FileIndex = index;
        }
        /// <summary>
        /// 修改文件夹名称
        /// </summary>
        /// <param name="currentPath">当前文件夹路径</param>
        /// <param name="targetPath">目标文件夹路径（即修改后的文件夹）</param>
        private void UpdateDirectoryName(string currentPath, string targetPath)
        {
            if (!Directory.Exists(currentPath))
            {
                return;
            }

            if (Directory.Exists(targetPath))
            {
                return;
            }

            Directory.Move(currentPath, targetPath);
        }
        private void Lnstall_Click(object sender, RoutedEventArgs e)
        {
            int[] ports = new int[] 
            {3000
            ,8090
            ,8082
            ,5000
            ,4649
            ,9999
            ,int.Parse(portTxt.Text)};
            var portsStaret = true;
            for (int i = 0; i < ports.Length; i++)
            {
                if (PortInUse(ports[i])) {
                    switch (ports[i])
                    {
                        case 3000:
                            //portsStaret
                            portsStaret = false;
                            MessageBox.Show("数据API端口被占用,是否忘记关闭地图的服务！,无法继续安装, 端口为：" + ports[i], "端口检测不通过！");
                            break;
                        case 8090:
                            //portsStaret
                            portsStaret = false;
                            MessageBox.Show("数据API端口被占用,是否忘记关闭地图的服务！,无法继续安装, 端口为：" + ports[i], "端口检测不通过！");
                            break;
                        case 8082:
                            //portsStaret
                            portsStaret = false;
                            MessageBox.Show("3D地图中间件集成被占用,是否忘记关闭地图的服务！,无法继续安装, 端口为：" + ports[i], "端口检测不通过！");
                            break;
                        case 5000:
                            //portsStaret
                            portsStaret = false;
                            MessageBox.Show("3D地图中间件集成端口被占用,是否忘记关闭地图的服务！,无法继续安装, 端口为：" + ports[i], "端口检测不通过！");
                            break;
                        case 9999:
                            //portsStaret
                            portsStaret = false;
                            MessageBox.Show("3D地图中间件集成交互api端口被占用,是否忘记关闭地图的服务！,无法继续安装, 端口为：" + ports[i], "端口检测不通过！");
                            break;
                        case 4649:
                            //portsStaret
                            portsStaret = false;
                            MessageBox.Show("3D地图中间件集成 对外转发服务端口被占用,是否忘记关闭地图的服务！,无法继续安装, 端口为：" + ports[i], "端口检测不通过！");
                            break;
                        default:
                            portsStaret = false;
                            MessageBox.Show("未知端口被占用,无法继续安装 但被开发人员设置成为预设端口！ 端口为：" + ports[i], "端口检测不通过！");
                            break;
                    }
                    break;
                }
            }
            if (!portsStaret) { return; }
            String DataPath = ConfigurationManager.ConnectionStrings["DataPath"].ToString();
            if (DataPath != "") {
                try
                {
                    Directory.SetCurrentDirectory(Directory.GetParent(DataPath).FullName);
                    var paths = Directory.GetCurrentDirectory();
                    UpdateDirectoryName(DataPath, paths + "\\" + Config.GetConfigValue("Database"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("自动修改文件夹错误："+ ex.Message + " 稍后请手动修改数据文件夹名称为：" + Config.GetConfigValue("Database") + ", 请勿使用中文名称！");
                   
                }
              
            }

            try
            {

                ItemContent path = this.comboBox1.SelectedItem as ItemContent;

                if (String.IsNullOrEmpty(path.CameraId) || String.IsNullOrEmpty(Pathcss)) {

                    InformationTips F2 = new InformationTips();
                    F2.Init("错误信息", $"API安装包未选择或安装路径未选择");
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
                else
                {
                    if (Regex.IsMatch(Pathcss, @"[\u4e00-\u9fa5]"))
                    {
                        InformationTips F2 = new InformationTips();
                        F2.Init("安装", $"请勿安装在中文目录下!");
                        F2.ShowDialog();
                        if (F2.needChangeUI)
                        {
                            //OK
                        }
                        return;
                    }
                    CreateDir(Pathcss + @"\" + path.Name);
                    string ZipPath = AppDomain.CurrentDomain.BaseDirectory + path.CameraId;
                    string GoalDir = Pathcss + @"\" + path.Name;
                    if (!System.IO.File.Exists(ZipPath))
                    { MessageBox.Show("zip文件路径无效"); return; }
                    if (!Directory.Exists(GoalDir))
                    { MessageBox.Show("解压路径无效"); return; }
                    Task.Factory.StartNew(() =>
                    {

                        //开始解压
                        zip.UnZip(ZipPath, GoalDir);
                    });
                    progressBar.Visibility = Visibility.Visible;
                    control.Visibility = Visibility.Hidden;
                    SynchronizationButton.IsEnabled = false;
                    ZipState = true;
                }


            }
            catch (Exception ex)
            {
                InformationTips F2 = new InformationTips();
                F2.Init("错误信息", $"文件解压失败或，配置过程出现异常，请联系相关开发人员" + ex.Message);
                F2.ShowDialog();
                if (F2.needChangeUI)
                {
                    //OK
                }
            }

        }
        /// <summary>
        /// 更新UI
        /// </summary>
        public class ModelUpData : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName = null)
            {
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// 解压进度数据模型
        /// </summary>
        public class ModelUnZipProgress : ModelUpData
        {
            private long totalCoutn = 100;
            private long totalIndex;
            public long TotalCoutn { get { return totalCoutn; } set { totalCoutn = value; OnPropertyChanged(nameof(TotalCoutn)); } }
            public long TotalIndex { get { return totalIndex; } set { totalIndex = value; OnPropertyChanged(nameof(TotalIndex)); } }


            private long fileCoutn = 100;
            private long fileIndex;
            public long FileCoutn { get { return fileCoutn; } set { fileCoutn = value; OnPropertyChanged(nameof(FileCoutn)); } }
            public long FileIndex { get { return fileIndex; } set { fileIndex = value; OnPropertyChanged(nameof(FileIndex)); } }

            private string filename;
            public string Filename { get { return filename; } set { filename = value; OnPropertyChanged(nameof(Filename)); } }

            public bool btnIsEnabled = false;
            public bool BtnIsEnabled { get { return btnIsEnabled; } set { btnIsEnabled = value; OnPropertyChanged(nameof(BtnIsEnabled)); } }
        }
        public void AddShortcut(String Path, String WorkingDirectory, String Name, Boolean Selfstarting, String IconLocation)
        {
            

            //实例化WshShell对象 
            WshShell shell = new WshShell();

            //通过该对象的 CreateShortcut 方法来创建 IWshShortcut 接口的实例对象 
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{Name}.lnk");

            //设置快捷方式的目标所在的位置(源程序完整路径) 
            shortcut.TargetPath = Path;

            //应用程序的工作目录 
            //当用户没有指定一个具体的目录时，快捷方式的目标应用程序将使用该属性所指定的目录来装载或保存文件。 
            shortcut.WorkingDirectory = WorkingDirectory;

            //目标应用程序窗口类型(1.Normal window普通窗口,3.Maximized最大化窗口,7.Minimized最小化) 
            shortcut.WindowStyle = 1;

            //快捷方式的描述 
            shortcut.Description = "集中平台";

            //可以自定义快捷方式图标.(如果不设置,则将默认源文件图标.) 

            if (IconLocation != null && IconLocation != "") {
                shortcut.IconLocation = IconLocation;
            }

            //设置应用程序的启动参数(如果应用程序支持的话) 
            //shortcut.Arguments = "/myword /d4s"; 

            //设置快捷键(如果有必要的话.) 
            //shortcut.Hotkey = "CTRL+ALT+D"; 

            //保存快捷方式 
            shortcut.Save();

            if (Selfstarting)
            {
                string StartupPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartup);
                System.IO.File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{Name}.lnk", StartupPath + $"\\{Name}.lnk", true);
            }
        }

        /// <summary>
        /// 开机自启动 
        /// </summary>
        /// <param name="ButtinPath">执行文件路径</param>
        public void RegisterAutoStartUP(string KeyName, string ButtinPath)//"FacePC客户端"
        {

            using (RegistryKey rk = Registry.LocalMachine)
            {
                RegistryKey runKey = rk.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (runKey == null)
                {
                    runKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }

                try
                {
                    runKey.SetValue(KeyName, ButtinPath);
                    runKey.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    runKey.Close();
                }

            }
        }

        String Pathcss = @"D:\3DMAPTY";
        private void CurrentCatalogue_Click(object sender, RoutedEventArgs e)
        {
            Pathcss = EstavlishIIS.SelectPath();
            if (!String.IsNullOrEmpty(Pathcss))
            {
                LabelPathcss.Content = Pathcss.Split('\\')[Pathcss.Split('\\').Length - 1];
            }
            else {
                Pathcss = @"D:\3DMAPTY";
            }
        }
        /// <summary>
        /// 初始化数据库数据
        /// </summary>
        private void InitPostgre() {
          
            var ToggleState = (Boolean)Toggle.IsChecked;
            var sqlname = SQLTXTPATH.Text;
            var PASSWORD = SQLTXTPATH_Copy1.Text;
            var ip = comboBox1_Copy.Text;
            var ID = SQLTXTPATH_Copy.Text;

            DatabaseAddress.Text = $"PORT=5432;DATABASE={sqlname};HOST={ip};PASSWORD={PASSWORD};USER ID={ID}";
            Config.UpdateConnectionStringsConfig("postgre", DatabaseAddress.Text);
            //[&DatabaseName&] [&password&]
            
            
        }
        /// <summary>
        /// 是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

        int ellcount = 0;
        int yeslcount = 0;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var ToggleState = (Boolean)Toggle.IsChecked;
            AppConfig.UpdateAppSettings("Database", SQLTXTPATH.Text);
            AppConfig.UpdateAppSettings("Server", ToggleState ? comboBox1_Copy.Text : "localhost");
            AppConfig.UpdateAppSettings("Pwd", SQLTXTPATH_Copy1.Text);

            Config.UpdateConnectionStringsConfig("PortTxt", portTxt.Text);
            InitPostgre();
            Boolean ell = true;
            new PGDataProcessing().ExecuteQuery($"SELECT * FROM \"sys_user\"", ref ell);
            if (ell) { ellcount++; button.Content = $"连接正常 ({ellcount})"; }
            else { yeslcount++; button.Content = $"无法连接 ({yeslcount})"; }

        }

        private void Toggle_Copy_Checked(object sender, RoutedEventArgs e)
        {

            //if ((Boolean)Toggle_Copy.IsChecked)
            //{
            //    ParasiticSettings.Visibility = Visibility.Visible;
            //}
            //else {
            //    ParasiticSettings.Visibility = Visibility.Hidden;
            //}
            InitPostgre();


        }

        private void Toggle_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Toggle_Copy_Click(object sender, RoutedEventArgs e)
        {
            if ((Boolean)Toggle_Copy.IsChecked)
            {
                ParasiticSettings.Visibility = Visibility.Visible;
            }
            else
            {
                ParasiticSettings.Visibility = Visibility.Hidden;
            }

        }

        private void SQLTXTPATH_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            InitPostgre();

        }

        private void SQLTXTPATH_PreviewTextInput(object sender, SelectionChangedEventArgs e)
        {
            InitPostgre();
        }

      
        private void portTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

      
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ZipState)
            {
                e.Cancel = true;
                MessageBox.Show("违规操作!,安装时无法关闭当前窗口");
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void ButtonSQ_Click(object sender, RoutedEventArgs e)
        {
            string file = ""; string ip = comboBox1_Copy.Text + ":" + portTxt.Text;

            string database = Config.GetConfigValue("Database");
            // StreamReader sr = new StreamReader(txtfile, Encoding.UTF8);


            string path = string.Empty;
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹

          
            if (System.IO.File.Exists(@"D:\3DMAPTY"))
            {
                openFileDialog.SelectedPath = @"D:\3DMAPTY";
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                file = openFileDialog.SelectedPath;
                
                var request = (HttpWebRequest)WebRequest.Create($"http://{ip}/api/{database}/sys/authorize/licence");
                string json = null;
                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    json = json.Replace(" ", "");
                    json = json.Replace("{\"code\":0,\"msg\":\"success\",\"data\":{\"licence\":\"", "").Replace("\"}}", "");

                    if (string.IsNullOrEmpty(json)) return;
                    path = file + $"\\{database}.lic";
                    DelectDir(path);
                    FileStream fs = new FileStream(path, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(json);
                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    MessageBoxResult dr = MessageBox.Show("授权完成! 是否打开地图平台", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (dr == MessageBoxResult.OK)
                    {
                        Process proc = new System.Diagnostics.Process(); proc.StartInfo.FileName = $"http://localhost:{ConfigurationManager.ConnectionStrings["PortTxt"].ToString()}/build"; proc.Start();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "授权失败！ 请核查地图服务是否开启");
                    return;
                }
            }
            else {
                MessageBox.Show( "授权失败！ 用户手动取消");
            }




        }
        public void DelectDir(string srcPath)
        {
            if (System.IO.File.Exists(srcPath))
            {
                System.IO.File.Delete(srcPath);
            }
        }
        //private void Window_Closed(object sender, EventArgs e)
        //{

        //}
    }
}
