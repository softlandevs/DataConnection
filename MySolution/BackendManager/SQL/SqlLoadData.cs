using BackendManager.Sync;
using Model;
using Model.Base;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.SQL
{
    internal class SqlLoadData : SqlTaskBase
    {
        public List<Model.Base.ManagedObjectBase> DataSet { get; set; }
        public Model.Base.ManagedMetaObject MetaObjectDescriptor { get; set; }
        public TableDescription TableDescription { get; set; }
        public ModelContainer ModelContainer { get; internal set; }
        public object DataStore { get; internal set; }

        public async Task<long> Run()
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

            var maxVersionNumber = ModelContainer.VersionHead;

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Debug.WriteLine("Opening connection");
                await conn.OpenAsync();

                using (var command = conn.CreateCommand())
                {
                    command.CommandText = CreateSelectCommand(TableDescription, ModelContainer);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var guidKey = GetKey(reader);
                            var data = GetOrCreateDataInstance(guidKey, DataSet, MetaObjectDescriptor, DataStore);
                            InjectDetailsintoDataInstance(data, reader, TableDescription);

                            maxVersionNumber = Math.Max(data.Verison, maxVersionNumber);
                        }
                    }
                }

                Debug.WriteLine("Closing connection");
            }

            // connection will be closed by the 'using' block
            Debug.WriteLine("Closing connection");

            return maxVersionNumber;
        }

        private string CreateSelectCommand(TableDescription tableDescription, ModelContainer modelContainer)
        {
            var commandText = String.Empty;

            commandText += "SELECT";

            var sVariables = new List<string>();
            foreach (var column in tableDescription.Columns.OrderBy(x => !x.IsKey)) //Key always needs to be the first variable
            {
                sVariables.Add($"`{column.Name}`");
            }
            commandText += $" {string.Join(", ", sVariables)}";
            commandText += " FROM";
            commandText += $" `{tableDescription.Name}`";
            commandText += $" WHERE {nameof(Model.Base.ManagedObjectBase.Verison)}";
            commandText += $" >";
            commandText += $" '{modelContainer.VersionHead}'";

            return commandText;
        }

        private string GetKey(DbDataReader reader)
        {
            return reader.GetString(0); //The first value is always the Key
        }

        private ManagedObjectBase GetOrCreateDataInstance(string guidKey, List<ManagedObjectBase> dataSet, ManagedMetaObject metaObjectDescriptor, object dataStore)
        {
            return GetInstanceFromModelContainer(guidKey, DataSet) ?? CreateInstanceOfManagedObject(guidKey, metaObjectDescriptor, dataStore);
        }

        private ManagedObjectBase CreateInstanceOfManagedObject(string guidKey, ManagedMetaObject metaObjectDescriptor, object dataStore)
        {
            ManagedObjectBase data = null;
            var ctor = metaObjectDescriptor.Type.GetConstructor(new Type[] { });
            data = ctor.Invoke(new object[] { }) as ManagedObjectBase;
            InsertDataIntoModelContainer(data, dataStore);
            return data;
        }

        private ManagedObjectBase GetInstanceFromModelContainer(string guidKey, List<ManagedObjectBase> dataSet)
        {
            return dataSet.FirstOrDefault(x => x.Key == guidKey);
        }

        private void InsertDataIntoModelContainer(ManagedObjectBase data, object dataStore)
        {
            var addMethodinfo = dataStore.GetType().GetMethod("Add");
            addMethodinfo.Invoke(dataStore, new object[] { data.Key, data });
        }

        private void InjectDetailsintoDataInstance(ManagedObjectBase data, DbDataReader reader, TableDescription tableDescription)
        {
            var i = 1;
            foreach (var column in tableDescription.Columns.OrderBy(x => !x.IsKey).Skip(1)) //Key always needs to be the first variable
            {
                object value = null;
                if (column.IsReference)
                {
                    value = reader.GetString(i);
                }
                else if (column.Type == typeof(string))
                {
                    value = reader.GetString(i);
                }
                else if (column.Type == typeof(int))
                {
                    value = reader.GetInt32(i);
                }
                else if (column.Type == typeof(Int64))
                {
                    value = reader.GetInt64(i);
                }
                else if (column.Type == typeof(bool))
                {
                    value = reader.GetInt32(i) != 0;
                }
                else if (column.Type == typeof(DateTime))
                {
                    value = reader.GetDateTime(i);
                }
                else if (column.Type == typeof(TimeSpan))
                {
                    var raw = reader.GetInt32(i);
                    value = TimeSpan.FromSeconds(raw);
                }
                else if (column.Type.IsEnum)
                {
                    var raw = reader.GetInt32(i);
                    value = Enum.Parse(column.Type, raw.ToString());
                }

                if (column.IsReference)
                {
                    var managedReference = data.GetType().GetProperty(column.Name).GetValue(data);
                    managedReference.GetType().GetProperty(nameof(Model.Base.ManagedReference<ManagedObjectBase, ManagedObjectBase>.Key)).SetValue(managedReference, value);
                }
                else
                {
                    data.GetType().GetProperty(column.Name).SetValue(data, value);
                }

                i++;
            }
        }

    }
}
