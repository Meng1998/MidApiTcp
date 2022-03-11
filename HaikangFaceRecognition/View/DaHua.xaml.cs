using DeploymentTools.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Web.SessionState;
using System.Windows.Forms.VisualStyles;
using OfficeOpenXml;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using D;
using H;
using static H.H;
using LinqToDB.Tools;
using System.Web.UI.WebControls;
using System.Xml;

namespace DeploymentTools.View
{
    /// <summary>
    /// DaHua_Login.xaml 的交互逻辑
    /// </summary>
    public partial class DaHua_Login : Window
    {
        public DaHua_Login()
        {
            InitializeComponent();
            DataListUI();
        }
        ObservableCollection<ExpandoObject> items = new ObservableCollection<ExpandoObject>();
        dynamic item = new ExpandoObject();
        public static Backup Backup = new Backup();
        Dictionary<string, OrgNode> OrgInfo = new Dictionary<string, OrgNode>();
        Dictionary<string, OrgNodeS> OrgInfoS = new Dictionary<string, OrgNodeS>();
        List<Parent> Parents = new List<Parent>();
        List<Child> Childs = new List<Child>();
        private void DataListUI()
        {
            items = new ObservableCollection<ExpandoObject>();
            item = new ExpandoObject();
            item.NmaeStr = "大华";
            item.KeyIP = Config.GetConfigValue($"DaHuaKeyList:KeyIP");
            item.KeyPort = Int32.Parse(Config.GetConfigValue($"DaHuaKeyList:KeyPort"));
            item.KeyUserName = Config.GetConfigValue($"DaHuaKeyList:KeyUserName");
            item.KeyappPassWord = Config.GetConfigValue($"DaHuaKeyList:KeyappPassWord");
            item.Index = $"0";
            items.Add(item);
            EventListData.ItemsSource = items;
        }





        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Click(object sender, RoutedEventArgs e)
        {
            InformationTips F2 = new InformationTips();

            var Str = Backup.init();
            F2.Init("提示", Str);
            F2.ShowDialog();
            Login_Info_t loginInfo = new Login_Info_t();
            loginInfo.szIp = item.KeyIP.ToString();//"10.35.92.116";
            loginInfo.nPort = (uint)item.KeyPort;
            loginInfo.szUsername = item.KeyUserName.ToString();
            loginInfo.szPassword = item.KeyappPassWord.ToString();
            loginInfo.nProtocol = dpsdk_protocol_version_e.DPSDK_PROTOCOL_VERSION_II;
            loginInfo.iType = 1;

            InformationTips F3 = new InformationTips();
            F3.Init("提示", Backup.Login(loginInfo));
            F3.ShowDialog();
        }


