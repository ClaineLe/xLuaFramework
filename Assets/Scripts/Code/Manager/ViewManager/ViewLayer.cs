using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Code.Widget;

namespace Framework
{
	namespace Code.Manager
	{
		public class ViewLayer
		{
			public GameObject gameObject{ get; private set; }

			public Transform transform{ get; private set; }

			public RectTransform rectTransform{ get; private set; }

			public Transform ViewRoot;

			static public ViewLayer Create (GameObject layerGo)
			{
				ViewLayer layer = new ViewLayer ();
				layer.gameObject = layerGo;
				layer.transform = layerGo.transform;
				layer.rectTransform = layerGo.transform as RectTransform;
				layer.ViewRoot = layer.transform.Find ("Root");
				return layer;
			}

			public void SetName (string name)
			{
				gameObject.name = name;
			}

			public void SetSortNum (int sortNum)
			{
			}

		}
	}
}