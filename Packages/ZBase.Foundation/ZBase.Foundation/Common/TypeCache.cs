using System;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation
{
    public static class TypeCache<T>
    {
        public static readonly Type Type = typeof(T);

        public static readonly string Name = Type.Name;

        public static readonly TypeHash Hash = Type;

        public static readonly bool IsReferenceOrContainsReference = RuntimeHelpers.IsReferenceOrContainsReferences<T>();
    }

    public static class TypeCache
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type Get<T>()
            => TypeCache<T>.Type;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is<T>(this Type type)
            => type == TypeCache<T>.Type;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeHash GetHash<T>()
            => TypeCache<T>.Hash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName<T>()
            => TypeCache<T>.Name;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type Type<T>(this object _)
            => TypeCache<T>.Type;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TypeName<T>(this object _)
            => TypeCache<T>.Name;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeHash TypeHash<T>(this object _)
            => TypeCache<T>.Hash;
    }
}
