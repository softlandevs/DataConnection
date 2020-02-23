using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.SQL
{
    internal class SqlTaskBase
    {
        public string Server { get; set; }
        public uint Port { get; set; }
        public string Database { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public MySqlSslMode SslMode { get; set; } = MySqlSslMode.None;

    }
}
