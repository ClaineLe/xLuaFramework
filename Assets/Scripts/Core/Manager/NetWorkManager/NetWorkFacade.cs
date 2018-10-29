using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Assistant;


namespace Framework.Core
{
	namespace Manager
	{
		using Manager = Framework.Game.Manager;
		public interface INetWorkFacade{
			ushort FacadeId{get;}
			void Release ();

		}
		public class NetWorkFacade:INetWorkFacade
		{
			private ushort m_FacadeId;
			public ushort FacadeId{
				get{
					return this.m_FacadeId;
				}
			}
			public void Release (){
				this.m_NetworkNotify.ClearObserver ();
				Framework.Game.Manager.NetWorkMgr.UnRegiestReceiver (this.FacadeId);
			}

			protected ObserverNotify<ushort,byte[]> m_NetworkNotify;

			private NetWorkFacade(ushort facadeId){
				this.m_FacadeId = facadeId;
				this.m_NetworkNotify = new ObserverNotify<ushort, byte[]> ();
				Manager.NetWorkMgr.RegiestReceiver (this.FacadeId,this.NetWorkMessageReceiver);
			}

			protected void NetWorkMessageReceiver (ushort cmd, byte[] netData){
				this.m_NetworkNotify.Notify (cmd, netData);
			}

			public void RegiestNetDataReceiver (ushort cmd, NotifyDelegate<byte[]> handle){
				this.m_NetworkNotify.AddObserver (cmd, handle);
			}

			public void UnRegiestNetDataReceiver(ushort cmd){
				this.m_NetworkNotify.RemoveNotify (cmd);
			}

		}
	}
}