using System;
using System.Threading.Tasks;

namespace Diamond.Jobs.Contracts
{
    public interface IExecutableJob : IDisposable
    {
        public Task<object?> ExecuteAsync();
    }
}