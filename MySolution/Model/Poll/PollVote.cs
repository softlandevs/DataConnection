using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Poll
{
    [ManagedClass()]
    public class PollVote : Base.ManagedObjectBase
    {
        [ManagedReferenceProperty()]
        public Base.ManagedReference<PollVote, Poll> Poll { get; set; }

        [ManagedReferenceProperty()]
        public Base.ManagedReference<PollVote, Personal.User> Voter { get; set; }

        [ManagedReferenceProperty()]
        public Base.ManagedReference<PollVote, PollOption> Option { get; set; }
    }
}
