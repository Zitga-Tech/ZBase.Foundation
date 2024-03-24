using UnityEngine;

namespace ZBase.Foundation.Singletons
{
    /// <summary>
    /// <para>Designed for decoupling Singleton pattern and <see cref="MonoBehaviour"/>-based classes.</para>
    /// <para>Usage: <see cref="SingleBehaviour"/>.Of&lt;MyMonoBehaviour&gt;()</para>
    /// </summary>
    public static partial class SingleBehaviour
    {
        public enum Lifetime
        {
            /// <summary>
            /// The singleton <see cref="MonoBehaviour"/> should exist
            /// through every scene.
            /// </summary>
            /// <remarks>
            /// <para>
            /// This option will only invoke
            /// <see cref="Object"/>.<see cref="Object.DontDestroyOnLoad(Object)"/>
            /// on the newly created <see cref="GameObject"/>.
            /// </para>
            /// <para>
            /// This option will NOT affect any existing <see cref="GameObject"/>.
            /// </para>
            /// </remarks>
            EveryScenes = 0,

            /// <summary>
            /// The singleton <see cref="MonoBehaviour"/> should only exist
            /// in the current scene.
            /// </summary>
            /// <remarks>
            /// This option will NOT affect any existing <see cref="GameObject"/>.
            /// </remarks>
            SingleScene = 1,
        }

        public static T Of<T>(Lifetime lifetime = Lifetime.EveryScenes) where T : MonoBehaviour
            => Single<T>.GetInstance(lifetime);

        internal static class Single<T> where T : MonoBehaviour
        {
            private static readonly object s_lock = new();
            private static T s_instance;

            /// <seealso href="https://docs.unity3d.com/Manual/DomainReloading.html"/>
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            static void Init()
            {
                s_instance = null;
            }

            public static void SetInstance(T instance, Lifetime lifetime)
            {
                if (instance == false || s_instance)
                {
                    return;
                }

                s_instance = instance;

                if (lifetime == Lifetime.EveryScenes)
                {
                    Object.DontDestroyOnLoad(instance.gameObject);
                }
            }

            public static T GetInstance(Lifetime lifetime, bool autoRename = true)
            {
                if (s_instance == false)
                {
                    lock (s_lock)
                    {
                        s_instance = Object.FindObjectOfType<T>();

                        if (s_instance == false)
                        {
                            var compType = typeof(T);
                            var gameObject = new GameObject(compType.Name);
                            s_instance = (T)gameObject.AddComponent(compType);
                        }

                        if (lifetime == Lifetime.EveryScenes)
                        {
                            Object.DontDestroyOnLoad(s_instance.gameObject);
                        }

                        if (autoRename)
                        {
                            s_instance.gameObject.name = $"[{nameof(Singleton)}] {typeof(T).Name}";
                        }
                    }
                }

                return s_instance;
            }
        }
    }
}
