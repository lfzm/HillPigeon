using HillPigeon.ApplicationModels;
using System.Collections.Generic;

namespace HillPigeon.ApplicationParts
{
    public class HillPigeonApplicationPartManager
    {
        public HillPigeonApplicationPartManager()
        {
            ApplicationParts = new List<ApplicationPart>();
        }
        /// <summary>
        /// Gets the list of <see cref="ApplicationPart"/> instances.
        /// <para>
        /// Instances in this collection are stored in precedence order. An <see cref="ApplicationPart"/> that appears
        /// earlier in the list has a higher precedence.
        /// An <see cref="IApplicationFeatureProvider"/> may choose to use this an interface as a way to resolve conflicts when
        /// multiple <see cref="ApplicationPart"/> instances resolve equivalent feature values.
        /// </para>
        /// </summary>
        public IList<ApplicationPart> ApplicationParts { get; } 

       
    }
}
