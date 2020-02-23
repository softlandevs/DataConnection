using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Infrastructure
{
    [ManagedClass()]
    public class Config : Base.ManagedObjectBase
    {
        [ManagedIntProperty]
        public int MaxNumberOfNextGamePreferences { get; set; }
    }
}
