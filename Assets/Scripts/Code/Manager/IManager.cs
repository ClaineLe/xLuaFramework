namespace Framework
{
    namespace Code.Manager
    {
        public interface IManager
        {
            void Init();
            void Tick();
            void Release();
        }
    }
}