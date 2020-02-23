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
            Key = Guid.NewGuid().ToString();
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

        public void ReloadConnections(ManagedMetaObject classMeta, ModelContainer modelContainer)
        {
            foreach(var referenceProperty in classMeta.Properties.Where(x => x.IsReference))
            {
                var propertyInfo = classMeta.Type.GetProperty(referenceProperty.Name);
                var property = propertyInfo.GetValue(this);
                var methodInfo = referenceProperty.Type.GetMethod(nameof(ManagedReference<ManagedObjectBase, ManagedObjectBase>.LoadConnectedObject));
                var method = methodInfo.Invoke(property, new []{ modelContainer});
            }
        }

    }
}
