using DeploymentTools.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
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
using static DeploymentTools.Mod.ListDataMOD;

namespace DeploymentTools.View
{
    /// <summary>
    /// EventSubscriptions.xaml 的交互逻辑
    /// </summary>
    public partial class EventSubscriptions : Window
    {
        GetDataSet GetSetData = new GetDataSet();

        Timer AsynchronousOKInformation = new Timer();
        Timer FullSubscriptionTimer = new Timer();
        ObservableCollection<EventListDataMod> items = new ObservableCollection<EventListDataMod>();//DataGridUI集合
        Dictionary<String, String[][]> DocDataDC = new Dictionary<String, String[][]>(InitXmlData());//处理XML到集合数据
        InitKey Init = new InitKey();
        public EventSubscriptions()
        {
            InitializeComponent();
            {//启动异步刷新UI数据
                AsynchronousOKInformation.Interval = 2000;
                AsynchronousOKInformation.Elapsed += InitListDataUI;
                AsynchronousOKInformation.AutoReset = false;
                AsynchronousOKInformation.Start();

                FullSubscriptionTimer.Elapsed += FullSubscriptionSet;
            }
            InitXmlData();
        }
        Dictionary<String, String[]> DataRegisterOK = new Dictionary<string, String[]>();
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {//得到textbox控件焦点
            if (textBox.Text == "搜索事件名称" || textBox.Text == "请等待程序加载完毕...")
            {
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                textBox.Text = null;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {//失去textbox控件焦点//135 135 135
            if (String.IsNullOrEmpty(textBox.Text))
            {
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 135, 135, 135));
                textBox.Text = "搜索事件名称";
            }
        }
        ObservableCollection<ExpandoObject> reduction = new ObservableCollection<ExpandoObject>();
        /// <summary>
        /// 取到全部事件类型列表
        /// </summary>
        private void InitListDataUI(object sender, ElapsedEventArgs e)
        {
            DataRegisterOK.Clear();
            EventListData.Dispatcher.Invoke(new Action(() =>//委托处理UI
            {
                EventListData.ItemsSource = null;
            }));

            ObservableCollection<ExpandoObject> items = new ObservableCollection<ExpandoObject>();
            {

                String EegionText = PostRegister();
                if (!String.IsNullOrEmpty(EegionText) && EegionText != "error : 远程服务器无法连接")
                {
                    JObject rbs = JsonConvert.DeserializeObject<JObject>(EegionText);
                    if (rbs["msg"].ToString() == "OK")//判断OK
                    {

                        Int32 errorInt = 0;
                        foreach (var item in rbs["data"])//判断是否有订阅
                        {
                            errorInt++;
                        }
                        if (errorInt > 0)
                        {
                            foreach (var item in rbs["data"]["detail"])
                            {
                                List<String> list = new List<String>();
                                foreach (var eventTypes in item["eventTypes"])
                                {
                                    list.Add(eventTypes.ToString());
                                }
                                DataRegisterOK.Add(item["eventDest"].ToString(), list.ToArray());
                            }
                        }
                    }
                }
                else
                {
                    AsynchronousOKInformation.Stop();

                    EventListData.Dispatcher.Invoke(new Action(() =>
                    {
                        InformationTips F2 = new InformationTips();
                        F2.Init("服务器连接状态", "服务器连接失败 Error" + EegionText);
                        F2.ShowDialog();
                        if (F2.needChangeUI)
                        {
                            //OK
                        }
                    }));
                    
                }
            }
            foreach (var itemName in DocDataDC.Keys)
            {
                for (int i = 0; i < DocDataDC[itemName][0].Length; i++)
                {
                    dynamic item = new ExpandoObject();
                    item.EventType = DocDataDC[itemName][0][i];
                    item.EventCode = DocDataDC[itemName][1][i];

                    item.SubscriptionType = DocDataDC[itemName][2][i];
                    item.TokenUrl = null;
                    item.SubscriptionStatus = null;
                    foreach (String eventDest in DataRegisterOK.Keys)
                    {
                        foreach (var eventDests in DataRegisterOK[eventDest])
                        {
                            if (DocDataDC[itemName][1][i] == eventDests)
                            {
                                item.TokenUrl = eventDest;
                                item.SubscriptionStatus = "订阅中";
                                break;
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(item.TokenUrl))
                    {
                        item.TokenUrl = "无";
                    }
                    if (String.IsNullOrEmpty(item.SubscriptionStatus))
                    {
                        item.SubscriptionStatus = "未注册/订阅";
                    }
                    item.CheckBoxColumnN = false;
                    items.Add(item);
                }

            }
            EventListData.Dispatcher.Invoke(new Action(() =>
            {
                EventListData.ItemsSource = items;
                CardN.Visibility = Visibility.Hidden;
                Subscribe.IsEnabled = true;
                TestB.IsEnabled = true;
                SubscriptionSelection.IsEnabled = true;
                Unsubscribe.IsEnabled = true;
            }));
            reduction = items;//全局变量保存数据 以在其他地方做还原或修改
            AsynchronousOKInformation.Stop();
        }
        private String PostRegister()
        {
            String parameter = Init.GetRegistrationEventsData();
            return parameter;
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String PostUnsubscribe(Int32[] str)
        {
            String parameter = Init.Unsubscribe(str);
            return parameter;
        }
        /// <summary>
        /// XML字段名称
        /// </summary>
        private static String[] DocName = new String[] {
                "VideoEvents" ,
                "ParkingIncidents",
                "AccessControlIncidents",
                "VisualIntercomEvents",
                "VisitorIncident",
                "LadderControlManagement",
                "CampusGapIncident",
                "IntrusionAlarmEvent",
                "FireIncidents",
                "DynamicLoopMonitoring",
                "FaceMonitoringEvents"
            };
        private static Dictionary<String, String[][]> InitXmlData()
        {
            Dictionary<String, String[][]> dc = new Dictionary<String, String[][]>();
            String xml = AppDomain.CurrentDomain.BaseDirectory + "EventData.xml";
            List<String> SubscriptionType = new List<String>();//临时存放数组集合数据
            List<String> EventType = new List<String>();//临时存放数组集合数据
            List<String> EventCode = new List<String>();//事件代码
            foreach (String item in DocName)
            {
                for (int i = 1; ; i++)
                {
                    String SubscriptionTypeName = XmlHelper.Read(xml, $"/doc/{item}/Site{i.ToString()}", "Name");
                    String Value = XmlHelper.Read(xml, $"/doc/{item}/Site{i.ToString()}", "Value");
                    String Master = XmlHelper.Read(xml, $"/doc/{item}/Site{i.ToString()}/Master", "");

                    if (String.IsNullOrEmpty(SubscriptionTypeName))
                    {
                        break;
                    }
                    else
                    {
                        SubscriptionType.Add(SubscriptionTypeName);
                        EventCode.Add(Value);
                        EventType.Add(Master);
                    }
                }
                dc.Add(item, new string[][] { SubscriptionType.ToArray(), EventCode.ToArray(), EventType.ToArray() });
                SubscriptionType.Clear();
                EventType.Clear();
                EventCode.Clear();
            }
            return dc;
            //读取特定属性值
            //Console.WriteLine("<div>" + XmlHelper.Read(xml, "/doc/Studio/Site[@Name='丁香鱼工作室']", "Url") + "</div>");
        }
        String ErrorStr = null; String TokenUrlStr = null;
        private void FullSubscriptionSet(object sender, ElapsedEventArgs e)
        {

            foreach (var itemName in DocDataDC.Keys)
            {
                for (int i = 0; i < DocDataDC[itemName][0].Length; i++)
                {
                    for (int ic = 0; ic < 100; ic++)//注销当前订阅
                    {
                        String Unsubscribe = PostUnsubscribe(new Int32[] { Int32.Parse(DocDataDC[itemName][1][i]) });
                        JObject Unsubscribes = JsonConvert.DeserializeObject<JObject>(Unsubscribe);
                        if (Unsubscribe != "error : 远程服务器无法连接" && Unsubscribes["msg"].ToString() == "OK")
                        {
                            ErrorStr = $"数据订阅 注销成功： {DocDataDC[itemName][1][i]}  {ErrorStr}";
                            break;
                        }
                        else
                        {
                            Unsubscribe = PostUnsubscribe(new Int32[] { Int32.Parse(DocDataDC[itemName][1][i]) });
                            ErrorStr = $"数据订阅 注销失败： {DocDataDC[itemName][1][i]}  {ErrorStr}";
                            if (ic == 99)
                            {
                                ErrorStr = $"数据订阅 注销失败： {DocDataDC[itemName][1][i]}  {ErrorStr}次数太多，线程即将退出";
                                FullSubscriptionTimer.Stop();
                            }
                        }
                    }
                    ErrorStr = Init.SetFullSubscriptionData(new Int32[] { Int32.Parse(DocDataDC[itemName][1][i]) }, TokenUrlStr, 2, new Int32[2]);

                    if (ErrorStr == "error : 远程服务器无法连接")
                    {
                        EventListData.Dispatcher.Invoke(new Action(() =>
                        {
                            Title = $"数据订阅 ： {DocDataDC[itemName][1][i]}  {ErrorStr}";
                        }));
                        break;
                    }
                    else
                    {
                        JObject rbs = JsonConvert.DeserializeObject<JObject>(ErrorStr);
                        if (rbs["msg"].ToString() == "OK")//判断OK
                        {
                            ErrorStr = "数据提交/订阅成功";
                            EventListData.Dispatcher.Invoke(new Action(() =>
                            {
                                Title = $"数据订阅 ： {DocDataDC[itemName][1][i]}  {ErrorStr}";
                            }));
                        }
                        else
                        {
                            ErrorStr = "数据提交/订阅失败";
                            EventListData.Dispatcher.Invoke(new Action(() =>
                            {
                                Title = $"数据订阅 ： {DocDataDC[itemName][1][i]}  {ErrorStr}";
                            }));
                            break;
                        }
                    }
                    EventListData.Dispatcher.Invoke(new Action(() =>
                    {
                        Title = $"数据订阅 ： {DocDataDC[itemName][1][i]}  {ErrorStr}";

                    }));
                }

            }
            EventListData.Dispatcher.Invoke(new Action(() =>
            {
                EventListData.ItemsSource = items;
                CardN.Visibility = Visibility.Hidden;
                Subscribe.IsEnabled = true;
                TestB.IsEnabled = true;
                SubscriptionSelection.IsEnabled = true;
                Unsubscribe.IsEnabled = true;
                DocDataDC = new Dictionary<String, String[][]>(InitXmlData());//处理XML到集合数据
                AsynchronousOKInformation.Start();
            }));
            EventListData.Dispatcher.Invoke(new Action(() =>
            {
                Title = "数据订阅 :" + ErrorStr;
            }));

            FullSubscriptionTimer.Stop();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!String.IsNullOrEmpty(TokenUrl.Text))
            {
                TokenUrlStr = TokenUrl.Text;
                Subscribe.IsEnabled = false;
                TestB.IsEnabled = false;
                SubscriptionSelection.IsEnabled = false;
                Unsubscribe.IsEnabled = false;
                CardN.Visibility = Visibility.Visible;
                FullSubscriptionTimer.Interval = 2000;
                FullSubscriptionTimer.AutoReset = false;
                FullSubscriptionTimer.Start();
            }
            else
            {

                InformationTips F2 = new InformationTips();
                F2.Init("提示", "回调地址不可为空，请设置您的回调链接 格式为：https://ip:port/eventRcv");
                F2.ShowDialog();
                if (F2.needChangeUI)
                {
                    //OK
                }
            }


        }
        //快速订阅全部
        private void TestB_Click(object sender, RoutedEventArgs e)
        {
            List<Int32> CodeList = new List<Int32>();
            dynamic items = EventListData.ItemsSource;
            foreach (var item in items)
            {
                
                CodeList.Add(Int32.Parse(item.EventCode));
            }

            Int32[] temporary = CodeList.ToArray();
            Int32 IndexCount = (int)Math.Ceiling(Convert.ToDouble(temporary.Length) / Convert.ToDouble(6));

            for (int i = 0; i < IndexCount; i++)
            {
                List<Int32> IndexCountTemporary = new List<Int32>();
                for (int IndexInt = i * 6; IndexInt - i * 6 < 6; IndexInt++)
                {
                    if (IndexInt >= temporary.Length)
                    {
                        break;
                    }
                    IndexCountTemporary.Add(temporary[IndexInt]);
                }
                String Unsubscribe = PostUnsubscribe(IndexCountTemporary.ToArray());
                if (Unsubscribe == "error : 远程服务器无法连接")
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("ISC平台连接", Unsubscribe);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
                else
                {
                    JObject rbs = JsonConvert.DeserializeObject<JObject>(Unsubscribe);
                    Title = $"数据订阅 ： {rbs["msg"].ToString()} {IndexCountTemporary[IndexCountTemporary.Count - 1]}";
                }
            }


            AsynchronousOKInformation.Start();

            //List<Int32> CodeList = new List<Int32>();
            //foreach (var item in DocDataDC.Keys)
            //{

            //    for (int i = 0; i < DocDataDC[item][0].Length; i++)
            //    {
            //        CodeList.Add(Int32.Parse(DocDataDC[item][1][i]));
            //    }
            //}
            //Int32[] temporary = CodeList.ToArray();
            //Int32 IndexCount = (int)Math.Ceiling(Convert.ToDouble(temporary.Length) / Convert.ToDouble(6));

            //for (int i = 0; i < IndexCount; i++)
            //{
            //    List<Int32> IndexCountTemporary = new List<Int32>();
            //    for (int IndexInt = i * 6; IndexInt - i * 6 < 6; IndexInt++)
            //    {
            //        if (IndexInt >= temporary.Length)
            //        {
            //            break;
            //        }
            //        IndexCountTemporary.Add(temporary[IndexInt]);
            //    }
            //    String Unsubscribe = PostUnsubscribe(IndexCountTemporary.ToArray());
            //    ErrorStr = Init.SetFullSubscriptionData(Init.StitchingParameters(Init.GetKey(), 5, 5, 5, 5), IndexCountTemporary.ToArray(), TokenUrl.Text, 2, new Int32[2]);

            //    if (ErrorStr == "error : 远程服务器无法连接")
            //    {
            //        InformationTips F2 = new InformationTips();
            //        F2.Init("ISC平台连接", ErrorStr);
            //        F2.ShowDialog();
            //        if (F2.needChangeUI)
            //        {
            //            //OK
            //        }
            //    }
            //    else
            //    {
            //        JObject rbs = JsonConvert.DeserializeObject<JObject>(ErrorStr);
            //        Title = $"数据订阅 ： {rbs["msg"].ToString()} {IndexCountTemporary[IndexCountTemporary.Count - 1]}";

            //        InformationTips F2 = new InformationTips();
            //        F2.Init("数据订阅", Title);
            //        F2.ShowDialog();
            //        if (F2.needChangeUI)
            //        {
            //            //OK
            //        }
            //    }
            //}
            ////MessageBox.Show(Title);
            //AsynchronousOKInformation.Start();
        }
        //订阅选中
        private void SubscriptionSelection_Click(object sender, RoutedEventArgs e)
        {// CheckBoxColumnN
            List<Int32> CodeList = new List<Int32>();
            dynamic items = EventListData.ItemsSource;
            foreach (var item in items)
            {
                if (item.CheckBoxColumnN)
                {
                    CodeList.Add(Int32.Parse(item.EventCode));
                }
            }

            Int32[] temporary = CodeList.ToArray();
            Int32 IndexCount = (int)Math.Ceiling(Convert.ToDouble(temporary.Length) / Convert.ToDouble(6));

            for (int i = 0; i < IndexCount; i++)
            {
                List<Int32> IndexCountTemporary = new List<Int32>();
                for (int IndexInt = i * 6; IndexInt - i * 6 < 6; IndexInt++)
                {
                    if (IndexInt >= temporary.Length)
                    {
                        break;
                    }
                    IndexCountTemporary.Add(temporary[IndexInt]);
                }
                ErrorStr = Init.SetFullSubscriptionData(IndexCountTemporary.ToArray(), TokenUrl.Text, 2, new Int32[2]);

                if (ErrorStr == "error : 远程服务器无法连接")
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("ISC平台连接", ErrorStr);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
                else
                {
                    JObject rbs = JsonConvert.DeserializeObject<JObject>(ErrorStr);
                    Title = $"数据订阅 ： {rbs["msg"].ToString()} {IndexCountTemporary[IndexCountTemporary.Count - 1]}";

                    InformationTips F2 = new InformationTips();
                    F2.Init("数据订阅", Title);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
            }
            AsynchronousOKInformation.Start();
        }
        //取消选中的订阅
        private void Unsubscribe_Click(object sender, RoutedEventArgs e)
        {// CheckBoxColumnN
            List<Int32> CodeList = new List<Int32>();
            dynamic items = EventListData.ItemsSource;
            foreach (var item in items)
            {
                if (item.CheckBoxColumnN)
                {
                    CodeList.Add(Int32.Parse(item.EventCode));
                }
            }

            Int32[] temporary = CodeList.ToArray();
            Int32 IndexCount = (int)Math.Ceiling(Convert.ToDouble(temporary.Length) / Convert.ToDouble(6));

            for (int i = 0; i < IndexCount; i++)
            {
                List<Int32> IndexCountTemporary = new List<Int32>();
                for (int IndexInt = i * 6; IndexInt - i * 6 < 6; IndexInt++)
                {
                    if (IndexInt >= temporary.Length)
                    {
                        break;
                    }
                    IndexCountTemporary.Add(temporary[IndexInt]);
                }
                String Unsubscribe = PostUnsubscribe(IndexCountTemporary.ToArray());
                if (Unsubscribe == "error : 远程服务器无法连接")
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("ISC平台连接", Unsubscribe);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
                else
                {
                    JObject rbs = JsonConvert.DeserializeObject<JObject>(Unsubscribe);
                    Title = $"数据订阅 ： {rbs["msg"].ToString()} {IndexCountTemporary[IndexCountTemporary.Count - 1]}";
                }
            }
               

            AsynchronousOKInformation.Start();
        }
        //搜索查询
        private void SearchBT_Click(object sender, RoutedEventArgs e)
        {
          
            if (items == null)
            {
                textBox.Text = ("请等待程序加载完毕...");
            }
            else
            {
                ObservableCollection<ExpandoObject> itemsExpandoObject = new ObservableCollection<ExpandoObject>();
                EventListData.ItemsSource = reduction;//还原当前控件的数据
                dynamic items = EventListData.ItemsSource;//取到当前data控件全部数据
                foreach (var item in items)
                {
                    String textStr = item.EventType;
                    if (textStr.ToLower().Contains(textBox.Text))//模糊查询判断
                    {
                        dynamic itemTemporary = new ExpandoObject();//取出
                        itemTemporary.EventType = item.EventType;
                        itemTemporary.EventCode = item.EventCode;

                        itemTemporary.SubscriptionType = item.SubscriptionType;
                        itemTemporary.TokenUrl = item.TokenUrl;
                        itemTemporary.SubscriptionStatus = item.SubscriptionStatus;
                        itemsExpandoObject.Add(item);
                    }
                }
                if (itemsExpandoObject.Count > 0)
                {
                    EventListData.ItemsSource = itemsExpandoObject;//重新赋值
                }
            }
            
        }
    }
}
