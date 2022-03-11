using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DeploymentTools.Logic;
using DeploymentTools.Mod;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using static DeploymentTools.Mod.ComBoxMod;

namespace DeploymentTools.View
{
    /// <summary>
    /// Synchronization.xaml 的交互逻辑
    /// </summary>
    public partial class Synchronization : Window
    {
        ISCPostJosn PostJosnData = new ISCPostJosn();
        GetDataSet GetSetData = new GetDataSet();

        List<String> EquipmentText = new List<String>();//json数据
        String EegionText;
        Boolean EegionState = false;//是否加载过
        Boolean EquipmentState = false;

        System.Timers.Timer SynchronizationTimer = new System.Timers.Timer(); System.Timers.Timer StateUI = new System.Timers.Timer();
        InitKey Init = new InitKey();
        //设备 页面数量
        Int32 EquipmentPage = 0;
        //数据库的上图设备数 （与需要更新的数据进行校对的数量）
        List<String> EquipmentPageIndex = new List<String>();
        public Synchronization()
        {
            InitializeComponent();
            #region 初始化控件状态及线程
            Load_animation.Visibility = Visibility.Hidden;
            SynchronizationTimer.AutoReset = false;
            SynchronizationTimer.Interval = 2000;
            SynchronizationTimer.Elapsed += SynchronizationData;

            StateUI.AutoReset = false;
            StateUI.Interval = 2000;
            StateUI.Elapsed += StateUIInit;
            StateUI.Start();
            CardN.Visibility = Visibility.Visible;

            #region 初始化两个按钮的状态 同步哪些数据
            ofEquipmentStateTiggleBT.IsChecked = bool.Parse(Config.GetConfigValue("DeviceBrowsing"));
            SPCCSelection.IsChecked = bool.Parse(Config.GetConfigValue("SPCCSelection")); 
            ofRegionStateTiggleBT.IsChecked = bool.Parse(Config.GetConfigValue("AreaBrowsing"));
            #endregion
            #endregion


            SynchronizationButton.Dispatcher.Invoke(new Action(() =>
            {
                SynchronizationButton.IsEnabled = false;
            }));


        }

