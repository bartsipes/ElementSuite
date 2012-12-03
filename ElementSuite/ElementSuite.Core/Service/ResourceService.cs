using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElementSuite.Core.Internal;

namespace ElementSuite.Core.Service
{
    public class ResourceService : IResourceService
    {
        static ConcurrentDictionary<string, IResourceStore> resourceStores = new ConcurrentDictionary<string, IResourceStore>();

        public IResourceStore RetrieveResourceStore(Type evidence)
        {
            IResourceStore store = null;
            if (resourceStores.TryGetValue(evidence.AssemblyQualifiedName, out store) == false)
            {
                store = new AssemblyResourceStore(evidence);
                store = resourceStores.GetOrAdd(evidence.AssemblyQualifiedName, store);
            }

            return store;
        }

        public void Dispose()
        {
            lock (resourceStores)
            {
                foreach (var item in resourceStores.Values)
                {
                    item.Dispose();
                }
            }
        }
    }
}
