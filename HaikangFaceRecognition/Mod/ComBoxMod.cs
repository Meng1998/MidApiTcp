using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
    class ComBoxMod
    {
        public class ItemContent
        {
            public string Name { get; set; }
            public string CameraId { get; set; }

        }
        public class ItemContentIP
        {
            public string Name { get; set; }
            public IPAddress IP { get; set; }

        }
    }
}
