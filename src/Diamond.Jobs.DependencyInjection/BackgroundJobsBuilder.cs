using Microsoft.Extensions.DependencyInjection;
using System;

namespace Diamond.Jobs.DependencyInjection
{
    public sealed class BackgroundJobsBuilder
    {
        public Type JobType { get; init; } = default!;

        public Type JobResultType { get; init; } = default!;

        public Type JobIdentifierType { get; init; } = default!;

        public IServiceCollection Services { get; init; } = default!;
    }
}