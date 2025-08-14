using System;

namespace NullSave.GDTK
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class AutoDocSuppress : Attribute { }
}
