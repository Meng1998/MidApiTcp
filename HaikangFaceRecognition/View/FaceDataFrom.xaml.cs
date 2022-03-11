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

namespace DeploymentTools.View
{
    /// <summary>
    /// FaceDataFrom.xaml 的交互逻辑
    /// </summary>
    public partial class FaceDataFrom : Window
    {
        Timer BlacklistPersonnelTimer = new Timer();
        InitKey Init = new InitKey();
        public FaceDataFrom()
        {
            InitializeComponent();

            {//启动异步刷新UI数据
                BlacklistPersonnelTimer.Interval = 2000;
                BlacklistPersonnelTimer.Elapsed += InitListDataUI;
                BlacklistPersonnelTimer.AutoReset = false;
                BlacklistPersonnelTimer.Start();
            }

        }

        private void InitListDataUI(object sender, ElapsedEventArgs e)
        {
            EventListData.Dispatcher.Invoke(new Action(() =>//委托处理UI
            {
                EventListData.ItemsSource = null;
            }));
            ObservableCollection<ExpandoObject> items = new ObservableCollection<ExpandoObject>();
            String EegionText = Init.GetKeyPersonnel( 1, 1000);
            if (!String.IsNullOrEmpty(EegionText) && EegionText != "error : 远程服务器无法连接")
            {
                JObject rbs = JsonConvert.DeserializeObject<JObject>(EegionText);
                if (rbs["msg"].ToString() == "success")//判断OK
                {
                    foreach (var item in rbs["data"]["list"])
                    {
                        dynamic itemc = new ExpandoObject();
                        itemc.NameN = item["faceInfo"]["name"].ToString();
                        itemc.IndexCode = item["indexCode"].ToString();
                        itemc.ImageUrl = item["facePic"]["faceUrl"].ToString();
                        
                        items.Add(itemc);
                    }
                    EventListData.Dispatcher.Invoke(new Action(() =>
                    {
                        EventListData.ItemsSource = items;
                    }));
                }
                else
                {
                    BlacklistPersonnelTimer.Stop();

                    InformationTips F2 = new InformationTips();
                    F2.Init("获取数据", "获取数据失败 Error" + EegionText);
                    F2.ShowDialog();
                    if (F2.needChangeUI)
                    {
                        //OK
                    }
                }
            }
            else
            {
                BlacklistPersonnelTimer.Stop();
                InformationTips F2 = new InformationTips();
                F2.Init("获取数据", "获取数据失败 Error" + EegionText);
                F2.ShowDialog();
                if (F2.needChangeUI)
                {
                    //OK
                }
            }
            BlacklistPersonnelTimer.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
