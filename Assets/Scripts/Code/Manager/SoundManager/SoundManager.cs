using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace Code.Manager
    {
        public partial class ManagerName
        {
            public const string Sound = "SoundManager";
        }

        public class SoundManager : BaseManager<SoundManager>, IManager
        {
            public void Init()
            {
            }


            public void Tick()
            {
            }
            public void Release()
            {
            }
        }
    }
}