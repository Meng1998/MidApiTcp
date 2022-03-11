using DeploymentTools.Logic;
using DeploymentTools.Mod;
using LinqToDB.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DeploymentTools.View
{
    /// <summary>
    /// TreeConfig.xaml 的交互逻辑
    /// </summary>
    public partial class TreeConfig : Window
    {
        public TreeConfig()
        {

            InitializeComponent();
        }
        TreeCareconfig treeCareconfig = new TreeCareconfig();
        JObject rb = null;
        
        bool lask = true;
        JObject jObject = null;
        //创建tree源
        TreeViewItem lss = new TreeViewItem();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string dir1 = System.IO.Path.GetFullPath("..");
          //  filepath = dir1 + "\\WebAPI\\config\\default.json";
            filepath = AppDomain.CurrentDomain.BaseDirectory + "default.json";

            //创建TreeView的数据源
            string json = Readjson("thirdParty", filepath);
            rb = JsonConvert.DeserializeObject<JObject>(json);

            treeView1.ItemsSource = null;
            treeView1.Items.Clear();
            //生成树
            BindTreeView2(rb, lss);
            treeView1.Items.Add(lss);

            string str = Directory.GetCurrentDirectory();
            string str1 = Process.GetCurrentProcess().MainModule.FileName;
             
            
            // treeView1.ItemsSource = rb.Children().Select(c => JsonHeaderLogic.FromJToken(c));

        }


        Boolean arryjudge = true;//集合验证
        /// <summary>
        /// 生成树形结构数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tree1"></param>
        public Boolean BindTreeView2(JObject mainarry, TreeViewItem tree1)
        {

            try
            {
                foreach (var item in mainarry)
                { //将递归遍历得到的文件夹路径与treeviewitem节点进行对应,并动态创建treeviewitem的Selected事件(选中事件),触发Selected事件,将该目录下得到的所有文件夹和文件路径添加到list1集合,若在文件夹之下遍历到子文件夹则创建子节点与子文件夹对应


                    string header = null;
                    int a = treeCareconfig.SubstringCount(item.ToString(), ",");
                    if (a > 1)
                    {
                        header = item.ToString().Substring(1).Split(',')[0];
                        arryjudge = true;
                    }
                    else
                    {
                        header = treeCareconfig.Ary(item.ToString())[0] + " : " + treeCareconfig.Ary(item.ToString())[1];
                        arryjudge = false;
                    }

                    TreeViewItem treeviewItem = new TreeViewItem();
                    treeviewItem.Header = header;


                    tree1.Items.Add(treeviewItem);

                    if (arryjudge)
                    {
                        //循环子集合
                        BindTreeView2((JObject)mainarry[$"{header}"], treeviewItem);
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("报错：" + ex.Message);
                return false;
            }

        }


        /// <summary>
        /// 读取json节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="jsonfile"></param>
        /// <returns></returns>
        public static string Readjson(string key, string jsonfile)
        {
            using (System.IO.StreamReader file = System.IO.File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    var value = o[key].ToString();
                    return value;
                }
            }
        }


        JObject rbjsonlis = null;
        List<String> listsd = new List<String>();
        int Txtindex;
        string filepath;
        List<String> list = new List<String>();
        /// <summary>
        /// 点击节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_Selected(object sender, RoutedEventArgs e)
        {

            Txtindex = 0;
            rbjsonlis = null;
            lask = true;
            parentnode = "";
            platid = 0;
            TreeViewItem treeViewItem = e.OriginalSource as TreeViewItem;

            string header = treeViewItem.Header.ToString();
          
            string file = File.ReadAllText(filepath);


            JObject rbjson = JsonConvert.DeserializeObject<JObject>(file);

            //循环搜索节点集合
            while (lask)
            {
                if (parentnode != "" && parentnode != null)
                {

                    rbjson = sty(rbjson, header);
                }

                else
                {
                    rbjson = sty(rbjson, header);
                }
            }

            // int indx = parentnode.Split('-').Count();

           // List<String> list = new List<String>();
            rbjsonlis = JsonConvert.DeserializeObject<JObject>(file);

            jObject = rbjsonlis; //main数据传递
            for (int i = 0; i < parentnode.Split('-').Count() - 1; i++)
            {
                string indsx = parentnode.Split('-')[i].ToString();
                rbjsonlis = (JObject)rbjsonlis[indsx];

                list.Add($"{indsx}");
            }

            Txtindex = rbjsonlis.Count;

            
            if (treeCareconfig.SubstringCount(rbjsonlis.ToString(), "{")==1)//判断“{”总数
            {
                List<string> listTxt = new List<string>();
                foreach (var item in rbjsonlis)
                {
                    listTxt.Add(item.ToString());
                   // Regex.Replace(item.ToString(), @"\[.*\]")
                }
                TxtLab(Txtindex, listTxt);
            }
            else
            {
                platform = null;

            }
           
          

        }


        /// <summary>
        /// 控件状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="obj"></param>
        public void TxtLab(int index, List<string> obj)
        {
            string a=null,b=null,c=null,d=null,e=null,f=null,g=null,h=null,i=null,j=null;
           
            switch (index)
            {
                case 1:
                    Txt1.Visibility = Visibility.Visible;
                    Lab1.Visibility = Visibility.Visible;
                    a = obj[0].ToString().Split(',')[0];
                    b = obj[0].ToString().Split(',')[1];
                    
                    Lab1.Content = a.Substring(1);  
                    Txt1.Text= b.Substring(0, b.Length - 1);

                    Txt2.Visibility = Visibility.Hidden;
                    Txt3.Visibility = Visibility.Hidden;
                    Txt4.Visibility = Visibility.Hidden;
                    Txt5.Visibility = Visibility.Hidden;

                    Lab2.Visibility = Visibility.Hidden;
                    Lab3.Visibility = Visibility.Hidden;
                    Lab4.Visibility = Visibility.Hidden;
                    Lab5.Visibility = Visibility.Hidden;
                    //   Lab1.Content = obj[0].ToString().Split(',')[0];
                    break;
                case 2:
                    Txt1.Visibility = Visibility.Visible;
                    Lab1.Visibility = Visibility.Visible;
                    Txt2.Visibility = Visibility.Visible;
                    Lab2.Visibility = Visibility.Visible;

                    a = obj[0].ToString().Split(',')[0];
                    b = obj[0].ToString().Split(',')[1];
                    c= obj[1].ToString().Split(',')[0];
                    d= obj[1].ToString().Split(',')[1];
                    Lab1.Content = a.Substring(1)+":";
                    Txt1.Text = b.Substring(0, b.Length - 1);
                    Lab2.Content = c.Substring(1)+":";
                    Txt2.Text = d.Substring(0, d.Length - 1);

                    Txt3.Visibility = Visibility.Hidden;
                    Txt4.Visibility = Visibility.Hidden;
                    Txt5.Visibility = Visibility.Hidden;
                    Lab3.Visibility = Visibility.Hidden;
                    Lab4.Visibility = Visibility.Hidden;
                    Lab5.Visibility = Visibility.Hidden;
                    //  Lab1.Content = (object)obj[0].ToString().Split(':')[0]; Lab2.Content =(object)obj[0].ToString().Split(':')[0];

                    break;
                case 3:
                    Txt1.Visibility = Visibility.Visible;
                    Txt2.Visibility = Visibility.Visible;
                    Txt3.Visibility = Visibility.Visible;
                    Lab1.Visibility = Visibility.Visible;
                    Lab2.Visibility = Visibility.Visible;
                    Lab3.Visibility = Visibility.Visible;

                    a = obj[0].ToString().Split(',')[0];
                    b = obj[0].ToString().Split(',')[1];
                    c = obj[1].ToString().Split(',')[0];
                    d = obj[1].ToString().Split(',')[1];
                    e = obj[2].ToString().Split(',')[0];
                    f = obj[2].ToString().Split(',')[1];

                    Lab1.Content = a.Substring(1) + ":";
                    Txt1.Text = b.Substring(0, b.Length - 1);
                    Lab2.Content = c.Substring(1) + ":";
                    Txt2.Text = d.Substring(0, d.Length - 1);
                    Lab3.Content = e.Substring(1) + ":";
                    Txt3.Text = f.Substring(0, f.Length - 1);

                    Txt4.Visibility = Visibility.Hidden;
                    Txt5.Visibility = Visibility.Hidden;
                    Lab4.Visibility = Visibility.Hidden;
                    Lab5.Visibility = Visibility.Hidden;
                    //  Lab1.Content = obj[0].Type; Lab2.Content = obj[1].Type; Lab3.Content = obj[2].Type;
                    break;
                case 4:
                    Txt1.Visibility = Visibility.Visible;
                    Txt2.Visibility = Visibility.Visible;
                    Txt3.Visibility = Visibility.Visible;
                    Txt4.Visibility = Visibility.Visible;
                    Lab1.Visibility = Visibility.Visible;
                    Lab2.Visibility = Visibility.Visible;
                    Lab3.Visibility = Visibility.Visible;
                    Lab4.Visibility = Visibility.Visible;

                    a = obj[0].ToString().Split(',')[0];
                    b = obj[0].ToString().Split(',')[1];
                    c = obj[1].ToString().Split(',')[0];
                    d = obj[1].ToString().Split(',')[1];
                    e = obj[2].ToString().Split(',')[0];
                    f = obj[2].ToString().Split(',')[1];
                    g = obj[3].ToString().Split(',')[0];
                    h = obj[3].ToString().Split(',')[1];

                    Lab1.Content = a.Substring(1) + ":";
                    Txt1.Text = b.Substring(0, b.Length - 1);
                    Lab2.Content = c.Substring(1) + ":";
                    Txt2.Text = d.Substring(0, d.Length - 1);
                    Lab3.Content = e.Substring(1) + ":";
                    Txt3.Text = f.Substring(0, f.Length - 1);
                    Lab4.Content = g.Substring(1) + ":";
                    Txt4.Text = h.Substring(0, h.Length - 1);

                    Txt5.Visibility = Visibility.Hidden;
                    Lab5.Visibility = Visibility.Hidden;
                    //   Lab1.Content = obj[0].Type; Lab2.Content = obj[1].Type; Lab3.Content = obj[2].Type; Lab4.Content = obj[3].Type;
                    break;
                case 5:
                    Txt1.Visibility = Visibility.Visible;
                    Txt2.Visibility = Visibility.Visible;
                    Txt3.Visibility = Visibility.Visible;
                    Txt4.Visibility = Visibility.Visible;
                    Txt5.Visibility = Visibility.Visible;
                    Lab1.Visibility = Visibility.Visible;
                    Lab2.Visibility = Visibility.Visible;
                    Lab3.Visibility = Visibility.Visible;
                    Lab4.Visibility = Visibility.Visible;
                    Lab5.Visibility = Visibility.Visible;

                    a = obj[0].ToString().Split(',')[0];
                    b = obj[0].ToString().Split(',')[1];
                    c = obj[1].ToString().Split(',')[0];
                    d = obj[1].ToString().Split(',')[1];
                    e = obj[2].ToString().Split(',')[0];
                    f = obj[2].ToString().Split(',')[1];
                    g = obj[3].ToString().Split(',')[0];
                    h = obj[3].ToString().Split(',')[1];
                    i = obj[4].ToString().Split(',')[0];
                    j = obj[4].ToString().Split(',')[1];

                    Lab1.Content = a.Substring(1) + ":";
                    Txt1.Text = b.Substring(0, b.Length - 1);
                    Lab2.Content = c.Substring(1) + ":";
                    Txt2.Text = d.Substring(0, d.Length - 1);
                    Lab3.Content = e.Substring(1) + ":";
                    Txt3.Text = f.Substring(0, f.Length - 1);
                    Lab4.Content = g.Substring(1) + ":";
                    Txt4.Text = h.Substring(0, h.Length - 1);
                    Lab5.Content = i.Substring(1) + ":";
                    Txt5.Text = j.Substring(0, j.Length - 1);


                    //  Lab1.Content = obj[0].Type; Lab2.Content = obj[1].Type; Lab3.Content = obj[2].Type; Lab4.Content = obj[3].Type; Lab5.Content = obj[4].Type;
                    break;

                default:
                    break;
            }
            Txt6.Text= platform.Substring(0, platform.Length - 1);
            platform = null;
        }

        /// <summary>
        /// 文本框数据导入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="lis"></param>
        /// <returns></returns>
        public List<string> TxtVal(int index, List<string> lis)
        {

            switch (index)
            {
                case 1:

                    lis.Add(Txt1.Text);
                    break;
                case 2:
                    lis.Add(Txt1.Text); lis.Add(Txt2.Text);
                    break;
                case 3:
                    lis.Add(Txt1.Text); lis.Add(Txt2.Text); lis.Add(Txt3.Text);
                    break;
                case 4:
                    lis.Add(Txt1.Text); lis.Add(Txt2.Text); lis.Add(Txt3.Text); lis.Add(Txt4.Text);
                    break;
                case 5:
                    lis.Add(Txt1.Text); lis.Add(Txt2.Text); lis.Add(Txt3.Text); lis.Add(Txt4.Text); lis.Add(Txt5.Text);
                    break;

                default:
                    break;
            }
            return lis;
        }

        
        string parentnode;
        string platform;int platid=0;
        /// <summary>
        /// 检索到节点
        /// </summary>
        /// <param name="rbjson"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public JObject sty(JObject rbjson, string header)
        {
            string ll = "";
            foreach (var item in rbjson)
            {
                string top = item.ToString();
                ll = top.Substring(1).Split(',')[0];

                if (ll == header)
                {
                    parentnode += ll + "-";
                    platform += ll + ".";
                    lask = false;
                    break;
                }
                if (top.Contains($"\"{header}\""))
                {
                    if (platid>0)
                    {
                        platform+= ll+".";
                    }
                    parentnode += ll + "-";
                    platid++;
                    break;
                  
                }
            }
            return (JObject)rbjson[ll];
        }



        
        

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //文本框数据
            listsd = TxtVal(Txtindex, listsd);

            JObject Addject = rbjsonlis;
            int j = 0;

            //节点数据导入
            foreach (var item in rbjsonlis)
            {
                Addject[item.Key] = listsd[j];
                j++;
            }

            string plat = Txt6.Text;
            jObject[list[0]]["config"] = plat;
            
            //节点json修改
            switch (list.Count - 1)
            {
                case 0:
                    jObject[list[0]] = Addject;
                    break;
                case 1:
                    jObject[list[0]][list[1]] = Addject;
                    break;
                case 2:
                    jObject[list[0]][list[1]][list[2]] = Addject;
                    break;
                case 3:
                    jObject[list[0]][list[1]][list[2]][list[3]] = Addject;
                    break;
                case 4:
                    jObject[list[0]][list[1]][list[2]][list[3]][list[4]] = Addject;
                    break;
                case 5:
                    jObject[list[0]][list[1]][list[2]][list[3]][list[4]][list[5]] = Addject;
                    break;
            }

            //写入文件
            File.WriteAllText(filepath, jObject.ToString());
           // MessageBox.Show("OK");
            treeView1.ItemsSource = null;
            treeView1.Items.Clear();
            
            TreeViewItem treelist = new TreeViewItem();
            string json = Readjson("thirdParty", filepath);
            rb = JsonConvert.DeserializeObject<JObject>(json);
            BindTreeView2(rb, treelist);
            treeView1.Items.Add(treelist);
            //Console.WriteLine(jObject.ToString());
        }
    }
}