        /// <summary>
        /// 初始化状态按钮
        /// </summary>
        private void StateUIInit(object sender, ElapsedEventArgs e) {

            StateUI.Stop();
            //清空两个列表
            listView1.Dispatcher.Invoke(new Action(() =>
            {
                listView1.Items.Clear();
                listView2.Items.Clear();
            }));

            //设备
            CardMess.Dispatcher.Invoke(new Action(() =>
            {
                CardMess.Content = null;
            }));
            //判斷是否為SPCC同步
            Boolean SpccSelsecrState = false;
            SPCCSelection.Dispatcher.Invoke(new Action(() =>
            {
                SpccSelsecrState = (Boolean)SPCCSelection.IsChecked;
            }));
            if (SpccSelsecrState)
            {   //SPCC监控点
                {
                    iniSetNumberEquipment(1);
                    EquipmentPage = (int)Math.Ceiling(Convert.ToDouble(AreaPages.GetNumberEquipment()) / Convert.ToDouble(1000));
                    for (int i = 0; i < EquipmentPage; i++)
                    {
                        EquipmentText.Add(SPCCPostequipmentData(i + 1));
                        if (!String.IsNullOrEmpty(EquipmentText[i]) && EquipmentText[i] != "error : 远程服务器无法连接")
                        {
                            EquipmentState = true;
                            JObject rbs = JsonConvert.DeserializeObject<JObject>(EquipmentText[i]);
                            Int32 errorInt = 0;
                            foreach (var item in rbs["data"])
                            {
                                errorInt++;
                            }
                            if (errorInt > 0)
                            {
                                foreach (var item in rbs["data"]["list"])
                                {
                                    listView1.Dispatcher.Invoke(new Action(() =>
                                    {
                                        listView1.Items.Add(item["name"].ToString());
                                    }));
                                }
                            }

                        }
                        else
                        {
                            EquipmentState = false;
                        }
                    }
                }
               
            }
            else
            {
                //ISC设备
                {
                    iniSetNumberEquipment(0); // 预先取出总页数
                                         //分页 设备信息
                    EquipmentPage = (int)Math.Ceiling(Convert.ToDouble(AreaPages.GetNumberEquipment()) / Convert.ToDouble(1000));
                    for (int i = 0; i < EquipmentPage; i++)
                    {

                        EquipmentText.Add(ISCPostequipmentData(i + 1));
                        if (!String.IsNullOrEmpty(EquipmentText[i]) && EquipmentText[i] != "error : 远程服务器无法连接")
                        {
                            EquipmentState = true;
                            JObject rbs = JsonConvert.DeserializeObject<JObject>(EquipmentText[i]);
                            Int32 errorInt = 0;
                            foreach (var item in rbs["data"])
                            {
                                errorInt++;
                            }
                            if (errorInt > 0)
                            {
                                foreach (var item in rbs["data"]["list"])
                                {
                                    listView1.Dispatcher.Invoke(new Action(() =>
                                    {
                                        listView1.Items.Add(item["cameraName"].ToString());
                                    }));
                                }
                            }

                        }
                        else
                        {
                            EquipmentState = false;
                        }

                    }
                }
            }

            //区域
            CardMess_Copy.Dispatcher.Invoke(new Action(() =>
            {
                CardMess_Copy.Content = null;
            }));

            if (SpccSelsecrState)
            {
                EegionText = PostRegion(1);
               
            }
            else
            {
                EegionText = PostRegion(0);
               
            }

            if (!String.IsNullOrEmpty(EegionText) && EegionText != "error : 远程服务器无法连接")
            {
                EegionState = true;
                JObject rbs = JsonConvert.DeserializeObject<JObject>(EegionText);
                foreach (var item in rbs["data"]["list"])
                {
                    listView2.Dispatcher.Invoke(new Action(() =>
                    {
                        listView2.Items.Add(item["name"].ToString());
                    }));

                }
            }
            else
            {
                SynchronizationButton.Dispatcher.Invoke(new Action(() =>
                {
                    SynchronizationButton.Content = "远程服务器无法连接";
                }));
                EegionState = false;
            }

            #region 初始化厂家类型及摄像头类型

            ObservableCollection<ItemContent> list = new ObservableCollection<ItemContent>();
            Boolean error = true;
            PGDataProcessing PGdata = new PGDataProcessing();
            DataSet Data = PGdata.ExecuteQuery($"SELECT * FROM \"sys_dictionary\" where pid = '15ad9ba70e024246b3a50d7de2393812'", ref error);
            if (error)
            {
                list = new ObservableCollection<ItemContent>();
                list.Add(new ItemContent() { Name = "外接设备类型", CameraId = null });
                for (int i = 0; i < Data.Tables[0].Rows.Count; i++)
                {
                    comboBox1.Dispatcher.Invoke(new Action(() =>
                    {
                        list.Add(new ItemContent() { Name = Data.Tables[0].Rows[i]["dic_name"].ToString(), CameraId = Data.Tables[0].Rows[i]["id"].ToString() });
                        this.comboBox1.ItemsSource = list;
                        this.comboBox1.DisplayMemberPath = "Name";
                    }));
                }
                Data = PGdata.ExecuteQuery($"SELECT * FROM \"sys_dictionary\" where pid = 'c603b6346b5249d1972ec364b4d68a6c'", ref error);
                list = new ObservableCollection<ItemContent>();
                list.Add(new ItemContent() { Name = "平台厂商", CameraId = null });
                for (int i = 0; i < Data.Tables[0].Rows.Count; i++)
                {

                    comboBox2.Dispatcher.Invoke(new Action(() =>
                    {
                        list.Add(new ItemContent() { Name = Data.Tables[0].Rows[i]["dic_name"].ToString(), CameraId = Data.Tables[0].Rows[i]["id"].ToString() });
                        this.comboBox2.ItemsSource = list;
                        this.comboBox2.DisplayMemberPath = "Name";
                    }));
                    
                }
               
            }
            else
            {
                SynchronizationButton.Dispatcher.Invoke(new Action(() =>
                {
                    SynchronizationButton.Content = "数据库无法连接";
                }));
                StateUI.Stop();
            }
            #endregion

            comboBox2.Dispatcher.Invoke(new Action(() =>
            {
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }));
            CardN.Dispatcher.Invoke(new Action(() =>
            {
                CardN.Visibility = Visibility.Hidden;
            }));

            if (EquipmentState && EegionState)
            {
                SynchronizationButton.Dispatcher.Invoke(new Action(() =>
                {
                    SynchronizationButton.IsEnabled = true;
                }));
            }
            StateUI.Stop();
        }
        private void iniSetNumberEquipment(Int32 type) {
            switch (type)
            {

                case 0:
                    String parameter = Init.GetEquipmentData(1, 1);
                    if (!String.IsNullOrEmpty(parameter) && parameter != "error : 远程服务器无法连接" &&  parameter != "未将对象引用设置到对象的实例。")
                    {
                        JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
                        String total = rb["data"]["total"].ToString();//取出单页最大数
                        AreaPages.SetNumberEquipment(Int32.Parse(total));
                    }
                    else
                    {
                        AreaPages.SetNumberEquipment(1);
                    }

                    break;


                case 1:
                     parameter = Init.GetMonitoringPoint(1, 1, out bool error);
                    if (!String.IsNullOrEmpty(parameter) && parameter != "error : 远程服务器无法连接" && parameter != "未将对象引用设置到对象的实例。")
                    {
                        try
                        {
                            JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
                            String total = rb["data"]["total"].ToString();//取出单页最大数
                            AreaPages.SetNumberEquipment(Int32.Parse(total));
                        }
                        catch (Exception)
                        {

                           
                        }
                    }
                    else
                    {
                        AreaPages.SetNumberEquipment(1);
                    }

                    break;
            }
        }
        /// <summary>
        /// ISC设备信息
        /// </summary>
        /// <returns></returns>
        private String ISCPostequipmentData(int PageIndex) {
                return Init.GetEquipmentData(PageIndex, 1000);
        }


