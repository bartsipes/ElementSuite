using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElementSuite.Addin.TestDistributed
{
    [Serializable]
    public class MatrixMultiplicationWorkResult : IWorkResult
    {
        public bool Success { get; set; }

        public Exception Error { get; set; }

        public int Result { get; set; }

        public Guid WorkItemId { get; set; }
    }
}
