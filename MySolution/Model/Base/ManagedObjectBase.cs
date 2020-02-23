using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public class ManagedObjectBase
    {
        public ManagedObjectBase()
        {
            Key = new Guid().ToString();
            Created = DateTime.Now;
            LastModified = DateTime.Now;
            Verison = -1;
        }

        [ManagedKeyProperty]
        public string Key { get; set; }

        [ManagedVersionProperty]
        public long Verison { get; set; }

        [ManagedDateProperty]
        public DateTime Created { get; set; }

        [ManagedDateProperty]
        public DateTime LastModified { get; set; }

    }
}
