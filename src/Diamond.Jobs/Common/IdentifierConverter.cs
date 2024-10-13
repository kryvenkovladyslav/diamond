using System;
using System.ComponentModel;

namespace Diamond.Jobs
{
    internal static class IdentifierConverter
    {
        public static TIdentifier? ConvertIdentifierFromString<TIdentifier>(string identifier)
            where TIdentifier : IEquatable<TIdentifier>
        {
            if (identifier == null)
            {
                return default;
            }

            return (TIdentifier?)TypeDescriptor.GetConverter(typeof(TIdentifier)).ConvertFromInvariantString(identifier);
        }

        public static string? ConvertIdentifierToString<TIdentifier>(TIdentifier identifier)
            where TIdentifier : IEquatable<TIdentifier>
        {
            if (Equals(identifier, default(TIdentifier)))
            {
                return null;
            }

            return identifier.ToString();
        }
    }
}