using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Poll
{
    [ManagedClass()]
    public class PollOption : Base.ManagedObjectBase
    {
        [ManagedReferenceProperty()]
        public Base.ManagedReference<PollOption, Poll> Poll { get; set; }

        [ManagedReferenceProperty()]
        public Base.ManagedReference<PollOption, Resource.Resource> Option { get; set; }
    }
}
