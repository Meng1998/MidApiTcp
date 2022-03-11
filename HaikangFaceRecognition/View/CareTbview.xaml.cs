using Glimpse.AspNet.Tab;
using DeploymentTools.Logic;
using DeploymentTools.Mod;
using ISCwebApi.Controllers;
using LinqToDB.Common;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static DeploymentTools.Mod.ComBoxMod;
using static System.Resources.ResXFileRef;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using RestSharp.Extensions;
using LinqToDB.Extensions;
using Glimpse.Core.Extensions;
using System.Configuration;

namespace DeploymentTools.View
{
    /// <summary>
    /// CareTbview.xaml 的交互逻辑
    /// </summary>
    public partial class CareTbview : Window
    {
        public CareTbview()
        {


            InitializeComponent();
        }
        string filepath = "";
        string dataname = "";
        string sqlstr = "";
        string basename = "";
        //string txtfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database.txt");
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubscriptionSelection_Click(object sender, RoutedEventArgs e)
        {

            Task<bool> t = SetData();
            Task.Run(async () =>
            {
                bool msg = await t;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    if (msg)
                    {

                        this.Dispatcher.Invoke(new Action(() =>
                        {


                            SubscriptionSelection.Visibility = Visibility.Visible;
                            // SubscriptionSelection.Background = new SolidColorBrush(Color.FromArgb(153, 28, 235, 200));
                            // SubscriptionSelection.materialDesign = ButtonProgressAssist.GetIsIndicatorVisible(true);
                            SubscriptionSelection.Content = "已完成";
                        }));
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("操作错误 无法继续!");
                    }

                }));
            });

        }

        /// <summary>
        /// 创建数据库处理操作
        /// </summary>
        /// <returns></returns>
        public Boolean sqlTask()
        {
            String PC_Txtstr = "", Post_Txtstr = "", User_Txtstr = "", Pwd_Txtstr = "";
            this.Dispatcher.Invoke(new Action(() =>
            {
                dataname = base_Txt.Text;
                PC_Txtstr = PC_Txt.Text; Post_Txtstr = Post_Txt.Text; User_Txtstr = User_Txt.Text; Pwd_Txtstr = Pwd_Txt.Text;
            }));
            if (string.IsNullOrEmpty(PC_Txtstr)) { return false; }
            sqlstr = $"Server={PC_Txtstr};Port={Post_Txtstr};UserId={User_Txtstr};Password={Pwd_Txtstr};";
            try
            {
                //创建数据库
                NpgsqlConnection conn = new NpgsqlConnection(sqlstr);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                string sql = $"CREATE DATABASE {dataname}";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    SubscriptionSelection.Visibility = Visibility.Hidden;
                    // SubscriptionSelection.Content = "请稍后。。";
                }));
                cmd.ExecuteNonQuery();



                conn.Close();
                //导入图表
                List<string> pathmen = new List<string>();
                string path = AppDomain.CurrentDomain.BaseDirectory + "Tview";
                Director(path, pathmen);

                sqlstr = sqlstr + $"Database={dataname.Replace("\"", "")};";
                NpgsqlConnection myconn = new NpgsqlConnection(sqlstr);
                if (myconn.State != ConnectionState.Open)
                {
                    myconn.Open();
                }
                string sqly = "CREATE EXTENSION postgis;";
                NpgsqlCommand cmdy = new NpgsqlCommand(sqly, myconn);
                cmdy.ExecuteNonQuery();


                //NpgsqlConnection TBLconn = new NpgsqlConnection(sqlstr);
                //if (TBLconn.State != ConnectionState.Open)
                //{
                //    TBLconn.Open();
                //}
                string sqlcare = null;
                foreach (var item in pathmen)
                {
                    sqlcare += File.ReadAllText($"{path}\\{item}");
                }

                byte[] mybyte = Encoding.UTF8.GetBytes(sqlcare);
                sqlcare = Encoding.UTF8.GetString(mybyte);



                NpgsqlCommand cmdly = new NpgsqlCommand(sqlcare, myconn);



                if (cmdly.ExecuteNonQuery() > 0)
                {
                    myconn.Close();
                }
                BKapf(dataname, PC_Txtstr, Post_Txtstr, User_Txtstr, Pwd_Txtstr);

                //  File.WriteAllText(txtfile, dataname);

                return true;
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }));

                return false;
            }



        }

        private async Task<bool> SetData()
        {
            var list = await Task.Run(() => sqlTask());
            return list;
        }


        /// <summary>
        /// 遍历文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="list"></param>
        public void Director(string dir, List<string> list)
        {
            DirectoryInfo theFolder = new DirectoryInfo(dir);
            FileInfo[] dirInfo = theFolder.GetFiles();
            //遍历文件夹
            foreach (FileInfo NextFolder in dirInfo)
            {

                list.Add(NextFolder.Name);
            }

        }


        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="database"></param>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="uid"></param>
        /// <param name="pwd"></param>
        public void BKapf(string database, string server, string port, string uid, string pwd)
        {
            AppConfig.UpdateAppSettings("Database", database);
            AppConfig.UpdateAppSettings("Server", server);
            AppConfig.UpdateAppSettings("Port", port);
            AppConfig.UpdateAppSettings("Uid", uid);
            AppConfig.UpdateAppSettings("Pwd", pwd);

            AppConfig.XmlSaveAppsetting("Database", database);
            AppConfig.XmlSaveAppsetting("Server", server);
            AppConfig.XmlSaveAppsetting("Port", port);
            AppConfig.XmlSaveAppsetting("Uid", uid);
            AppConfig.XmlSaveAppsetting("Pwd", pwd);
        }


        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubscriptionSelectionSql_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Sql.Existstable("map_build") && Sql.Existstable("map_floor") && Sql.Existstable("map_model"))
                {
                    this.SubscriptionSelectionSql.Content = AppConfig.GetAppSetting("Database") + ",表齐全";
                    // this.SubscriptionSelectionSql.ForeColor = Color.Black;
                }
                else
                {
                    this.SubscriptionSelectionSql.Content = AppConfig.GetAppSetting("Database") + ",表缺失！";
                    //  this.datesql.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }



        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        TreeViewItem ss = new TreeViewItem();



        /// <summary>
        /// 生成树形图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubscriptionSelectionFild_Click(object sender, RoutedEventArgs e)
        {

            treeView.ItemsSource = null;
            treeView.Items.Clear();

            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();  //选择文件夹
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                filepath = openFileDialog.SelectedPath;


            }
            try
            {
                DirectoryInfo di = new DirectoryInfo(filepath);
                basename = di.Name;


                BindTreeView2(filepath, ss);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }





            //treeView.Items.Add(ss);

            treeView.ItemsSource = ss.Items;

        }



        List<string> list1 = new List<string>();
        List<string> list2 = new List<string>();



        /// <summary>
        /// 生成树形结构数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tree1"></param>
        public Boolean BindTreeView2(string path, TreeViewItem tree1)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] dirs = di.GetDirectories();
            foreach (var item in dirs)
            {
                list2.Add(item.ToString());
            }
            //string abc = dirs[1].ToString();
            //  System.Windows.Controls.TreeView root = new System.Windows.Controls.TreeView();

            //  Regex rex = new Regex(@"^\d+$");
            try
            {
                if (dirs.Length > 10)
                {
                    Array.Sort(dirs, (x1, x2) => int.Parse(Regex.Match(x1.Name, @"\d+").Value).CompareTo(int.Parse(Regex.Match(x2.Name, @"\d+").Value)));
                }
                foreach (DirectoryInfo i in dirs)
                { //将递归遍历得到的文件夹路径与treeviewitem节点进行对应,并动态创建treeviewitem的Selected事件(选中事件),触发Selected事件,将该目录下得到的所有文件夹和文件路径添加到list1集合,若在文件夹之下遍历到子文件夹则创建子节点与子文件夹对应

                    TreeViewItem ziDt = new TreeViewItem();
                    ziDt.Header = i.Name;
                    tree1.Items.Add(ziDt);

                    ziDt.Selected += new RoutedEventHandler(delegate (object shabi, RoutedEventArgs r)
                    {  //选中节点，通过 MessageBox.Show打印 节点对应文件夹下的所有文件夹和文件路径
                        list1.Clear(); //清空之前选中节点所取得的所有路径
                        string c = null;
                        string[] directory1 = Directory.GetDirectories(i.FullName);
                        foreach (string a in directory1)  //将目录下的文件夹路径加到list1
                        {
                            list1.Add(a);
                        }
                        //FileInfo[] fileInfos = i.GetFiles("tileset.json");
                        //if (fileInfos.Length>0)
                        //{
                        string[] file1 = Directory.GetFiles(i.FullName);
                        foreach (string a in file1)      //将目录下的文件路径加到list1
                        {
                            list1.Add(a);
                        }
                        //  }
                        foreach (string a in list1)
                        {
                            c = c + "\r\n" + a;
                        }


                    });
                    BindTreeView2(i.FullName, ziDt);

                }
                //this.Dispatcher.Invoke(new Action(() =>
                //{
                //    Probar.Visibility = Visibility.Hidden;
                //}));
                return true;
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show("报错：" + ex.Message);
                return false;
            }


        }



        /// <summary>
        /// 数据入库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubscriptionSelectionSqlCome_Click(object sender, RoutedEventArgs e)
        {

            int Node = list2.Count;
            if (Node != 0)
            {
                for (int i = 0; i < 5; i++)
                {

                    string dirname = list2[i].ToString();
                    if (dirname == "normal")
                    {
                        Grouping("normal");
                    }
                    if (dirname == "virtual")
                    {
                        Grouping("virtual");
                    }
                }

                try
                {

                    Config.UpdateConnectionStringsConfig("DataPath", filepath);
                    Sqldata();
                    string database = Config.GetConfigValue("Database");
                    string path = filepath;
                    Directory.SetCurrentDirectory(Directory.GetParent(path).FullName);
                    string parentPath = Directory.GetCurrentDirectory();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }


        }

        /// <summary>
        /// 那个分组获取数据
        /// </summary>
        /// <param name="grouping">normal还是virtual</param>
        public void Grouping(string grouping)
        {
            string path = filepath + "\\" + grouping;
            string[] a = Directory.GetDirectories(path);//返回指定目录所有子目录名称，包括路径
            if (a.Count() > 0)
            {
                path = path + "\\3dtiles";
                if (a[0] == path)
                {
                    string[] b = Directory.GetDirectories(path);
                    try
                    {


                        foreach (var item in b)
                        {
                            string fid;
                            string[] fileInfo = Directory.GetFiles(item, "*.json");

                            string name = System.IO.Path.GetFileName(item);//jz
                            if (fileInfo.Length > 0)
                            {
                                //3dtiles下得模型
                                Datas("0", name, "3dtiles", grouping, name, true);
                            }
                            else
                            {
                                //不是模型,是3dtiles下文件JZ
                                fid = Datas("0", name, "group", grouping, "", null);//获取jz得id并录入
                                string[] c = Directory.GetDirectories(item);
                                if (c.Count() > 10)
                                {
                                    c = Orderlist(c);
                                }
                                foreach (var itemc in c)
                                {
                                    //DirectoryInfo TheFolder = new DirectoryInfo(dir);

                                    //DirectoryInfo[] files = TheFolder.GetDirectories();
                                    fileInfo = Directory.GetFiles(itemc, "*.json");

                                    string namec = System.IO.Path.GetFileName(itemc);//1
                                    if (fileInfo.Length > 0)
                                    {
                                        //是jz下模型1
                                        Datas(fid, namec, "3dtiles", grouping, name + "/" + namec, true);
                                    }
                                    else
                                    {
                                        //不是模型,是JZ下文件1
                                        fid = Datas(fid, namec, "group", grouping, "", null);//获取1得id并录入
                                        string[] d = Directory.GetDirectories(itemc);
                                        int js = 0;
                                        if (d.Count() > 10)
                                        {
                                            d = Orderlist(d);
                                        }
                                        foreach (var itemd in d)
                                        {
                                            fileInfo = Directory.GetFiles(itemd, "*.json");
                                            string named = System.IO.Path.GetFileName(itemd);//1
                                            if (fileInfo.Length > 0)
                                            {
                                                //是1下模型1F
                                                if (js == d.Length - 1)
                                                {
                                                    Datas(fid, named, "3dtiles", grouping, name + "/" + namec + "/" + named, true);
                                                }
                                                else
                                                {
                                                    Datas(fid, named, "3dtiles", grouping, name + "/" + namec + "/" + named, false);
                                                }
                                            }
                                            else
                                            {
                                                //不是模型，是1下文件1F
                                            }
                                            js++;
                                        }
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }

                }
            }


        }


        int ids = 0;



        /// <summary>
        /// 模型信息存入数组
        /// </summary>
        /// <param name="pid">父id</param>
        /// <param name="node_name">文件名</param>
        /// <param name="node_type">文件是否为模型是为3dtiles否为group</param>
        /// <param name="data_type">文件所属normal还是virtual</param>
        /// <param name="data_url">文件路径$serverURL$</param>
        /// <param name="visible">模型是否显示t/f</param>
        public string Datas(string pid, string node_name, string node_type, string data_type, string data_url, bool? visible)
        {
            //string id= Guid.NewGuid().ToString("N");
            ids++;
            if (node_name == "DXS")
            {
                ids++;
            }
            else if (node_name == "JZ" || node_name == "SN")
            {
                ids--;

            }
            string id = Createid(data_type, node_type, node_name, ids.ToString());

            try
            {

                string url;
                if (data_url == "")
                {
                    url = "";
                }
                else
                {
                    url = "$serverURL$/3dtiles/" + data_url + "/tileset.json";
                }
                //建筑入库
                if (pid != "0" && url == "")
                {
                    map_build map_Build = new map_build()
                    {
                        id = id,
                        group_id = id,
                        build_name = node_name
                    };
                    map_Builds.Add(map_Build);
                }
                //楼层入库
                string[] getAry = data_url.Split(new char[] { '/' });
                if (getAry.Length > 1 && getAry[0] == "JZ")
                {
                    map_floor map_Floor = new map_floor()
                    {
                        id = id,
                        group_id = id,
                        floor_name = node_name,
                        build_id = pid,
                        model_url = url
                    };
                    map_Floors.Add(map_Floor);
                }

                ////地下室入库

                if (getAry.Length > 1 && getAry[0] == "DXS")
                {
                    map_under map_Under = new map_under()
                    {
                        id = id,
                        group_id = id,
                        floor_name = node_name,
                        model_url = url
                    };
                    map_under.Add(map_Under);
                }

                //模型入库
                map_model datainfo = new map_model()
                {
                    id = id,
                    pid = pid,
                    node_name = node_name,
                    node_type = node_type,
                    data_type = data_type,
                    data_url = url,
                    visible = visible
                };
                datainfos.Add(datainfo);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return id;
        }



        /// <summary>
        /// 生成id
        /// </summary>
        /// <param name="dtyoe"></param>
        /// <param name="ntype"></param>
        /// <param name="name"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string Createid(string dtyoe, string ntype, string name, string pid)
        {
            //n/v(datatype)+y/n(nodetype)+name
            string id = null;
            string id1 = null;
            string id2 = null;
            if (dtyoe == "normal")
            {
                id1 = "n";
            }
            else
            {
                id1 = "v";
            }
            if (ntype == "3dtiles")
            {
                id2 = "y";
            }
            else
            {
                id2 = "n";
            }


            if (name.Replace("F", "").Length == 1)
            {
                name = "F0" + name.Replace("F", "");
            }
            id = id1 + id2 + name + pid;
            return id;
        }



        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        public string[] Orderlist(string[] lists)
        {
            try
            {


                List<String> list = new List<string>(lists);
                string s = list[0].ToString();
                List<Sort> sort = new List<Sort>();
                string path = s.Substring(0, s.LastIndexOf("\\"));
                foreach (var item in list)
                {

                    Sort sort1 = new Sort();
                    sort1.name = item.Split('\\').Last();
                    sort.Add(sort1);
                }

                // Array.Sort(list, (x1, x2) => int.Parse(Regex.Match(x1.Name, @"\d+").Value).CompareTo(int.Parse(Regex.Match(x2.Name, @"\d+").Value)));
                sort.Sort((Sort p1, Sort p2) => int.Parse(Regex.Match(p1.name, @"\d+").Value).CompareTo(int.Parse(Regex.Match(p2.name, @"\d+").Value)));
                string[] syu = new string[sort.Count()];
                for (int i = 0; i < sort.Count(); i++)
                {
                    syu[i] = path + "\\" + sort[i].name.ToString();
                }

                return lists = syu.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }







        /// <summary>
        /// 获取本地IP
        /// </summary>
        string GetAddressIP()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        List<map_model> datainfos = new List<map_model>();
        List<map_build> map_Builds = new List<map_build>();
        List<map_floor> map_Floors = new List<map_floor>();
        List<map_under> map_under = new List<map_under>();


        /// <summary>
        /// 数据处理
        /// </summary>
        public void Sqldata()
        {
            //数据处理
            using (SqlSugarClient client = new SqlSugarClient(Sql.ConnectSql()))
            {
                try
                {
                    client.Open();

                    //清空表
                    client.Deleteable<map_floor>().ExecuteCommand();
                    client.Deleteable<map_build>().ExecuteCommand();
                    client.Deleteable<map_model>().ExecuteCommand();
                    client.Deleteable<map_under>().ExecuteCommand();

                    //楼层入库
                    List<int> blist = new List<int>();
                    List<int> flist = new List<int>();

                    string data_uri = $"http://{GetAddressIP()}/{basename}";
                    sys_config stu1 = client.Queryable<sys_config>().Where(q => q.id == "1").First();
                    stu1.data_server_uri = data_uri;
                    client.Updateable(stu1).ExecuteCommand();



                    foreach (var item in map_Builds)
                    {
                        //区分楼层
                        List<map_floor> bf = new List<map_floor>();
                        int zunum = map_Floors.Count();
                        foreach (var fitem in map_Floors)
                        {
                            if (item.id == fitem.build_id)
                            {
                                bf.Add(fitem);
                            }
                        }
                        //区分BF
                        int bnum = 0;//3
                        int fnum = 0;//6
                        List<map_floor> bfb = new List<map_floor>();
                        List<map_floor> bff = new List<map_floor>();
                        //获取BF多少
                        foreach (var bfitem in bf)
                        {
                            int num = bfitem.floor_name.IndexOf("B");
                            if (num >= 0)
                            {
                                bnum++;
                                bfb.Add(bfitem);
                            }
                            else
                            {
                                fnum++;
                                bff.Add(bfitem);
                            }
                        }
                        bfb.OrderByDescending(a => a.floor_name).ToList();//降序
                        bff.OrderBy(a => a.floor_name).ToList();//升序



                        int i = 1;
                        foreach (var itembfb in bfb)
                        {
                            var insertObj = new map_floor() { id = itembfb.id, group_id = itembfb.group_id, order_num = i++, floor_name = itembfb.floor_name, build_id = itembfb.build_id, model_url = itembfb.model_url };
                            int a = client.Insertable(insertObj).ExecuteCommand();
                        }
                        i += bnum - 1;

                        foreach (var itembff in bff)
                        {
                            try
                            {

                                var insertObj = new map_floor() { id = itembff.id, group_id = itembff.group_id, order_num = i++, floor_name = itembff.floor_name, build_id = itembff.build_id, model_url = itembff.model_url };
                                int a = client.Insertable(insertObj).ExecuteCommand();

                            }
                            catch (Exception)
                            {

                                MessageBox.Show(itembff.id + "---" + itembff.group_id + "---" + i++ + "---" + itembff.floor_name + "---" + itembff.build_id + "---" + itembff.model_url);
                            }

                        }

                    }
                    int k = 1;
                    foreach (var item in map_under)
                    {
                        var insertObj = new map_under() { id = item.id, group_id = item.group_id, floor_name = item.floor_name, order_num = k++, model_url = item.model_url };
                        int a = client.Insertable(insertObj).ExecuteCommand();
                    }



                    //建筑入库
                    foreach (var item in map_Builds)
                    {
                        var insertObj = new map_build() { id = item.id, group_id = item.group_id, build_name = item.build_name };
                        int a = client.Insertable(insertObj).ExecuteCommand();
                    }

                    //模型入库
                    foreach (var item in datainfos)
                    {
                        if (item.visible == null)
                        {
                            var insertObj = new map_model() { id = item.id, pid = item.pid, node_name = item.node_name, node_type = item.node_type, data_type = item.data_type, data_url = item.data_url };
                            int a = client.Insertable(insertObj).ExecuteCommand();
                        }
                        else
                        {
                            var insertObj = new map_model() { id = item.id, pid = item.pid, node_name = item.node_name, node_type = item.node_type, data_type = item.data_type, data_url = item.data_url, visible = item.visible };
                            int b = client.Insertable(insertObj).ExecuteCommand();
                        }
                    }


                    client.Close();
                    // SubscriptionSelectionSqlCome.Background = new SolidColorBrush(Color.FromArgb(153, 28, 235, 200));
                    SubscriptionSelectionSqlCome.Content = "已完成";

                    // this.Close();
                }

                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("入库失败。错误信息：" + ex.Message);
                }
            }
        }



        /// <summary>
        /// 生成lic文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PackIcon_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        public string BDip()
        {
            ObservableCollection<ItemContentIP> IPlist = new ObservableCollection<ItemContentIP>();
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {

                if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPlist.Add(new ItemContentIP() { Name = _IPAddress.ToString(), IP = _IPAddress });
                }
            }

            return AddressIP;
        }

        /// <summary>
        /// 生产lic文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string file = ""; string ip = "127.0.0.1:" + ConfigurationManager.ConnectionStrings["PortTxt"].ToString();

            string database = Config.GetConfigValue("Database");
            // StreamReader sr = new StreamReader(txtfile, Encoding.UTF8);

            file = EstavlishIIS.SelectPath();
            if (string.IsNullOrEmpty(file)) return;

            var request = (HttpWebRequest)WebRequest.Create($"http://{ip}/api/{database}/sys/authorize/licence");
            string json = null;
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                json = json.Replace(" ", "");
                json = json.Replace("{\"code\":0,\"msg\":\"success\",\"data\":{\"licence\":\"", "").Replace("\"}}", "");

                if (string.IsNullOrEmpty(json)) return;
                string path = file + $"\\{database}.lic";
                DelectDir(path);
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(json);
                sw.Flush();
                sw.Close();
                fs.Close();

                MessageBoxResult dr = MessageBox.Show("授权完成! 是否打开地图平台", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.OK)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "授权失败！ 请核查地图服务是否开启");
                return;
            }

            //txt.Text = json;//{"code":0,"msg":"success","data":{"licence":"71F91741583631A558CE4458D68EB59260CBC5674794F873BD7FBF04D5F721137DC7C1F596CC38D3CDFBB256EE349C28577534F5FEDFE14C753F96C9CEF9E7F8F685DD3444BD6292467EE4206E47F4CE"}}


        }

        public void DelectDir(string srcPath)
        {
            if (File.Exists(srcPath))
            {
                File.Delete(srcPath);
            }
        }






        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ObservableCollection<ItemContentIP> IPlist = new ObservableCollection<ItemContentIP>();

            //foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            //{
            //    if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        IPlist.Add(new ItemContentIP() { Name = _IPAddress.ToString(), IP = _IPAddress });
            //    }
            //}

            //com_box.ItemsSource = IPlist;
            //this.com_box.DisplayMemberPath = "Name";
            //com_box.SelectedIndex = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {


                OpenFileDialog dialog = new OpenFileDialog();
                // dialog.Multiselect = true;//该值确定是否可以选择多个文件
                string filelog = ""; string destFile = "";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filelog = dialog.FileName;
                }
                // var sourceFilePath = "c:/dir/1.jpg";
                var file = new FileInfo(filelog);

                FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) { destFile = openFileDialog.SelectedPath; }

                var destFileName = destFile + "\\" + file.Name;
                DelectDir(destFileName);
                File.Copy(filelog, destFileName);
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