        /// <summary>
        /// SPCC设备信息
        /// </summary>
        /// <returns></returns>
        private String SPCCPostequipmentData(int PageIndex)
        {
            return Init.GetMonitoringPoint(PageIndex, 1000, out bool error);
        }

        /// <summary>
        /// 取全部区域
        /// </summary>
        /// <returns></returns>
        private String PostRegion(Int32 type) {
            String parameter = null;
            switch (type)
            {
                case 0:
                    parameter = Init.GetPagingAreaData(1, 1000);
                    break;
                case 1:
                    parameter = Init.SPCCGetPagingAreaData(1, 1000);

                    break;
            }
            return parameter;
        }
        PGDataProcessing PGdata = new PGDataProcessing();
        /// <summary>
        /// 上传设备信息至数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SynchronizationData(object sender, ElapsedEventArgs e) {

            ItemContent TypeN = new ItemContent();
            ItemContent Iregion_idN = new ItemContent() ;

            String Title = null;
            SynchronizationButton.Dispatcher.Invoke(new Action(() =>
            {
                SynchronizationButton.IsEnabled = false;

                TypeN = comboBox1.SelectedItem as ItemContent;
                Iregion_idN = comboBox2.SelectedItem as ItemContent;
            }));


            if (String.IsNullOrEmpty(TypeN.CameraId) || String.IsNullOrEmpty(Iregion_idN.CameraId))
            {
                SynchronizationButton.Dispatcher.Invoke(new Action(() =>
                {
                    SynchronizationButton.IsEnabled = true;
                }));
                Load_animation.Dispatcher.Invoke(new Action(() =>
                {
                    SynchronizationTimer.Stop();
                    InformationTips F2 = new InformationTips();
                    F2.Init("提示", "厂商类型为空或设备类型为空");
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }));


                SynchronizationTimer.Close();
                SynchronizationTimer.Stop();
                return;
            }
                Boolean[] errors = new Boolean[]{ true,true};
            //如果设备信息需要执行 进行删表操作
            ofEquipmentStateTiggleBT.Dispatcher.Invoke(new Action(() =>
            {
                if (ofEquipmentStateTiggleBT.IsChecked == true)
                {
                    PGdata.ExecuteQuery("TRUNCATE TABLE dock_region", ref errors[0]);//区域信息
                }
                if (ofRegionStateTiggleBT.IsChecked == true)
                {
                    ///删表操作
                    {
                      
                        //PGdata.ExecuteQuery("TRUNCATE TABLE dock_region", ref errors[0]);
                        //删除全部没有上图的设备
                        PGdata.ExecuteQuery($"DELETE FROM dock_device WHERE on_map = false and device_type='{TypeN.CameraId}'", ref errors[0]);
                        DataSet EPage = PGdata.ExecuteQuery("SELECT * FROM dock_device", ref errors[0]);
                        if (errors[0])
                        {
                            //存入设备唯一标识码 后期跳过
                            for (int i = 0; i < EPage.Tables[0].Rows.Count; i++)
                            {
                                EquipmentPageIndex.Add(EPage.Tables[0].Rows[i]["device_code"].ToString());
                            }
                        }
                    }
                }
            }));
           
           
            //致命报错
            if (!errors[0])
            {
                SynchronizationButton.Dispatcher.Invoke(new Action(() =>
                {
                    SynchronizationButton.IsEnabled = true;
                }));
                Load_animation.Dispatcher.Invoke(new Action(() =>
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("致命错误", "初始化表结构数据错误，这是致命的！或许有数据的丢失，请重视并且发送邮件或传真给开发人员");
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }));
                SynchronizationTimer.Stop();

