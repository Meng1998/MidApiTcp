using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DeploymentTools.Logic
{
    public class TreeCareconfig
    {
        /// <summary>
        /// 去【】括号
        /// </summary>
        /// <param name="lis"></param>
        /// <returns></returns>
        public string[] Ary(string lis)
        {
            Regex rgx = new Regex(@"(?i)(?<=\[)(.*)(?=\])");//中括号[]
            string tmp = rgx.Match(lis).Value;//中括号[]
            string[] sArray = tmp.Split(',');
            return sArray;
        }
        /// <summary>
        /// 重复次数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="substring"></param>
        /// <returns></returns>
        public int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }

            return 0;
        }

        /// <summary>
        /// 去除字母
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public String getLetter(String str)
        {

            string Mdex1 = str.Split(':')[0].ToString();
            string six = Regex.Match(Mdex1, "[a-zA-Z]+").ToString();
            if (six != null && six != "") { return Regex.Match(Mdex1, "[a-zA-Z]+").ToString(); }
            else
            {
                return Regex.Replace(Mdex1, @"[^0-9]+", "");
            }

        }
   



    }
}
