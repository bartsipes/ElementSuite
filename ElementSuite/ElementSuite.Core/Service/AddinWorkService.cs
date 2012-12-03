using ElementSuite.Addin.Interface;
using ElementSuite.Common.Interface;
using ElementSuite.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementSuite.Core.Service
{
    internal class AddinWorkService : IWorkService
    {
        public IAddinWorkQueue<TWorkItem, TWorkResult> CreateWorkQueue<TWorkItem, TWorkResult>()
            where TWorkItem : IWorkItem
            where TWorkResult : IWorkResult
        {
            return new DistributedAddinWorkQueue<TWorkItem, TWorkResult>();
        }
    }
}
