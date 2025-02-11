using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGoogle.Development.Data
{
    public interface IProcs
    {
        void RunSPReturnReader(string proc);

        Task RunSPReturnReaderAsync(string proc);

        int RunSP(string proc);

        Task<int> RunSPAsync(string proc);

        object RunSPReturnScalar(string proc);

        Task<object> RunSPReturnScalarAsync(string proc);


    }
}
