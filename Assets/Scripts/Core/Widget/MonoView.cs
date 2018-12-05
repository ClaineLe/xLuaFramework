using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Framework.Core.Widget;

namespace Framework.Core
{
	namespace Assistant
	{
        public class MonoView : MonoBehaviour, IWidget
        {
            private MonoView _parentView;
            public MonoView ParentView {
                get {
                    return _parentView;
                }
                set {
                    if (_parentView == null || !_parentView.Equals(value))
                        _parentView = value;
                }
            }
            public string refName;
            public string RefName {
                get {
                    return refName;
                }
                set {
                    if (refName == null || !refName.Equals(value))
                        refName = value;
                }
            }

            public MonoView Init(MonoView parentMonoView = null) {
                this.ParentView = parentMonoView;
                return this;
            }

            public List<MonoView> _subMonoView;
            public List<UIBehaviour> _widgets;
        
            public void SetDefaultAnchor() {
                RectTransform rectView = transform as RectTransform;
                rectView.anchorMin = Vector2.zero;
                rectView.anchorMax = Vector2.one;
                rectView.anchoredPosition3D = Vector3.zero;
                rectView.localScale = Vector3.one;
            }

#if UNITY_EDITOR

            public void Refresh()
            {
                RefreshBase(true);
            }

            private void RefreshBase(bool self){
                if(self)
                {
                    ParentView = null;
                    refName = string.Empty;
                }
                _subMonoView = new List<MonoView>();
                _widgets = new List<UIBehaviour>();

                List<IWidget> widgets = GetCom(transform);
                for (int i = 0; i < widgets.Count; i++)
                {
                    widgets[i].ParentView = this;
                    if (widgets[i] is MonoView)
                    {
                        MonoView subMonoView = widgets[i] as MonoView;
                        subMonoView.RefreshBase(false);
                        if (string.IsNullOrEmpty(widgets[i].RefName))
                            continue;
                        _subMonoView.Add(subMonoView);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(widgets[i].RefName))
                            continue;
                        _widgets.Add(widgets[i] as UIBehaviour);
                    }
                }
            }

            private MonoView GetRootMonoView(){
                Transform parent = transform;
                MonoView monoView = null;
                while(parent != null)
                {
                    MonoView tmpMonoView = parent.GetComponent<MonoView>();
                    if (tmpMonoView != null)
                        monoView = tmpMonoView;
                    parent = parent.parent;
                }
                return monoView;
            }

            private bool IsParentHasView(){
                MonoView[] parentMonoViews = transform.GetComponentsInParent<MonoView>();
                return parentMonoViews.Length > 1;
            }

            private List<IWidget> GetCom(Transform monoView)
            {
                List<IWidget> widgetList = new List<IWidget>();
                int cnt = monoView.childCount;
                for (int i = 0; i < cnt; i++)
                {
                    Transform child = monoView.GetChild(i);
                    MonoView subMonoView = child.GetComponent<MonoView>();
                    if (subMonoView == null)
                    {
                        IWidget subWidget = child.GetComponent<IWidget>();
                        if (subWidget != null)
                        {
                            widgetList.Add(subWidget);
                        }
                        widgetList.AddRange(GetCom(child));
                    }
                    else
                    {
                        widgetList.Add(subMonoView);
                    }
                }
                return widgetList;
            }


#endif
        }
    }
}