using Diamond.Jobs.Contracts;
using System.Threading.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs
{
    public class BackgroundJobQueue<TJobItem> : IBackgroundJobQueue<TJobItem>
        where TJobItem : JobItem
    {
        public virtual int DefaultChannelCapacity { get; private init; }

        protected virtual Channel<TJobItem> QueueChannel { get; private init; }

        public BackgroundJobQueue()
        {
            this.DefaultChannelCapacity = 10;
            this.QueueChannel = Channel.CreateBounded<TJobItem>(CreateChannelOptions());
        }

        public virtual async ValueTask WriteJobAsync(TJobItem jobItem, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (jobItem == null)
            {
                return;
            }

            await this.QueueChannel.Writer.WriteAsync(jobItem, cancellationToken);
        }

        public virtual async ValueTask<TJobItem> ReadJobAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await this.QueueChannel.Reader.ReadAsync(cancellationToken);
        }

        protected virtual BoundedChannelOptions CreateChannelOptions(int? channelCapacity = default) =>
            new BoundedChannelOptions(channelCapacity ?? this.DefaultChannelCapacity) { FullMode = BoundedChannelFullMode.Wait };
    }
}