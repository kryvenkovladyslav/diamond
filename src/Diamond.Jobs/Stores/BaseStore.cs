using System;
using System.ComponentModel;

namespace Diamond.Jobs
{
    public abstract class BaseStore<TIdentifier> : IDisposable
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected bool IsDisposed { get; private set; }

        public BaseStore(bool isDisposed) => this.IsDisposed = isDisposed;

        public virtual string? ConvertIdentifierToString(TIdentifier identifier) =>
            TypeDescriptor.GetConverter(typeof(string)).ConvertToInvariantString(identifier);

        public virtual void Dispose() => this.IsDisposed = true;

        public virtual TIdentifier? ConvertIdentifierFromString(string identifier)
        {
            if (identifier == null) return default;

            return (TIdentifier?)TypeDescriptor.GetConverter(typeof(TIdentifier)).ConvertFromInvariantString(identifier);
        }

        protected virtual void ThrowIfDisposed()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}