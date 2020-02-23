using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public class ManagedMetaObjectFactory
    {
        public ManagedMetaObject CreateMetaObject(Type source)
        {
            var metaObject = new ManagedMetaObject();

            metaObject.Name = source.Name;
            metaObject.Type = source;

            CreateProperties(source, metaObject);
            
            return metaObject;
        }

        private List<ManagedMetaProperty> CreateProperties(Type source, ManagedMetaObject metaObject)
        {
            var propList = new List<ManagedMetaProperty>();

            foreach (var property in source.GetProperties())
            {
                ManagedMetaProperty metaProp = null;
                var propertyType = property.PropertyType;

                Attribute[] attrs = System.Attribute.GetCustomAttributes(property);
                var attr = attrs.FirstOrDefault(x => typeof(DataInfoFramework.Annotation.ManagedPropertyAttribute).IsAssignableFrom(x.GetType())) as DataInfoFramework.Annotation.ManagedPropertyAttribute;
                if (attr != null)
                {
                    metaProp = new ManagedMetaProperty(metaObject, propertyType,attr)
                    {
                        Name = property.Name
                    };

                    if (attr is DataInfoFramework.Annotation.ManagedKeyPropertyAttribute)
                    {
                        metaProp.IsKey = true;
                    }
                    else if (attr is DataInfoFramework.Annotation.ManagedReferencePropertyAttribute)
                    {
                        metaProp.IsReference = true;
                    }
                }

                if (metaProp != null) propList.Add(metaProp);
            }

            metaObject.Properties.AddRange(propList.OrderBy(x => x.Attribute.Priority));

            return metaObject.Properties;
        }
    }
}
