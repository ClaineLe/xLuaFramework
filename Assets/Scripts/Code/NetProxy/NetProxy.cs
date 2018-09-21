using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	namespace Code.NetProxy
	{
		public class NetProxy : INetProxy
		{
			public void RegiestNetMsg (ushort cmd, NetMessageListenDelegate netMsgHandle)
			{
				throw new System.NotImplementedException ();
			}
			public void UnRegiestNetMsg (ushort cmd)
			{
				throw new System.NotImplementedException ();
			}
			public void Release ()
			{
				throw new System.NotImplementedException ();
			}
		}
	}
}