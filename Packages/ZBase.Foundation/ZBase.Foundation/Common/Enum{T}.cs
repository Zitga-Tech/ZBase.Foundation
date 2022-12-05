using System;

namespace ZBase.Foundation
{
    public static class Enum<T>
        where T : unmanaged, System.Enum
    {
        private static readonly System.Type s_type;
        private static readonly System.Type s_underlyingType;
        private static readonly T[] s_values;
        private static readonly string[] s_names;

        static Enum()
        {
            s_type = TypeCache.Get<T>();
            s_underlyingType = System.Enum.GetUnderlyingType(s_type);
            s_values = (T[])System.Enum.GetValues(s_type);
            s_names = System.Enum.GetNames(s_type);
        }

        public static System.Type UnderlyingType => s_underlyingType;

        public static ReadOnlyMemory<T> Values => s_values;

        public static ReadOnlyMemory<string> Names => s_names;

        public static int ValueCount => s_values.Length;
    }
}
