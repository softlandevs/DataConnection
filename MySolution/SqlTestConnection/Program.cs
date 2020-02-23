using BackendManager.Configuration;
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
        }
    }
}
