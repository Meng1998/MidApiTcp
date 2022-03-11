using DeploymentTools.Logic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeploymentTools.View
{
    /// <summary>
    /// EquipmentConfig.xaml 的交互逻辑
    /// </summary>
    public partial class EquipmentConfig : Window
    {
        NotifyIcon notifyIcon;
        public EquipmentConfig()
        {
            InitializeComponent();

            {
                this.notifyIcon = new NotifyIcon();
                this.notifyIcon.BalloonTipText = "欢迎使用图洋，摄像机配置工具!";
                this.notifyIcon.Text = "NotifyIcon";
                this.notifyIcon.Icon = Properties.Resources.Ico;
                TestTimer.Elapsed += TestAPI;

            }
            SQLTXTPATH.Text = ConfigurationManager.ConnectionStrings["postgre"].ToString();
            DataListUI();
        }
        ObservableCollection<ExpandoObject> items = new ObservableCollection<ExpandoObject>();

        private void DataListUI() {
            items = new ObservableCollection<ExpandoObject>();

            item = new ExpandoObject();
            item.NmaeStr = "ISC平台密钥参数";
                item.Key_Context = Config.GetConfigValue($"ISCKeyList:KeyContext");
                item.Key_Port = Int32.Parse(Config.GetConfigValue($"ISCKeyList:KeyPort"));
                item.Key_Host = Config.GetConfigValue($"ISCKeyList:KeyHost");
                item.Key_appKey = Config.GetConfigValue($"ISCKeyList:KeyappKey");
                item.Key_appSecret = Config.GetConfigValue($"ISCKeyList:KeyappSecret");
                item.Index = $"0";
                items.Add(item);

            item = new ExpandoObject();
            item.NmaeStr = "SPCC平台密钥参数";
                item.Key_Context = Config.GetConfigValue($"SPCCKeyList:KeyContext");
                item.Key_Port = Int32.Parse(Config.GetConfigValue($"SPCCKeyList:KeyPort"));
                item.Key_Host = Config.GetConfigValue($"SPCCKeyList:KeyHost");
                item.Key_appKey = Config.GetConfigValue($"SPCCKeyList:KeyappKey");
                item.Key_appSecret = Config.GetConfigValue($"SPCCKeyList:KeyappSecret");
                item.Index = $"1";
                items.Add(item);


            Load_animation.Visibility = Visibility.Hidden;
            EventListData.ItemsSource = items;

            
        }
        System.Timers.Timer TestTimer = new System.Timers.Timer();
        Int32 dataCount = 0;

         public class Objectitem
        {
            public String Key_Context { get; set; }
            public object Key_Port { get; set; }
            public String Key_Host { get; set; }
            public String Key_appKey { get; set; }
            public  String Key_appSecret { get; set; }
            public String NmaeStr { get; set; }
            public String Index { get; set; }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //HttpPost
            //ExpandoObjectitems = EventListData.SelectedItems;
            ExpandoObjectitems = new List<Objectitem>();
            foreach (var item in EventListData.SelectedItems) {
                dynamic a = (ExpandoObject)item;
                String Index = a.Index;
                String NmaeStr = a.NmaeStr;
                //ExpandoObject a = new ExpandoObject();
                ExpandoObjectitems.Add(
                    new Objectitem()
                    {
                        NmaeStr = NmaeStr,
                        Index = Index
                    }
                );
            }
            dataCount = EventListData.SelectedItems.Count;
            new GetDataSet().InitParameters();
            //System.Windows.MessageBox.Show(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString());
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString())) {
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&ISC-KeyContext&]", Config.GetConfigValue($"ISCKeyList:KeyContext"));
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&ISC-KeyPort&]", Config.GetConfigValue($"ISCKeyList:KeyPort"));
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&ISC-KeyHost&]", Config.GetConfigValue($"ISCKeyList:KeyHost"));
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&ISC-KeyappKey&]", Config.GetConfigValue($"ISCKeyList:KeyappKey"));
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&ISC-KeyappSecret&]", Config.GetConfigValue($"ISCKeyList:KeyappSecret"));

                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&SPCC-KeyContext&]", Config.GetConfigValue($"SPCCKeyList:KeyContext"));
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&SPCC-KeyPort&]", Config.GetConfigValue($"SPCCKeyList:KeyPort"));
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&SPCC-KeyHost&]", Config.GetConfigValue($"SPCCKeyList:KeyHost"));
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&SPCC-KeyappKey&]", Config.GetConfigValue($"SPCCKeyList:KeyappKey"));
                th(ConfigurationManager.ConnectionStrings["InstallationPath"].ToString() + @"\Server\appsettings.json", "[&SPCC-KeyappSecret&]", Config.GetConfigValue($"SPCCKeyList:KeyappSecret"));
            }


            
            DataListUI();


            TestTimer.Interval = 2000;
            TestTimer.AutoReset = false;
            TestTimer.Start();
            Load_animation.Visibility = Visibility.Visible;
        }

        private void th(String Pathstr, String Replacestr1, String Replacestr2)
        {

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

        dynamic item = new ExpandoObject();
        private void EventListData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

         
            Modify();
        }

        List<Objectitem> ExpandoObjectitems = new List<Objectitem>();// = new System.Collections.IList;
        private void TestAPI(object sender, ElapsedEventArgs e)
        {
      
            if (dataCount > 0)
            {
                TestTimer.Stop();
                Load_animation.Dispatcher.Invoke(new Action(() =>
                {
                    Load_animation.Visibility = Visibility.Visible;
                }));
                {
                    bt.Dispatcher.Invoke(new Action(() =>
                    {
                        bt.Content = "测试网关连接中.";
                        bt.IsEnabled = false;
                    }));
                }


                //EventListData.Dispatcher.Invoke(new Action(() =>
                //{
                //}));
                
                String ErrorStr = null;
                Int32 i = 0;//共有多少条数据
                foreach (var item in ExpandoObjectitems)
                {
                    i++;
                }
                foreach (var item in ExpandoObjectitems)
                {

                    Boolean bl = false;
                    InitKey Init = new InitKey();

                    String EegionText = Init.TestKetPost(1, 1, (item.Index));
                    if (!String.IsNullOrEmpty(EegionText) && EegionText != "error : 远程服务器无法连接")
                    {
                        bl = GetDataSet.GetISCmsgSuccessfulState(EegionText);
                        if (bl)
                        {
                            new GetDataSet().InitParameters();
                        }
                    }
                    Load_animation.Dispatcher.Invoke(new Action(() =>
                    {
                        Load_animation.Value += 100 / i;
                    }));


                    if (bl)
                    {
                        ErrorStr += $"{item.NmaeStr} 密钥连接平台: 连接正常\r\n";
                    }
                    else
                    {
                        ErrorStr += $"{item.NmaeStr} 密钥连接平台: 无法连接\r\n";
                    }

                }
                Load_animation.Dispatcher.Invoke(new Action(() =>
                {
                    Load_animation.Visibility = Visibility.Hidden;
                }));
                bt.Dispatcher.Invoke(new Action(() =>
                {
                   
                    bt.Content = "测试选中网关"; Load_animation.Value = 0;
                    bt.IsEnabled = true;

                    if (!String.IsNullOrEmpty(ErrorStr)) {
                        InformationTips F2 = new InformationTips();
                        F2.Init("网关测试信息", ErrorStr);
                        F2.ShowDialog();
                        if (F2.needChangeUI)
                        {
                            //OK
                        }
                    }
                    
                }));
            }

        }
        public async Task Modify() {
            
            await Task.Delay(500);
            ExpandoObjectitems = new List<Objectitem>();
            foreach (var item in EventListData.ItemsSource)
            {
                dynamic a = (ExpandoObject)item;
                String cIndex = a.Index;
                String cNmaeStr = a.NmaeStr;
                String cKey_Context = a.Key_Context;
                var    cKey_Port = a.Key_Port;
                String cKey_Host = a.Key_Host;
                String cKey_appKey = a.Key_appKey;
                String cKey_appSecret = a.Key_appSecret;
                //ExpandoObject a = new ExpandoObject();
                ExpandoObjectitems.Add(
                    new Objectitem()
                    {
                        Key_Context = cKey_Context,
                        Key_Port = cKey_Port,
                        Key_Host = cKey_Host,
                        Key_appKey = cKey_appKey,
                        Key_appSecret = cKey_appSecret,

                        NmaeStr = cNmaeStr,
                        Index = cIndex
                    }
                );
            }

            foreach (var item in ExpandoObjectitems)
            {

                await Task.Delay(1000);
                String NmaeStr = item.NmaeStr;
                String Key_Context = item.Key_Context;
                var Key_Port = item.Key_Port;
                String Key_Host = item.Key_Host;
                String Key_appKey = item.Key_appKey;
                String Key_appSecret = item.Key_appSecret;
                String Index = item.Index;

                if (!String.IsNullOrEmpty(NmaeStr)
                && !String.IsNullOrEmpty(Key_Context)
                && !String.IsNullOrEmpty(Key_Port.ToString())
                && !String.IsNullOrEmpty(Key_Host)
                && !String.IsNullOrEmpty(Key_appKey)
                && !String.IsNullOrEmpty(Key_appSecret)
                && CheckInt(Key_Port.ToString())
                )
                {
                    bool[] bl = new bool[5];
                    switch (Index)
                    {
                        case "0":
                            bl[0] = Config.SetConfigValue($"{"ISCKeyList:"}KeyContext", Key_Context);
                            bl[1] = Config.SetConfigValue($"{"ISCKeyList:"}KeyPort", Key_Port.ToString());
                            bl[2] = Config.SetConfigValue($"{"ISCKeyList:"}KeyHost", Key_Host);
                            bl[3] = Config.SetConfigValue($"{"ISCKeyList:"}KeyappKey", Key_appKey);
                            bl[4] = Config.SetConfigValue($"{"ISCKeyList:"}KeyappSecret", Key_appSecret);
                            break;
                        case "1":
                            bl[0] = Config.SetConfigValue($"{"SPCCKeyList:"}KeyContext", Key_Context);
                            bl[1] = Config.SetConfigValue($"{"SPCCKeyList:"}KeyPort", Key_Port.ToString());
                            bl[2] = Config.SetConfigValue($"{"SPCCKeyList:"}KeyHost", Key_Host);
                            bl[3] = Config.SetConfigValue($"{"SPCCKeyList:"}KeyappKey", Key_appKey);
                            bl[4] = Config.SetConfigValue($"{"SPCCKeyList:"}KeyappSecret", Key_appSecret);
                            break;
                    }
                    DataListUI();
                    Boolean error = true; String ErrorMsg = null;

                    String TypeName = null;
                    switch (Index)
                    {
                        case "0":
                            TypeName = "ISC相关配置";
                            break;
                        case "1":
                            TypeName = "SPCC相关配置";
                            break;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (!bl[i])
                        {
                            error = false;

                            switch (i)
                            {
                                case 0:

                                    ErrorMsg += $"{TypeName} API上下文修改错误\r\n";
                                    break;
                                case 1:
                                    ErrorMsg += $"{TypeName} 端口号修改错误\r\n";
                                    break;
                                case 2:
                                    ErrorMsg += $"{TypeName} 网关修改错误\r\n";
                                    break;
                                case 3:
                                    ErrorMsg += $"{TypeName} appKey修改错误\r\n";
                                    break;
                                case 4:
                                    ErrorMsg += $"{TypeName} appSecret修改错误\r\n";
                                    break;

                            }
                        }

                    }
                    if (!error)
                    {
                        InformationTips F2 = new InformationTips();
                        F2.Init("网关测试信息", ErrorMsg);
                        F2.ShowDialog();
                        if (F2.needChangeUI)
                        {
                            //OK
                        }

                        notifyIcon.Visible = true;
                        string info = null;
                        info = string.Format("名称:{0} 相关修改失败,不可以为空或某些字段只能为数字！", NmaeStr);
                        notifyIcon.ShowBalloonTip(1000, "自动保存失败", info, ToolTipIcon.Info);
                    }
                    else
                    {
                        notifyIcon.Visible = true;
                        string info = null;
                        info = string.Format("名称:{0} 相关修改,已经自动保存到程序配置文件！", NmaeStr);
                        notifyIcon.ShowBalloonTip(1000, "自动保存", info, ToolTipIcon.Info);
                    }



                }
                else
                {
                    notifyIcon.Visible = true;
                    string info = null;
                    info = string.Format("名称:{0} 相关修改失败,不可以为空或某些字段只能为数字！", NmaeStr);
                    notifyIcon.ShowBalloonTip(1000, "自动保存失败", info, ToolTipIcon.Info);
                }

            }



        }
        public bool CheckInt(string str)
        {
            try
            {
                Convert.ToInt32(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIcon.Dispose();
        }
        int ellcount = 0; int yeslcount = 0;
        private void bt2_Click(object sender, RoutedEventArgs e)
        {
            Config.UpdateConnectionStringsConfig("postgre",SQLTXTPATH.Text);
            Boolean ell = true;
            new PGDataProcessing().ExecuteQuery($"SELECT * FROM \"sys_user\"", ref ell);
            if (ell) { ellcount++; bt2.Content = $"连接正常 ({ellcount})";  }
            else { yeslcount++; bt2.Content = $"无法连接 ({yeslcount})";}
                

        }
    }
}
