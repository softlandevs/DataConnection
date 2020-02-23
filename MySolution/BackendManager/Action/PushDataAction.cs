using BackendManager.Configuration;
using Model;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.Action
{
    public class PushDataAction
    {
        public PushDataAction(BackendConfiguration newConfig, ModelContainer model)
        {
            Config = newConfig;
            Model = model;
        }

        public BackendConfiguration Config { get; }
        public ModelContainer Model { get; }

        public async void Invoke()
        {
            var managedClassesDescription = Model.GetAllManagedClasses();
            var tables = new Sync.TableBuilder(Model).Build();

            foreach (var classEntry in managedClassesDescription)
            {
                var dictionarywithData = classEntry.Key.GetValue(Model);
                var activeTable = tables.First(x => x.Name == classEntry.Value.Name);
                var data = classEntry.Key.PropertyType.GetProperty("Values").GetValue(dictionarywithData) as IEnumerable<ManagedObjectBase>;
                var metaModel = new ManagedMetaObjectFactory().CreateMetaObject(classEntry.Value);

                var task = new SQL.SqlPushData()
                {
                    Database = Config.DataBase,
                    UserID = Config.DataBaseUserId,
                    Password = Config.DataBaseUserPassword,
                    Port = Config.Port,
                    Server = Config.Server,
                    DataSet = data.ToList(),
                    MetaObjectDescriptor = metaModel,
                    TableDescription = activeTable,
                    ModelContainer = Model
                };

                await task.Run();
            }
        }
    }
}
