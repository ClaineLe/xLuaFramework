using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	namespace Code.NetProxy
	{
		public delegate string NetMessageListenDelegate (byte[] netData);

		public interface INetProxy
		{
			void RegiestNetMsg(ushort cmd, NetMessageListenDelegate netMsgHandle);
			void UnRegiestNetMsg(ushort cmd);
			void Release();
		}
	}
}