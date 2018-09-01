using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace Code.Manager
    {
        public partial class ManagerName
        {
            public const string Timer = "TimerManager";
        }
        public class TimerManager : BaseManager<TimerManager>, IManager
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

            public void Test()
            {
                Debug.Log("CSharp - Test");
            }
        }
    }
}