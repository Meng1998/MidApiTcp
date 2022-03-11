using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeploymentTools.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DeploymentTools.Mod;
using System.Timers;

namespace DeploymentTools.View
{

    /// <summary>
    /// Sidebar.xaml 的交互逻辑
    /// </summary>
    public partial class Sidebar : UserControl
    {
        Int32 IntCountClick = 0;//lable单击事件按下弹起计数
        Int32 dingGridISVisibility = 0;//控制侧边弹出栏是否可见
        Int32 NumberCount = 0;
        Timer thread = new Timer();
        InitKey Init = new InitKey();
        public Sidebar()
        {
            InitializeComponent();
            HiddenSidePop_up();

            thread.AutoReset = false;
            thread.Interval = 1000;
            thread.Elapsed += InitKeyParameter;
            new GetDataSet().InitParameters();
            //Thread thread = new Thread(InitKeyParameter);
            thread.Start();
            LoadingState.SetUpdateState("正在加载监控点...");
        }
        TreeViewItem mtrNode = new TreeViewItem();
        private void InitKeyParameter(object sender, ElapsedEventArgs e)
        {
            FileTree.Dispatcher.Invoke(new Action(() =>
            {
                FileTree.Items.Clear();//清除树形控件内容
            }));

          

            String parameter = null;
            try
            {
                parameter = Init.GetPagingAreaData( 1, 1);

                //parameter = "{\"code\":\"0\",\"msg\":\"SUCCESS\",\"data\":{\"total\":76,\"pageNo\":1,\"pageSize\":1,\"list\":[{\"indexCode\":\"root000000\",\"name\":\"中南财经政法大学（南湖校区）\",\"parentIndexCode\":null,\"treeCode\":\"0\"}]}}";
                if (!String.IsNullOrEmpty(parameter) && parameter != "error : 远程服务器无法连接" && JsonConvert.DeserializeObject<JObject>(parameter)["msg"].ToString() == "SUCCESS")
                {
                    JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
                    String total = rb["data"]["total"].ToString();//取出单页最大数
                    AreaPages.SetPages(Int32.Parse(total));//给到全局最大单页数以后好取
                    //分页
                    Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));
                    for (int i = 0; i < page; i++)
                    {
                        String OverallMonitoring = /*parameter;//*/Init.GetPagingAreaData((i + 1), 1000);//取出全部区域
                        Style myStyle = (Style)this.FindResource("MaterialDesignTreeViewItem");//TabItemStyle 这个洋式是引用的资源文件中的洋式名称
                        JObject rbs = JsonConvert.DeserializeObject<JObject>(OverallMonitoring);
                        //test = rbs["data"]["list"][0]["name"].ToString();


                        foreach (var item in rbs["data"]["list"])
                        {
                            Dispatcher.BeginInvoke(new Action(delegate
                            {
                                mtrNode = new TreeViewItem();
                                mtrNode.Header = item["name"].ToString();//树形控件头部(一级)
                            }));

                            {//二级菜单
                                String MonitoringPoint =
                                    //"{\"code\":\"0\",\"msg\":\"SUCCESS\",\"data\":{\"total\":4,\"pageNo\":1,\"pageSize\":1,\"list\":[{\"altitude\":null,\"cameraIndexCode\":\"13a04197267a4e6eb2677cee8db7c277\",\"cameraName\":\"亲民路.望湖公寓T字路口(二)S16008\",\"cameraType\":0,\"cameraTypeName\":\"枪机\",\"capabilitySet\":\"event_ias,io,vss,ptz,event_io,event_rule,net,maintenance,event_pdc,event_device,status\",\"capabilitySetName\":null,\"intelligentSet\":null,\"intelligentSetName\":null,\"channelNo\":\"1\",\"channelType\":\"analog\",\"channelTypeName\":\"模拟通道\",\"createTime\":\"2019-07-05T12:44:33.434+08:00\",\"encodeDevIndexCode\":\"3f74fe01d1f34ddc9bf67b8155364281\",\"encodeDevResourceType\":null,\"encodeDevResourceTypeName\":null,\"gbIndexCode\":null,\"installLocation\":null,\"keyBoardCode\":null,\"latitude\":null,\"longitude\":null,\"pixel\":null,\"ptz\":null,\"ptzName\":null,\"ptzController\":null,\"ptzControllerName\":null,\"recordLocation\":null,\"recordLocationName\":null,\"regionIndexCode\":\"daa92fb6-6a7a-4564-a6dd-4191872af653\",\"status\":null,\"statusName\":null,\"transType\":1,\"transTypeName\":\"TCP\",\"treatyType\":null,\"treatyTypeName\":null,\"viewshed\":null,\"updateTime\":\"2019-07-05T12:44:33.483+08:00\"},{\"altitude\":null,\"cameraIndexCode\":\"fe4915bb15b14658b30f84b6bbe27fc9\",\"cameraName\":\"济世路.文添楼东南角(二).S16005\",\"cameraType\":0,\"cameraTypeName\":\"枪机\",\"capabilitySet\":\"event_ias,io,vss,ptz,event_io,event_rule,net,maintenance,event_pdc,event_device,status\",\"capabilitySetName\":null,\"intelligentSet\":null,\"intelligentSetName\":null,\"channelNo\":\"1\",\"channelType\":\"analog\",\"channelTypeName\":\"模拟通道\",\"createTime\":\"2019-07-05T12:44:33.435+08:00\",\"encodeDevIndexCode\":\"e209d46f479f4984953c1915a1e7d8f5\",\"encodeDevResourceType\":null,\"encodeDevResourceTypeName\":null,\"gbIndexCode\":null,\"installLocation\":null,\"keyBoardCode\":null,\"latitude\":null,\"longitude\":null,\"pixel\":null,\"ptz\":null,\"ptzName\":null,\"ptzController\":null,\"ptzControllerName\":null,\"recordLocation\":null,\"recordLocationName\":null,\"regionIndexCode\":\"daa92fb6-6a7a-4564-a6dd-4191872af653\",\"status\":null,\"statusName\":null,\"transType\":1,\"transTypeName\":\"TCP\",\"treatyType\":null,\"treatyTypeName\":null,\"viewshed\":null,\"updateTime\":\"2019-07-05T12:44:33.483+08:00\"},{\"altitude\":null,\"cameraIndexCode\":\"4f2db17699704b809a0c590d94fc4d24\",\"cameraName\":\"济世路.文添楼西北角(二).S16002\",\"cameraType\":0,\"cameraTypeName\":\"枪机\",\"capabilitySet\":\"event_ias,io,vss,ptz,event_io,event_rule,net,maintenance,event_pdc,event_device,status\",\"capabilitySetName\":null,\"intelligentSet\":null,\"intelligentSetName\":null,\"channelNo\":\"1\",\"channelType\":\"analog\",\"channelTypeName\":\"模拟通道\",\"createTime\":\"2019-07-05T12:44:33.437+08:00\",\"encodeDevIndexCode\":\"c9d73bca637749aa8642b0709122271a\",\"encodeDevResourceType\":null,\"encodeDevResourceTypeName\":null,\"gbIndexCode\":null,\"installLocation\":null,\"keyBoardCode\":null,\"latitude\":null,\"longitude\":null,\"pixel\":null,\"ptz\":null,\"ptzName\":null,\"ptzController\":null,\"ptzControllerName\":null,\"recordLocation\":null,\"recordLocationName\":null,\"regionIndexCode\":\"daa92fb6-6a7a-4564-a6dd-4191872af653\",\"status\":null,\"statusName\":null,\"transType\":1,\"transTypeName\":\"TCP\",\"treatyType\":null,\"treatyTypeName\":null,\"viewshed\":null,\"updateTime\":\"2019-07-05T12:44:33.483+08:00\"},{\"altitude\":null,\"cameraIndexCode\":\"d0a6591e1e4a4700a97c8c876fc09228\",\"cameraName\":\"济世路.文添楼西南角(二).S16004\",\"cameraType\":0,\"cameraTypeName\":\"枪机\",\"capabilitySet\":\"event_ias,io,vss,ptz,event_io,event_rule,net,maintenance,event_pdc,event_device,status\",\"capabilitySetName\":null,\"intelligentSet\":null,\"intelligentSetName\":null,\"channelNo\":\"1\",\"channelType\":\"analog\",\"channelTypeName\":\"模拟通道\",\"createTime\":\"2019-07-05T12:44:33.436+08:00\",\"encodeDevIndexCode\":\"e468a3b5167a46e499be58427fede5e4\",\"encodeDevResourceType\":null,\"encodeDevResourceTypeName\":null,\"gbIndexCode\":null,\"installLocation\":null,\"keyBoardCode\":null,\"latitude\":null,\"longitude\":null,\"pixel\":null,\"ptz\":null,\"ptzName\":null,\"ptzController\":null,\"ptzControllerName\":null,\"recordLocation\":null,\"recordLocationName\":null,\"regionIndexCode\":\"daa92fb6-6a7a-4564-a6dd-4191872af653\",\"status\":null,\"statusName\":null,\"transType\":1,\"transTypeName\":\"TCP\",\"treatyType\":null,\"treatyTypeName\":null,\"viewshed\":null,\"updateTime\":\"2019-07-05T12:44:33.483+08:00\"}]}}";
                                    Init.GetMonitoringPointData( item["indexCode"].ToString(), 2);//树形二级
                                if (!String.IsNullOrEmpty(MonitoringPoint) && MonitoringPoint != "error : 远程服务器无法连接")
                                {
                                    JObject MonitoringPointData = JsonConvert.DeserializeObject<JObject>(MonitoringPoint);
                                    foreach (var MonitoringName in MonitoringPointData["data"]["list"])
                                    {
                                        NumberCount++;
                                        Dispatcher.BeginInvoke(new Action(delegate
                                        {
                                            TreeViewItem SecondLevel = new TreeViewItem
                                            {
                                                Header = MonitoringName["cameraName"].ToString(),//树形二级
                                                Style = myStyle,
                                                Tag = MonitoringName["cameraIndexCode"].ToString()
                                            };//二级
                                            mtrNode.Items.Add(SecondLevel);
                                            mtrNode.Style = myStyle;
                                        }));
                                    }

                                }
                                else
                                {
                                    LoadingState.SetUpdateState("与服务器连接失败！，请检查服务器是否已下线或在同一网关中 错误信息：" + parameter);
                                }
                            }
                            Dispatcher.BeginInvoke(new Action(delegate
                            {
                                FileTree.Items.Add(mtrNode);
                            }));

                        }
                    }
                    LoadingState.SetUpdateState("就绪");
                    NumberCount = 0;
                }
                else
                {
                    LoadingState.SetUpdateState("与服务器连接失败！，请检查服务器是否已下线或在同一网关中 错误信息：" + parameter);
                    UpdateStatus.Dispatcher.Invoke(new Action(() =>
                    {
                        UpdateStatus.Visibility = Visibility.Hidden;
                    }));
                }
                UpdateStatus.Dispatcher.Invoke(new Action(() =>
                {
                    UpdateStatus.Visibility = Visibility.Hidden;
                }));

            }
            catch (Exception ex)
            {
                LoadingState.SetUpdateState("错误信息：" + ex.Message);
                Log.WriteLog("error", ex.Message);
            }
            thread.Close();
            thread.Stop();
        }

        /// <summary>
        /// 隐藏侧边弹出
        /// </summary>
        private void HiddenSidePop_up()
        {
            dingGridISVisibility = 0;
            SideUnfoldingGrid.Visibility = Visibility.Collapsed;//侧边弹出栏隐藏
        }
        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            ////FaceButton
            ////ButtonBorder
            //FaceButton.BorderBrush = new SolidColorBrush(Color.FromArgb(100, 0, 122, 204));
            ////ButtonBorders.Background = new SolidColorBrush(Color.FromArgb(100, 0, 122, 204));
            //MessageBox.Show("hello");
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ENDBox.Background = new SolidColorBrush(Color.FromArgb(255, 20, 134, 211));
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {

            ENDBox.Background = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {//得到textbox控件焦点
            if (textBox.Text == "搜索监控点")
            {
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 238));
                textBox.Text = null;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {//失去textbox控件焦点//135 135 135
            if (String.IsNullOrEmpty(textBox.Text))
            {
                textBox.Foreground = new SolidColorBrush(Color.FromArgb(255, 135, 135, 135));
                textBox.Text = "搜索监控点";
            }
        }

        private void FaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (SideUnfoldingGrid.Visibility == Visibility.Collapsed)
            {
                SideUnfoldingGrid.Visibility = Visibility.Visible;

            }
            else
            {
                SideUnfoldingGrid.Visibility = Visibility.Collapsed;
            }
        }


        private void BorderT1_MouseEnter(object sender, MouseEventArgs e)
        {
            Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 134, 222));
            FaceButton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 134, 222));
        }

        private void BorderT1_MouseLeave(object sender, MouseEventArgs e)
        {
            FaceButton.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 63, 63, 70));
            Monitor.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
        }

        private void Monitor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IntCountClick < 1)
            {
                IntCountClick++;
            }
            else if (IntCountClick >= 1)
            {
                IntCountClick = 0;
                SideUnfoldingGrid.Visibility = Visibility.Collapsed;

                if (dingGridISVisibility == 0)
                {
                    dingGridISVisibility++;
                    SideUnfoldingGrid.Visibility = Visibility.Visible;
                }
                else
                if (dingGridISVisibility > 0)
                {
                    dingGridISVisibility = 0;
                    SideUnfoldingGrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Monitor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IntCountClick < 1)
            {
                IntCountClick++;
            }
            else if (IntCountClick >= 1)
            {
                IntCountClick = 0;
                if (dingGridISVisibility > 0)
                {
                    dingGridISVisibility = 0;
                    SideUnfoldingGrid.Visibility = Visibility.Collapsed;
                }
                else
                if (dingGridISVisibility == 0)
                {
                    dingGridISVisibility++;
                    SideUnfoldingGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void ENDBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HiddenSidePop_up();
        }
        //private  MainWindow F1 = new MainWindow();
        private void FileTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem items = FileTree.SelectedItem as TreeViewItem;
            try
            {
                string result = items.Tag.ToString();
                if (!String.IsNullOrEmpty(result))
                {
                    InitKey Init = new InitKey();
                    String FlowURL = Init.GetFlowURL( result, 0, "rtsp", 1);
                    JObject FlowURLJson = JsonConvert.DeserializeObject<JObject>(FlowURL);
                    String url = FlowURLJson["data"]["url"].ToString();//取流

                    if (!String.IsNullOrEmpty(url))
                    {
                        new ControlOperation().VlcControlPlay(url);//vlc播放
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != "未将对象引用设置到对象的实例。")
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("错误信息", $"值不可为空或其他 错误信息：{ex.Message}");
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
