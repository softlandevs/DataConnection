using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInfoFramework.Annotation
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ManagedClassAttribute : Attribute
    {

    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public abstract class ManagedPropertyAttribute : Attribute
    {
        public virtual int Priority { get; }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedKeyPropertyAttribute : ManagedPropertyAttribute
    {
        public override int Priority => 0;
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedVersionPropertyAttribute : ManagedPropertyAttribute
    {
        public override int Priority => 1;
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedReferencePropertyAttribute : ManagedPropertyAttribute
    {
        public override int Priority => 2;
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedStringPropertyAttribute : ManagedPropertyAttribute
    {
        public ManagedStringPropertyAttribute(bool largeString = false)
        {
            this.LargeString = largeString;
        }

        public bool LargeString { get; }
        public override int Priority => 5;
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedIntPropertyAttribute : ManagedPropertyAttribute
    {
        public ManagedIntPropertyAttribute(bool longInt = false)
        {
            this.LongInt = longInt;
        }

        public bool LongInt { get; }
        public override int Priority => 5;
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedBoolPropertyAttribute : ManagedPropertyAttribute
    {
        public override int Priority => 5;
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedDatePropertyAttribute : ManagedPropertyAttribute
    {
        public override int Priority =>5;
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedTimeSpanPropertyAttribute : ManagedPropertyAttribute
    {
        public override int Priority => 5;
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ManagedEnumPropertyAttribute : ManagedPropertyAttribute
    {
        public override int Priority => 5;
    }
}
