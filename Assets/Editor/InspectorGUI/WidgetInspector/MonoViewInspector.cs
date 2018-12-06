using UnityEngine;
using UnityEditor;
using Framework.Core.Widget;
using Framework.Game;
using Framework.Core.Assistant;
using UnityEngine.EventSystems;

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

            protected override void OnHeaderGUI()
            {
                WidgetEditor.WidgetCommondInspector(target);
            }

            public override void OnInspectorGUI()
			{
                base.DrawHeader();
                if (m_Target._widgets != null && m_Target._widgets.Count > 0)
                {
                    EditorGUILayout.Space();
                    showWidgetList = EditorGUILayout.Foldout(showWidgetList, "部件");
                    if(showWidgetList)
                    {
                        using (GUILayout.VerticalScope vs_0 = new GUILayout.VerticalScope())
                        {
                            for (int i = 0; i < m_Target._widgets.Count; i++)
                            {
                                IWidget widget = m_Target._widgets[i].GetComponent<IWidget>();
                                if (widget != null)
                                {
                                    using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                                    {
                                        GUILayout.Label(widget.RefName, GUILayout.Width(100));
                                        if (GUILayout.Button((widget as UIBehaviour).name + string.Format(" ({0})", widget.GetType().Name), GUI.skin.FindStyle("LargeTextField")))
                                        {
                                            EditorGUIUtility.PingObject(m_Target._widgets[i].GetInstanceID());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    EditorGUILayout.Space();
                }
            }
        }
	}
}