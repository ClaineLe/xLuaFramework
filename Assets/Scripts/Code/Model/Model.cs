using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	namespace Code.Model
	{
		public class Model : IModel
		{
			private int m_Id;
			public int ModelId {
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

			public Model (int modelId, string modelName)
			{
				this.m_Id = modelId;
				this.m_Name = modelName;
			}

			public void RegiestNetProxy(){
				
			}

			public void Release ()
			{
			}
		}
	}
}