using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HillPigeon.ApplicationModels
{
    public interface IActionModelConvention
    {
        void Apply(ActionModel actionModel);
    }
}
