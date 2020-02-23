using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Personal
{
    [ManagedClass]
    public class User : Base.ManagedObjectBase
    {
        [ManagedStringProperty]
        public string Token { get; set; }

        [ManagedStringProperty]
        public string DisplayName { get; set; }
    }
}
