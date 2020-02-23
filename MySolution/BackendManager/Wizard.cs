using BackendManager.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager
{
    public class Wizard
    {
        public void SetupNewBackend(BackendConfiguration config, BackendConfiguration superConfig)
        {
            CreateDataBase(config, superConfig);
            CreateNewUserWithRights(config, superConfig);
        }

        public void TeardownBackend(BackendConfiguration config, BackendConfiguration superConfig)
        {
            DropUser(config, superConfig);
            DropDataBase(config, superConfig);
        }

        public Model.ModelContainer ConnectWithBackend(BackendConfiguration config)
        {
            var model = new Model.ModelContainer();
            UpdateTableSchema(model,config);

            return model;
        }

        public void PushDataToBackend(BackendConfiguration newConfig, ModelContainer model)
        {
            var action = new Action.PushDataAction(newConfig, model);
            action.Invoke();
        }

        private async void CreateDataBase(BackendConfiguration config, BackendConfiguration superConfig)
        {
            var task = new SQL.SqlSetupDataBase()
            {
                Database = config.DataBase,
                UserID = superConfig.DataBaseUserId,
                Password = superConfig.DataBaseUserPassword,
                Port = config.Port,
                Server = config.Server
            };

            await task.Run();
        }

        private async void CreateNewUserWithRights(BackendConfiguration config, BackendConfiguration superConfig)
        {
            var task = new SQL.SqlCreateUserWithRights()
            {
                Database = config.DataBase,
                UserID = superConfig.DataBaseUserId,
                Password = superConfig.DataBaseUserPassword,
                Port = config.Port,
                Server = config.Server,
                NewUserID = config.DataBaseUserId,
                NewUserPassword = config.DataBaseUserPassword
            };

            await task.Run();
        }

        private async void DropDataBase(BackendConfiguration config, BackendConfiguration superConfig)
        {
            var task = new SQL.SqlDropDataBase()
            {
                Database = config.DataBase,
                UserID = superConfig.DataBaseUserId,
                Password = superConfig.DataBaseUserPassword,
                Port = config.Port,
                Server = config.Server
            };

            await task.Run();
        }

        private async void DropUser(BackendConfiguration config, BackendConfiguration superConfig)
        {
            var task = new SQL.SqlDropUser()
            {
                Database = config.DataBase,
                UserID = superConfig.DataBaseUserId,
                Password = superConfig.DataBaseUserPassword,
                Port = config.Port,
                Server = config.Server,
                NewUserID = config.DataBaseUserId,
                NewUserPassword = config.DataBaseUserPassword
            };

            await task.Run();
        }

        private async void UpdateTableSchema(Model.ModelContainer model, BackendConfiguration config)
        {
            var tables = new Sync.TableBuilder(model).Build();

            foreach (var table in tables)
            {
                var task = new SQL.SqlCreateTable()
                {
                    Database = config.DataBase,
                    UserID = config.DataBaseUserId,
                    Password = config.DataBaseUserPassword,
                    Port = config.Port,
                    Server = config.Server,
                    TableDescription = table
                };
                
                await task.Run();
            }
        }

    }
}
