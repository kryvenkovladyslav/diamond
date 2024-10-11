using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs.Contracts
{
    public interface IBackgroundJobQueue<TJobItem> where TJobItem : class
    {
        public ValueTask WriteJobAsync(TJobItem jobItem, CancellationToken cancellationToken = default);

        public ValueTask<TJobItem> ReadJobAsync(CancellationToken cancellationToken = default);
    }
}