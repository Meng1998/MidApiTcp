using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    class AreaPages
    {
        private static Int32 NumberEquipment;
        private static Int32 Pages;
        /// <summary>
        /// 区域最大页
        /// </summary>
        /// <returns></returns>
        public static Int32 GetPages() {
            return Pages;
        }
        public static void SetPages(Int32 Value)
        {
            Pages = Value;
        }
        public static Int32 GetNumberEquipment()
        {
            return NumberEquipment;
        }
        public static void SetNumberEquipment(Int32 Value)
        {
            NumberEquipment = Value;
        }
    }
}
