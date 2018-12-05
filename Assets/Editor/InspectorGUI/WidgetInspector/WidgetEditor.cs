using Framework.Core.Assistant;
using Framework.Core.Widget;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    namespace Widget
    {
        public class WidgetEditor
        {
            public static void WidgetCommondInspector<T>(Object target) where T : IWidget
            {
                IWidget widget = target as IWidget;
                EditorGUI.BeginChangeCheck();
                widget.RefName = EditorGUILayout.TextField("引用标签", widget.RefName);
                if (EditorGUI.EndChangeCheck()) {
                    Debug.Log("EditorUtility.SetDirty(target), param:" + widget.RefName);
                    EditorUtility.SetDirty(target);
                }

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("父视图", widget.ParentView, typeof(MonoView));
                EditorGUI.EndDisabledGroup();
                EditorApplication.RepaintHierarchyWindow ();
            }
        }
    }
}