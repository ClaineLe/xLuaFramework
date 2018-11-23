using System.Collections.Generic;
using UnityEngine;
using Framework.Game;
using Framework.Core.Assistant;

namespace Framework.Core
{
    namespace Manager
    {
        public partial class ManagerName
        {
            public const string View = "ViewManager";
        }
        public class ViewManager : BaseManager<ViewManager>, IManager
        {
			
			public enum eViewLayer
			{
				Normal = 1000,
			}

			private GameObject _prefab_ViewRoot;
			private GameObject _prefab_ViewLayer;

			private Dictionary<string, View> m_ViewDic;
			private Dictionary<eViewLayer, ViewLayer> m_ViewLayer;
			private GameObject m_ViewRoot;

			public void Init()
            {
				this.m_ViewDic = new Dictionary<string, View>();
				this.m_ViewLayer = new Dictionary<eViewLayer, ViewLayer> ();
				this._prefab_ViewRoot = Framework.Game.Manager.AssetMgr.LoadAsset (PathConst.ViewRoot_Path,typeof(GameObject)) as GameObject;
				this._prefab_ViewLayer = Framework.Game.Manager.AssetMgr.LoadAsset (PathConst.ViewLayer_Path,typeof(GameObject)) as GameObject;
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

			public XLua.LuaTable LoadViewNode(string viewName){
				GameObject viewAsset = Framework.Game.Manager.AssetMgr.LoadAsset (_GetViewPath (viewName), typeof(GameObject)) as GameObject;
				GameObject viewGo = GameObject.Instantiate (viewAsset);
				viewGo.name = viewName;
				ConfigView(viewGo);
                View view = View.Create(viewName).SetupViewGo(viewGo);
                Presender presender = Presender.Create(viewName).SetupView(view);
                return presender.m_LuaTable;
			}

			public void LoadViewNodeAsync(string viewName, System.Action<XLua.LuaTable> callback){
				Framework.Game.Manager.AssetMgr.LoadAssetAsync (_GetViewPath (viewName),viewAsset=>{
					if(callback != null){
						GameObject viewGo = GameObject.Instantiate (viewAsset) as GameObject;
						viewGo.name = viewName;
						ConfigView(viewGo);

                        View view = View.Create(viewName).SetupViewGo(viewGo);
                        Presender presender = Presender.Create(viewName).SetupView(view);
                        callback(presender.m_LuaTable);
					}
				},typeof(GameObject));
			}

			private void ConfigView(GameObject viewGo){
				viewGo.transform.SetParent(m_ViewLayer [eViewLayer.Normal].ViewRoot);
				RectTransform rectView = viewGo.transform as RectTransform;
				rectView.anchorMin = Vector2.zero;
				rectView.anchorMax = Vector2.one;
				rectView.anchoredPosition3D = Vector3.zero;
				rectView.localScale = Vector3.one;
			}

			private string _GetViewPath(string viewName){
				return string.Format (PathConst.ViewAsset_Path,viewName);
			}



            public void Release()
            {
                if (this.m_ViewDic != null) {
					foreach (var view in this.m_ViewDic) {
						view.Value.Release ();
					}
                    this.m_ViewDic.Clear();
                    this.m_ViewDic = null;
                }
            }
        }
    }
}