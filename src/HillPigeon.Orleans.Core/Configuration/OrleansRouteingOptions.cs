using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.Orleans.Core.Configuration
{
    public class OrleansRouteingOptions
    {
        public Func<ControllerModel, string> ControllerNameRuleFunction { get; set; }
    }
}
