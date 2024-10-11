using System;

namespace Diamond.Jobs.Abstract
{
    public class JobResult<TIdentifier> : Entity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        public virtual object? JsonResult { get; set; }
    }
}