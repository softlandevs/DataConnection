using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.Configuration
{
    public class BackendConfiguration
    {
        public string DataBaseUserId { get; set; }
        public string DataBaseUserPassword { get; set; }
        public string Server { get; set; }
        public uint Port { get; set; }
        public string DataBase { get; set; }

        public override string ToString()
        {
            return DataBaseUserId + ":" + DataBaseUserPassword + "@" + Server + ":" + Port.ToString() + " " + DataBase;
        }
    }
}
