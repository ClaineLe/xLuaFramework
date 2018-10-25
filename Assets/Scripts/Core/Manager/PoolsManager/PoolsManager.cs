using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace Core.Manager
    {
        public partial class ManagerName
        {
            public const string Pools = "PoolsManager";
        }
        public class PoolsManager : BaseManager<PoolsManager>, IManager
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