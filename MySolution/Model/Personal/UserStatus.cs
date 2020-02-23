using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Personal
{
    [ManagedClass]
    public class UserStatus : Base.ManagedObjectBase
    {
        [ManagedReferenceProperty()]
        public Base.ManagedReference<UserStatus, User> User { get; set; }
    }
}