                return;
            }

            Dispatcher.Invoke(new Action(() =>
            {
                Load_animation.Visibility = Visibility.Visible;
                Load_animation.Value = 50;
            }));
           
            ofRegionStateTiggleBT.Dispatcher.Invoke(new Action(() =>
            {
                EquipmentState = (Boolean)ofRegionStateTiggleBT.IsChecked;
            }));
            int indexcount = 0;
            //上传设备信息
            for (int i = 0; i < EquipmentPage; i++)
            {

                if (EquipmentState && !String.IsNullOrEmpty(EquipmentText[i]) && EquipmentText[i] != "error : 远程服务器无法连接")
                {
                    Load_animation.Dispatcher.Invoke(new Action(() =>
                    {
                        Load_animation.Value = 80;
                    }));

                    JObject rbs = JsonConvert.DeserializeObject<JObject>(EquipmentText[i]);
                    Int32 ListCount = 1;
                    foreach (var item in rbs["data"]["list"])
                    {
                        ListCount++;
                        Dispatcher.Invoke(new Action(() =>
                        {
                            Load_animation.Value = (Int32)0;
                        }));
                    }
                
                    foreach (var item in rbs["data"]["list"])
                    {
                        indexcount++;
                        PGAddequipmentDataMod Toadd = new PGAddequipmentDataMod();

                        Dispatcher.Invoke(new Action(() =>
                        {
                            Load_animation.Value += (Int32)(100 / ListCount);
                        }));

                        try
                        {
                            //判斷是否為SPCC同步
                            Boolean SpccSelsecrState = false;
                            SPCCSelection.Dispatcher.Invoke(new Action(() =>
                            {
                                SpccSelsecrState = (Boolean)SPCCSelection.IsChecked;
                            }));

                            if (SpccSelsecrState)
                            {
                                comboBox1.Dispatcher.Invoke(new Action(() =>
                                {
                                   
                                    Toadd.Id = Guid.NewGuid().ToString("N");
                                    Toadd.Type = TypeN.CameraId;//平台类型
                                    Toadd.Code = item["cameraIndexCode"].ToString();
                                    Toadd.Name = item["name"].ToString();
                                    Toadd.Iregion_id = Iregion_idN.CameraId;//设备类型
                                    Toadd.Info = item.ToString();
                                    Toadd.Map = "false";
                                }));
                            }
                            else
                            {
                                comboBox1.Dispatcher.Invoke(new Action(() =>
                                {
                                   
                                    Toadd.Id = Guid.NewGuid().ToString("N");
                                    Toadd.Type = TypeN.CameraId;
                                    Toadd.Code = item["cameraIndexCode"].ToString();
                                    Toadd.Name = item["cameraName"].ToString();
                                    Toadd.Iregion_id = Iregion_idN.CameraId;
                                    Toadd.Info = item.ToString();
                                    Toadd.Map = "false";
                                }));
                            }
                      
                        }
                        catch (Exception)
                        {

                            errors[0] = false;
                            break;
                        }
                       
                        //EquipmentSqlpage //device_code
                        Boolean whether = true;//是否上图
                        foreach (var IndexPage in EquipmentPageIndex)
                        {
                            if (IndexPage == Toadd.Code)
                            {
                                whether = false;
                            }
                        }
                     
                        if (whether)//过已上图数据
                        {
                            PGdata.ExecuteQuery($"INSERT INTO\"public\".\"dock_device\"(\"id\",\"device_type\",\"device_code\",\"device_name\",\"region_id\",\"device_info\",\"on_map\")VALUES('{Toadd.Id}','{Toadd.Type}','{Toadd.Code}','{Toadd.Name}','{Toadd.Iregion_id}','{Toadd.Info}','{Toadd.Map}')RETURNING*", ref errors[0]);
                        }
                        if (!errors[0])
                        {
                            break;
                        }
                    }

                }
            }
            int count = indexcount;