        /// <summary>
        /// 获取监控设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Copy_Click(object sender, RoutedEventArgs e)
        {
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load("XMLFile1.xml");
            //string strXML = xmlDoc.InnerXml;


            InformationTips F2 = new InformationTips();
           string strXML = Backup.GetXml();
            Log.WriteLog("大华设备结构", strXML);
            if (strXML == "")
            {
                F2.Init("提示", "导出失败");
            }
            else
            {
             //zhongliang(strXML);
             
            CreateOrgTreeByXML(strXML.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", ""));
            //CreateOrgTreeByXML(strXML);
            F2.Init("提示", "导出成功");
        }
        F2.ShowDialog();
        }



        /// <summary>
        /// 处理XML,导出成execl
        /// </summary>
        /// <param name="strXML">xml格式的文件</param>
        /// <returns></returns>
        private string CreateOrgTreeByXML(String strXML)
        {
            // Log.WriteLog("获取到的数据", strXML);
            OrgInfo.Clear();
            OrgInfoS.Clear();
            try
            {

                string strOrg = strXML.Split(new string[] { "<Devices>" }, StringSplitOptions.None)[0];//[0]取组织树信息字符串
                string[] strDepartment = strOrg.Split(new string[] { "<Department" }, StringSplitOptions.None);//Department分割组织节点信息
                int i = 0;
                //  List<OrgNode> OrgInfo = new List<OrgNode>();//保存节点信息列表
                foreach (string str in strDepartment)
                {
                    if (i == 1)//第一个节点
                    {
                        OrgNode node = new OrgNode();
                        node.child = new List<string>();

                        int pos1 = str.IndexOf('"', 0);
                        int pos2 = str.IndexOf('"', pos1 + 1);
                        string coding = str.Substring(pos1 + 1, pos2 - pos1 - 1);
                        pos1 = str.IndexOf('"', pos2 + 1);
                        pos2 = str.IndexOf("modifytime", pos1 + 1);
                        string name = str.Substring(pos1 + 1, pos2 - pos1 - 1).Trim();
                        string strName = name.Substring(0, name.Length - 1);//去除"
                                                                            //byte[] btArr = Encoding.Default.GetBytes(strName);
                                                                            //byte[] tempArr = Encoding.Convert(Encoding.UTF8, Encoding.Default, btArr);
                                                                            //string szName = Encoding.Default.GetString(tempArr, 0, tempArr.Length);//避免中文名称乱码
                        node.code = coding;
                        node.name = strName;
                        //node.name = szName;
                        pos1 = str.IndexOf('>', 0);//找到当前节点department结束位置
                        string pStr = str.Substring(pos1 + 1);
                        string[] strInfo = pStr.Split(new string[] { "/>" }, StringSplitOptions.None);
                        List<string> strAllDecive = new List<string>();//保存当前节点的所有设备ID
                        List<string> strAllChannel = new List<string>();//保存当前节点的所有通道ID
                        foreach (string info in strInfo)
                        {
                            if (info.IndexOf("Device") > 0)
                            {
                                pos1 = info.IndexOf('"', 0);
                                pos2 = info.IndexOf('"', pos1 + 1);
                                string devId = info.Substring(pos1 + 1, pos2 - pos1 - 1);
                                if (!strAllDecive.Contains(devId))
                                {
                                    strAllDecive.Add(devId);
                                }
                            }
                            else if (info.IndexOf("Channel") > 0)
                            {
                                pos1 = info.IndexOf('"', 0);
                                pos2 = info.IndexOf('"', pos1 + 1);
                                string chnlId = info.Substring(pos1 + 1, pos2 - pos1 - 1);
                                pos1 = chnlId.IndexOf('$');
                                string ownDev = chnlId.Substring(0, pos1);
                                if (!strAllDecive.Contains(ownDev))//如果该通道不具有设备ID，解析出设备ID
                                {
                                    strAllDecive.Add(ownDev);
                                }
                                strAllChannel.Add(chnlId);
                            }
                        }
                        node.strDev = strAllDecive;
                        node.strChnl = strAllChannel;
                        OrgInfo.Add(node.code, node);
                    }
                    else if (i > 1)
                    {
                        OrgNode node = new OrgNode();
                        node.child = new List<string>();
                        //////////////////////////////////////////////////////////////////////////
                        int pos1 = str.IndexOf('"', 0);
                        int pos2 = str.IndexOf('"', pos1 + 1);
                        string coding = str.Substring(pos1 + 1, pos2 - pos1 - 1);
                        pos1 = str.IndexOf('"', pos2 + 1);
                        pos2 = str.IndexOf("modifytime", pos1 + 1);
                        string name = str.Substring(pos1 + 1, pos2 - pos1 - 1).Trim();
                        string szName = name.Substring(0, name.Length - 1);//去除"
                                                                           //byte[] btArr = Encoding.Default.GetBytes(strName);
                                                                           //byte[] tempArr = Encoding.Convert(Encoding.UTF8, Encoding.Default, btArr);
                                                                           //string szName = Encoding.Default.GetString(tempArr, 0, tempArr.Length);
                        node.code = coding;
                        //////////////////////////////////////////////////////////////////////////
                        List<string> codeList = new List<string>();
                        codeList.AddRange(OrgInfo.Keys);
                        foreach (string t in codeList)
                        {
                            if (coding.IndexOf(t) == 0)
                            {
                                node.parent = OrgInfo[t].code;
                                OrgNode pNode = new OrgNode();
                                pNode.parent = OrgInfo[t].parent;  //关联父节点
                                pNode.code = OrgInfo[t].code;
                                pNode.strDev = OrgInfo[t].strDev;
                                pNode.strChnl = OrgInfo[t].strChnl;
                                pNode.name = OrgInfo[t].name;
                                pNode.child = OrgInfo[t].child;     //关联子节点
                                pNode.child.Add(coding);
                                OrgInfo.Remove(t);
                                OrgInfo[t] = pNode;
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////
                        node.name = szName;
                        pos1 = str.IndexOf('>', 0);//找到当前节点department结束位置
                        string pStr = str.Substring(pos1 + 1);
                        string[] strInfo = pStr.Split(new string[] { "/>" }, StringSplitOptions.None);
                        List<string> strAllDecive = new List<string>();//保存当前节点的所有设备ID
                        List<string> strAllChannel = new List<string>();//保存当前节点的所有通道ID
                        foreach (string info in strInfo)
                        {
                            if (info.IndexOf("Device") > 0)
                            {
                                pos1 = info.IndexOf('"', 0);
                                pos2 = info.IndexOf('"', pos1 + 1);
                                string devId = info.Substring(pos1 + 1, pos2 - pos1 - 1);
                                if (!strAllDecive.Contains(devId))
                                {
                                    strAllDecive.Add(devId);
                                }
                            }
                            else if (info.IndexOf("Channel") > 0)
                            {
                                pos1 = info.IndexOf('"', 0);
                                pos2 = info.IndexOf('"', pos1 + 1);
                                string chnlId = info.Substring(pos1 + 1, pos2 - pos1 - 1);
                                pos1 = chnlId.IndexOf('$');
                                string ownDev = chnlId.Substring(0, pos1);
                                if (!strAllDecive.Contains(ownDev))//如果该通道不具有设备ID，解析出设备ID
                                {
                                    strAllDecive.Add(ownDev);
                                }
                                strAllChannel.Add(chnlId);
                            }
                        }
                        node.strDev = strAllDecive;
                        node.strChnl = strAllChannel;
                        OrgInfo.Add(node.code, node);
                    }
                    ++i;
                }


                strOrg = strXML.Split(new string[] { "<Devices>" }, StringSplitOptions.None)[1];//取组织树设备详情
                strDepartment = strOrg.Split(new string[] { "<Device" }, StringSplitOptions.None);//分割组织节点信息
                i = 0;
                //  List<OrgNode> OrgInfo = new List<OrgNode>();//保存节点信息列表
                //OrgInfo.Clear();
                String ss(String str, String name)
                {
                    try
                    {
                        String info = str.Split(new string[] { name }, StringSplitOptions.None)[1];
                        int pos1 = info.IndexOf('"', 0);
                        int pos2 = info.IndexOf('"', pos1 + 1);
                        return info.Substring(pos1 + 1, pos2 - pos1 - 1);
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }

                //设备下面有可能有多种单元类型，编码单元（type =”1”）,解码单元（type =”2”），
                //报警输入单元（type =”3”），报警输出单元（type =”4”），电视墙输入单元（type =”5”），电视
                //   墙输出单元（type =”6”），门禁单元（type =”7”），对讲单元（type =”8”），动环单元（type =”10”），
                //闸道单元（type =”14”），LED 单元（type =”15”），周界单元（type =”16”）。
                //Channelnum 代表单元下的通道数。
                //编码单元，下面全是编码通道，每个编码通道对于一个前端摄像头，如果此通道未接入
                //摄像头，则此通道是离线的。
                //一般客户做开发，组织树展示只需要展示编码通道，因此只需要解析编码单元
                String aa(String type)
                {
                    switch (type)
                    {
                        case "1":
                            return "编码单元-摄像机";
                        case "2":
                            return "解码单元";
                        case "3":
                            return "报警输入单元";
                        case "4":
                            return "报警输出单元";
                        case "5":
                            return "电视墙输入单元";
                        case "6":
                            return "电视墙输出单元";
                        case "7":
                            return "门禁单元";
                        case "8":
                            return "对讲单元";
                        case "10":
                            return "动环单元";
                        case "14":
                            return "闸道单元";
                        case "15":
                            return "LED 单元";
                        case "16":
                            return "周界单元";
                        default:
                            return "未知类型的大华设备-" + type;
                    }
                }
                foreach (string str in strDepartment)
                {
                    if (i != 0)//第一个节点
                    {
                        OrgNodeS node = new OrgNodeS();
                        String s = str.Split(new string[] { "<UnitNodes index" }, StringSplitOptions.None)[0];
                        var key = ss(s, "id");
                        node.parent = ss(s, "id");
                        node.parentIP = ss(s, "ip");
                        node.parentNmae = ss(s, "name");
                        node.parentPort = ss(s, "port");
                        node.parentUser = ss(s, "user");
                        node.parentPassword = ss(s, "password");
                        node.devicePort = ss(s, "devicePort");
                        node.c = new List<OrgNodeSC>();

                        int countIndex = 0;
                        foreach (var item in str.Split(new string[] { "<UnitNodes index" }, StringSplitOptions.None))
                        {//Channel
                            if (countIndex != 0)
                            {
                                int Index = 0;
                                foreach (var items in item.Split(new string[] { "<Channel" }, StringSplitOptions.None))
                                {//id="1000001$1$0$0"name="3.50 南门出入口1"desc=""status="0"channelType="1"channelSN="20001"rights="1000000000000000000000000101000000101000010110011111101111"cameraType="1"CtrlId="20001"latitude=""longitude=""viewDomain=""cameraFunctions="0"multicastIp=""multicastPort="0"NvrChnlIp=""channelRemoteType=""subMulticastIp=""subMulticastPort="0"/>
                                    if (Index != 0)
                                    {
                                        var a = items;
                                        OrgNodeSC nodes = new OrgNodeSC();
                                        nodes.channelType = ss(s, "type");
                                        nodes.code = ss(a, "id");
                                        nodes.name = ss(a, "name");
                                        nodes.status = ss(a, "status");
                                        nodes.child = ss(a, "CtrlId");
                                        node.c.Add(nodes);
                                    }
                                    Index++;
                                }
                            }
                            countIndex++;
                        }
                        //OrgInfoS.Add(node.code, node);
                        OrgInfoS.Add(key, node);
                    }
                    ++i;
                }

                System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
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
                                int ic = 2;
                                // Log.WriteLog("数据",EquipmentText.ToString());
                                //foreach (var items in EquipmentText)
                                {

                                    foreach (var item in OrgInfo.Keys)
                                    {

                                        var a = OrgInfo[item];
                                        JObject rbs = null;
                                        var ParentNode = a.code;
                                        foreach (var items in a.strChnl)
                                        {
                                            String name = items;
                                            String type = "未知的大华设备";

                                            foreach (var itemc in OrgInfoS.Keys)
                                            {
                                                foreach (var itemd in OrgInfoS[itemc].c)
                                                {
                                                    if (itemd.code == items)
                                                    {
                                                        rbs = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(itemd));
                                                        name = itemd.name;
                                                        type = aa(itemd.channelType);
                                                    }
                                                }
                                            }
                                            //type = "65542" NVR，type = "65537" DVR，type = "65538" 大华 IPC 枪机，type = "65541" MDVR，
                                            //type = "65548" Smart IPC ，type = "131073" NVD，type = "262145" DSCON1000，type = "65550"
                                            //Smart NVR，type = "65539" NVS，type = "65546" EVS，type = "65545" PVR。
                                            worksheet.Cells[ic, 1].Value = System.Guid.NewGuid().ToString("N");
                                            worksheet.Cells[ic, 2].Value = ParentNode;
                                            worksheet.Cells[ic, 3].Value = type;
                                            worksheet.Cells[ic, 4].Value = items;
                                            worksheet.Cells[ic, 5].Value = name;
                                            worksheet.Cells[ic, 6].Value = rbs == null ? "" : rbs.ToString();
                                            ic++;
                                        }

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("设备结构错误：" + ex.Message);
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
                                foreach (var item in OrgInfo.Keys)
                                {
                                    var a = OrgInfo[item];
                                }
                                int ic = 2;
                                foreach (var item in OrgInfo.Keys)
                                {
                                    //1   蓬莱戒毒所   0
                                    worksheetEegion.Cells[ic, 1].Value = item;
                                    worksheetEegion.Cells[ic, 2].Value = OrgInfo[item].name;
                                    worksheetEegion.Cells[ic, 3].Value = OrgInfo[item].parent;
                                    ic++;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("错误：" + ex.Message);
                            }
                            package.Save();
                        }
                    }
                }
                return "t";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        private string zhongliang(string strXML) {
            Log.WriteLog("数据",strXML);
            try
            {
                Parents.Clear();
                Childs.Clear();

                String aa(String type)
                {
                    switch (type)
                    {
                        case "1":
                            return "编码单元-摄像机";
                        case "2":
                            return "解码单元";
                        case "3":
                            return "报警输入单元";
                        case "4":
                            return "报警输出单元";
                        case "5":
                            return "电视墙输入单元";
                        case "6":
                            return "电视墙输出单元";
                        case "7":
                            return "门禁单元";
                        case "8":
                            return "对讲单元";
                        case "10":
                            return "动环单元";
                        case "14":
                            return "闸道单元";
                        case "15":
                            return "LED 单元";
                        case "16":
                            return "周界单元";
                        default:
                            return "未知类型的大华设备-" + type;
                    }
                }


                string strOrg = strXML.Split(new string[] { "<Devices>" }, StringSplitOptions.None)[1];//[0]取组织树信息字符串
                string[] strDepartment = strOrg.Split(new string[] { "<Device" }, StringSplitOptions.RemoveEmptyEntries);//Department分割组织节点信息
                List<string[]> list = new List<string[]>();
                foreach (string item in strDepartment)
                {
                    list.Add(item.Split(new string[] { "<Channel" }, StringSplitOptions.None));

                }
                System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹

                foreach (string[] item in list)
                {
                    string key = "";
                    string type = "";
                    for (int i = 0; i < item.Length; i++)
                    {
                        string temp = item[i].Replace(" ", "");   
                        if (i == 0) {

                            key = temp.Substring(4, temp.IndexOf("type") - 5);
                            type = temp.Substring(temp.IndexOf("type") + 6, temp.IndexOf("name") - temp.IndexOf("type") - 7);
                            Parent p = new Parent();
                            p.parentid = "0";
                            p.name = temp.Substring(temp.IndexOf("name") + 6, temp.IndexOf("manufacturer") - temp.IndexOf("name") - 7);
                            p.id = key;
                            Parents.Add(p);
                        }
                        else
                        {
                            Child c = new Child();
                           
                            c.name = temp.Substring(temp.IndexOf("name") + 6, temp.IndexOf("desc") - temp.IndexOf("name") - 7);
                            c.code = temp.Substring(4, temp.IndexOf("name") - 5);
                            c.parentid = key;
                            c.type = aa(type);
                            c.id= Guid.NewGuid().ToString("N");
                            Childs.Add(c);
                        }
                    }
                }

                System.Windows.Forms.FolderBrowserDialog openFileDialog2 = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    string paths = openFileDialog2.SelectedPath;
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
                                int ic = 2;
                                // Log.WriteLog("数据",EquipmentText.ToString());
                                //foreach (var items in EquipmentText)
                                {
                                    foreach (var item in Childs)
                                    {
                                        worksheet.Cells[ic, 1].Value = item.id;
                                        worksheet.Cells[ic, 2].Value = item.parentid;
                                        worksheet.Cells[ic, 3].Value = item.type;
                                        worksheet.Cells[ic, 4].Value = item.code;
                                        worksheet.Cells[ic, 5].Value = item.name;
                                        worksheet.Cells[ic, 6].Value = JsonConvert.SerializeObject(item);
                                        ic++;
                                    }
                               
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("设备结构错误：" + ex.Message);
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
                                int ic = 2;

                                foreach (var item in Parents)
                                {
                                    worksheetEegion.Cells[ic, 1].Value = item.id;
                                    worksheetEegion.Cells[ic, 2].Value = item.name;
                                    worksheetEegion.Cells[ic, 3].Value = item.parentid;
                                    ic++;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("错误：" + ex.Message);
                            }
                            package.Save();
                        }
                    }
                }

                return "t";
            }
            catch (Exception ex)
            {
                Log.WriteLog("报错",ex.Message);
                return ex.ToString();
            }

        }
    }

    public class Parent{
        public string id { get; set; }
        public string name { get; set; }
        public string parentid { get; set; }
    }
    public class Child { 
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public string parentid { get; set;
        }
    }
}
