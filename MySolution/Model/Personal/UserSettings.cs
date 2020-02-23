using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Personal
{
    [ManagedClass]
    public class UserSettings : Base.ManagedObjectBase
    {
        [ManagedReferenceProperty()]
        public Base.ManagedReference<UserSettings, User> User { get; set; }

        [ManagedBoolProperty]
        public bool ShowCallToArms { get; set; }
    }
}
