using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElementSuite.Common.Interface
{
    /// <summary>
    /// Service that provides a resource store that is specific to the assembly that requests the store.
    /// </summary>
    public interface IResourceService : IService, IDisposable
    {
        /// <summary>
        /// Retrieve a resource store for the requesting assembly.
        /// </summary>
        /// <param name="evidence">Any type from the requesting assembly.</param>
        /// <returns>Resource store for the requesting assembly</returns>
        IResourceStore RetrieveResourceStore (Type evidence);
    }
}
