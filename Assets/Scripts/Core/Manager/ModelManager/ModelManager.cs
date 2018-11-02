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
			private Dictionary<string, Model> m_ModelDic;
			public void Init ()
			{
				this.m_ModelDic = new Dictionary<string, Model> ();
			}

			public void Tick ()
			{
			}

			public void Release ()
			{
				if (this.m_ModelDic != null) {
					this.m_ModelDic.Clear();
					this.m_ModelDic = null;
				}
			}


			public Model GetModel(string modelName, bool autoCreate = true){
				if(!this.m_ModelDic.ContainsKey(modelName)){
					Model model = this.CreateModel (modelName);
					this.m_ModelDic [modelName] = model;
				}
				return this.m_ModelDic [modelName];
			}

			public Model CreateModel(string modelName){
                Model model = Model.Create(modelName);
                return model;
			}
		}
	}
}