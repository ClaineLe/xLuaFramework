namespace Framework
{
    namespace Core.Manager
    {
        public interface IManager
        {
            void Init();
            void Tick();
            void Release();
        }
    }
}