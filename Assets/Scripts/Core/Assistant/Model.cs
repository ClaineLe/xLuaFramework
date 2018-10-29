using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Manager;
using Framework.Game;

namespace Framework.Core
{
	namespace Assistant
	{
		using Manager = Framework.Game.Manager;
		public class Model
		{

			private string m_Name;
			public string ModelName {
				get {
					return m_Name;
				}
			}

			protected virtual bool m_IsLuaModel{
				get{
					return true;	
				}
			}

			private XLua.LuaTable m_LuaModel;
			public XLua.LuaTable LuaModel{
				get{ 
					return m_LuaModel;
				}
			}
			protected virtual void OnInit (){
			}
			protected virtual void OnRelease(){
			}
			protected ObserverNotify<string,object> m_ModelNotify;
			private INetWorkFacade m_NetWorkFacade;
			public Model (string modelName)
			{
				this.m_Name = modelName;
				this.m_ModelNotify = new ObserverNotify<string, object> ();
				this.Init ();
			}

			public void Init(){
				if (this.m_IsLuaModel) {
					this.InitLuaModel ();
				}
				this.OnInit ();
			}

			private void InitLuaModel(){
				string luaPath = string.Format (ResPathConst.FORMAT_MODEL_NAME,this.m_Name,this.m_Name);
				XLua.LuaTable luaTmp = Framework.Game.Manager.LuaMgr.TblRequire (luaPath);
				this.m_LuaModel = luaTmp.Get<XLua.LuaFunction> ("Create").Call (luaTmp, this)[0] as XLua.LuaTable;
			}

			public virtual void Release ()
			{
				this.m_ModelNotify.ClearObserver ();
				if (this.m_NetWorkFacade != null)
					this.m_NetWorkFacade.Release ();
				this.OnRelease ();
			}

			public void RegiestNetWorkFacade(INetWorkFacade netWorkFacade){
				this.m_NetWorkFacade = netWorkFacade;
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