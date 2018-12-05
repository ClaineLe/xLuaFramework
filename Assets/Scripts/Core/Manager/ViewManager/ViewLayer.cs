using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Widget;
using Framework.Core.Assistant;

namespace Framework
{
	namespace Core.Manager
	{
		public class ViewLayer
		{
			public struct Option
			{
				public string Name;
				public int Depth;
				public int Near;
				public int Far;
				public int FOV;
			}
			
			public GameObject gameObject{ get; private set; }

			public Transform transform{ get; private set; }

			public RectTransform rectTransform{ get; private set; }

			public Transform ViewRoot;

			private Camera m_Camera;

			private List<View> m_CacheViewList;
			private Stack<View> m_CacheViewStack;

			static public ViewLayer Create (GameObject layerGo)
			{
				ViewLayer layer = new ViewLayer ();
				layer.m_CacheViewList = new List<View> ();
				layer.m_CacheViewStack = new Stack<View> ();
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

			public void Push(View view, bool isCache = true){
				view.SetLayer (this);
				if (isCache)
					m_CacheViewStack.Push (view);
				else
					m_CacheViewList.Add (view);
			}

			public void AddChild(MonoView monoView){
				monoView.transform.SetParent (transform);
				monoView.SetDefaultAnchor ();
			}

			public View Pop(){
				return m_CacheViewStack.Pop ();
			}

			public void ClearCacheList(){
				if (m_CacheViewList != null && m_CacheViewList.Count >= 0) {
					for (int i = 0; i < m_CacheViewList.Count; i++) {
						m_CacheViewList [i].Release ();
					}
					m_CacheViewList.Clear ();
				}
			}
		}
	}
}