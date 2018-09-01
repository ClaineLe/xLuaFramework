using Framework.Code.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Framework
{
    namespace Game
    {
        public class StartUp : MonoBehaviour
        {
            public void Start()
            {
                AppFacade.Instance.AddManager<AssetManager>(ManagerName.Asset);
                AppFacade.Instance.AddManager<EventManager>(ManagerName.Event);
                AppFacade.Instance.AddManager<LevelManager>(ManagerName.Level);
                AppFacade.Instance.AddManager<LuaManager>(ManagerName.Lua);
                AppFacade.Instance.AddManager<NetWorkManager>(ManagerName.NetWork);
                AppFacade.Instance.AddManager<PoolsManager>(ManagerName.Pools);
                AppFacade.Instance.AddManager<SoundManager>(ManagerName.Sound);
                AppFacade.Instance.AddManager<TimerManager>(ManagerName.Timer);
                AppFacade.Instance.AddManager<ViewManager>(ManagerName.View);

                AppFacade.Instance.StartUp();

                Manager.LuaMgr.StartUpLuaFramework();
            }

            public void OnApplicationQuit()
            {
                AppFacade.Instance.Release();
                Debug.Log("OnApplicationQuit");
            }
        }
    }
}