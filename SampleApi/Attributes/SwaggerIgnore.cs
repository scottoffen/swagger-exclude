using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class SwaggerIgnoreAttribute : Attribute { }
}