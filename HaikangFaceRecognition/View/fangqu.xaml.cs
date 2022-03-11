using DeploymentTools.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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

namespace DeploymentTools.View
{
    /// <summary>
    /// fangqu.xaml 的交互逻辑
    /// </summary>
    public partial class fangqu : Window
    {
        public fangqu()
        {
            InitializeComponent();
        }

        List<Parents> PList = new List<Parents>();
        List<childs> CList = new List<childs>();
        InitKey Init = new InitKey();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string paths = openFileDialog.SelectedPath;
                if (paths != null)
                {

                    string zhuji = ISCbaojingzhuji(1);
                    string zhujijitongdaohao = GetFangquzhujitongdaohao(1);
                    try
                    {
                        JObject zhujiList = Newtonsoft.Json.Linq.JObject.Parse(zhuji);
                        JObject tongdaohaoList = Newtonsoft.Json.Linq.JObject.Parse(zhujijitongdaohao);

                        //首先模拟建立将要导出的数据，这些数据都存于DataTable中
                        FileInfo newFile = new FileInfo(paths + @"\防区设备列表.xlsx");
                        FileInfo newFileEegion = new FileInfo(paths + @"\防区组织结构.xlsx");
                        if (newFile.Exists)
                        {
                            newFile.Delete();
                            newFile = new FileInfo(paths + @"\防区组织结构.xlsx");
                        }

                        if (newFileEegion.Exists)
                        {
                            newFileEegion.Delete();
                            newFileEegion = new FileInfo(paths + @"\防区设备列表.xlsx");
                        }
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        using (ExcelPackage package = new ExcelPackage(newFile))
                        {
                           ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("防区组织结构");
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
                                foreach (var item in tongdaohaoList["data"]["list"])
                                {
                                    worksheet.Cells[i, 1].Value = Guid.NewGuid().ToString("N");
                                    worksheet.Cells[i, 2].Value = item["parentIndexCode"].ToString();
                                    worksheet.Cells[i, 3].Value = "防区设备";
                                    worksheet.Cells[i, 4].Value = item["indexCode"].ToString();
                                    worksheet.Cells[i, 5].Value = item["name"].ToString();
                                    worksheet.Cells[i, 6].Value = item.ToString();
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
                            ExcelWorksheet worksheetEegion = package.Workbook.Worksheets.Add("防区设备列表");
                            //组织机构id 组织机构名称  父节点id
                            worksheetEegion.Cells[1, 1].Value = "组织机构id";
                            worksheetEegion.Cells[1, 2].Value = "组织机构名称";
                            worksheetEegion.Cells[1, 3].Value = "父节点id";
                            try
                            {
                                int i = 2;
                                foreach (var item in zhujiList["data"]["list"])
                                {
                                    worksheetEegion.Cells[i, 1].Value = item["indexCode"].ToString() ;
                                    worksheetEegion.Cells[i, 2].Value = item["name"].ToString();
                                    worksheetEegion.Cells[i, 3].Value = item["regionIndexCode"].ToString();
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
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString()); ;
                    }
                }
            }
        }


        private String ISCbaojingzhuji(int PageIndex)
        {
            return Init.GetFangquzhuji(1, 1000);
        }

        private String GetFangquzhujitongdaohao(int PageIndex)
        {
            return Init.GetFangquzhujitongdaohao(1, 1000, "defence");
        }


    }


   

}
