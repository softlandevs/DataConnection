using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataStash
{
    public  abstract class FileHandlerBase
    {
        protected readonly StashDetails _details;

        public FileHandlerBase(StashDetails details)
        {
            _details = details;
        }

        protected bool CheckDirectoryExsists()
        {
            return System.IO.Directory.Exists(_details.DirectoryPath);
        }

        protected bool CheckFileExsists()
        {
            return System.IO.File.Exists(_details.FullFilePath);
        }

    }
}
