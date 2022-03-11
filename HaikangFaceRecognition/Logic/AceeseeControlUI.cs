using DeploymentTools.Mod;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static DeploymentTools.Mod.ComBoxMod;

namespace DeploymentTools.Logic
{
    
    class AceeseeControlUI
    {
        GetDataSet GetSetData = new GetDataSet();
        PGDataProcessing PGdata = new PGDataProcessing();
        PGAddequipmentDataMod Toadd = new PGAddequipmentDataMod();
        //数据库的上图设备数 （与需要更新的数据进行校对的数量）
        List<String> EquipmentPageIndex = new List<String>();
        InitKey Init = new InitKey();
        public List<JObject> GetAceesee() {
            String parameter = Init.GetAccessControlList(GetSetData.StitchingParameters(GetSetData.GetISCkey(9, 0)));
            JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
            List<JObject> list = new List<JObject>();
            list.Add(rb);
            if (GetDataSet.GetISCmsgSuccessfulState(parameter))
            {
                
                
                //处理设备超出通用操作，开始追加数据
                String total = rb["data"]["total"].ToString();//取出单页最大数
                Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));
                if (page > 1)
                {
                    for (int i = 2; i < page + 1; i++)
                    {
                        parameter = Init.GetAccessControlList(GetSetData.StitchingParameters(GetSetData.GetISCkey(9, 0)), i, 1000);
                        rb = JsonConvert.DeserializeObject<JObject>(parameter);
                        list.Add(rb);
                    }
                }
            }

