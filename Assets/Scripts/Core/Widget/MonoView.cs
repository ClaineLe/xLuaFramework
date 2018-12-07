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
#if UNITY_EDITOR
            public string EditorSiblingPath = string.Empty;
            public int EditorChildCnt;
#endif

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

        }
    }
}