using BackendManager.Configuration;
using BackendManager.SQL;
using Model;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendManager.Sync
{
    internal partial class TableBuilder
    {
        public ModelContainerBase Model { get; }

        public TableBuilder(ModelContainerBase model)
        {
            Model = model;
        }

        public List<TableDescription> Build()
        {
            var managedClassTypes = Model.GetAllManagedClassTypes();
            var tables = new List<TableDescription>();

            foreach (var managedClassType in managedClassTypes)
            {
                tables.Add(BuildTable(managedClassType));
            }

            return tables;                        
        }

        private TableDescription BuildTable(Type managedClassType)
        {
            var metaModel = new ManagedMetaObjectFactory().CreateMetaObject(managedClassType);

            var table = new TableDescription() { Name = metaModel.Name };
            foreach (var metaProp in metaModel.Properties)
            {
                table.Columns.Add(new ColumnDescription()
                {
                    Name = metaProp.Name,
                    Type = metaProp.Type,
                    SqlType = GetSqlColumnType(metaProp),
                    IsKey = metaProp.IsKey,
                    IsReference = metaProp.IsReference
                });
            }

            return table;
        }

        private string GetSqlColumnType(ManagedMetaProperty metaProp)
        {
            if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedKeyPropertyAttribute)
            {
                return "VARCHAR(50)";
            }
            else if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedVersionPropertyAttribute)
            {
                return "BIGINT";
            }
            else if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedReferencePropertyAttribute)
            {
                return "VARCHAR(50)";
            }
            else if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedBoolPropertyAttribute)
            {
                return "INT";
            }
            else if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedDatePropertyAttribute)
            {
                return "DATETIME";
            }
            else if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedTimeSpanPropertyAttribute)
            {
                return "INT";
            }
            else if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedEnumPropertyAttribute)
            {
                return "INT";
            }
            else if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedIntPropertyAttribute)
            {
                return (metaProp.Attribute as DataInfoFramework.Annotation.ManagedIntPropertyAttribute).LongInt ? "BIGINT" : "INT";
            }
            else if (metaProp.Attribute is DataInfoFramework.Annotation.ManagedStringPropertyAttribute)
            {
                return (metaProp.Attribute as DataInfoFramework.Annotation.ManagedStringPropertyAttribute).LargeString ? "TEXT" : "VARCHAR(200)";
            }

            throw new Exception("Unknown Type for Sql Column: " + metaProp.Type.ToString());
        }

    }
}
