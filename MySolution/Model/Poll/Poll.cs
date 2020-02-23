using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Poll
{
    [ManagedClass()]
    public class Poll : Base.ManagedObjectBase
    {
        [ManagedReferenceProperty()]
        public Base.ManagedReference<Poll, Personal.User> Cerator { get; set; }

        [ManagedStringProperty]
        public string Question { get; set; }

        [ManagedStringProperty]
        public string Description { get; set; }

        [ManagedStringProperty]
        public TimeSpan RunTime { get; set; }

        [ManagedStringProperty]
        public long MaxVotesPerUser { get; set; }

    }
}
