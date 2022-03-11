using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
    class PGAddequipmentDataMod
    {
            public String Id   { get; set; }
            public String Type { get; set; }
            public String Code { get; set; }
            public String Name { get; set; }
            public String Iregion_id { get; set; }
            public String Info { get; set; }
            public String Map { get; set; }

    }
    class PGAddEegionDataMod
    {
        public String Id { get; set; }
        public String Pid { get; set; }
        public String Name { get; set; }
        public String Platform { get; set; }

    }
}
