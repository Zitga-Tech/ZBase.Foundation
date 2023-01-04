using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZBase.Foundation
{
    public static class Enum<T>
        where T : unmanaged, Enum
    {
        private static readonly Type s_type;
        private static readonly Type s_underlyingType;
        private static readonly T[] s_values;
        private static readonly string[] s_names;

        private static Dictionary<T, string> s_nameMap;

        static Enum()
        {
            s_type = TypeCache.Get<T>();
            s_underlyingType = Enum.GetUnderlyingType(s_type);
            s_values = (T[])Enum.GetValues(s_type);
            s_names = Enum.GetNames(s_type);

            Init();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            var nameMap = s_nameMap ??= new Dictionary<T, string>();
            var values = s_values;
            var names = s_names;
            var length = values.Length;

            nameMap.Clear();

            for (var i = 0; i < length; i++)
            {
                nameMap[values[i]] = names[i];
            }
        }

        public static Type UnderlyingType => s_underlyingType;

        public static ReadOnlyMemory<T> Values => s_values;

        public static ReadOnlyMemory<string> Names => s_names;

        public static int ValueCount => s_values.Length;

        public static string GetName(T value)
        {
            if (s_nameMap.TryGetValue(value, out var name))
            {
                return name;
            }

            return string.Empty;
        }

        public static bool TryGetName(T value, out string name)
            => s_nameMap.TryGetValue(value, out name);
    }
}
