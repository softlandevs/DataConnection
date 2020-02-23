using BackendManager.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTestConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunTestSql();
            //RunConfigTest();
            RunNewBackendTest();
        }

        private async static void RunTestSql()
        {
            await MySqlCreate.Main(new[] { "" });
        }

        private static void RunConfigTest()
        {
            var manager = new ConfigurationManager();
            
            var config = manager.CreateDefaultConfiguration();

            manager.SaveBackendConfiguration(config);

            var loadedConfig = manager.LoadBackendConfiguration();

            Console.WriteLine("Original");
            Console.WriteLine(config.ToString());
            Console.WriteLine("");
            Console.WriteLine("Loaded");
            Console.WriteLine(loadedConfig.ToString());
        }

        private static void RunNewBackendTest()
        {
            var myWizard = new BackendManager.Wizard();

            var newConfig = new ConfigurationManager().CreateDefaultConfiguration();
            newConfig.Server = "xxx.xxx.xxx.xxx";
            newConfig.Port = 3306;

            var superConfig = new BackendConfiguration()
            {
                DataBaseUserId = "xxxxuser",
                DataBaseUserPassword = "************"
            };

            myWizard.TeardownBackend(newConfig,superConfig);
            myWizard.SetupNewBackend(newConfig,superConfig);
            var model = myWizard.ConnectWithBackend(newConfig);

            CreateTestModel(model);

            myWizard.PushDataToBackend(newConfig,model);

            UpdateTestModel(model);

            myWizard.PushDataToBackend(newConfig,model);
        }

        private static void CreateTestModel(ModelContainer model)
        {
            var user = new Model.Personal.User() { Token = "test@user.com", DisplayName = "Master 1" };
            var state = new Model.Personal.UserStatus() { User = new Model.Base.ManagedReference<Model.Personal.UserStatus, Model.Personal.User>() { Value = user } };
            model.Users.Add(user.Key, user);
            model.UserStatus.Add(state.Key, state);

            user = new Model.Personal.User() { Token = "assFace@user.com", DisplayName = "Master 2" };
            model.Users.Add(user.Key, user);

            user = new Model.Personal.User() { Token = "Donkey@user.com", DisplayName = "Master Blood" };
            model.Users.Add(user.Key, user);

            user = new Model.Personal.User() { Token = "mssah@user.com", DisplayName = "Master Maash" };
            model.Users.Add(user.Key, user);
        }

        private static void UpdateTestModel(ModelContainer model)
        {
            model.Users.First().Value.DisplayName += " FuckYou";
        }
    }
}
