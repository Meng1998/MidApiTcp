using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBAPI.C;
using WEBAPI.M;


namespace LMWEBAPI.Controllers
{
    
    /// <summary>
    /// 蚌埠
    /// </summary>
    [Produces("application/json")]
    [Route("bengbu")]
    [ApiController]
    public class bengbu : Controller
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public JObject login([FromBody] User Parameter)
        {
            var client = new RestClient("http://28.152.1.41:8089/LocateService/login.action");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(Parameter), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JObject msg = JsonConvert.DeserializeObject<JObject>(response.Content);
           
            return msg;
        }


        /// <summary>
        /// getRegionStat
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getRegionStat")]
        public JObject getRegionStat([FromBody] getRegionStat Parameter) {

            string url = "http://28.152.1.41:8089/LocateService/getRegionStat.action?token=" + Parameter.token + "&place_id="+Parameter.place_id+"&floor_id="+Parameter.floor_id + "&building_id="+ Parameter.building_id;
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JObject msg = JsonConvert.DeserializeObject<JObject>(response.Content);
           
            return msg;
        }

        /// <summary>
        /// getToken
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getToken")]
        public JObject getToken([FromBody] getToken Parameter)
        {
           string url = "http://28.152.1.9:6100/register/getToken?key="+ Parameter.key+ "&secret="+Parameter.secret;
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject msg = JsonConvert.DeserializeObject<JObject>(response.Content);
           
            return msg;
        }

        /// <summary>
        /// getPoliceCountByDistrict
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getPoliceCountByDistrict")]
        public JObject getToken([FromBody] getPoliceCountByDistrict Parameter)
        {

            var client = new RestClient("http://28.152.1.9:6100/register/getPoliceCountByDistrict?token="+Parameter.token);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject msg = JsonConvert.DeserializeObject<JObject>(response.Content); 
            return msg;
        }

        /// <summary>
        /// getPoliceCountByDistrict
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getPoliceById")]
        public JObject getPoliceById([FromBody] getPoliceById Parameter)
        {

            var client = new RestClient("http://28.152.1.9:6100/register/getPoliceById?token="+ Parameter.token + "&districtId="+Parameter.districtId);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            JObject msg = JsonConvert.DeserializeObject<JObject>(response.Content);
           
            return msg;
        }
    }

    public class User {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class getRegionStat
    {
        public string token { get; set; }
        public string place_id { get; set; }

        public string floor_id { get; set; }
        public string building_id { get; set; }
    }

    public class getToken
    {
        public string key { get; set; }
        public string secret { get; set; }
    }

    public class getPoliceCountByDistrict
    {
        public string token { get; set; }
  
    }

    public class getPoliceById
    {
        public string token { get; set; }
        public string districtId { get; set; }
    }
}