using Model;
using Model.Base;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BackendManager.Sync.TableBuilder;

namespace BackendManager.SQL
{
    internal class SqlLoadData : SqlTaskBase
    {
        public List<Model.Base.ManagedObjectBase> DataSet { get; set; }
        public Model.Base.ManagedMetaObject MetaObjectDescriptor { get; set; }
        public TableDescription TableDescription { get; set; }
        public ModelContainer ModelContainer { get; internal set; }

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

                var dataToInsert = DataSet.Where(x => x.Verison < 0).ToList();
                var dataToUpdate = DataSet.Where(x => x.Verison > ModelContainer.VersionHead).ToList();

                using (var command = conn.CreateCommand())
                {

                    var commandText = CreateInsertCommand(dataToInsert, MetaObjectDescriptor, TableDescription);
                    if (commandText != null)
                    {
                        command.CommandText = commandText;
                        int rowCount = await command.ExecuteNonQueryAsync();
                        Debug.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
                    }

                    RunUpdateCommands(command, dataToUpdate, MetaObjectDescriptor, TableDescription);
                }

                // connection will be closed by the 'using' block
                Debug.WriteLine("Closing connection");
            }

            // connection will be closed by the 'using' block
            Debug.WriteLine("Closing connection");
        }

        private async void RunUpdateCommands(MySqlCommand command, IEnumerable<ManagedObjectBase> dataSet, ManagedMetaObject metaObjectDescriptor, TableDescription tableDescription)
        {
            var keyColumnDescription = tableDescription.Columns.First(x => x.IsKey);

            foreach (var data in dataSet)
            {
                var commandText = CreateUpdateCommand(data, MetaObjectDescriptor, TableDescription, keyColumnDescription);
                command.CommandText = commandText;
                int rowCount = await command.ExecuteNonQueryAsync();
                Debug.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
            }

        }

        private string CreateUpdateCommand(ManagedObjectBase data, ManagedMetaObject metaObjectDescriptor, TableDescription tableDescription, ColumnDescription keyColumnDescriptor)
        {
            var commandText = @String.Empty;
            data.Verison = ModelContainer.VersionHead;

            commandText += "UPDATE";
            commandText += $" `{tableDescription.Name}`";
            commandText += $" SET ";

            var propertyAssignement = new List<string>();
            foreach (var columnDescription in tableDescription.Columns.Where(x => !x.IsKey))
            {
                var value = GetValueString(metaObjectDescriptor, columnDescription, data);
                propertyAssignement.Add($"`{columnDescription.Name}` = '{value}'");
            }
            commandText += $"{string.Join(", ", propertyAssignement)}";

            commandText += " WHERE";

            var keyValue = GetValueString(metaObjectDescriptor, keyColumnDescriptor, data);
            commandText += $" `{keyColumnDescriptor.Name}` = '{keyValue}'";

            return commandText;
        }

        private string CreateInsertCommand(IEnumerable<Model.Base.ManagedObjectBase> dataSet, Model.Base.ManagedMetaObject metaObjectDescriptor, TableDescription tableDescription)
        {
            if (!dataSet.Any()) return null;

            var commandText = @String.Empty;

            commandText += "INSERT INTO";
            commandText += $" `{tableDescription.Name}`";
            commandText += $" (";

            var columns = new List<string>();
            foreach (var columnDescription in tableDescription.Columns)
            {
                columns.Add($"`{columnDescription.Name}`");
            }
            commandText += $"{string.Join(", ", columns)}";
            commandText += $")";

            commandText += $" VALUES";

            var sValues = new List<string>();

            foreach (var entry in dataSet)
            {
                entry.Verison = ModelContainer.VersionHead;

                var sValue = new List<string>();
                foreach (var columnDescription in tableDescription.Columns)
                {
                    var value = GetValueString(metaObjectDescriptor, columnDescription, entry);
                    sValue.Add($"'{value.ToString()}'");

                }
                sValues.Add($"(" + String.Join(", ", sValue) + $")");
            }

            commandText += $" {string.Join(", ", sValues)}";
            commandText += ";";

            return commandText;
        }

        private string GetValueString(ManagedMetaObject metaObjectDescriptor, ColumnDescription columnDescription, ManagedObjectBase entry)
        {
            var property = metaObjectDescriptor.Type.GetProperty(columnDescription.Name);
            var value = property.GetValue(entry);

            if (value == null)
            {
                return "NULL";
            }
            else
            {
                if (columnDescription.IsReference)
                {
                    var data = columnDescription.Type.GetProperty("Key").GetValue(value);
                    return data.ToString();
                }
                else if (value.GetType() == typeof(DateTime))
                {
                    DateTime? date = value as DateTime?;
                    if (date.HasValue)
                    {
                        return date.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else
                {
                    return value.ToString();
                }
            }

            return null;
        }
    }
}
