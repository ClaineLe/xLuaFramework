using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core.Widget;

namespace Framework
{
    namespace Core.Manager
    {
        public partial class ManagerName
        {
            public const string View = "ViewManager";
        }
        public class ViewManager : BaseManager<ViewManager>, IManager
        {
			private const string ViewRoot_Path = "Prefabs/#View/ViewRoot.prefab";
			private const string ViewLayer_Path = "Prefabs/#View/Layer.prefab";
			private const string ViewAsset_Path = "Prefabs/#View/{0}.prefab";

			public enum eViewLayer
			{
				Normal = 1000,
			}

			private GameObject _prefab_ViewRoot;
			private GameObject _prefab_ViewLayer;

            private Dictionary<string, GameObject> m_ViewDic;
			private Dictionary<eViewLayer, ViewLayer> m_ViewLayer;
			private GameObject m_ViewRoot;

			public void Init()
            {
                this.m_ViewDic = new Dictionary<string, GameObject>();
				this.m_ViewLayer = new Dictionary<eViewLayer, ViewLayer> ();
				this._prefab_ViewRoot = Framework.Game.Manager.AssetMgr.LoadAsset (ViewRoot_Path,typeof(GameObject)) as GameObject;
				this._prefab_ViewLayer = Framework.Game.Manager.AssetMgr.LoadAsset (ViewLayer_Path,typeof(GameObject)) as GameObject;
				this.CreateViewRoot ();
				this.AddLayer (eViewLayer.Normal);
            }

			private void CreateViewRoot(){
				this.m_ViewRoot = GameObject.Instantiate (this._prefab_ViewRoot);
				this.m_ViewRoot.name = "ViewRoot";
				this.m_ViewRoot.transform.SetParent (Framework.Game.AppFacade.Instance.transform);
			}

			public void AddLayer(eViewLayer viewLayer){
				GameObject layerGo = GameObject.Instantiate (this._prefab_ViewLayer);
				layerGo.transform.SetParent (this.m_ViewRoot.transform);
				ViewLayer layer = ViewLayer.Create (layerGo);
				layer.SetName (string.Format ("Layer_{0}", viewLayer));
				layer.SetSortNum ((int)viewLayer);
				this.m_ViewLayer [viewLayer] = layer;
			}


			public void Tick()
			{
			}

			public Presender LoadViewNode(string viewName){
				GameObject viewAsset = Framework.Game.Manager.AssetMgr.LoadAsset (_GetViewPath (viewName), typeof(GameObject)) as GameObject;
				GameObject viewGo = GameObject.Instantiate (viewAsset);
				viewGo.name = viewName;
				ConfigView(viewGo);
				return Presender.Create (viewGo);
			}

			public void LoadViewNodeAsync(string viewName, System.Action<Presender> callback){
				Framework.Game.Manager.AssetMgr.LoadAssetAsync (_GetViewPath (viewName),viewAsset=>{
					if(callback != null){
						GameObject viewGo = GameObject.Instantiate (viewAsset) as GameObject;
						viewGo.name = viewName;
						ConfigView(viewGo);
						callback(Presender.Create (viewGo));
					}
				},typeof(GameObject));
			}

			private void ConfigView(GameObject viewGo){
				viewGo.transform.SetParent(m_ViewLayer [eViewLayer.Normal].ViewRoot);
				RectTransform rectView = viewGo.transform as RectTransform;
				Debug.Log (rectView);
				rectView.anchorMin = Vector2.zero;
				rectView.anchorMax = Vector2.one;
				rectView.anchoredPosition3D = Vector3.zero;
				rectView.localScale = Vector3.one;
			}

			private string _GetViewPath(string viewName){
				return string.Format (ViewAsset_Path,viewName);
			}



            public void Release()
            {
                if (this.m_ViewDic != null) {
                    this.m_ViewDic.Clear();
                    this.m_ViewDic = null;
                }
            }
        }
    }
}