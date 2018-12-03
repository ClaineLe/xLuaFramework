using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Widget;

namespace Framework
{
	namespace Core.Manager
	{
		public class ViewLayer
		{
			public GameObject gameObject{ get; private set; }

			public Transform transform{ get; private set; }

			public RectTransform rectTransform{ get; private set; }

			public Transform ViewRoot;

			private Camera m_Camera;

			static public ViewLayer Create (GameObject layerGo)
			{
				ViewLayer layer = new ViewLayer ();
				layer.gameObject = layerGo;
				layer.transform = layerGo.transform;
				layer.rectTransform = layerGo.transform as RectTransform;
				layer.ViewRoot = layer.transform.Find ("Root");
				layer.m_Camera = layer.transform.Find ("Camera").GetComponent<Camera>();
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