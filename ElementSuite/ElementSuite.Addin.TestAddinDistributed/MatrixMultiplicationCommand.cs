using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementSuite.Addin.TestDistributed
{
    [Export(typeof(IWorkCommand))]
    public class MatrixMultiplicationCommand : IWorkCommand
    {
        public IWorkResult Execute(IWorkItem workItem)
        {
            var workResult = new MatrixMultiplicationWorkResult();
            workResult.Success = false;

            try
            {
                var concreteWorkItem = workItem as MatrixMultiplicationWorkItem;

                if (concreteWorkItem != null)
                {
                    workResult.Result = concreteWorkItem.GridAValue * concreteWorkItem.GridBValue;
                    workResult.Success = true;
                }
                else
                {
                    workResult.Error = new ArgumentNullException(string.Concat("The workItem was either null or could not be cast to a instance of ", typeof(MatrixMultiplicationWorkItem).Name));
                }

            }
            catch (Exception e)
            {
                workResult.Error = e;
            }

            return workResult;
        }

        void ParalleledMatrixMultiplicationMS(int[,] a, int[,] b, int[,] c)
        {
            int s = a.GetLength(0);

            Parallel.For(0, s, delegate(int i)
            {
                for (int j = 0; j < s; j++)
                {
                    int v = 0;

                    for (int k = 0; k < s; k++)
                    {
                        v += a[i, k] * b[k, j];
                    }

                    c[i, j] = v;
                }
            });
        }
    }
}
