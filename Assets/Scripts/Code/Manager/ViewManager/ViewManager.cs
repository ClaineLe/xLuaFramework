using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            private Dictionary<string, GameObject> m_ViewDic;
            public void Init()
            {
                this.m_ViewDic = new Dictionary<string, GameObject>();
            }

            public void Tick()
            {
            }

            public void GetView(string viewName) {

            }

            public void CreateView(string viewName) {
                
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