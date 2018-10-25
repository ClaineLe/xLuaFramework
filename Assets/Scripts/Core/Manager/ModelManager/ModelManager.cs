using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
	namespace Manager
	{
		using Assistant;
		public partial class ManagerName
		{
			public const string Model = "ModelManager";
		}

		public class ModelManager : BaseManager<ModelManager>, IManager
		{
			public void Init ()
			{
			}

			public void Tick ()
			{
			}

			public void Release ()
			{
			}
		}
	}
}