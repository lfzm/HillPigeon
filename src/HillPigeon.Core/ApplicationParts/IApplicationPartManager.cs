using System;
using System.Collections.Generic;
using System.Text;

namespace HillPigeon.ApplicationParts
{
   public interface IApplicationPartManager
    {
        /// <summary>
        /// Gets the list of <see cref="ApplicationPart"/>s.
        /// </summary>
        IReadOnlyList<ApplicationPart> ApplicationParts { get; }

        /// <summary>
        /// Adds an application part.
        /// </summary>
        /// <param name="part">The application part.</param>
        IApplicationPartManager AddApplicationPart(ApplicationPart part);
    }
}
