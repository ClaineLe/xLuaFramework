
using UnityEngine;

namespace Framework
{
    namespace Core.Singleton
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
                        _instance.Init();
                        DontDestroyOnLoad(mono);
                    }
                    return _instance;
                }
            }

            public void Init()
            {
                this.onInit();
            }

            public void Release()
            {
                GameObject.Destroy(_instance.gameObject);
                _instance = null;
                this.onRelease();
            }

            protected abstract void onInit();
            protected abstract void onRelease();
        }
    }
}