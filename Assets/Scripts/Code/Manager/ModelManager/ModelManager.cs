using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Code
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
				if (_model != null)
					_model.Tick ();
			}

			public void Release ()
			{
				this._model.Release ();
			}

			private Model _model;
			public void ActiveModel(Model model){
				this._model = model;
				this._model.Start ();
			}
		}
	}
}