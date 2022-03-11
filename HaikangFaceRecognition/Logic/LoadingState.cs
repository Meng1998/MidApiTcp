using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    class LoadingState
    {
        private static String State;
        public static void SetUpdateState(String SetStr)
        {
            State = SetStr;
        }
        public static String GetLoadingState() {
            return State;
        }
    }
}
