using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	namespace Code.Model
	{
		public interface IModel
		{
			int ModelId{ get; }
			string ModelName{ get; }
			void Release();
		}
	}
}