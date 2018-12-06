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

                if (self)
                {
                    ParentView = null;
                    refName = string.Empty;
                    SetChildsInHierarchy(transform, true);
                }

                _subMonoView = new List<MonoView>();
                _widgets = new List<UIBehaviour>();

                List<IWidget> widgets = GetChildWidget(transform);
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

            private bool IsParentHasView(){
                MonoView[] parentMonoViews = transform.GetComponentsInParent<MonoView>();
                return parentMonoViews.Length > 1;
            }

            private List<IWidget> GetChildWidget(Transform monoView)
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
                        widgetList.AddRange(GetChildWidget(child));
                    }
                    else
                    {
                        SetChildsInHierarchy(subMonoView.transform,false);
                        widgetList.Add(subMonoView);
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