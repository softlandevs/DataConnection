using BackendManager.Sync;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.SQL
{
    internal class SqlCreateTable : SqlTaskBase
    {
        public TableDescription TableDescription { get; set; }

        public async Task Run()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = this.Server,
                Port = this.Port,
                Database = this.Database,
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
                    command.CommandText = CreateTableCommand(TableDescription);
                    Debug.WriteLine("SqlCommand: " + command.CommandText);
                    await command.ExecuteNonQueryAsync();
                    Debug.WriteLine("Finished creating table");
                }

                // connection will be closed by the 'using' block
                Debug.WriteLine("Closing connection");
            }

            // connection will be closed by the 'using' block
            Debug.WriteLine("Closing connection");
        }

        private string CreateTableCommand(TableDescription tableDescription)
        {
            var command = String.Empty;

            command += "CREATE TABLE";
            command += $" `{tableDescription.Name}`";
            command += $" (";
            
            var columns = new List<string>();
            foreach(var columnDescription in tableDescription.Columns)
            {
                columns.Add($"`{columnDescription.Name}` {columnDescription.SqlType}");
            }
            command += $"{string.Join(", " , columns)}";
            command += $" );";


            return command;
        }
    }
}
