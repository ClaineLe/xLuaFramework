using Framework.Core.Assistant;
using Framework.Core.Widget;
using Framework.Game;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Editor
{
    namespace Widget
    {
        public class WidgetInspector : UnityEditor.Editor
        {
            public static void DrawHeaderGUI(Object target)
            {
                IWidget widget = target as IWidget;
                if (widget.ParentView != null)
                {
                    using (GUILayout.VerticalScope vs = new GUILayout.VerticalScope("IN GameObjectHeader"))
                    {
                        using (EditorGUI.ChangeCheckScope ccs = new EditorGUI.ChangeCheckScope())
                        {
                            using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                            {
                                GUILayout.Label("【引用标签】", GUILayout.Width(100));
                                widget.RefName = EditorGUILayout.TextField(widget.RefName);
                            }
                            if (ccs.changed)
                            {
                                EditorUtility.SetDirty(target);
                                Transform parent = (target as UIBehaviour).transform;
                                MonoView monoView = null;
                                while (parent != null)
                                {
                                    MonoView tmpMonoView = parent.GetComponent<MonoView>();
                                    if (tmpMonoView != null)
                                        monoView = tmpMonoView;
                                    parent = parent.parent;
                                }
                                MonoViewInspector.EditorRefreshWidgets(monoView);
                            }
                        }
                        using (GUILayout.HorizontalScope hs_0 = new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label("【父面板】", GUILayout.Width(100));
                            if (widget.ParentView)
                            {
                                if (GUILayout.Button(widget.ParentView.name + " (MonoView)", "LargeTextField", GUILayout.MinWidth(140)))
                                {
                                    EditorGUIUtility.PingObject(widget.ParentView);
                                }
                            }
                            else
                            {
                                GUILayout.Label("无父物体", "LargeTextField", GUILayout.MinWidth(140));
                            }
                        }
                    }
                }
                
                EditorApplication.RepaintHierarchyWindow();
            }

        }
    }
}