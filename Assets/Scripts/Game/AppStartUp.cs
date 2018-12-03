using UnityEngine;
using Framework.Core.Manager;
using Framework.Util;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Framework.Core.Assistant;
using Newtonsoft.Json.Linq;

namespace Framework
{
    namespace Game
    {
        public class AppStartUp
        {
			[RuntimeInitializeOnLoadMethod]
			public static void StartUp(){
                AppFacade.Instance.StartUp();
#if BUNDLE_MODEL
                UpdaterModel.Instance.Lanucher(StartUpFramework);
#else
				StartUpFramework();
#endif
            }

			public static void StartUpFramework()
            {
#if !UNITY_EDITOR || BUNDLE_MODEL
                BundleInfoCacher.Init();
#endif
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

