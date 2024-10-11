using Diamond.Jobs.Abstract;
using Diamond.Jobs.DataAccess.Resources;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace Diamond.Jobs.DataAccess
{
    internal sealed class JobEntityConfiguration<TJob, TIdentifier> : IEntityTypeConfiguration<TJob>
        where TJob : Job<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        public void Configure(EntityTypeBuilder<TJob> builder)
        {
            var jobTable = builder.ToTable(JobTableResources.Table);
            
            jobTable.HasKey(job => job.Identifier);

            jobTable
                .Property(job => job.Identifier)
                .HasColumnName(JobTableResources.Identifier)
                .IsRequired();

            jobTable
                .Property(job => job.RequestID)
                .HasDefaultValueSql(SqlResources.NewID)
                .HasColumnName(JobTableResources.RequestIdentifier)
                .IsRequired();

            jobTable
                .Property(job => job.Status)
                .HasDefaultValue(JobStatus.Started)
                .HasColumnName(JobTableResources.Status)
                .IsRequired();

            jobTable
                .Property(job => job.CreatedAt)
                .HasDefaultValueSql(SqlResources.GetDate)
                .HasColumnName(JobTableResources.CreatedAt)
                .IsRequired();
        }
    }
}