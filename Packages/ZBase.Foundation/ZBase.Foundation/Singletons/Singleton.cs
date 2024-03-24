using System;
using UnityEngine;

namespace ZBase.Foundation.Singletons
{
    /// <summary>
    /// <para>Designed for decoupling Singleton pattern and user classes.</para>
    /// <para>Usage: <see cref="Singleton"/>.Of&lt;MyClass&gt;()</para>
    /// </summary>
    public static partial class Singleton
    {
        public static T Of<T>() where T : class, new()
            => Single<T>.GetInstance(() => new T());

        public static T Of<T>(Func<T> instantiator) where T : class
        {
            if (instantiator == null)
                throw new ArgumentNullException(nameof(instantiator));

            return Single<T>.GetInstance(instantiator);
        }

        internal static class Single<T> where T : class
        {
            private static T s_instance;

            /// <seealso href="https://docs.unity3d.com/Manual/DomainReloading.html"/>
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            static void Init()
            {
                s_instance = null;
            }

            public static T GetInstance(Func<T> instantiator)
            {
                if (s_instance == null)
                {
                    s_instance = instantiator();
                }

                return s_instance;
            }
        }
    }
}