            ofEquipmentStateTiggleBT.Dispatcher.Invoke(new Action(() =>
            {
                EegionState = (Boolean)ofEquipmentStateTiggleBT.IsChecked;
            }));

            
            //上传区域信息
            if (EegionState && !String.IsNullOrEmpty(EegionText) && EegionText != "error : 远程服务器无法连接")
            {
                Load_animation.Dispatcher.Invoke(new Action(() =>
                {
                    Load_animation.Value = 80;
                }));

                JObject rbs = JsonConvert.DeserializeObject<JObject>(EegionText);
                Int32 ListCount = 1;
                foreach (var item in rbs["data"]["list"])
                {
                    ListCount++;
                    Load_animation.Dispatcher.Invoke(new Action(() =>
                    {
                        Load_animation.Value = 0;
                    }));
                }
                foreach (var item in rbs["data"]["list"])
                {
                    PGAddEegionDataMod Toadd = new PGAddEegionDataMod();

                    Load_animation.Dispatcher.Invoke(new Action(() =>
                    {
                        Load_animation.Value += 100 / ListCount;
                    }));

                    try
                    {
                        comboBox2.Dispatcher.Invoke(new Action(() =>
                        {
                            Toadd.Id = item["indexCode"].ToString();
                            Toadd.Name = item["name"].ToString();
                            Toadd.Pid = item["parentIndexCode"].ToString();
                            Toadd.Platform = Iregion_idN.CameraId;
                        }));
                    }
                    catch (Exception ex)
                    {
                        errors[1] = false;
                        break;
                    }
                    PGDataProcessing PGdata = new PGDataProcessing();
                    PGdata.ExecuteQuery($"INSERT INTO \"public\".\"dock_region\"(\"id\", \"pid\", \"region_name\", \"platform\") VALUES ('{Toadd.Id}', '{Toadd.Pid}', '{Toadd.Name}', '{Toadd.Platform}') RETURNING *", ref errors[1]);
                    if (!errors[1])
                    {
                        break;
                    }
                }
            }
            
           
            if (!errors[0] || !errors[1] || !EquipmentState || !EegionState)
            {
                Load_animation.Dispatcher.Invoke(new Action(() =>
                {
                    Load_animation.Visibility = Visibility.Hidden;

                    Title += ("很遗憾，同步无法进行，数据库错误或其他\r\n");


                }));


                if (EegionState)
                {
                    Load_animation.Dispatcher.Invoke(new Action(() =>
                    {
                        Load_animation.Visibility = Visibility.Hidden;
                        Load_animation.Value = 100;
                        Title += "同步数据 :" + ("设备信息同步数据完成。\r\n");

                    }));
                }
                else
                {
                    Load_animation.Dispatcher.Invoke(new Action(() =>
                    {
                        Load_animation.Visibility = Visibility.Hidden;
                        Title += "同步数据 :" + ("设备信息无法同步，您设置不同步\r\n");

                    }));

                }
                if (EquipmentState)
                {
                    Load_animation.Dispatcher.Invoke(new Action(() =>
                    {
                        Load_animation.Visibility = Visibility.Hidden;
                        Load_animation.Value = 100;
                        Title += "同步数据 :" + ("区域信息同步数据完成。\r\n");

                    }));
                }
                else
                {
                    Load_animation.Dispatcher.Invoke(new Action(() =>
                    {
                        Load_animation.Visibility = Visibility.Hidden;
                        Title += "同步数据 :" + ("区域信息无法同步，您设置不同步\r\n");

                    }));

                }
            }
            else if(errors[0] && errors[1] && EquipmentState && EegionState)
            {
                Load_animation.Dispatcher.Invoke(new Action(() =>
                {
                    Load_animation.Visibility = Visibility.Hidden;
                    Load_animation.Value = 100;
                    Title += "同步数据 :" + ("设备信息和区域数据同步数据完成。\r\n");

                }));
            }
            SynchronizationButton.Dispatcher.Invoke(new Action(() =>
            {
                SynchronizationButton.IsEnabled = true;
            }));
            Load_animation.Dispatcher.Invoke(new Action(() =>
            {
                SynchronizationTimer.Stop();
                InformationTips F2 = new InformationTips();
                F2.Init("提示", Title);
                F2.ShowDialog();
                if (F2.needChangeUI)
                {
                    //OK
                }
            }));
            

