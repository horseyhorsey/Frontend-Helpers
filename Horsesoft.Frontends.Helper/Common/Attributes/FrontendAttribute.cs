using Horsesoft.Frontends.Helper.Model;
using System;

namespace Horsesoft.Frontends.Helper.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class FrontendAttribute : Attribute
    {
        public FrontendType FrontendType;

        public FrontendAttribute(FrontendType frontendType = FrontendType.Hyperspin)
        {
            FrontendType = frontendType;
        }
    }
}
