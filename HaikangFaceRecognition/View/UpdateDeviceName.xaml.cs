using DeploymentTools.Logic;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
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

namespace DeploymentTools.View
{
    /// <summary>
    /// UpdateDeviceName.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateDeviceName : Window
    {
        public UpdateDeviceName()
        {
            InitializeComponent();
        }
        String data;
        Dictionary<String, String> EquipmentPageIndex = new Dictionary<String, String>();
        String sqlstr = null; ObservableCollection<Object> items = new ObservableCollection<Object>();
        private void But_Import_Click(object sender, RoutedEventArgs e)
        {
            //创建一个打开文件式的对话框
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;//该值确定是否可以选择多个文件
            ofd.Title = "请选择文件";
            ofd.Filter = "JSON 文件 (*.json)|*.json";//过滤选项设置，文本文件，所有文件。
            if (ofd.ShowDialog() == true)
            {
                string file = ofd.FileName;
                data = System.IO.File.ReadAllText(file);
                PGDataProcessing PGdata = new PGDataProcessing();
                Boolean error = true;
                DataSet map_device = PGdata.ExecuteQuery("SELECT * FROM map_device", ref error);

                if (error)
                {
                    listView1.Items.Clear(); 
                    listView1_Copy.Items.Clear();
                    EquipmentPageIndex.Clear();
                    for (int i = 0; i < map_device.Tables[0].Rows.Count; i++)
                    {
                        EquipmentPageIndex.Add(map_device.Tables[0].Rows[i]["device_code"].ToString(), map_device.Tables[0].Rows[i]["device_name"].ToString());
                    }
                    if (EquipmentPageIndex.Count <= 0) {
                        DataSet dock_device = PGdata.ExecuteQuery("SELECT * FROM dock_device", ref error);
                        for (int i = 0; i < dock_device.Tables[0].Rows.Count; i++)
                        {
                            EquipmentPageIndex.Add(dock_device.Tables[0].Rows[i]["device_code"].ToString(), dock_device.Tables[0].Rows[i]["device_name"].ToString());
                        }
                    }
                    JObject rb;
                    try
                    {
                        rb = JsonConvert.DeserializeObject<JObject>(data);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                        return;
                    }

                    foreach (var item in rb["data"]["list"])
                    {
                        bool code = true;
                        foreach (var items in EquipmentPageIndex.Keys)
                        {
                            if (items == item["cameraIndexCode"].ToString())
                            {
                                if (item["cameraName"].ToString() == EquipmentPageIndex[items]) {
                                    code = false;
                                }
                            }
                        }
                        if (code)
                        {
                            DataSet EPages = PGdata.ExecuteQuery($"SELECT * FROM map_device where device_code = '{item["cameraIndexCode"].ToString()}'", ref error);
                            DataSet dock_device = PGdata.ExecuteQuery($"SELECT * FROM map_device where dock_device = '{item["cameraIndexCode"].ToString()}'", ref error);
                            if (EPages.Tables[0].Rows.Count != 0 && dock_device.Tables[0].Rows.Count != 0)
                            {
                                listView1.Items.Add(new { Name = item["cameraName"].ToString(), Code = item["cameraIndexCode"].ToString() });
                                sqlstr += $"UPDATE \"public\".\"map_device\" SET  \"device_name\" = '{item["cameraName"].ToString()}' WHERE \"device_code\" = '{item["cameraIndexCode"].ToString()}';";
                                sqlstr += $"UPDATE \"public\".\"dock_device\" SET  \"device_name\" = '{item["cameraName"].ToString()}' WHERE \"device_code\" = '{item["cameraIndexCode"].ToString()}';";

                            }
                            else {
                                listView1_Copy.Items.Add(new { Name = item["cameraName"].ToString(), Code = item["cameraIndexCode"].ToString() });
                            }
                            
                        }
                    }
                   
                }
                else
                {
                    InformationTips F2 = new InformationTips();
                    F2.Init("数据库", "有可能出现了数据库连接错误");
                    F2.ShowDialog();
                }
            }
        }
     
        private void But_Toupdate_Click(object sender, RoutedEventArgs e)
        {
            Boolean error = true;
            if (!String.IsNullOrEmpty(sqlstr))
                new PGDataProcessing().ExecuteQuery(sqlstr, ref error);
            
            if (error && !String.IsNullOrEmpty(sqlstr))
            {
                InformationTips F2 = new InformationTips();
                F2.Init("更新监控点", "监控点更新完成。请前往网站后台核实是否完成");
                F2.ShowDialog();
            }
            else
            {
                InformationTips F2 = new InformationTips();
                F2.Init("更新监控点", "监控点更新失败。");
                F2.ShowDialog();
            }
        }
    }
}
