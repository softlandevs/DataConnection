using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resource
{
    public class Resource : Base.ManagedObjectBase
    {
        [ManagedStringProperty]
        public string Token { get; set; }

        [ManagedEnumProperty]
        public ResourceTyp Type { get; set; }

        [ManagedEnumProperty]
        public ResourceStorageType StoreageType { get; set; }
    }

    public enum ResourceTyp
    {
        image,
        sound,
        video,
        text
    }

    public enum ResourceStorageType
    {
        database,
        file,
        url
    }
}
