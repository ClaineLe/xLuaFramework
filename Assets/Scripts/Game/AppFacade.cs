using Framework.Core.Manager;
using Framework.Core.Singleton;
using System.Collections.Generic;
using System.IO;

namespace Framework
{
	namespace Game
	{
		public class AppFacade : SingletonMono<AppFacade>
		{
            private bool m_IsDone;
			private Dictionary<string, IManager> m_Managers = new Dictionary<string, IManager> ();

			private delegate void Delegate_Manager_Init ();
			private delegate void Delegate_Manager_Tick ();
			private delegate void Delegate_Manager_Release ();

			private Delegate_Manager_Init m_MgrInit_Handle;
			private Delegate_Manager_Tick m_MgrTick_Handle;
			private Delegate_Manager_Release m_MgrRelease_Handle;

            public string AssetVersion { get; private set; }

            public void StartUp(){
                string versionFilePath = PathConst.StreamAssetPath + PathConst.BundleDirName + "/AssetVersion.txt";
                AssetVersion = File.ReadAllText(versionFilePath).Trim();
            }

            protected override void onInit()
            {
                m_IsDone = false;
            }
            public void InitManager ()
			{
				if (m_MgrInit_Handle != null)
					m_MgrInit_Handle ();
				this.m_IsDone = true;
			}

			public void AddManager<T> (string mgrName) where T : IManager, new()
			{
				T mgr = new T ();
				this.m_MgrInit_Handle += mgr.Init;
				this.m_MgrTick_Handle += mgr.Tick;
				this.m_MgrRelease_Handle += mgr.Release;
				this.m_Managers [mgrName] = mgr;
			}

			public void RemoveManager (string mgrName)
			{
				if (this.m_Managers.ContainsKey (mgrName)) {
					this.m_Managers [mgrName].Release ();
					this.m_MgrInit_Handle -= this.m_Managers [mgrName].Init;
					this.m_MgrTick_Handle -= this.m_Managers [mgrName].Tick;
					this.m_MgrRelease_Handle -= this.m_Managers [mgrName].Release;
					this.m_Managers.Remove (mgrName);
				}
			}

			public T GetManager<T> (string mgrName) where T : IManager
			{
				if (this.m_Managers.ContainsKey (mgrName))
					return (T)this.m_Managers [mgrName];
				return default(T);
			}

            protected override void onRelease ()
			{
				if (this.m_MgrRelease_Handle != null)
					this.m_MgrRelease_Handle ();
			}

			public void TickManager ()
			{
				if (this.m_MgrTick_Handle != null)
					this.m_MgrTick_Handle ();
			}

			private void Update ()
			{
				if (this.m_IsDone) {
					this.TickManager ();
				}
			}

			public void OnApplicationQuit()
			{
				Release();
			}

        }
	}
}