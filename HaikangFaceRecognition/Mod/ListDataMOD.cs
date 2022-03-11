using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
    public class ListDataMOD
    {
        public class EventListDataMod
        {
            public String Code { get; set; }

            public String Subscriptiontype { get; set; }

            public String Eventcode { get; set; }

            public String Callbacklink { get; set; }

            public String SubscriptionStatus { get; set; }

        }
        public class LnstallIISListData {
            public String IISName { get; set; }
            public String IISPort { get; set; }
        }

    }
}
