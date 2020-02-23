using BackendManager.Configuration;
using BackendManager.Sync;
using Model;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.Action
{
    internal class LoadDataAction
    {
        public LoadDataAction(BackendConfiguration newConfig, ModelContainer model)
        {
            Config = newConfig;
            Model = model;
        }

        public BackendConfiguration Config { get; }
        public ModelContainer Model { get; }

        public async void Invoke()
        {
            var managedClassPropertyAndTypeMap = Model.GetAllManagedClasses();
            var tables = new Sync.TableBuilder(Model).Build();
            var maxVersionNumber = Model.VersionHead;

            foreach (var managedClassEntry in managedClassPropertyAndTypeMap)
            {
                var dictionarywithData = managedClassEntry.Key.GetValue(Model);
                var activeTable = tables.First(x => x.Name == managedClassEntry.Value.Name);
                var data = managedClassEntry.Key.PropertyType.GetProperty("Values").GetValue(dictionarywithData) as IEnumerable<ManagedObjectBase>;
                var classMeta = new ManagedMetaObjectFactory().CreateMetaObject(managedClassEntry.Value);

                var task = new SQL.SqlLoadData()
                {
                    Database = Config.DataBase,
                    UserID = Config.DataBaseUserId,
                    Password = Config.DataBaseUserPassword,
                    Port = Config.Port,
                    Server = Config.Server,
                    DataSet = data.ToList(),
                    MetaObjectDescriptor = classMeta,
                    TableDescription = activeTable,
                    ModelContainer = Model,
                    DataStore = dictionarywithData
                };

                var readVersionHead = await task.Run();
                maxVersionNumber = Math.Max(maxVersionNumber, readVersionHead);
            }

            ReLoadReferences(Model, managedClassPropertyAndTypeMap, tables);
            Model.VersionHead = Math.Max(Model.VersionHead, maxVersionNumber);
        }

        private void ReLoadReferences(ModelContainer model, Dictionary<System.Reflection.PropertyInfo, Type> managedClassPropertyAndTypeMap, List<TableDescription> tables)
        {

            foreach (var managedClassEntry in managedClassPropertyAndTypeMap)
            {
                var dictionarywithData = managedClassEntry.Key.GetValue(Model);
                var activeTable = tables.First(x => x.Name == managedClassEntry.Value.Name);
                var data = managedClassEntry.Key.PropertyType.GetProperty("Values").GetValue(dictionarywithData) as IEnumerable<ManagedObjectBase>;
                var classMeta = new ManagedMetaObjectFactory().CreateMetaObject(managedClassEntry.Value);

                foreach (var instance in data)
                {
                    instance.ReloadConnections(classMeta, model);
                }
            }
        }
    }
}
