using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Manager;
using Framework.Game;

namespace Framework.Core
{
	namespace Assistant
	{
		public class Model : LuaCompatible<Model>
        {
			protected ObserverNotify<string,object> m_ModelNotify;
			private INetWorkFacade m_NetWorkFacade;

            protected override string _luaPath
            {
                get
                {
                    return string.Format(PathConst.FORMAT_MODEL_NAME, m_Name, m_Name);
                }
            }

            protected override void onCreate()
            {
                this.m_ModelNotify = new ObserverNotify<string, object>();
                base.InitLuaTable(this);
            }

            public void SetupNetWorkFacade(INetWorkFacade netWorkFacade)
            {
                this.m_NetWorkFacade = netWorkFacade;
            }
            protected override void onRelease ()
			{
				this.m_ModelNotify.ClearObserver ();
				if (this.m_NetWorkFacade != null)
					this.m_NetWorkFacade.Release ();
			}

			public void Notify(string notifyName,object param){
				this.m_ModelNotify.Notify (notifyName, param);
			}

			public void AddNotify(string notifyName, NotifyDelegate<object> handle){
				this.m_ModelNotify.AddObserver (notifyName, handle);
			}

			public void RemoveNotify(string notifyName, NotifyDelegate<object> handle = null){
				this.m_ModelNotify.RemoveNotify (notifyName, handle);
			}
		}
	}
}