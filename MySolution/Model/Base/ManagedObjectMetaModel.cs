using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public class ManagedMetaObject
    {
        public ManagedMetaObject()
        {
            Properties = new List<ManagedMetaProperty>();
        }

        public string Name { get; set; }
        public Type Type { get; set; }

        public List<ManagedMetaProperty> Properties { get; }
    }

    public class ManagedMetaProperty
    {
        public ManagedMetaProperty(ManagedMetaObject owner,Type type,DataInfoFramework.Annotation.ManagedPropertyAttribute attribute)
        {
            Owner = owner;
            Type = type;
            IsReference = false;
            IsKey = false;
            Attribute = attribute;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsReference { get; set; }
        public bool IsKey { get; set; }
        public ManagedMetaObject Owner { get; }
        public DataInfoFramework.Annotation.ManagedPropertyAttribute Attribute { get; }
    }
}
