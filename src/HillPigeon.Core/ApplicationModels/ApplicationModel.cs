using System.Collections.Generic;

namespace HillPigeon
{
    public class ApplicationModel
    {
        public ApplicationModel()
        {
            this.Controllers = new List<ControllerModel>();
        }
        public IList<ControllerModel> Controllers { get; }
    }
}
