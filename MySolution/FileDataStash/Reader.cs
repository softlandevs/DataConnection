using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataStash
{
    public class Reader<T> : FileHandlerBase
    {
        public Reader(StashDetails details) : base(details)
        {
        }

        public T Read()
        {
            var sdata = ReadFromFile();
            T data = Deserialize(sdata);
            return data;
        }

        private string ReadFromFile()
        {
            if (!CheckDirectoryExsists())
            {
                throw new Exception($"Directory not found: {_details.DirectoryPath}");
            }
            if (!CheckFileExsists())
            {
                throw new Exception($"File not found: {_details.FullFilePath}");
            }

            var data = System.IO.File.ReadAllText(_details.FullFilePath);
            return data;
        }
        
        private T Deserialize(string sdata)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(sdata);
        }
    }
}
