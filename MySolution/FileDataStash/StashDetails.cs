using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDataStash
{
    public class StashDetails
    {
        public string DirectoryPath { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string FullFilePath           
        {
            get { return DirectoryPath + FileName + "." + Extension; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var data = value.Split('.');
                    var extension = data.Last();
                    var startOfFileName = value.LastIndexOf("/");
                    var path = value.Substring(0, startOfFileName);
                    var name = value.Substring(value.LastIndexOf("/")+ 1);
                    name = name.Substring(0,name.LastIndexOf("."));

                    if(string.IsNullOrEmpty(extension))
                    {
                        throw new ArgumentException($"Filename {value} has no extension.");
                    }
                    else if(string.IsNullOrEmpty(path))
                    {
                        throw new ArgumentException($"Path {value} has no identifyable path.");
                    }
                    else if(string.IsNullOrEmpty(name))
                    {
                        throw new ArgumentException($"Path {value} has no filename.");
                    }

                    DirectoryPath = path;
                    FileName = name;
                    Extension =extension;
                }
            }
        }

    }
}
