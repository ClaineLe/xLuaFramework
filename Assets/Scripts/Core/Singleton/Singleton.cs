namespace Framework
{
    namespace Core.Singleton
    {
        public abstract class Singleton<T> : ISingleton where T : ISingleton, new()
        {
            private static T _instance;
            public static T Instance
            {
                get
                {
                    if (_instance == null)
                        _instance = new T();
                    return _instance;
                }
            }

            public void Init()
            {
                this.onInit();

            }

            public void Release()
            {
                this.onRelease();
            }


            protected virtual void onInit() { }
            protected virtual void onRelease() { }
        }
    }
}