using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace LntegratedMiddleware.CQLJ
{
    class xmlInfo
    {
        public xmlInfo()
        {
            Test1();

           // GetXmlData();
           /// Test3();
        }


        public void Test1()
        {
            String aa = DateTime.Now.ToString();
            JObject sTemplate = JObject.Parse(GetData("200.json"));
            String sql = "", devicename = "摄像机", devicetype = "10001";
            Int32 index = 625;
            foreach (var item in sTemplate["data"])
            {
               

                //String str = item["list_style"].ToString();
                ///JObject dd = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(item["list_style"].ToString()));
                //JObject da = JObject.Parse();
                string id = Guid.NewGuid().ToString("N").ToUpper();
                //if (item["name"].ToString().Contains("主机"))
                //{
                //    sql += $"INSERT INTO \"public\".\"device_info\" VALUES ('{Guid.NewGuid().ToString("N")}', '671E08CE172240248CA13BF122D0617E', '对讲', '', '', '6a0a2ab8-5bac-4005-8e7c-53a2fbc4c69d_1A95B980729DEA45D1DA5636FEFCF587', '对讲主机', '{item["ID"]}', '{item["name"]}', 'f', NULL, NULL, NULL, NULL, NULL, NULL, 'f', {index}, 'f');";
                //}
                //else
                //{
                //    sql += $"INSERT INTO \"public\".\"device_info\" VALUES ('{Guid.NewGuid().ToString("N")}', '671E08CE172240248CA13BF122D0617E', '对讲', '', '', '6a0a2ab8-5bac-4005-8e7c-53a2fbc4c69d_1A95B980729DEA45D1DA5636FEFCF587', '对讲分机', '{item["ID"]}', '{item["name"]}', 'f', NULL, NULL, NULL, NULL, NULL, NULL, 'f', {index}, 'f');";
                //}
                //sql+=$"INSERT INTO \"public\".\"grave_area\" VALUES ('{item["id"]}', '{item["row"]}', '{item["colun"]}', '{item["name"]}', '{item["orgin"]}', '{item["remark"]}', '{item["isfame"]}', '{item["area_name"]}', '{item["slope"]}', {index});";

                sql += $"INSERT INTO \"public\".\"device_info\" VALUES ('{Guid.NewGuid().ToString("N")}', 'F14A1E66B2624375810244CD7541BC6F', '报警', '', '', '6a0a2ab8-5bac-4005-8e7c-53a2fbc4c69d_1A95B980729DEA45D1DA5636FEFCF587', '报警', '{item["pointinfoid"]}', '{item["remark"]}', 'f', NULL, NULL, NULL, NULL, NULL, NULL, 'f', {index}, 'f');";
                //sql += "INSERT INTO \"public\".\"device_info\" VALUES ('" + Guid.NewGuid().ToString("N") + "', '10009', '" + item["deviceIp"] + "', NULL, '" + item["deviceName"] + "', '" + devicetype + "', '" + item["channelId"] + "', '" + item["channelName"] + "', NULL, 0, '{}');";
                //sql += $"update grave_camera set device_info='{item["list_style"].ToString()}' where device_code='{item["device_code"]}';";
                //sql += $"INSERT INTO \"public\".\"grave_area\" VALUES ('{item["id"]}', '{item["row"]}', '{item["columns"]}', '{item["name"]}', '{item["origin"]}', '{item["remark"]}', '{item["isfame"]}', '{item["area_name"]}', '{item["slope"]}', '{index}');";
                index++;
            }
            Console.WriteLine("");

            ////写文件
            //FileStream file = new FileStream(System.IO.Directory.GetCurrentDirectory() + "\\200.txt", FileMode.OpenOrCreate);

            //String text = @"更加务实的Surface
            //              负责Surface业务的副总裁Panos Panay表示，他们的目标不是重新发明轮子，而是要大力改进它。从这句话也可以看得出，
            //                这一代Surface不会给人们带来任何惊艳，取而代之的是务实的修改风，比如克服续航短板，带来更高的移动工作能力等。";
            //byte[] array = System.Text.Encoding.UTF8.GetBytes(text);
            //file.Write(array, 0, array.Length);
            //file.Close();

            ////读文件
            //array = new byte[1024];
            //file = new FileStream("C:\\test1.txt", FileMode.Open);
            //while (file.Read(array, 0, 1024) > 0)
            //{
            //    Console.Write(System.Text.Encoding.UTF8.GetString(array));
            //}
            //file.Close();
            //Console.WriteLine("Test1完成操作");
        }

        public void Test2()
        {
            FileStream stream = new FileStream("C:\\test2.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            for (int i = 0; i < 10; i++)
            {
                writer.WriteLine("不是重新发明轮子___" + i.ToString());
            }
            writer.Flush();
            writer.Close();
            stream.Close();


            StreamReader reader = new StreamReader("C:\\test2.txt");
            String str = String.Empty;
            while ((str = reader.ReadLine()) != null)
            {
                Console.WriteLine(str);
            }

            reader.Close();

            Console.WriteLine("Test2完成操作");

        }

        public void Test3()
        {
            //写文件
            FileStream file = new FileStream(System.IO.Directory.GetCurrentDirectory() + $"\\{DateTime.Now.Millisecond}Device.txt", FileMode.OpenOrCreate);
            BufferedStream writer = new BufferedStream(file, 1024);

            String text = @"要承认的是，设备的融合必将是未来的一种趋势，如今众多形态的计算设备，
                    如笔记本电脑、超级本和平板电脑未来很有可能融合成一个设备，这些设备的融合先决条件就是出现一个设备能将这些设备的形态和功能这两者囊括在一个中，
                    值得高兴的是，Surface就是拥有这种潜力的设备";
            byte[] array = System.Text.Encoding.UTF8.GetBytes(text);
            writer.Write(array, 0, array.Length);
            writer.Flush();
            writer.Close();
            file.Close();

            //读文件
            file = new FileStream("C:\\test3.txt", FileMode.Open);
            BufferedStream reader = new BufferedStream(file, 1024);

            array = new byte[1024];
            while (reader.Read(array, 0, 1024) > 0)
            {
                Console.Write(System.Text.Encoding.UTF8.GetString(array));
            }
            reader.Close();
            file.Close();

            Console.WriteLine("Test1完成操作");

        }





        public static String GetXmlData()
        {
            string sTemplate = GetData("Device656.xml");

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(sTemplate);

            XmlElement root = xdoc.DocumentElement;
            //按节点名称查找出所有同名节点
            XmlNodeList nodeList = root.GetElementsByTagName("Channel");
            List<Infoxml> infoxmls = new List<Infoxml>();
            foreach (XmlNode item in nodeList)
            {
                infoxmls.Add(new Infoxml()
                {
                    DeviceCode = item.Attributes["id"].Value,
                    DeviceName = item.Attributes["name"].Value
                }); ;
            }
            return JsonConvert.SerializeObject(new
            {
                success= true,
                data = infoxmls
            });
            //String sql = "";
            //foreach (XmlNode node in nodeList)
            //{
            //    //获取节点属性值
            //    //String aa = node.Attributes["id"].Value;
            //    //String bb = node.Attributes["name"].Value;

            //    //guid大写.ToUpper()
            //    //sql += $"INSERT INTO \"public\".\"device_info\" VALUES ('{Guid.NewGuid().ToString("")}', '19242F0F746DDE464239EF7ECE11E63F', '摄像头', 'B88458DECFE22140B0F880EA2C49BB27', '枪机', '64fdeef7-c4fd-48bc-b1c2-d2bca2f694c4', '摄像头', '{node.Attributes["id"].Value}', '{node.Attributes["name"].Value}', 'f', NULL, NULL, NULL, NULL, NULL, NULL, 'f', NULL, 'f');";
            //    sql += "INSERT INTO \"public\".\"device_info\"  VALUES ('"+Guid.NewGuid().ToString("")+"', '10001', '摄像机', NULL, '摄像机', '268da78fa66e49a69743cc67ca2313ed', '"+ node.Attributes["id"].Value + "', '"+ node.Attributes["name"].Value + "', NULL, 0, '{}');";


            //    //string tmp = node["…"].InnerText;
            //}
        }

        public static string GetData(string FileName)
        {
            String FliePath = System.IO.Directory.GetCurrentDirectory() + "\\" + FileName ;
            string json = "";
            if (File.Exists(FliePath))
            {
                StreamReader MyReader = null;
                try
                {
                    MyReader = new StreamReader(FliePath, System.Text.Encoding.GetEncoding("utf-8"));
                    json = MyReader.ReadToEnd();
                    if (MyReader != null)
                    {
                        MyReader.Close();
                    }
                }
                catch (Exception)
                {

                }
            }
            return json;
        }

        public class Infoxml
        {
            public String DeviceCode { get; set; }
            public String DeviceName { get; set; }
        }
    }
}
