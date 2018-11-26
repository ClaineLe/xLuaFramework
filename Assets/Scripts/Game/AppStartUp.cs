﻿using Framework.Core.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace Game
    {
        public class AppStartUp : MonoBehaviour
        {
            public void Start()
            {

                AppFacade.Instance.StartUp();
                UpdaterModel.Instance.Lanucher(StartUpFramework);
                //StartUpFramework();
            }


            public void StartUpFramework()
            {
                AppFacade.Instance.AddManager<AssetManager>(ManagerName.Asset);
                AppFacade.Instance.AddManager<EventManager>(ManagerName.Event);
                AppFacade.Instance.AddManager<LevelManager>(ManagerName.Level);
                AppFacade.Instance.AddManager<LuaManager>(ManagerName.Lua);
                AppFacade.Instance.AddManager<NetWorkManager>(ManagerName.NetWork);
                AppFacade.Instance.AddManager<PoolsManager>(ManagerName.Pools);
                AppFacade.Instance.AddManager<SoundManager>(ManagerName.Sound);
                AppFacade.Instance.AddManager<TimerManager>(ManagerName.Timer);
                AppFacade.Instance.AddManager<ModelManager>(ManagerName.Model);
				AppFacade.Instance.AddManager<ViewManager>(ManagerName.View);
				AppFacade.Instance.AddManager<ConfigManager>(ManagerName.Config);
                AppFacade.Instance.InitManager();
                Manager.LuaMgr.StartUpLuaFramework();
            }
		}
    }
}

