using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataStash
{
    public class Writer : FileHandlerBase
    {
        public Writer(StashDetails details) : base(details)
        {
        }

        public void Write(object data)
        {
            var sdata = Serialize(data);
            WriteToFile(sdata);
        }

        private string Serialize(object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        private void WriteToFile(string sdata)
        {
            if (!CheckDirectoryExsists())
            {
                System.IO.Directory.CreateDirectory(_details.DirectoryPath);
            }
            if (CheckFileExsists())
            {
                System.IO.File.Delete(_details.FullFilePath);
            }

            System.IO.File.WriteAllText(_details.FullFilePath, sdata);
        }
    }
}
