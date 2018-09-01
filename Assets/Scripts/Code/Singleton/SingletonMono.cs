
using UnityEngine;

namespace Framework
{
    namespace Code.Singleton
    {
        public abstract class SingletonMono<T> : MonoBehaviour, ISingleton where T : MonoBehaviour, ISingleton
        {
            private static T _instance;
            public static T Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        GameObject mono = new GameObject(typeof(T).Name);
                        _instance = mono.AddComponent<T>();
                        DontDestroyOnLoad(mono);
                    }
                    return _instance;
                }
            }

            public virtual void Init()
            {

            }

            public virtual void Release()
            {
                GameObject.DestroyObject(_instance.gameObject);
            }
        }
    }
}