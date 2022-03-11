using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    class TreeValue
    {
        private static String Nmae;
        private static String Value;
        public static String GetName() {
            return Nmae;
        }
        public static void SetName(String mc) {
            Nmae = mc;
        }
        public  static String GetValue()
        {
            return Value;
        }
        public static void SetValue(String mc)
        {
            Value = mc;
        }
    }
}
