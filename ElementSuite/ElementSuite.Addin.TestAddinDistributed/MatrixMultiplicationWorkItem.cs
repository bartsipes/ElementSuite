using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElementSuite.Addin.TestDistributed
{
    [Serializable]
    public class MatrixMultiplicationWorkItem : IWorkItem
    {
        public MatrixMultiplicationWorkItem()
        {
            Id = Guid.NewGuid();
        }

        public int GridAValue { get; set; }
        public int GridBValue { get; set; }

        public Guid Id { get; private set; }
    }
}
