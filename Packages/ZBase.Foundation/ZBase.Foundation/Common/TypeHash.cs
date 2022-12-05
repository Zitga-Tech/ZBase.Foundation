using System;

namespace ZBase.Foundation
{
    /// <summary>
    /// Represents the hash code of <see cref="System"/>.<see cref="Type"/>.
    /// </summary>
    public readonly struct TypeHash : IEquatable<TypeHash>
    {
        private readonly int _value;

        public TypeHash(Type type)
        {
            _value = type.GetHashCode();
        }

        public override bool Equals(object obj)
            => obj switch {
                TypeHash typeHash => _value == typeHash._value,
                int hashCode => _value == hashCode,
                Type type => _value == type.GetHashCode(),
                _ => false
            };

        public bool Equals(TypeHash other)
            => _value == other._value;

        public override int GetHashCode()
            => _value;

        public static implicit operator int(TypeHash value)
            => value._value;

        public static implicit operator TypeHash(Type type)
            => new(type);
    }
}
