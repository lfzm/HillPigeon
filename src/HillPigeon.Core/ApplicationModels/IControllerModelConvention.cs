using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.ApplicationModels
{
    public interface IControllerModelConvention
    {
        void Apply(ControllerModel controllerModel);
    }
}
