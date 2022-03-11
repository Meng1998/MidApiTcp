using DeploymentTools.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Configuration;
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
    /// parkList.xaml 的交互逻辑
    /// </summary>
    public partial class parkList : Window
    {
        public parkList()
        {
            InitializeComponent();
        }
        InitKey Init = new InitKey();
        Newtonsoft.Json.Linq.JObject PartList = null;

        List<Parents> PList = new List<Parents>();
        List<childs> CList = new List<childs>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string paths = openFileDialog.SelectedPath;
                if (paths != null)
                {
                    string Part = ISCPostParkList(1);
                    string roadway = "";
                    try
                    {
                        PartList = Newtonsoft.Json.Linq.JObject.Parse(Part);
                        foreach (var item in PartList["data"])
                        {
                            string roadwaystr = roadwayList(1, item["parkIndexCode"].ToString());
                            PList.Add(new Parents { id = item["parkIndexCode"].ToString(), name = item["parkName"].ToString(), pid = item["parentParkIndexCode"].ToString() });

                            JObject roadwayJSON = null;
                            try
                            {
                                roadwayJSON = JObject.Parse(roadwaystr);

                                if (roadwayJSON["data"].ToArray().Length > 0)
                                {
                                    foreach (var items in roadwayJSON["data"])
                                    {
                                        CList.Add(new childs { id = Guid.NewGuid().ToString("N"), name = items["roadwayName"].ToString(), pid = items["entranceIndexCode"].ToString(), code = items["roadwayIndexCode"].ToString() });
                                    }
                                }
                            }
                            catch (Exception)
                            {

                                MessageBox.Show(roadwaystr);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Part);
                    }

                    //首先模拟建立将要导出的数据，这些数据都存于DataTable中
                    FileInfo newFile = new FileInfo(paths + @"\Roadway.xlsx");
                    FileInfo newFileEegion = new FileInfo(paths + @"\Part.xlsx");
                    if (newFile.Exists)
                    {
                        newFile.Delete();
                        newFile = new FileInfo(paths + @"\Roadway.xlsx");
                    }

                    if (newFileEegion.Exists)
                    {
                        newFileEegion.Delete();
                        newFileEegion = new FileInfo(paths + @"\Part.xlsx");
                    }
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(newFile))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Roadway");
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
                            foreach (childs item in CList)
                            {
                                worksheet.Cells[i, 1].Value = item.id.ToString();
                                worksheet.Cells[i, 2].Value = item.pid.ToString();
                                worksheet.Cells[i, 3].Value = "闸机";
                                worksheet.Cells[i, 4].Value = item.code.ToString();
                                worksheet.Cells[i, 5].Value = item.name.ToString();
                                worksheet.Cells[i, 6].Value = JsonConvert.SerializeObject(item) ;
                                i++;
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
                        ExcelWorksheet worksheetEegion = package.Workbook.Worksheets.Add("Roadway");
                        //组织机构id 组织机构名称  父节点id
                        worksheetEegion.Cells[1, 1].Value = "组织机构id";
                        worksheetEegion.Cells[1, 2].Value = "组织机构名称";
                        worksheetEegion.Cells[1, 3].Value = "父节点id";
                        try
                        {
                            int i = 2;
                            foreach (Parents item in PList)
                            {
                                worksheetEegion.Cells[i, 1].Value = item.id.ToString();
                                worksheetEegion.Cells[i, 2].Value = item.name.ToString();
                                worksheetEegion.Cells[i, 3].Value = item.pid.ToString();
                                i++;
                            }
                        }
                        catch (Exception ex)
                        {

                            System.Windows.MessageBox.Show("错误：" + ex.Message);
                        }
                        package.Save();
                    }
                    MessageBox.Show("完成");
                }
            }

        }

        private String ISCPostParkList(int PageIndex)
        {
            return Init.GetParkList(1, 1000);
        }

        private String roadwayList(int PageIndex, string code)
        {
            return Init.roadwayList(1, 1000, code);
        }
    }

    class Parents
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string name { get; set; }
    }
    class childs
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }
}
