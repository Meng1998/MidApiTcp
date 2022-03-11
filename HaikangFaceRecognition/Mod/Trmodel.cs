using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
    class Trmodel
    {
        public Trmodel()
        {
            this.Nodes = new List<Trmodel>();
        }
        public Int32 ID { get; set; }
        public Int32 PID { get; set; }
        public String Name { get; set; }
        public Int32? DiaID { get; set; }

        public List<Trmodel> Nodes { get; set; }

    }
}
