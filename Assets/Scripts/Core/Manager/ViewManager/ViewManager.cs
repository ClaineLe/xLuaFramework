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
            private Dictionary<string, GameObject> m_PreLoadCache;

            private GameObject m_ViewRoot;
            public Transform m_HideNode;
            private Transform m_ViewCache;
         

            public void Init()
            {
				this.m_ViewDic = new Dictionary<string, View>();
				this.m_ViewLayer = new Dictionary<string, ViewLayer> ();
                this.m_PreLoadCache = new Dictionary<string, GameObject>();

                this._prefab_ViewRoot = Framework.Game.Manager.AssetMgr.LoadAsset (PathConst.ViewRoot_Path,typeof(GameObject)) as GameObject;
				this._prefab_ViewLayer = Framework.Game.Manager.AssetMgr.LoadAsset (PathConst.ViewLayer_Path,typeof(GameObject)) as GameObject;
				this.CreateViewRoot ();
            }

			private void CreateViewRoot(){
				this.m_ViewRoot = GameObject.Instantiate (this._prefab_ViewRoot);
				this.m_ViewRoot.name = "ViewRoot";
                this.m_ViewRoot.transform.SetParent (Framework.Game.AppFacade.Instance.transform);
                this.m_HideNode = this.m_ViewRoot.transform.Find("HideNode");
                this.m_ViewCache = this.m_ViewRoot.transform.Find("ViewCache");
            }

            public void ShowView(View view)
            {
                view.Show();
            }

            public void HideView(View view)
            {
                view.Hide();
                view.SetParent(this.m_HideNode);
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

            public void PreLoadViewNode(string viewName)
            {
                if (!HasPreLoadCache(viewName))
                {
                    GameObject viewAsset = Framework.Game.Manager.AssetMgr.LoadAsset(_GetViewPath(viewName), typeof(GameObject)) as GameObject;
                    InsertPreLoadCache(viewAsset as GameObject);
                }
            }

            public void PreLoadViewNodeAsync(string viewName, System.Action callback)
            {
                Framework.Game.Manager.AssetMgr.LoadAssetAsync(_GetViewPath(viewName), viewAsset => {
                    {
                        InsertPreLoadCache(viewAsset as GameObject);
                        if (callback != null)
                            callback();
                    }
                }, typeof(GameObject));
            }

            private void InsertPreLoadCache(GameObject viewAsset) {
                GameObject viewGo = GameObject.Instantiate(viewAsset);
                viewGo.name = viewAsset.name;
                viewGo.transform.SetParent(this.m_ViewCache);
                if (!m_PreLoadCache.ContainsKey(viewGo.name))
                {
                    m_PreLoadCache.Add(viewGo.name, viewGo);
                }
            }

            private bool HasPreLoadCache(string viewName)
            {
                return m_PreLoadCache.ContainsKey(viewName);
            }

            private GameObject GetViewNodeFromPreLoadCache(string viewName)
            {
                if (HasPreLoadCache(viewName))
                    return m_PreLoadCache[viewName];
                return null;
            }

            public XLua.LuaTable LoadViewNode(string viewName, string layerName, bool isCache = true){
                GameObject viewNode = null;
                if (!HasPreLoadCache(viewName))
                {
                    PreLoadViewNode(viewName);
                }
                viewNode = GetViewNodeFromPreLoadCache(viewName);
                if (viewNode != null)
                {
                    Presender presender = ViewUtility.CreatePresender(viewNode);
                    this.m_ViewLayer[layerName].Push(presender.m_View, isCache);
                    return presender.m_LuaTable;
                }
                else {
                    Debug.LogError("ViewNode Load Fail. view:" + viewName);
                }
                return null;
            }

			public void LoadViewNodeAsync(string viewName, System.Action<XLua.LuaTable> callback, string layerName, bool isCache = true){

                if (!HasPreLoadCache(viewName))
                {
                    PreLoadViewNodeAsync(viewName,()=> {
                        GameObject viewGo = GetViewNodeFromPreLoadCache(viewName);
                        if (viewGo != null)
                        {
                            Presender presender = ViewUtility.CreatePresender(viewGo);
                            this.m_ViewLayer[layerName].Push(presender.m_View, isCache);
                            callback(presender.m_LuaTable);
                        }
                        else {
                            Debug.LogError("ViewNode Load Fail. view:" + viewName);
                        }
                    } );
                }
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