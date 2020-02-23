using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ModelContainerBase
    {
        public Dictionary<PropertyInfo, Type> GetAllManagedClasses()
        {
            var managedObjectType = typeof(Base.ManagedObjectBase);
            var managedClasses = new Dictionary<PropertyInfo, Type>();

            var propertyInfos = this.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                Type t = propertyInfo.PropertyType;
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    //This is a dictionary
                    var valueTyp = t.GetGenericArguments()[1];
                    if (valueTyp.IsSubclassOf(managedObjectType))
                    {
                        Attribute[] attrs = System.Attribute.GetCustomAttributes(valueTyp);
                        if (attrs.Any(x => x is DataInfoFramework.Annotation.ManagedClassAttribute))
                        {
                            managedClasses.Add(propertyInfo, valueTyp);
                        }
                    }
                }
            }

            return managedClasses;
        }

        public List< Type> GetAllManagedClassTypes()
        {
            return GetAllManagedClasses()?.Values.ToList();
        }

    }
}