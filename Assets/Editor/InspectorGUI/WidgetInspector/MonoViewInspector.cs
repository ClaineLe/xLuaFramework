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

                EditorGUI.BeginDisabledGroup(true);
                if (m_Target._subMonoView != null && m_Target._subMonoView.Count > 0)
                {
                    if (showSubViewList = EditorGUILayout.Foldout(showSubViewList, "子面板"))
                    {
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        for (int i = 0; i < m_Target._subMonoView.Count; i++)
                        {
                            IWidget widget = (IWidget)m_Target._subMonoView[i];
                            if (widget != null)
                            {
                                EditorGUILayout.ObjectField(widget.RefName, m_Target._subMonoView[i], typeof(MonoView));
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }

                if (m_Target._widgets != null && m_Target._widgets.Count > 0)
                {
                    if (showWidgetList = EditorGUILayout.Foldout(showWidgetList, "UI部件"))
                    {
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        for (int i = 0; i < m_Target._widgets.Count; i++)
                        {
                            IWidget widget = (IWidget)m_Target._widgets[i];
                            if (widget != null)
                            {
                                EditorGUILayout.ObjectField(widget.RefName, m_Target._widgets[i], typeof(UIBehaviour));
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
        }
	}
}