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

			private GameObject _prefab_ViewRoot;
			private GameObject _prefab_ViewLayer;

			private Dictionary<string, View> m_ViewDic;
			private Dictionary<string, ViewLayer> m_ViewLayer;
			private GameObject m_ViewRoot;

			public void Init()
            {
				this.m_ViewDic = new Dictionary<string, View>();
				this.m_ViewLayer = new Dictionary<string, ViewLayer> ();
				this._prefab_ViewRoot = Framework.Game.Manager.AssetMgr.LoadAsset (PathConst.ViewRoot_Path,typeof(GameObject)) as GameObject;
				this._prefab_ViewLayer = Framework.Game.Manager.AssetMgr.LoadAsset (PathConst.ViewLayer_Path,typeof(GameObject)) as GameObject;
				this.CreateViewRoot ();
            }

			private void CreateViewRoot(){
				this.m_ViewRoot = GameObject.Instantiate (this._prefab_ViewRoot);
				this.m_ViewRoot.name = "ViewRoot";
				this.m_ViewRoot.transform.SetParent (Framework.Game.AppFacade.Instance.transform);
			}

			public void AddLayer(ViewLayer.Option option){
				GameObject layerGo = GameObject.Instantiate (this._prefab_ViewLayer);
				layerGo.transform.SetParent (this.m_ViewRoot.transform);
				ViewLayer layer = ViewLayer.Create (layerGo);
				layer.SetName (string.Format ("Layer_{0}", option.Name));
				this.m_ViewLayer [option.Name] = layer;
			}

			public void Tick()
			{
			}

			public XLua.LuaTable LoadViewNode(string viewName, string layerName, bool isCache = true){
				GameObject viewAsset = Framework.Game.Manager.AssetMgr.LoadAsset (_GetViewPath (viewName), typeof(GameObject)) as GameObject;
				GameObject viewGo = GameObject.Instantiate (viewAsset);
				viewGo.name = viewName;
                View view = View.Create(viewName).SetupViewGo(viewGo);
				this.m_ViewLayer[layerName].Push(view, isCache);
                Presender presender = Presender.Create(viewName).SetupView(view);
                return presender.m_LuaTable;
			}

			public void LoadViewNodeAsync(string viewName, System.Action<XLua.LuaTable> callback, string layerName, bool isCache = true){
				Framework.Game.Manager.AssetMgr.LoadAssetAsync (_GetViewPath (viewName),viewAsset=>{
					if(callback != null){
						GameObject viewGo = GameObject.Instantiate (viewAsset) as GameObject;
						viewGo.name = viewName;
                        View view = View.Create(viewName).SetupViewGo(viewGo);
						this.m_ViewLayer[layerName].Push(view, isCache);
                        Presender presender = Presender.Create(viewName).SetupView(view);
                        callback(presender.m_LuaTable);
					}
				},typeof(GameObject));
			}

			private string _GetViewPath(string viewName){
				return string.Format (PathConst.ViewAsset_Path,viewName);
			}

			public void ClearViewLayer(string layerName = null){
				if (string.IsNullOrEmpty (layerName)) {
					Dictionary<string, ViewLayer>.Enumerator iter = this.m_ViewLayer.GetEnumerator ();
					while (iter.MoveNext () != null) {
						iter.Current.Value.ClearCacheList ();
					}
				} else {
					if (this.m_ViewLayer.ContainsKey (layerName)) {
						this.m_ViewLayer [layerName].ClearCacheList ();
					} else {
						Debug.LogError ("Found out Layer. layerName:" + layerName);
					}
				}
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