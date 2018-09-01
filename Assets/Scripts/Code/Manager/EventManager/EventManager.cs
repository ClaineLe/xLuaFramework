using UnityEngine;

namespace Framework
{
    namespace Code.Manager
    {
        public partial class ManagerName
        {
            public const string Event = "EventManager";
        }
        public class EventManager : BaseManager<EventManager>, IManager
        {
            public void Init()
            {
            }

            public void Release()
            {
            }

            public void Tick()
            {
            }
        }
    }
}