            SynchronizationTimer.Close();
            SynchronizationTimer.Stop();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SynchronizationTimer.Start();
            for (int i = 0; i < EquipmentText.Count; i++)
            {

                string path = "./tempCode" + i + ".Json";
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(EquipmentText[i]);
                sw.Flush();
                sw.Close();
                fs.Close();
                
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Config.SetConfigValue("DeviceBrowsing", ofEquipmentStateTiggleBT.IsChecked.ToString());

            Config.SetConfigValue("AreaBrowsing", ofRegionStateTiggleBT.IsChecked.ToString());

            Config.SetConfigValue("SPCCSelection", SPCCSelection.IsChecked.ToString());
        }

        private void OfRegionStateTiggleBT_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void OfEquipmentStateTiggleBT_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                string paths = openFileDialog.SelectedPath;
                if (paths != null)
                {
                    for (int i = 0; i < EquipmentText.Count; i++)
                    {
                        string path = paths + "/tempCode" + i + ".Json";
                        FileStream fs = new FileStream(path, FileMode.Append);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(EquipmentText[i]);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                    }
                    if (EquipmentText.Count != 0) { System.Windows.MessageBox.Show("完成"); } else { System.Windows.MessageBox.Show("无数据"); }
                    
                }
            }
           
            
        }
    
        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {//JsonConvert.DeserializeObject<JObject>
            //EquipmentText.Add(("{\"code\":\"0\",\"msg\":\"success\",\"data\":{\"total\":251,\"pageNo\":1,\"pageSize\":1,\"list\":[{\"altitude\":null,\"cameraIndexCode\":\"87c41632e21d418da9b472f6343a546f\",\"cameraName\":\"D1栋2F中区02\",\"cameraType\":0,\"cameraTypeName\":\"枪机\",\"capabilitySet\":\"event_face_detect_alarm,event_ias,event_vss,io,record,vss,event_io,event_rule,net,maintenance,event_device,status\",\"capabilitySetName\":\"人脸侦测告警,入侵报警事件能力,视频事件能力,IO能力,录像能力,视频能力,IO事件能力,行为分析事件能力,网络参数配置能力,设备维护能力,设备事件能力,状态能力\",\"intelligentSet\":null,\"intelligentSetName\":null,\"channelNo\":\"1\",\"channelType\":\"analog\",\"channelTypeName\":\"模拟通道\",\"createTime\":\"2020-08-31T20:17:19.809+08:00\",\"encodeDevIndexCode\":\"5d5ceb6863ae480493b5f30cf7694e50\",\"encodeDevResourceType\":null,\"encodeDevResourceTypeName\":null,\"gbIndexCode\":null,\"installLocation\":\"\",\"keyBoardCode\":null,\"latitude\":null,\"longitude\":null,\"pixel\":null,\"ptz\":null,\"ptzName\":null,\"ptzController\":null,\"ptzControllerName\":null,\"recordLocation\":\"0\",\"recordLocationName\":\"中心存储\",\"regionIndexCode\":\"01778231-f1b5-4883-98ee-6789e69bb7ae\",\"status\":null,\"statusName\":null,\"transType\":1,\"transTypeName\":\"TCP\",\"treatyType\":null,\"treatyTypeName\":null,\"viewshed\":null,\"updateTime\":\"2020-08-31T20:17:47.392+08:00\"}]}}"));
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {

            
                string paths = openFileDialog.SelectedPath;
                if (paths != null)
                {

                   
                    //首先模拟建立将要导出的数据，这些数据都存于DataTable中
                    FileInfo newFile = new FileInfo(paths + @"\EquipmentStructure.xlsx");
                    FileInfo newFileEegion = new FileInfo(paths + @"\EegionStructure.xlsx");
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(paths + @"\EquipmentStructure.xlsx");
                    }

                    if (newFileEegion.Exists)
                    {
                        newFileEegion.Delete();
                        newFileEegion = new FileInfo(paths + @"\EegionStructure.xlsx");
                    }
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("EquipmentStructure");
                        //id	所属组织机构	设备类型	设备编码	设备名称	详情信息
                        worksheet.Cells[1, 1].Value = "id";
                        worksheet.Cells[1, 2].Value = "所属组织机构";
                        worksheet.Cells[1, 3].Value = "设备类型";
                        worksheet.Cells[1, 4].Value = "设备编码";
                        worksheet.Cells[1, 5].Value = "设备名称";
                        worksheet.Cells[1, 6].Value = "详情信息";
                        try
                        {

                            int i = 2;
                           // Log.WriteLog("数据",EquipmentText.ToString());

                            foreach (var items in EquipmentText)
                            {
                                JObject rbs = JsonConvert.DeserializeObject<JObject>(items);
                               
                                foreach (var item in rbs["data"]["list"])
                                {
                                    var IsEnabled = false;
                                    SPCCSelection.Dispatcher.Invoke(new Action(() =>
                                    {
                                        IsEnabled = (Boolean)SPCCSelection.IsChecked;
                                    }));
                                    if (IsEnabled)
                                    {
                                        item["cameraName"] = item["name"].ToString();
                                        item["regionIndexCode"] = item["unitIndexCode"].ToString();
                                    }
                                    worksheet.Cells[i, 1].Value = item["cameraIndexCode"].ToString();
                                    worksheet.Cells[i, 2].Value = item["regionIndexCode"].ToString();
                                    worksheet.Cells[i, 3].Value = item["cameraTypeName"].ToString();
                                    worksheet.Cells[i, 4].Value = item["cameraIndexCode"].ToString();
                                    worksheet.Cells[i, 5].Value = item["cameraName"].ToString();
                                    worksheet.Cells[i, 6].Value = item.ToString();
                                    i++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            System.Windows.MessageBox.Show("设备结构错误：" + ex.Message);
                        }
                  

                        package.Save();
                    }
                    using (ExcelPackage package = new ExcelPackage(newFileEegion))
                    {
                        ExcelWorksheet worksheetEegion = package.Workbook.Worksheets.Add("EquipmentStructure");
                        //组织机构id 组织机构名称  父节点id
                        worksheetEegion.Cells[1, 1].Value = "组织机构id";
                        worksheetEegion.Cells[1, 2].Value = "组织机构名称";
                        worksheetEegion.Cells[1, 3].Value = "父节点id";
                        try
                        {
                            JObject rbs = JsonConvert.DeserializeObject<JObject>(EegionText);
                            int i = 2;
                            foreach (var item in rbs["data"]["list"])
                            {
                                //1   蓬莱戒毒所   0
                            

                                worksheetEegion.Cells[i, 1].Value = item["indexCode"].ToString();
                                worksheetEegion.Cells[i, 2].Value = item["name"].ToString();
                                worksheetEegion.Cells[i, 3].Value = item["parentIndexCode"].ToString();
                                i++;
                            }
                        }
                        catch (Exception ex)
                        {

                            System.Windows.MessageBox.Show("错误：" + ex.Message);
                        }
                        package.Save();
                    }

                    if (EquipmentText.Count != 0 && !String.IsNullOrEmpty(EegionText)) { System.Windows.MessageBox.Show("完成"); } else { System.Windows.MessageBox.Show("无数据"); }

                }
            }



          


        }
 
    }
}
