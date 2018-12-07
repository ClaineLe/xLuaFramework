using UnityEngine;
using UnityEditor;
using Framework.Core.Widget;
using Framework.Game;
using Framework.Core.Assistant;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Framework.Editor
{
	namespace Widget
	{
		[CustomEditor(typeof(MonoView))]
		public class MonoViewInspector : UnityEditor.Editor
		{
			private MonoView m_Target{
				get{
					return target as MonoView;
				}
			}

            private bool showSubViewList = true;
            private bool showWidgetList = true;

            private List<UIBehaviour> _WidgetList {
                get {
                    return m_Target._widgets.FindAll(a => !(a is MonoView));
                }
            }

            private List<UIBehaviour> _SubViewList {
                get {
                    return m_Target._widgets.FindAll(a => (a is MonoView));
                }
            }

            protected override void OnHeaderGUI()
            {
                WidgetInspector.DrawHeaderGUI(target);
            }

            private void FoldOutPageList(string title, List<UIBehaviour> list, bool canSelect,ref bool foldoutState)
            {
                if (list.Count <= 0)
                    return;
                foldoutState = EditorGUILayout.Foldout(foldoutState, title);
                if (foldoutState)
                {
                    using (EditorGUI.DisabledScope ds = new EditorGUI.DisabledScope(canSelect))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            IWidget widget = list[i].GetComponent<IWidget>();
                            using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label(widget.RefName, GUILayout.Width(100));
                                if (GUILayout.Button((widget as UIBehaviour).name + string.Format(" ({0})", widget.GetType().Name), "LargeTextField"))
                                {
                                    EditorGUIUtility.PingObject(list[i].GetInstanceID());
                                }
                            }
                        }
                    }
                }
            }

            public override void OnInspectorGUI()
			{
                base.DrawHeader();
                if (m_Target._widgets != null && m_Target._widgets.Count > 0)
                {
                    EditorGUILayout.Space();
                    FoldOutPageList("【子面板】", _SubViewList, m_Target.ParentView, ref showSubViewList);
                    FoldOutPageList("【控件】", _WidgetList, m_Target.ParentView, ref showWidgetList);
                    EditorGUILayout.Space();
                }
            }
        }
	}
}