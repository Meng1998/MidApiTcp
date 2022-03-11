using DeploymentTools.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace DeploymentTools.View
{
    /// <summary>
    /// SynchronousTalkback.xaml 的交互逻辑
    /// </summary>
    public partial class SynchronousTalkback : Window
    {
        List<JObject> ListData = new List<JObject>();//设备列表json
        PGDataProcessing PGdata = new PGDataProcessing();//pg数据库操作类

        public SynchronousTalkback()
        {
            InitializeComponent();
            comboBoxInit();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task<String> t = GetData((Boolean)SPCCSelection.IsChecked);
            Task.Run(async () =>
            {
                String msg = await t;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(msg);
                }));
            });
        }

        //更新数据库
        private async Task<String> GetData(bool vl)
        {
            var list = await Task.Run(() => new SynchronousTalkbackUI().EquipmentLibrary(ListData, new ComboBox[] { comboBox1, comboBox2 }, vl));
            return list;
        }

        private async Task<List<JObject>> DataSpccInit()
        {
            var list = await Task.Run(() => new SynchronousTalkbackUI().GetSpccAceesee());
            return list;
        }
        /// <summary>
        /// 初始化下拉框列表
        /// </summary>
        /// <returns></returns>
        private void comboBoxInit()
        {

            #region 初始化厂家类型及摄像头类型
            ObservableCollection<ItemContent> list = new ObservableCollection<ItemContent>();
            Boolean error = true;
            DataSet Data = PGdata.ExecuteQuery($"SELECT * FROM \"sys_dictionary\" where pid = '15ad9ba70e024246b3a50d7de2393812'", ref error);
            if (error)
            {
                list = new ObservableCollection<ItemContent>();
                list.Add(new ItemContent() { Name = "外接设备类型", CameraId = null });
                for (int i = 0; i < Data.Tables[0].Rows.Count; i++)
                {
                    list.Add(new ItemContent() { Name = Data.Tables[0].Rows[i]["dic_name"].ToString(), CameraId = Data.Tables[0].Rows[i]["id"].ToString() });
                    this.comboBox1.ItemsSource = list;
                    this.comboBox1.DisplayMemberPath = "Name";
                }
                Data = PGdata.ExecuteQuery($"SELECT * FROM \"sys_dictionary\" where pid = 'c603b6346b5249d1972ec364b4d68a6c'", ref error);
                list = new ObservableCollection<ItemContent>();

                list.Add(new ItemContent() { Name = "平台厂商", CameraId = null });
                for (int i = 0; i < Data.Tables[0].Rows.Count; i++)
                {
                    list.Add(new ItemContent() { Name = Data.Tables[0].Rows[i]["dic_name"].ToString(), CameraId = Data.Tables[0].Rows[i]["id"].ToString() });
                    this.comboBox2.ItemsSource = list;
                    this.comboBox2.DisplayMemberPath = "Name";

                }
            }
            else
            {
                SynchronizationButton.Content = "数据库无法连接";
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            #endregion
        }
        private void SPCCSelection_Checked(object sender, RoutedEventArgs e)
        {

            var SpccSelsecrState = (Boolean)SPCCSelection.IsChecked;
            listView1.Items.Clear();
            Task<List<JObject>> t;
            if (!SpccSelsecrState)
            {

            }
            else
            {
                t = DataSpccInit();

                if (t == null) { return; }
                SynchronizationButton.IsEnabled = false;//关闭按钮点击
                Boolean error = true;
                Task.Run(async () =>
                {
                    ListData = await t;
                    foreach (var rbs in ListData)
                    {
                        if (!GetDataSet.GetISCmsgSuccessfulState(JsonConvert.SerializeObject(rbs)))
                        {
                            listView1.Dispatcher.Invoke(new Action(() =>
                            {
                                listView1.Items.Add(rbs["msg"].ToString());//添加错误信息
                            }));
                            error = false;
                            break;
                        }
                        zho.Dispatcher.Invoke(new Action(() =>
                        {
                            zho.Content = "设备总数：";
                        }));
                        Int32 i = 0;
                        foreach (var item in rbs["data"]["list"])
                        {
                            item["doorIndexCode"] = item["channelIndexCode"].ToString();
                            item["doorName"] = item["device"]["deviceName"].ToString();
                            listView1.Dispatcher.Invoke(new Action(() =>
                            {
                                i++;
                                listView1.Items.Add(item["doorName"].ToString());//添加设备名称
                                zho.Content = $"设备总数：{i}";
                            }));
                        }
                    }
                    ///委托 开启同步按钮
                    SynchronizationButton.Dispatcher.Invoke(new Action(() =>
                    {
                        SynchronizationButton.IsEnabled = true;
                    }));

                });
            }

        }
    }
}
