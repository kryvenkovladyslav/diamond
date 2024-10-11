using Diamond.Jobs.Abstract;
using Diamond.Jobs.DataAccess.Resources;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace Diamond.Jobs.DataAccess
{
    internal sealed class JobResultEntityConfiguration<TJobResult, TJob, TIdentifier> : IEntityTypeConfiguration<TJobResult>
        where TJob : Job<TIdentifier>
        where TJobResult : JobResult<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        public void Configure(EntityTypeBuilder<TJobResult> builder)
        {
            var jobResultTable = builder.ToTable(JobResultTableResources.Table);

            jobResultTable.HasKey(job => job.Identifier);

            jobResultTable
                .Property(job => job.Identifier)
                .HasColumnName(JobResultTableResources.Identifier)
                .IsRequired();

            jobResultTable.HasOne<TJob>().WithOne()
                .HasForeignKey<TJobResult>(result => result.Identifier)
                .HasPrincipalKey<TJob>(job => job.Identifier);

            var jsonSerializerSettings = CreateJsonSerializerSettings();

            jobResultTable
                .Property(job => job.JsonResult)
                .HasColumnName(JobResultTableResources.Result)
                .HasConversion(
                    obj => JsonConvert.SerializeObject(obj, jsonSerializerSettings),
                    obj => JsonConvert.DeserializeObject(obj, jsonSerializerSettings));
        }

        private JsonSerializerSettings CreateJsonSerializerSettings() => new JsonSerializerSettings
        {
            MaxDepth = int.MaxValue,
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };  
    }
}