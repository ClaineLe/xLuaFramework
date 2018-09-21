using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Code.Widget;

namespace Framework
{
    namespace Code.Manager
    {
        public partial class ManagerName
        {
            public const string View = "ViewManager";
        }
        public class ViewManager : BaseManager<ViewManager>, IManager
        {
			private const string ViewRoot_Path = "Prefabs/View/ViewRoot.prefab";
			private const string ViewAsset_Path = "Prefabs/View/{0}.prefab";

            private Dictionary<string, GameObject> m_ViewDic;

			private GameObject m_ViewRoot;

			public void Init()
            {
                this.m_ViewDic = new Dictionary<string, GameObject>();
				this.CreateViewRoot ();
            }

			private void CreateViewRoot(){
				GameObject viewRoot = Framework.Game.Manager.AssetMgr.LoadAsset (ViewRoot_Path,typeof(GameObject)) as GameObject;
				this.m_ViewRoot = GameObject.Instantiate (viewRoot);
				this.m_ViewRoot.name = "ViewRoot";
				this.m_ViewRoot.transform.SetParent (Framework.Game.AppFacade.Instance.transform);
			}

            public void Tick()
            {
				
            }

			public WidgetNode LoadViewNode(string viewName){
				GameObject viewAsset = Framework.Game.Manager.AssetMgr.LoadAsset (_GetViewPath (viewName), typeof(GameObject)) as GameObject;
				GameObject viewGo = GameObject.Instantiate (viewAsset);
				return viewGo.GetComponent<WidgetNode> ();
			}

			public void LoadViewNodeAsync(string viewName, System.Action<WidgetNode> callback){
				Framework.Game.Manager.AssetMgr.LoadAssetAsync (_GetViewPath (viewName),viewAsset=>{
					if(callback != null){
						GameObject viewGo = GameObject.Instantiate (viewAsset) as GameObject;
						callback(viewGo.GetComponent<WidgetNode> ());
					}
				},typeof(GameObject));
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