            try
            {
                return list;
            }
            catch (Exception)
            {
                var err = new
                {
                    msg = "error",
                    Remarks = "接口超时或报错！错误信息为:" + parameter
                };
                var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));
                
                list.Add(a);
                return list;
            }
        }

        public List<JObject> GetSpccAceesee()
        {
            String parameter = Init.SPCCGetAccessControlAccess(1, 300);
            List<JObject> list = new List<JObject>();
            try
            {

                JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
                list.Add(rb);
                if (GetDataSet.GetISCmsgSuccessfulState(parameter))
                {
                    //处理设备超出通用操作，开始追加数据
                    String total = rb["data"]["total"].ToString();//取出单页最大数
                    Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(300));
                    if (page > 1)
                    {
                        for (int i = 2; i < page + 1; i++)
                        {
                            parameter = Init.SPCCGetAccessControlAccess(i, 300);
                            rb = JsonConvert.DeserializeObject<JObject>(parameter);
                            list.Add(rb);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                var err = new
                {
                    msg = "error" + ex.Message,
                    Remarks = "接口超时或报错！错误信息为:" + parameter
                };
                var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));

                list.Add(a);
                return list;
            }
        }


        public String EquipmentLibrary(List<JObject> Data,ComboBox[] CB,bool vl) {
            Boolean error = true;

            ItemContent TypeCB = new ItemContent();
            ItemContent Iregion_idCB = new ItemContent();
            CB[0].Dispatcher.Invoke(new Action(() =>
            {
                TypeCB = CB[0].SelectedItem as ItemContent;
                Iregion_idCB = CB[1].SelectedItem as ItemContent;
            }));
            

            ///删表操作
            if (!String.IsNullOrEmpty(TypeCB.CameraId) && !String.IsNullOrEmpty(Iregion_idCB.CameraId))
            {
                PGDataProcessing PGdata = new PGDataProcessing();
                //PGdata.ExecuteQuery("TRUNCATE TABLE dock_region", ref error);
                //删除全部没有上图的设备
                PGdata.ExecuteQuery($"DELETE FROM dock_device WHERE  on_map = false and device_type='{TypeCB.CameraId}'", ref error);
                DataSet EPage = PGdata.ExecuteQuery($"SELECT * FROM dock_device", ref error);
                if (error)
                {
                    //存入设备唯一标识码 后期跳过
                    for (int i = 0; i < EPage.Tables[0].Rows.Count; i++)
                    {
                        EquipmentPageIndex.Add(EPage.Tables[0].Rows[i]["device_code"].ToString());
                    }
                }
                else
                {
                    return "删除数据时出现不可预料的错误，为避免损失程序强行中断了操作。";
                }
            }
            else
            {
                return "设备类型为空或厂商类型为空";

            }


            foreach (var rbs in Data)
            {
                if (!GetDataSet.GetISCmsgSuccessfulState(JsonConvert.SerializeObject(rbs)))
                {

                    return "数据异常，被系统拒绝！请重新查询。";
                }
                foreach (var item in rbs["data"]["list"])
                {
                    if (vl) {
                        item["doorIndexCode"] = item["channelIndexCode"].ToString();
                        item["doorName"] = item["device"]["deviceName"].ToString();
                    }
                    Toadd.Id = Guid.NewGuid().ToString("N");
                    Toadd.Type = TypeCB.CameraId;//设备类型
                    Toadd.Code = item["doorIndexCode"].ToString();
                    Toadd.Name = item["doorName"].ToString();
                    Toadd.Iregion_id = Iregion_idCB.CameraId;//平台類型 regionIndexCode
                    Toadd.Info = item.ToString();
                    Toadd.Map = "false";

                    Boolean whether = true;//是否上图
                    foreach (var IndexPage in EquipmentPageIndex)
                    {
                        if (IndexPage == Toadd.Code)
                        {
                            whether = false;
                        }
                    }
                    if (whether)//过已上图数据
                    {
                        PGdata.ExecuteQuery($"INSERT INTO\"public\".\"dock_device\"(\"id\",\"device_type\",\"device_code\",\"device_name\",\"region_id\",\"device_info\",\"on_map\")VALUES('{Toadd.Id}','{Toadd.Type}','{Toadd.Code}','{Toadd.Name}','{Toadd.Iregion_id}','{Toadd.Info}','{Toadd.Map}')RETURNING*", ref error);
                    }

                    if (!error)
                    {
                        return "数据库添加时出现异常！同步数据被迫中断。";
                    }
                }
            }
            return "数据同步完成！";
        }
    }


    class SynchronousTalkbackUI
    {
        GetDataSet GetSetData = new GetDataSet();
        PGDataProcessing PGdata = new PGDataProcessing();
        PGAddequipmentDataMod Toadd = new PGAddequipmentDataMod();
        //数据库的上图设备数 （与需要更新的数据进行校对的数量）
        List<String> EquipmentPageIndex = new List<String>();
        InitKey Init = new InitKey();
        public List<JObject> GetAceesee()
        {
            String parameter = Init.GetAccessControlList(GetSetData.StitchingParameters(GetSetData.GetISCkey(9, 0)));
            JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
            List<JObject> list = new List<JObject>();
            list.Add(rb);
            if (GetDataSet.GetISCmsgSuccessfulState(parameter))
            {


                //处理设备超出通用操作，开始追加数据
                String total = rb["data"]["total"].ToString();//取出单页最大数
                Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));
                if (page > 1)
                {
                    for (int i = 2; i < page + 1; i++)
                    {
                        parameter = Init.GetAccessControlList(GetSetData.StitchingParameters(GetSetData.GetISCkey(9, 0)), i, 1000);
                        rb = JsonConvert.DeserializeObject<JObject>(parameter);
                        list.Add(rb);
                    }
                }
            }

            try
            {
                return list;
            }
            catch (Exception)
            {
                var err = new
                {
                    msg = "error",
                    Remarks = "接口超时或报错！错误信息为:" + parameter
                };
                var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));

                list.Add(a);
                return list;
            }
        }

        public List<JObject> GetSpccAceesee()
        {
            String parameter = Init.SPCCGetDockingChannel(1, 1000);
            List<JObject> list = new List<JObject>();
            try
            {
                JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
                list.Add(rb);
                if (GetDataSet.GetISCmsgSuccessfulState(parameter))
                {
                    //处理设备超出通用操作，开始追加数据
                    String total = rb["data"]["total"].ToString();//取出单页最大数
                    Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));
                    if (page > 1)
                    {
                        for (int i = 2; i < page + 1; i++)
                        {
                            parameter = Init.SPCCGetDockingChannel(i, 1000);
                            rb = JsonConvert.DeserializeObject<JObject>(parameter);
                            list.Add(rb);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                var err = new
                {
                    msg = "error" + ex.Message,
                    Remarks = "接口超时或报错！错误信息为:" + parameter
                };
                var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));

                list.Add(a);
                return list;
            }
        }


        public String EquipmentLibrary(List<JObject> Data, ComboBox[] CB, bool vl)
        {
            Boolean error = true;

            ItemContent TypeCB = new ItemContent();
            ItemContent Iregion_idCB = new ItemContent();
            CB[0].Dispatcher.Invoke(new Action(() =>
            {
                TypeCB = CB[0].SelectedItem as ItemContent;
                Iregion_idCB = CB[1].SelectedItem as ItemContent;
            }));


            ///删表操作
            if (!String.IsNullOrEmpty(TypeCB.CameraId) && !String.IsNullOrEmpty(Iregion_idCB.CameraId))
            {
                PGDataProcessing PGdata = new PGDataProcessing();
                //PGdata.ExecuteQuery("TRUNCATE TABLE dock_region", ref error);
                //删除全部没有上图的设备
                PGdata.ExecuteQuery($"DELETE FROM dock_device WHERE  on_map = false and device_type='{TypeCB.CameraId}'", ref error);
                DataSet EPage = PGdata.ExecuteQuery($"SELECT * FROM dock_device", ref error);
                if (error)
                {
                    //存入设备唯一标识码 后期跳过
                    for (int i = 0; i < EPage.Tables[0].Rows.Count; i++)
                    {
                        EquipmentPageIndex.Add(EPage.Tables[0].Rows[i]["device_code"].ToString());
                    }
                }
                else
                {
                    return "删除数据时出现不可预料的错误，为避免损失程序强行中断了操作。";
                }
            }
            else
            {
                return "设备类型为空或厂商类型为空";

            }


            foreach (var rbs in Data)
            {
                if (!GetDataSet.GetISCmsgSuccessfulState(JsonConvert.SerializeObject(rbs)))
                {

                    return "数据异常，被系统拒绝！请重新查询。";
                }
                foreach (var item in rbs["data"]["list"])
                {
                    if (vl)
                    {
                        item["doorIndexCode"] = item["channelIndexCode"].ToString();
                        item["doorName"] = item["device"]["deviceName"].ToString();
                    }
                    Toadd.Id = Guid.NewGuid().ToString("N");
                    Toadd.Type = TypeCB.CameraId;//设备类型
                    Toadd.Code = item["doorIndexCode"].ToString();
                    Toadd.Name = item["doorName"].ToString();
                    Toadd.Iregion_id = Iregion_idCB.CameraId;//平台類型 regionIndexCode
                    Toadd.Info = item.ToString();
                    Toadd.Map = "false";

                    Boolean whether = true;//是否上图
                    foreach (var IndexPage in EquipmentPageIndex)
                    {
                        if (IndexPage == Toadd.Code)
                        {
                            whether = false;
                        }
                    }
                    if (whether)//过已上图数据
                    {
                        PGdata.ExecuteQuery($"INSERT INTO\"public\".\"dock_device\"(\"id\",\"device_type\",\"device_code\",\"device_name\",\"region_id\",\"device_info\",\"on_map\")VALUES('{Toadd.Id}','{Toadd.Type}','{Toadd.Code}','{Toadd.Name}','{Toadd.Iregion_id}','{Toadd.Info}','{Toadd.Map}')RETURNING*", ref error);

                        Log.WriteLog("msg", $"INSERT INTO\"public\".\"dock_device\"(\"id\",\"device_type\",\"device_code\",\"device_name\",\"region_id\",\"device_info\",\"on_map\")VALUES('{Toadd.Id}','{Toadd.Type}','{Toadd.Code}','{Toadd.Name}','{Toadd.Iregion_id}','{Toadd.Info}','{Toadd.Map}')RETURNING*");

                    }

                    if (!error)
                    {
                        return "数据库添加时出现异常！同步数据被迫中断。";
                    }
                }
            }
            return "数据同步完成！";
        }
    }
    class ZoneSynchronizationUI
    {
        GetDataSet GetSetData = new GetDataSet();
        PGDataProcessing PGdata = new PGDataProcessing();
        PGAddequipmentDataMod Toadd = new PGAddequipmentDataMod();
        //数据库的上图设备数 （与需要更新的数据进行校对的数量）
        List<String> EquipmentPageIndex = new List<String>();
        InitKey Init = new InitKey();
        public Dictionary<String, JObject> GetAceesee()
        {

            Dictionary<String, JObject> ListSE = new Dictionary<String, JObject>();
            List<JObject> Iaslist = new List<JObject>();

            //获取报警主机
            {
                String parameter = Init.GetIasSensorList(GetSetData.StitchingParameters(GetSetData.GetISCkey(11, 0)));
                JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);

                Iaslist.Add(rb);
                if (GetDataSet.GetISCmsgSuccessfulState(parameter))
                {
                    //处理设备超出通用操作，开始追加数据
                    String total = rb["data"]["total"].ToString();//取出单页最大数
                    Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));
                    if (page > 1)
                    {
                        for (int i = 2; i < page + 1; i++)
                        {
                            parameter = Init.GetIasSensorList(GetSetData.StitchingParameters(GetSetData.GetISCkey(11, 0)), i, 1000);
                            rb = JsonConvert.DeserializeObject<JObject>(parameter);
                            Iaslist.Add(rb);
                        }
                    }
                }
            }
            try
            {   //获取主机下的防区
                foreach (var item in Iaslist)
                {
                    List<JObject> rbs = new List<JObject>();
                    foreach (var list in item["data"]["list"])
                    {
                        //item["data"]["list"]["indexCode"]

                        String parameter = Init.GetiasDevicelDefenceList(GetSetData.StitchingParameters(GetSetData.GetISCkey(12, 0)), list["indexCode"].ToString());
                        JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
                        //rbs.Add(rb);
                        ListSE.Add(list["name"].ToString(), rb);
                    }
                    
                }

                return ListSE;
            }
            catch (Exception ex)
            {
                var err = new
                {
                    msg = "error",
                    Remarks = "接口超时或报错" + ex.Message
                };
                var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));

                ListSE.Add("异常" + ex.Message ,  a );
                return ListSE;
            }
        }

        public List<JObject> GetSpccAceesee()
        {
            String parameter = Init.SPCCGetZoneSynchronization(1, 300);
            List<JObject> list = new List<JObject>();
            try
            {

                JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
                list.Add(rb);
                if (GetDataSet.GetISCmsgSuccessfulState(parameter))
                {
                    //处理设备超出通用操作，开始追加数据
                    String total = rb["data"]["total"].ToString();//取出单页最大数
                    Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(300));
                    if (page > 1)
                    {
                        for (int i = 2; i < page + 1; i++)
                        {
                            parameter = Init.SPCCGetZoneSynchronization(i, 300);
                            rb = JsonConvert.DeserializeObject<JObject>(parameter);
                            list.Add(rb);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                var err = new
                {
                    msg = "error" + ex.Message,
                    Remarks = "接口超时或报错！错误信息为:" + parameter
                };
                var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));

                list.Add(a);
                return list;
            }
        }


        public String EquipmentLibrary(List<JObject> Data, ComboBox[] CB, bool vl)
        {
            Boolean error = true;

            ItemContent TypeCB = new ItemContent();
            ItemContent Iregion_idCB = new ItemContent();
            CB[0].Dispatcher.Invoke(new Action(() =>
            {
                TypeCB = CB[0].SelectedItem as ItemContent;
                Iregion_idCB = CB[1].SelectedItem as ItemContent;
            }));


            ///删表操作
            if (!String.IsNullOrEmpty(TypeCB.CameraId) && !String.IsNullOrEmpty(Iregion_idCB.CameraId))
            {
                PGDataProcessing PGdata = new PGDataProcessing();
                //PGdata.ExecuteQuery("TRUNCATE TABLE dock_region", ref error);
                //删除全部没有上图的设备
                PGdata.ExecuteQuery($"DELETE FROM dock_device WHERE  on_map = false and device_type='{TypeCB.CameraId}'", ref error);
                DataSet EPage = PGdata.ExecuteQuery($"SELECT * FROM dock_device", ref error);
                if (error)
                {
                    //存入设备唯一标识码 后期跳过
                    for (int i = 0; i < EPage.Tables[0].Rows.Count; i++)
                    {
                        EquipmentPageIndex.Add(EPage.Tables[0].Rows[i]["device_code"].ToString());
                    }
                }
                else
                {
                    return "删除数据时出现不可预料的错误，为避免损失程序强行中断了操作。";
                }
            }
            else
            {
                return "设备类型为空或厂商类型为空";

            }


            foreach (var rbs in Data)
            {
                if (!GetDataSet.GetISCmsgSuccessfulState(JsonConvert.SerializeObject(rbs)))
                {

                    return "数据异常，被系统拒绝！请重新查询。";
                }
                foreach (var item in rbs["data"]["list"])
                {
                    if (vl)
                    {
                        item["doorIndexCode"] = item["channelIndexCode"].ToString();
                        item["doorName"] = item["device"]["deviceName"].ToString();
                    }
                    Toadd.Id = Guid.NewGuid().ToString("N");
                    Toadd.Type = TypeCB.CameraId;//设备类型
                    Toadd.Code = item["doorIndexCode"].ToString();
                    Toadd.Name = item["doorName"].ToString();
                    Toadd.Iregion_id = Iregion_idCB.CameraId;//平台類型 regionIndexCode
                    Toadd.Info = item.ToString();
                    Toadd.Map = "false";

                    Boolean whether = true;//是否上图
                    foreach (var IndexPage in EquipmentPageIndex)
                    {
                        if (IndexPage == Toadd.Code)
                        {
                            whether = false;
                        }
                    }
                    if (whether)//过已上图数据
                    {
                        PGdata.ExecuteQuery($"INSERT INTO\"public\".\"dock_device\"(\"id\",\"device_type\",\"device_code\",\"device_name\",\"region_id\",\"device_info\",\"on_map\")VALUES('{Toadd.Id}','{Toadd.Type}','{Toadd.Code}','{Toadd.Name}','{Toadd.Iregion_id}','{Toadd.Info}','{Toadd.Map}')RETURNING*", ref error);
                    }

                    if (!error)
                    {
                        return "数据库添加时出现异常！同步数据被迫中断。";
                    }
                }
            }
            return "数据同步完成！";
        }
    }

    class SynchronousSensorUI
    {
        GetDataSet GetSetData = new GetDataSet();
        PGDataProcessing PGdata = new PGDataProcessing();
        PGAddequipmentDataMod Toadd = new PGAddequipmentDataMod();
        //数据库的上图设备数 （与需要更新的数据进行校对的数量）
        List<String> EquipmentPageIndex = new List<String>();
        InitKey Init = new InitKey();
        public List<JObject> GetAceesee()
        {
            String parameter = Init.GetAccessSensorList(GetSetData.StitchingParameters(GetSetData.GetISCkey(10, 0)));
            JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
            List<JObject> list = new List<JObject>();
            list.Add(rb);
            if (GetDataSet.GetISCmsgSuccessfulState(parameter))
            {


                //处理设备超出通用操作，开始追加数据
                String total = rb["data"]["total"].ToString();//取出单页最大数
                Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));
                if (page > 1)
                {
                    for (int i = 2; i < page + 1; i++)
                    {
                        parameter = Init.GetAccessSensorList(GetSetData.StitchingParameters(GetSetData.GetISCkey(10, 0)), i, 1000);
                        rb = JsonConvert.DeserializeObject<JObject>(parameter);
                        list.Add(rb);
                    }
                }
            }

            try
            {
                return list;
            }
            catch (Exception)
            {
                var err = new
                {
                    msg = "error",
                    Remarks = "接口超时或报错！错误信息为:" + parameter
                };
                var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));

                list.Add(a);
                return list;
            }
        }

        public List<JObject> GetSpccAceesee()
        {
            String parameter = Init.SPCCGetAccessControlAccess(1, 1000);
            List<JObject> list = new List<JObject>();
            //try
            //{

            //    JObject rb = JsonConvert.DeserializeObject<JObject>(parameter);
            //    list.Add(rb);
            //    if (GetDataSet.GetISCmsgSuccessfulState(parameter))
            //    {
            //        //处理设备超出通用操作，开始追加数据
            //        String total = rb["data"]["total"].ToString();//取出单页最大数
            //        Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));
            //        if (page > 1)
            //        {
            //            for (int i = 2; i < page + 1; i++)
            //            {
            //                parameter = Init.SPCCGetAccessControlAccess(i, 1000);
            //                rb = JsonConvert.DeserializeObject<JObject>(parameter);
            //                list.Add(rb);
            //            }
            //        }
            //    }




            //    return list;
            //}
            //catch (Exception ex)
            //{
            //    var err = new
            //    {
            //        msg = "error" + ex.Message,
            //        Remarks = "接口超时或报错！错误信息为:" + parameter
            //    };
            //    var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));

            //    list.Add(a);
            //    return list;
            //}

            var err = new
            {
                msg = "error" + "不支持SPCC",
                Remarks = "接口超时或报错！错误信息为:" + "不支持SPCC"
            };
            var a = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(err));

            list.Add(a);
            return list;
        }


        public String EquipmentLibrary(List<JObject> Data, ComboBox[] CB, bool vl)
        {
            Boolean error = true;

            ItemContent TypeCB = new ItemContent();
            ItemContent Iregion_idCB = new ItemContent();
            CB[0].Dispatcher.Invoke(new Action(() =>
            {
                TypeCB = CB[0].SelectedItem as ItemContent;
                Iregion_idCB = CB[1].SelectedItem as ItemContent;
            }));


            ///删表操作
            if (!String.IsNullOrEmpty(TypeCB.CameraId) && !String.IsNullOrEmpty(Iregion_idCB.CameraId))
            {
                PGDataProcessing PGdata = new PGDataProcessing();
                //PGdata.ExecuteQuery("TRUNCATE TABLE dock_region", ref error);
                //删除全部没有上图的设备
                PGdata.ExecuteQuery($"DELETE FROM dock_device WHERE  on_map = false and device_type='{TypeCB.CameraId}'", ref error);
                DataSet EPage = PGdata.ExecuteQuery($"SELECT * FROM dock_device", ref error);
                if (error)
                {
                    //存入设备唯一标识码 后期跳过
                    for (int i = 0; i < EPage.Tables[0].Rows.Count; i++)
                    {
                        EquipmentPageIndex.Add(EPage.Tables[0].Rows[i]["device_code"].ToString());
                    }
                }
                else
                {
                    return "删除数据时出现不可预料的错误，为避免损失程序强行中断了操作。";
                }
            }
            else
            {
                return "设备类型为空或厂商类型为空";

            }


            foreach (var rbs in Data)
            {
                if (!GetDataSet.GetISCmsgSuccessfulState(JsonConvert.SerializeObject(rbs)))
                {

                    return "数据异常，被系统拒绝！请重新查询。";
                }
                foreach (var item in rbs["data"]["list"])
                {
                    if (vl)
                    {
                        item["doorIndexCode"] = item["channelIndexCode"].ToString();
                        item["doorName"] = item["device"]["deviceName"].ToString();
                    }
                    Toadd.Id = Guid.NewGuid().ToString("N");
                    Toadd.Type = TypeCB.CameraId;//设备类型
                    Toadd.Code = item["doorIndexCode"].ToString();
                    Toadd.Name = item["doorName"].ToString();
                    Toadd.Iregion_id = Iregion_idCB.CameraId;//平台類型 regionIndexCode
                    Toadd.Info = item.ToString();
                    Toadd.Map = "false";

                    Boolean whether = true;//是否上图
                    foreach (var IndexPage in EquipmentPageIndex)
                    {
                        if (IndexPage == Toadd.Code)
                        {
                            whether = false;
                        }
                    }
                    if (whether)//过已上图数据
                    {
                        PGdata.ExecuteQuery($"INSERT INTO\"public\".\"dock_device\"(\"id\",\"device_type\",\"device_code\",\"device_name\",\"region_id\",\"device_info\",\"on_map\")VALUES('{Toadd.Id}','{Toadd.Type}','{Toadd.Code}','{Toadd.Name}','{Toadd.Iregion_id}','{Toadd.Info}','{Toadd.Map}')RETURNING*", ref error);
                    }

                    if (!error)
                    {
                        return "数据库添加时出现异常！同步数据被迫中断。";
                    }
                }
            }
            return "数据同步完成！";
        }
    }

}
