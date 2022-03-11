using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    static class Expand
    {
        /// <summary>
        /// 删除字符串空格字符
        /// </summary>
        /// <param name="ExpandClear"></param>
        /// <returns></returns>
        public static String ClearBlanks(this String ExpandClear) {

            return ExpandClear.Replace(" ", "");
        }
    }
}
