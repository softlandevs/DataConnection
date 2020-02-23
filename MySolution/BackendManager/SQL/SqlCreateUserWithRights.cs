using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.SQL
{

    /*  
     *  CREATE USER 'myuser'@'%' IDENTIFIED BY '***';
     *  GRANT USAGE ON *.* TO 'myuser'@'%' REQUIRE NONE WITH MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0 MAX_USER_CONNECTIONS 0;
     *  CREATE DATABASE IF NOT EXISTS `myuser`;
     *  GRANT ALL PRIVILEGES ON `myuser`.* TO 'myuser'@'%';
     */

    class SqlCreateUserWithRights:SqlTaskBase
    {
        public string NewUserID { get; set; }
        public string NewUserPassword { get; set; }

        public async Task Run()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = this.Server,
                Port = this.Port,
                UserID = this.UserID,
                Password = this.Password,
                SslMode = this.SslMode,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Debug.WriteLine("Opening connection");
                await conn.OpenAsync();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = $"CREATE USER '{NewUserID}'@'%' IDENTIFIED BY '{NewUserPassword}';";
                    await command.ExecuteNonQueryAsync();
                    Debug.WriteLine("Finished creating User");

                    command.CommandText = $"GRANT USAGE ON *.* TO '{NewUserID}'@'%' REQUIRE NONE WITH MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0 MAX_USER_CONNECTIONS 0;";
                    await command.ExecuteNonQueryAsync();
                    Debug.WriteLine("Finished granting general rights");

                    command.CommandText = $"GRANT ALL PRIVILEGES ON `{Database}`.* TO '{NewUserID}'@'%';";
                    await command.ExecuteNonQueryAsync();
                    Debug.WriteLine("Finished granting general rights");
                }
            }

            // connection will be closed by the 'using' block
            Debug.WriteLine("Closing connection");
        }
    }
}
