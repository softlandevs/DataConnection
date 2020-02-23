using System.Collections.Generic;

namespace BackendManager.Sync
{
    internal class TableDescription
    {
        public string Name { get; set; }

        public List<ColumnDescription> Columns { get; } = new List<ColumnDescription>();
    }
}
