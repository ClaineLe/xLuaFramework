using System.Collections.Generic;

namespace Framework.Core
{
    namespace Manager
    {
        public partial class ManagerName
        {
            public const string NetWork = "NetWorkManager";
        }
		public delegate void NetWorkMessageReceiver(ushort cmd, byte[] netData);
        public class NetWorkManager : BaseManager<NetWorkManager>, IManager
        {
			private Dictionary<ushort, NetWorkMessageReceiver> m_DicModelReceiver;
            public void Init()
            {
				this.m_DicModelReceiver = new Dictionary<ushort, NetWorkMessageReceiver> ();
            }

            public void Tick()
            {
				
            }

            public void Release()
            {
				
            }

			public void Invoke(ushort modelId, ushort command, byte[] netData){
				if (this.m_DicModelReceiver.ContainsKey (modelId)) {
					this.m_DicModelReceiver [modelId].Invoke (command,netData);
				}
			}

			public void RegiestReceiver(ushort modelId, NetWorkMessageReceiver receiveHandle){
				if (!this.m_DicModelReceiver.ContainsKey (modelId)) {
					this.m_DicModelReceiver [modelId] = receiveHandle;
				}
			}

			public void UnRegiestReceiver(ushort modelId){
				if (this.m_DicModelReceiver.ContainsKey (modelId)) {
					this.m_DicModelReceiver.Remove (modelId);
				}
			}
        }
    }
}