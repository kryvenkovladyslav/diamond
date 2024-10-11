using Diamond.Jobs.Abstract;
using Microsoft.EntityFrameworkCore;
using System;

namespace Diamond.Jobs.DataAccess
{
    public class JobDatabaseContext<TJob, TJobResult, TIdentifier> : DbContext
        where TJob : Job<TIdentifier>
        where TJobResult : JobResult<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        public JobDatabaseContext() { }

        public JobDatabaseContext(DbContextOptions<JobDatabaseContext<TJob, TJobResult, TIdentifier>> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JobEntityConfiguration<TJob, TIdentifier>());
            modelBuilder.ApplyConfiguration(new JobResultEntityConfiguration<TJobResult, TJob, TIdentifier>());
        }
    }
}