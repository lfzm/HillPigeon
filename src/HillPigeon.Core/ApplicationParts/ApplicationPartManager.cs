using HillPigeon.ApplicationModels;
using System.Collections.Generic;

namespace HillPigeon.ApplicationParts
{
    public class ApplicationPartManager : IApplicationPartManager
    {
        private readonly List<ApplicationPart> applicationParts = new List<ApplicationPart>();
        public ApplicationPartManager()
        {
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
        public IReadOnlyList<ApplicationPart> ApplicationParts => applicationParts;

        /// <summary>
        /// Add ApplicationPart
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public IApplicationPartManager AddApplicationPart(ApplicationPart part)
        {
            if (!this.applicationParts.Contains(part)) this.applicationParts.Add(part);
            return this;
        }
    }
}
