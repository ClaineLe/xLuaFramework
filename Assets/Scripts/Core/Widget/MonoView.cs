using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Framework.Core.Widget;

namespace Framework.Core
{
	namespace Assistant
	{
        public class MonoView : UIBehaviour, IWidget
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

            public List<UIBehaviour> _widgets;

            public void SetDefaultAnchor() {
                RectTransform rectView = transform as RectTransform;
                rectView.anchorMin = Vector2.zero;
                rectView.anchorMax = Vector2.one;
                rectView.anchoredPosition3D = Vector3.zero;
                rectView.localScale = Vector3.one;
            }

            
#if UNITY_EDITOR


            public void Refresh(bool rootMonoView = true){
                if (rootMonoView)
                {
                    ParentView = null;
                    refName = string.Empty;
                    SetChildsInHierarchy(transform, true);
                }

                if (_widgets == null)
                    _widgets = new List<UIBehaviour>();
                else
                    _widgets.Clear();

                List<IWidget> widgets = GetChildWidget(transform);
                for (int i = 0; i < widgets.Count; i++)
                {
                    widgets[i].ParentView = this;
                    if (widgets[i] is MonoView)
                    {
                        MonoView subMonoView = widgets[i] as MonoView;
                        subMonoView.Refresh(false);
                    }
                    if(!string.IsNullOrEmpty(widgets[i].RefName))
                        _widgets.Add(widgets[i] as UIBehaviour);
                }
            }

            private void SetChildsInHierarchy(Transform dst, bool show)
            { 
                Transform[] childGos = dst.GetComponentsInChildren<Transform>();
                for (int i = 0; i < childGos.Length;i++)
                {
                    if (childGos[i] != dst)
                    {
                        childGos[i].hideFlags = show ? HideFlags.None : HideFlags.HideInHierarchy;
                    }
                }
            }

            private List<IWidget> GetChildWidget(Transform monoView)
            {
                List<IWidget> widgetList = new List<IWidget>();
                int cnt = monoView.childCount;
                for (int i = 0; i < cnt; i++)
                {
                    Transform child = monoView.GetChild(i);
                    UIBehaviour subMonoView = child.GetComponent<UIBehaviour>();
                    if (subMonoView is MonoView)
                    {
                        SetChildsInHierarchy(subMonoView.transform, false);
                        widgetList.Add(subMonoView as IWidget);
                    }
                    else
                    {
                        if (subMonoView is IWidget)
                        {
                            widgetList.Add(subMonoView.GetComponent<IWidget>());
                        }
                        widgetList.AddRange(GetChildWidget(child));
                    }
                }
                return widgetList;
            }


            private string _siblingPath = string.Empty;
            private int childCnt;
            public bool IsChangeInHierarchy()
            {
                string curSiblingPath = GetSiblingPathInHierarchy();
                int curChildCnt = GetAllChildCount();
                if (_siblingPath != curSiblingPath || childCnt != curChildCnt)
                    return true;

                _siblingPath = curSiblingPath;
                childCnt = curChildCnt;
                return false;
            }

            public int GetAllChildCount()
            {
                return transform.GetComponentsInChildren<Transform>().Length - 1;
            }
            private string GetSiblingPathInHierarchy() {
                string path = string.Empty;
                Transform parent = transform;
                while (parent != null)
                {
                    path += parent.GetSiblingIndex();
                    parent = parent.parent;
                }
                return path;
            }
#endif
        }
    }
}