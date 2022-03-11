using DeploymentTools.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace DeploymentTools.View
{
    /// <summary>
    /// ZoneSynchronization.xaml 的交互逻辑
    /// </summary>
    public partial class ZoneSynchronization : Window
    {
        PGDataProcessing PGdata = new PGDataProcessing();//pg数据库操作类
        Dictionary<String, JObject> ListData = new Dictionary<String, JObject>();//设备列表json
        List<JObject> SPCCListData = new List<JObject>();//SPCC设备列表json
        public ZoneSynchronization()
        {
            InitializeComponent();

            listView1_Copy.Items.Clear();
            listView1.Items.Clear();
            Task<Dictionary<String, JObject>> t = DataInit();
            SynchronizationButton.IsEnabled = false;//关闭按钮点击
            Boolean error = true;
            Task.Run(async () =>
            {
                ListData = await t;
      
                zho.Dispatcher.Invoke(new Action(() =>
                {
                    zho.Content = "防区总数：";
                   
                }));
                Int32 i = 0;
                foreach (var rbs in ListData.Keys)
                {
                    if (!GetDataSet.GetISCmsgSuccessfulState(JsonConvert.SerializeObject(ListData[rbs])))
                    {
                        listView1.Dispatcher.Invoke(new Action(() =>
                        {
                            listView1_Copy.Items.Add(rbs);//添加错误信息
                            listView1.Items.Add(rbs);//添加错误信息
                        }));
                        error = false;
                        break;
                    }
                    listView1.Dispatcher.Invoke(new Action(() =>
                    {
                        listView1_Copy.Items.Add(rbs);
                    }));

                    foreach (var item in ListData[rbs]["data"]["list"])
                    {

                        listView1.Dispatcher.Invoke(new Action(() =>
                        {
                            i++;
                            listView1.Items.Add(item["defenceName"].ToString());//添加防区名称
                            zho.Content = $"防区总数：{i}";
                        }));
                    }
                  
                }
                if (error)
                {
                    comboBoxInit();
                }
                ///委托 开启同步按钮
                SynchronizationButton.Dispatcher.Invoke(new Action(() =>
                {
                    SynchronizationButton.IsEnabled = true;
                }));

            });
           

        }



        /// <summary>
        /// 初始化下拉框列表
        /// </summary>
        /// <returns></returns>
        private void comboBoxInit() {

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
        private async Task<Dictionary<String, JObject>> DataInit() {
            var list  = await Task.Run(() => new ZoneSynchronizationUI().GetAceesee());
            return list;
        }

        private async Task<List<JObject>> DataSpccInit()
        {
            var list = await Task.Run(() => new ZoneSynchronizationUI().GetSpccAceesee());
            return list;
        }



        //private async Task<String> GetData(bool vl)
        //{
        //    var list = await Task.Run(() => new ZoneSynchronizationUI().EquipmentLibrary(ListData, new ComboBox[] { comboBox1, comboBox2 }, vl));

        //    return list;
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Task<String> t = GetData((Boolean)SPCCSelection.IsChecked);
            //Task.Run(async () =>
            //{
            //    String msg = await t;
            //    this.Dispatcher.Invoke(new Action(() =>
            //    {
            //        MessageBox.Show(msg);
            //    }));
            //});
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
                Int32 i = 0;
                t = DataSpccInit();
                if (t == null) { return; }
                SynchronizationButton.IsEnabled = false;//关闭按钮点击
                Boolean error = true;
                Task.Run(async () =>
                {
                    SPCCListData = await t;
                    //listView1
                    foreach (var items in SPCCListData)
                    {
                        foreach (var item in items["data"]["list"])
                        {
                            listView1.Dispatcher.Invoke(new Action(() =>
                            {
                                listView1.Items.Add(item["channelName"].ToString());//添加错误信息
                                i++;
                                zho.Content = $"防区总数：{i}";
                            }));
                        }
                    }
               
                });
            }

        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                string paths = openFileDialog.SelectedPath;
                if (paths != null)
                {
                    //首先模拟建立将要导出的数据，这些数据都存于DataTable中
                    FileInfo newFile = new FileInfo(paths + @"/防区列表.xlsx");
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(paths + @"/防区列表.xlsx");
                    }


                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("防区列表");
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
                           
                            if ((Boolean)SPCCSelection.IsChecked)
                            {
                                foreach (var items in SPCCListData)
                                {
                                    foreach (var item in items["data"]["list"])
                                    {
                                        listView1.Dispatcher.Invoke(new Action(() =>
                                        {
                                            //listView1.Items.Add(item["channelName"].ToString());//添加错误信息
                                            worksheet.Cells[i, 1].Value = System.Guid.NewGuid().ToString("N"); ;
                                            worksheet.Cells[i, 2].Value = item["regionIndexCode"].ToString();
                                            worksheet.Cells[i, 3].Value = "防区主机";
                                            worksheet.Cells[i, 4].Value = item["channelIndexCode"].ToString();
                                            worksheet.Cells[i, 5].Value = item["channelName"].ToString();
                                            worksheet.Cells[i, 6].Value = item.ToString();
                                            i++;
                                        }));
                                    }
                                }
                            }
                            else {
                                foreach (var rbss in ListData.Keys)
                                {
                                    foreach (var item in ListData[rbss]["data"]["list"])
                                    {
                                        //if ((Boolean)SPCCSelection.IsChecked
                                        //    )
                                        //{
                                        //    item["doorIndexCode"] = item["channelIndexCode"].ToString();
                                        //    item["doorName"] = item["device"]["deviceName"].ToString();
                                        //}
                                        worksheet.Cells[i, 1].Value = System.Guid.NewGuid().ToString("N"); ;
                                        worksheet.Cells[i, 2].Value = item["regionIndexCode"].ToString();
                                        worksheet.Cells[i, 3].Value = "防区";
                                        worksheet.Cells[i, 4].Value = item["defenceIndexCode"].ToString();
                                        worksheet.Cells[i, 5].Value = item["defenceName"].ToString();
                                        worksheet.Cells[i, 6].Value = item.ToString();
                                        i++;
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                            System.Windows.MessageBox.Show("设备结构错误：" + ex.Message);
                        }


                        package.Save();
                    }
                    if (!(Boolean)SPCCSelection.IsChecked)
                        if (ListData.Count != 0) { System.Windows.MessageBox.Show("完成"); } else { System.Windows.MessageBox.Show("无数据"); }
                    else if (SPCCListData.Count != 0) { System.Windows.MessageBox.Show("完成"); } else { System.Windows.MessageBox.Show("无数据"); }

                }
            }


        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            //if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            //{
            //    string paths = openFileDialog.SelectedPath;
            //    if (paths != null)
            //    {
            //        for (int i = 0; i < ListData.Count; i++)
            //        {
            //            string path = paths + "/tempCode" + i + ".Json";
            //            FileStream fs = new FileStream(path, FileMode.Append);
            //            StreamWriter sw = new StreamWriter(fs);
            //            sw.Write(ListData[i]);
            //            sw.Flush();
            //            sw.Close();
            //            fs.Close();

            //        }
            //        if (ListData.Count != 0) { System.Windows.MessageBox.Show("完成"); } else { System.Windows.MessageBox.Show("无数据"); }

            //    }
            //}
        }
    }
}
