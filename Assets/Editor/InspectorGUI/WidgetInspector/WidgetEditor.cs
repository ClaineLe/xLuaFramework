using Framework.Code.Widget;
using UnityEditor;


namespace Framework.Editor
{
    namespace Widget
    {
        public class WidgetEditor
        {
            public static void WidgetCommondInspector<T>(object target) where T : IWidget
            {
                IWidget widget = target as IWidget;
                widget.RefName = EditorGUILayout.DelayedTextField("引用标签", widget.RefName);
            }
        }
    }
}