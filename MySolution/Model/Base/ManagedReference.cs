using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Base
{
    public class ManagedReference<Onwer, Target> where Onwer : ManagedObjectBase where Target : ManagedObjectBase
    {
        protected Target _cachedTarget;

        public string Key { get; set; }

        public Target Value
        {
            get
            {
                return _cachedTarget;
            }
            set
            {
                Key = value.Key;
                _cachedTarget = value;
            }
        }

        public void LoadConnectedObject(ModelContainer modelContainer)
        {
            if (!string.IsNullOrEmpty(Key) && (_cachedTarget == null || _cachedTarget.Key != Key))
            {
                Dictionary<string, Target> store = modelContainer.GetStore<Target>();
                if (store == null) return;

                store.TryGetValue(Key, out _cachedTarget);
            }
        }

    }
}
