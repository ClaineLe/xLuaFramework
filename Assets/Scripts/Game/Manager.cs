using Framework.Code.Manager;

namespace Framework
{
    namespace Game
    {
        public class Manager
        {
            private static LuaManager m_LuaMgr;
            private static ViewManager m_ViewMgr;
            private static EventManager m_EventMgr;
            private static LevelManager m_LevelMgr;
            private static AssetManager m_AssetMgr;
            private static TimerManager m_TimerMgr;
            private static SoundManager m_SoundMgr;
            private static PoolsManager m_PoolsMgr;
            private static NetWorkManager m_NetWorkMgr;
			private static ModelManager m_ModelMgr;
            public static LuaManager LuaMgr
            {
                get
                {
                    if (m_LuaMgr == null)
                        m_LuaMgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
                    return m_LuaMgr;
                }
            }
            public static ViewManager ViewMgr
            {
                get
                {
                    if (m_ViewMgr == null)
                        m_ViewMgr = AppFacade.Instance.GetManager<ViewManager>(ManagerName.View);
                    return m_ViewMgr;
                }
            }
            public static AssetManager AssetMgr
            {
                get
                {
                    if (m_AssetMgr == null)
                        m_AssetMgr = AppFacade.Instance.GetManager<AssetManager>(ManagerName.Asset);
                    return m_AssetMgr;
                }
            }
            public static TimerManager TimerMgr
            {
                get
                {
                    if (m_TimerMgr == null)
                        m_TimerMgr = AppFacade.Instance.GetManager<TimerManager>(ManagerName.Timer);
                    return m_TimerMgr;
                }
            }
            public static SoundManager SoundMgr
            {
                get
                {
                    if (m_SoundMgr == null)
                        m_SoundMgr = AppFacade.Instance.GetManager<SoundManager>(ManagerName.Sound);
                    return m_SoundMgr;
                }
            }
            public static PoolsManager PoolsMgr
            {
                get
                {
                    if (m_PoolsMgr == null)
                        m_PoolsMgr = AppFacade.Instance.GetManager<PoolsManager>(ManagerName.Pools);
                    return m_PoolsMgr;
                }
            }
            public static NetWorkManager NetWorkMgr
            {
                get
                {
                    if (m_NetWorkMgr == null)
                        m_NetWorkMgr = AppFacade.Instance.GetManager<NetWorkManager>(ManagerName.NetWork);
                    return m_NetWorkMgr;
                }
            }

            public static EventManager EventMgr
            {
                get
                {
                    if (m_EventMgr == null)
                        m_EventMgr = AppFacade.Instance.GetManager<EventManager>(ManagerName.Event);
                    return m_EventMgr;
                }
            }
            public static LevelManager LevelMgr
            {
                get
                {
                    if (m_LevelMgr == null)
                        m_LevelMgr = AppFacade.Instance.GetManager<LevelManager>(ManagerName.Level);
                    return m_LevelMgr;
                }
            }

			public static ModelManager ModelMgr{
				get{ 
					if (m_ModelMgr == null)
						m_ModelMgr = AppFacade.Instance.GetManager<ModelManager> (ManagerName.Model);
					return m_ModelMgr;
				}
			}
        }
    }
}