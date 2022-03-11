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
    /// EventSubscriptions.xaml �Ľ����߼�
    /// </summary>
    public partial class EventSubscriptions : Window
    {
        GetDataSet GetSetData = new GetDataSet();

        Timer AsynchronousOKInformation = new Timer();
        Timer FullSubscriptionTimer = new Timer();
        ObservableCollection<EventListDataMod> items = new ObservableCollection<EventListDataMod>();//DataGridUI����
        Dictionary<String, String[][]> DocDataDC = new Dictionary<String, String[][]>(InitXmlData());//����XML����������
        InitKey Init = new InitKey();
        public EventSubscriptions()
        {
            InitializeComponent();
            {//�����첽ˢ��UI����
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
        {//�õ�textbox�ؼ�����
            if (textBox.Text == "�����¼�����" || textBox.Text == "��ȴ�����������...")
            {
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                textBox.Text = null;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {//ʧȥtextbox�ؼ�����//135 135 135
            if (String.IsNullOrEmpty(textBox.Text))
            {
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 135, 135, 135));
                textBox.Text = "�����¼�����";
            }
        }
        ObservableCollection<ExpandoObject> reduction = new ObservableCollection<ExpandoObject>();
        /// <summary>
        /// ȡ��ȫ���¼������б�
        /// </summary>
        private void InitListDataUI(object sender, ElapsedEventArgs e)
        {
            DataRegisterOK.Clear();
            EventListData.Dispatcher.Invoke(new Action(() =>//ί�д���UI
            {
                EventListData.ItemsSource = null;
            }));

            ObservableCollection<ExpandoObject> items = new ObservableCollection<ExpandoObject>();
            {

                String EegionText = PostRegister();
                if (!String.IsNullOrEmpty(EegionText) && EegionText != "error : Զ�̷������޷�����")
                {
                    JObject rbs = JsonConvert.DeserializeObject<JObject>(EegionText);
                    if (rbs["msg"].ToString() == "OK")//�ж�OK
                    {

                        Int32 errorInt = 0;
                        foreach (var item in rbs["data"])//�ж��Ƿ��ж���
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
                        F2.Init("����������״̬", "����������ʧ�� Error" + EegionText);
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
                                item.SubscriptionStatus = "������";
                                break;
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(item.TokenUrl))
                    {
                        item.TokenUrl = "��";
                    }
                    if (String.IsNullOrEmpty(item.SubscriptionStatus))
                    {
                        item.SubscriptionStatus = "δע��/����";
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
            reduction = items;//ȫ�ֱ����������� ���������ط�����ԭ���޸�
            AsynchronousOKInformation.Stop();
        }
        private String PostRegister()
        {
            String parameter = Init.GetRegistrationEventsData();
            return parameter;
        }
        /// <summary>
        /// ȡ������
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String PostUnsubscribe(Int32[] str)
        {
            String parameter = Init.Unsubscribe(str);
            return parameter;
        }
        /// <summary>
        /// XML�ֶ�����
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
            List<String> SubscriptionType = new List<String>();//��ʱ������鼯������
            List<String> EventType = new List<String>();//��ʱ������鼯������
            List<String> EventCode = new List<String>();//�¼�����
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
            //��ȡ�ض�����ֵ
            //Console.WriteLine("<div>" + XmlHelper.Read(xml, "/doc/Studio/Site[@Name='�����㹤����']", "Url") + "</div>");
        }
        String ErrorStr = null; String TokenUrlStr = null;
        private void FullSubscriptionSet(object sender, ElapsedEventArgs e)
        {

            foreach (var itemName in DocDataDC.Keys)
            {
                for (int i = 0; i < DocDataDC[itemName][0].Length; i++)
                {
                    for (int ic = 0; ic < 100; ic++)//ע����ǰ����
                    {
                        String Unsubscribe = PostUnsubscribe(new Int32[] { Int32.Parse(DocDataDC[itemName][1][i]) });
                        JObject Unsubscribes = JsonConvert.DeserializeObject<JObject>(Unsubscribe);
                        if (Unsubscribe != "error : Զ�̷������޷�����" && Unsubscribes["msg"].ToString() == "OK")
                        {
                            ErrorStr = $"���ݶ��� ע���ɹ��� {DocDataDC[itemName][1][i]}  {ErrorStr}";
                            break;
                        }
                        else
                        {
                            Unsubscribe = PostUnsubscribe(new Int32[] { Int32.Parse(DocDataDC[itemName][1][i]) });
                            ErrorStr = $"���ݶ��� ע��ʧ�ܣ� {DocDataDC[itemName][1][i]}  {ErrorStr}";
                            if (ic == 99)
                            {
                                ErrorStr = $"���ݶ��� ע��ʧ�ܣ� {DocDataDC[itemName][1][i]}  {ErrorStr}����̫�࣬�̼߳����˳�";
                                FullSubscriptionTimer.Stop();
                            }
                        }
                    }
                    ErrorStr = Init.SetFullSubscriptionData(new Int32[] { Int32.Parse(DocDataDC[itemName][1][i]) }, TokenUrlStr, 2, new Int32[2]);

                    if (ErrorStr == "error : Զ�̷������޷�����")
                    {
                        EventListData.Dispatcher.Invoke(new Action(() =>
                        {
                            Title = $"���ݶ��� �� {DocDataDC[itemName][1][i]}  {ErrorStr}";
                        }));
                        break;
                    }
                    else
                    {
                        JObject rbs = JsonConvert.DeserializeObject<JObject>(ErrorStr);
                        if (rbs["msg"].ToString() == "OK")//�ж�OK
                        {
                            ErrorStr = "�����ύ/���ĳɹ�";
                            EventListData.Dispatcher.Invoke(new Action(() =>
                            {
                                Title = $"���ݶ��� �� {DocDataDC[itemName][1][i]}  {ErrorStr}";
                            }));
                        }
                        else
                        {
                            ErrorStr = "�����ύ/����ʧ��";
                            EventListData.Dispatcher.Invoke(new Action(() =>
                            {
                                Title = $"���ݶ��� �� {DocDataDC[itemName][1][i]}  {ErrorStr}";
                            }));
                            break;
                        }
                    }
                    EventListData.Dispatcher.Invoke(new Action(() =>
                    {
                        Title = $"���ݶ��� �� {DocDataDC[itemName][1][i]}  {ErrorStr}";

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
                DocDataDC = new Dictionary<String, String[][]>(InitXmlData());//����XML����������
                AsynchronousOKInformation.Start();
            }));
            EventListData.Dispatcher.Invoke(new Action(() =>
            {
                Title = "���ݶ��� :" + ErrorStr;
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
                F2.Init("��ʾ", "�ص���ַ����Ϊ�գ����������Ļص����� ��ʽΪ��https://ip:port/eventRcv");
                F2.ShowDialog();
                if (F2.needChangeUI)
                {
                    //OK
                }
            }


        }
        //���ٶ���ȫ��
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
                if (Unsubscribe == "error : Զ�̷������޷�����")
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("ISCƽ̨����", Unsubscribe);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
                else
                {
                    JObject rbs = JsonConvert.DeserializeObject<JObject>(Unsubscribe);
                    Title = $"���ݶ��� �� {rbs["msg"].ToString()} {IndexCountTemporary[IndexCountTemporary.Count - 1]}";
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

            //    if (ErrorStr == "error : Զ�̷������޷�����")
            //    {
            //        InformationTips F2 = new InformationTips();
            //        F2.Init("ISCƽ̨����", ErrorStr);
            //        F2.ShowDialog();
            //        if (F2.needChangeUI)
            //        {
            //            //OK
            //        }
            //    }
            //    else
            //    {
            //        JObject rbs = JsonConvert.DeserializeObject<JObject>(ErrorStr);
            //        Title = $"���ݶ��� �� {rbs["msg"].ToString()} {IndexCountTemporary[IndexCountTemporary.Count - 1]}";

            //        InformationTips F2 = new InformationTips();
            //        F2.Init("���ݶ���", Title);
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
        //����ѡ��
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

                if (ErrorStr == "error : Զ�̷������޷�����")
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("ISCƽ̨����", ErrorStr);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
                else
                {
                    JObject rbs = JsonConvert.DeserializeObject<JObject>(ErrorStr);
                    Title = $"���ݶ��� �� {rbs["msg"].ToString()} {IndexCountTemporary[IndexCountTemporary.Count - 1]}";

                    InformationTips F2 = new InformationTips();
                    F2.Init("���ݶ���", Title);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
            }
            AsynchronousOKInformation.Start();
        }
        //ȡ��ѡ�еĶ���
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
                if (Unsubscribe == "error : Զ�̷������޷�����")
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("ISCƽ̨����", Unsubscribe);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
                else
                {
                    JObject rbs = JsonConvert.DeserializeObject<JObject>(Unsubscribe);
                    Title = $"���ݶ��� �� {rbs["msg"].ToString()} {IndexCountTemporary[IndexCountTemporary.Count - 1]}";
                }
            }
               

            AsynchronousOKInformation.Start();
        }
        //������ѯ
        private void SearchBT_Click(object sender, RoutedEventArgs e)
        {
          
            if (items == null)
            {
                textBox.Text = ("��ȴ�����������...");
            }
            else
            {
                ObservableCollection<ExpandoObject> itemsExpandoObject = new ObservableCollection<ExpandoObject>();
                EventListData.ItemsSource = reduction;//��ԭ��ǰ�ؼ�������
                dynamic items = EventListData.ItemsSource;//ȡ����ǰdata�ؼ�ȫ������
                foreach (var item in items)
                {
                    String textStr = item.EventType;
                    if (textStr.ToLower().Contains(textBox.Text))//ģ����ѯ�ж�
                    {
                        dynamic itemTemporary = new ExpandoObject();//ȡ��
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
                    EventListData.ItemsSource = itemsExpandoObject;//���¸�ֵ
                }
            }
            
        }
    }
}
