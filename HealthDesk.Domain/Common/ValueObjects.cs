using System.Text.RegularExpressions;

namespace HealthDesk.Domain.Common;

public abstract class ValueObjects
{
    protected static bool EqualOperator(ValueObjects? left, ValueObjects? right)
        => ReferenceEquals(left, right) || (left is not null && left.Equals(right));

    protected static bool NotEqualOperator(ValueObjects? left, ValueObjects? right)
        => !(EqualOperator(left, right));

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public sealed class Email : ValueObjects
    {
         public string Address { get; }

        private static readonly Regex _emailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email cannot be empty.", nameof(address));

            address = address.Trim().ToLowerInvariant();

            if (!_emailRegex.IsMatch(address))
                throw new ArgumentException("Invalid email format.", nameof(address));

            Address = address;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Address;
        }

        public override string ToString() => Address;
    
    }
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;

        var other = (ValueObjects)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
        => GetEqualityComponents()
            .Aggregate(0, (hash, obj) => HashCode.Combine(hash, obj?.GetHashCode() ?? 0));
}
