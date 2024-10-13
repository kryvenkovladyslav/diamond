using Diamond.Jobs.Contracts;
using System;

namespace Diamond.Jobs
{
    public class JobItem : IDisposable
    {
        protected bool IsDisposed { get; private set; }

        public virtual string Identifier { get; private init; }

        public virtual IExecutableJob ExecutionPlan { get; private init; }

        public JobItem(string identifier, IExecutableJob executionPlan)
        {
            this.IsDisposed = false;
            this.Identifier = identifier;
            this.ExecutionPlan = executionPlan ?? throw new ArgumentNullException(nameof(executionPlan));
        }

        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.ExecutionPlan.Dispose();
            this.IsDisposed = true;
        }
    }
}