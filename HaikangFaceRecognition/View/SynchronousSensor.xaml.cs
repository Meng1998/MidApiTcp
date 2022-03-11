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
    /// SynchronousSensor.xaml 的交互逻辑
    /// </summary>
    public partial class SynchronousSensor : Window
    {
     
        PGDataProcessing PGdata = new PGDataProcessing();//pg数据库操作类
        List<JObject> ListData = new List<JObject>();//设备列表json
        public SynchronousSensor()
        {
            InitializeComponent();


            listView1.Items.Clear();
            Task<List<JObject>> t = DataInit();
            SynchronizationButton.IsEnabled = false;//关闭按钮点击

            SynchronizationButton.Content = "暂不支持4.0同步";
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
                        listView1.Dispatcher.Invoke(new Action(() =>
                        {
                            i++;
                            listView1.Items.Add(item["name"].ToString());//添加设备名称
                            zho.Content = $"设备总数：{i}";
                        }));
                    }
                }
                ///委托 开启同步按钮
                ////SynchronizationButton.Dispatcher.Invoke(new Action(() =>
                //{
                //    //SynchronizationButton.IsEnabled = true;

                //    //MessageBox.Show(ListData[0].ToString());
                //}));

            });
            if (error)
            {
                comboBoxInit();
            }

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
                //SynchronizationButton.Content = "数据库无法连接";
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            #endregion
        }
        private async Task<List<JObject>> DataInit()
        {
            var list = await Task.Run(() => new SynchronousSensorUI().GetAceesee());
            return list;
        }

        private async Task<List<JObject>> DataSpccInit()
        {
            var list = await Task.Run(() => new SynchronousSensorUI().GetSpccAceesee());
            return list;
        }



        private async Task<String> GetData(bool vl)
        {
            var list = await Task.Run(() => new SynchronousSensorUI().EquipmentLibrary(ListData, new ComboBox[] { comboBox1, comboBox2 }, vl));

            return list;
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
                //SynchronizationButton.IsEnabled = false;//关闭按钮点击
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
                        //SynchronizationButton.IsEnabled = true;
                    }));

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
                    FileInfo newFile = new FileInfo(paths + @"/消防传感器列表.xlsx");
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(paths + @"/消防传感器列表.xlsx");
                    }


                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("消防传感器列表");
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
                            foreach (var items in ListData)
                            {
                                JObject rbs = items;
                                foreach (var item in rbs["data"]["list"])
                                {
                                    if ((Boolean)SPCCSelection.IsChecked
                                        )
                                    {
                                        //item["doorIndexCode"] = item["channelIndexCode"].ToString();
                                        //item["doorName"] = item["device"]["deviceName"].ToString();
                                        MessageBox.Show("暂不支持 SPCC。");
                                        break;
                                    }
                                    worksheet.Cells[i, 1].Value = System.Guid.NewGuid().ToString("N"); ;
                                    worksheet.Cells[i, 2].Value = item["regionIndexCode"].ToString();
                                    worksheet.Cells[i, 3].Value = "消防传感器";
                                    worksheet.Cells[i, 4].Value = item["indexCode"].ToString();
                                    worksheet.Cells[i, 5].Value = item["name"].ToString();
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

                    if (ListData.Count != 0) { System.Windows.MessageBox.Show("完成"); } else { System.Windows.MessageBox.Show("无数据"); }

                }
            }


        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                string paths = openFileDialog.SelectedPath;
                if (paths != null)
                {
                    for (int i = 0; i < ListData.Count; i++)
                    {
                        string path = paths + "/tempCode" + i + ".Json";
                        FileStream fs = new FileStream(path, FileMode.Append);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Write(ListData[i]);
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                    }
                    if (ListData.Count != 0) { System.Windows.MessageBox.Show("完成"); } else { System.Windows.MessageBox.Show("无数据"); }

                }
            }
        }


    }
}
