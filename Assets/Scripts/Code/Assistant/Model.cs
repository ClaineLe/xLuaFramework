using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Code
{
	namespace Assistant
	{
		using Manager = Framework.Game.Manager;
		public class Model
		{
			private ushort m_Id;
			public ushort ModelId {
				get {
					return m_Id;
				}
			}

			private string m_Name;
			public string ModelName {
				get {
					return m_Name;
				}
			}

			protected ObserverNotify<string,UnityEngine.Object> m_ModelNotify;
			protected ObserverNotify<ushort,byte[]> m_NetworkNotify;

			public Model (ushort modelId, string modelName)
			{
				this.m_Id = modelId;
				this.m_Name = modelName;
				this.m_ModelNotify = new ObserverNotify<string, Object> ();
				this.m_NetworkNotify = new ObserverNotify<ushort, byte[]> ();
				this.Init ();
			}

			public void Init(){
				Manager.NetWorkMgr.RegiestReceiver (this.ModelId, this.NetWorkMessageReceiver);
			}


			public virtual void Release ()
			{
				this.m_ModelNotify.ClearObserver ();
				this.m_NetworkNotify.ClearObserver ();
				Framework.Game.Manager.NetWorkMgr.UnRegiestReceiver (this.ModelId);
			}

			protected void NetWorkMessageReceiver (ushort cmd, byte[] netData)
			{
				this.m_NetworkNotify.Notify (cmd, netData);
			}

			public void RegiestNetDataReceiver (ushort cmd, NotifyDelegate<byte[]> handle){
				this.m_NetworkNotify.AddObserver (cmd, handle);
			}

			public void UnRegiestNetDataReceiver(ushort cmd){
				this.m_NetworkNotify.RemoveObserver (cmd);
			}

			public void AddNotify(string eventName, NotifyDelegate<UnityEngine.Object> handle){
				this.m_ModelNotify.AddObserver (eventName, handle);
			}

			public void RemoveNotify(string eventName, NotifyDelegate<UnityEngine.Object> handle){
				this.m_ModelNotify.RemoveNotify (eventName, handle);
			}

			public void RemoveEvent(string eventName){
				this.m_ModelNotify.RemoveObserver (eventName);
			}
		}
	}
}