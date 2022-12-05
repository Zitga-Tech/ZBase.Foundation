using System;
using System.Collections.Generic;

namespace ZBase.Foundation.Singletons
{
    public class SingletonContainer<TBase>
        : IDisposable
        where TBase : class
    {
        private readonly Dictionary<TypeHash, TBase> _singletons = new();

        public bool Contains<T>()
            => _singletons.ContainsKey(this.TypeHash<T>());

        public bool Contains<T>(T instance)
            => _singletons.TryGetValue(this.TypeHash<T>(), out var obj)
               && ReferenceEquals(obj, instance);

        public bool TryAdd<T>()
            where T : TBase, new()
        {
            if (_singletons.ContainsKey(this.TypeHash<T>()))
            {
#if (UNITY_EDITOR || DEBUG) && !DISABLE_DEBUG
                UnityEngine.Debug.LogError($"An instance of {this.TypeName<T>()} has already been existing");
#endif

                return false;
            }

            _singletons.Add(this.TypeHash<T>(), new T());
            return true;
        }

        public bool TryAdd<T>(T instance)
            where T : TBase
        {
            if (instance == null)
            {
#if (UNITY_EDITOR || DEBUG) && !DISABLE_DEBUG
                throw new ArgumentNullException(nameof(instance));
#else
                return false;
#endif
            }

            if (_singletons.ContainsKey(this.TypeHash<T>()))
            {
#if (UNITY_EDITOR || DEBUG) && !DISABLE_DEBUG
                UnityEngine.Debug.LogError($"An instance of {this.Type<T>()} has already been existing");
#endif

                return false;
            }

            _singletons.Add(this.TypeHash<T>(), instance);
            return true;
        }

        public bool TryGetOrAdd<T>(out T instance)
            where T : TBase, new()
        {
            if (_singletons.TryGetValue(this.TypeHash<T>(), out var obj))
            {
                if (obj is T inst)
                {
                    instance = inst;
                    return true;
                }
#if (UNITY_EDITOR || DEBUG) && !DISABLE_DEBUG
                else
                {
                    throw new InvalidCastException(
                        $"Cannot cast an instance of type {obj.GetType()} to {this.Type<T>()}" +
                        $"even though it is registered for {this.Type<T>()}"
                    );
                }
#endif
            }

            _singletons[this.TypeHash<T>()] = instance = new();
            return true;
        }

        public bool TryGet<T>(out T instance)
            where T : TBase
        {
            if (_singletons.TryGetValue(this.TypeHash<T>(), out var obj))
            {
                if (obj is T inst)
                {
                    instance = inst;
                    return true;
                }
#if (UNITY_EDITOR || DEBUG) && !DISABLE_DEBUG
                else
                {
                    throw new InvalidCastException(
                        $"Cannot cast an instance of type {obj.GetType()} to {this.Type<T>()}"
                    );
                }
#endif
            }

            instance = default;
            return false;
        }

        public void Dispose()
        {
            foreach (var disposable  in _singletons)
            {
                if (disposable is IDisposable)
                {
                    (disposable as IDisposable).Dispose();
                }
            }

            _singletons.Clear();
        }
    }
}
