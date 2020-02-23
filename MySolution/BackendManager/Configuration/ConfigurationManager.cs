using FileDataStash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.Configuration
{
    public class ConfigurationManager
    {
        private string ConfigPath = "/config";
        private string BackendConfig = "backend.config";

        public BackendConfiguration CreateDefaultConfiguration()
        {
            return new BackendConfiguration()
            {
                DataBase = "LanManager",
                Server = "127.0.0.1",
                Port = 3306,
                DataBaseUserId = "LanManager",
                DataBaseUserPassword = "PassW?r4dTopS3cr!t"
            };
        }

        public void SaveBackendConfiguration(BackendConfiguration config)
        {
            var writer = new Writer(GetDetailsForBackendConfig());
            writer.Write(config);
        }

        public BackendConfiguration LoadBackendConfiguration()
        {
            var reader = new Reader<BackendConfiguration>(GetDetailsForBackendConfig());
            var config = reader.Read();
            return config;
        }

        private StashDetails GetDetailsForBackendConfig()
        {
            var path = System.IO.Directory.GetCurrentDirectory();
            StashDetails details = new StashDetails() { FullFilePath = path + ConfigPath + "/" + BackendConfig };
            return details;
        }

    }
}
