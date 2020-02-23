using System;

namespace BackendManager.Sync
{
    internal partial class TableBuilder
    {
        internal class ColumnDescription
        {
            public string Name { get; set; }
            public string SqlType { get; set; }
            public Type Type { get; set; }
            public bool IsKey { get; set; }
            public bool IsReference { get; set; }

        }


    }
}
