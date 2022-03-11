using LntegratedMiddleware.NBE.M;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;


namespace LntegratedMiddleware.NBE.C
{
    public class XmlJson
    {
        public static string InsertFormat(string input, int interval, string value)
        {
            for (int i = interval; i < input.Length; i += interval + 1)
                input = input.Insert(i, value);
            return input;
        }
        public static string MidStrEx_New(string sourse, string startstr, string endstr)
        {
            Regex rg = new Regex("(?<=(" + startstr + "))[.\\s\\S]*?(?=(" + endstr + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(sourse).Value;
        }

        public static string GetxmlAppjson(string xmlstr, int type)
        {
            string path = "<?xml version=\"1.0\" encoding=\"gb2312\"?>";
            string[] resAry = xmlstr.Split(new string[] { "<?xml version=\"1.0\" encoding=\"gb2312\"?>" }, StringSplitOptions.RemoveEmptyEntries);
            ArrayList arrayList = new ArrayList();
             
            switch (type)
            {
                case 0:
                    foreach (string item in resAry)
                    {

                        if (item.Length > 20)
                        {
                            string lemt = "<MESSAGE" + MidStrEx_New(item, "<MESSAGE", "</MESSAGE>") + "</MESSAGE>";
                            arrayList.Add(path + lemt);
                        }
                    }
                    break;
                case 1:
                    foreach (string item in resAry)  
                    {
                        if (item.Length>20)
                        {
                            string lemt = "<MESSAGE"+ MidStrEx_New(item, "<MESSAGE", "</MESSAGE>")+ "</MESSAGE>";
                            arrayList.Add(path + lemt);
                        }
                         
                      //  string lemt = item.Replace("5A6B00000335", "").Replace("5A6B00000451", "").Replace("5A6B00000111", "").Replace("5A6B00000449","").Replace("5A6B00000570","");
                        //if (lemt.Length>20)
                        //{
                        //    arrayList.Add(path + lemt);
                        //}

                    }
                    break;
                default:
                    break;
            }
             
           

            Dictionary<String, String> JsonData = new Dictionary<String, String>();
            List<Dsuess> suess = new List<Dsuess>();
            


            foreach (string item in arrayList)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(item);
                XmlNode rootNode = xmlDoc.SelectSingleNode("MESSAGE");
              
                string Eqname = rootNode.Attributes["NAME"].Value;
                switch (type)
                {
                    case 0:
                        if (Eqname == "DOORSTATE")
                        {
                            string doorNo = rootNode.SelectSingleNode("DoorNo").InnerText;
                            #region xml无法读取数字“5”

                            //if (Addk > 0)
                            //{
                            //    if (doorNo.Length < 3)
                            //    {
                            //        doorNo = doorNo + "5";
                            //    }
                            //    doorNo = InsertFormat(doorNo, 2, "5");
                            //    Addk--;
                            //}

                            //if (doorNo.Length == 3)
                            //{
                            //    doorNo = doorNo + "5";
                            //}

                            //if (rootNode.SelectSingleNode("DoorNo").InnerText == "0049")
                            //{
                            //    Addk = 10;
                            //}
                            #endregion
                            string doorState = rootNode.SelectSingleNode("DoorState").InnerText;
                            // JsonData.Add(name, txt);
                            suess.Add(new Dsuess()
                            {
                                DoorNo = doorNo,
                                DoorState = doorState

                            });


                        }
                        break;
                    case 1:
                        if (Eqname == "DOOREVENT"&& rootNode.ChildNodes.Count==20)
                        {
                          //  int dd = rootNode.ChildNodes.Count;
                            string doorNo = rootNode.SelectSingleNode("DoorNo").InnerText;

                            string IoState = rootNode.SelectSingleNode("IoState").InnerText;
                            string EventTime = rootNode.SelectSingleNode("EventTime").InnerText;
                            string EmpNo = rootNode.SelectSingleNode("EmpNo").InnerText;
                            string EmpName = rootNode.SelectSingleNode("EmpName").InnerText;
                            string DeptName = rootNode.SelectSingleNode("DeptName").InnerText;
                            // JsonData.Add(name, txt);
                            TCPGated.record.Add(new Record()
                            {
                                DoorNo = doorNo,
                                IoState = IoState,
                                EventTime = EventTime,
                                EmpNo = EmpNo,
                                EmpName = EmpName,
                                DeptName= DeptName
                            });
                        }
                        break;
                    default:
                        break;
                }



            }

            return JsonConvert.SerializeObject(new
            {

                msg = "Success",
                MsgTxt = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(suess))
            });
        }
    }
}


