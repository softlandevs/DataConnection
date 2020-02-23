using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.SQL
{
    internal class SqlSetupDataBase : SqlTaskBase
    {
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
                    command.CommandText = $"CREATE DATABASE `{Database}`;";
                    await command.ExecuteNonQueryAsync();
                    Debug.WriteLine("Finished creating database");
                }
            }

            // connection will be closed by the 'using' block
            Debug.WriteLine("Closing connection");
        }
    }
}
