
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(ScrollRectEx))]
        public class ScrollRectExInspector : ScrollRectEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<ScrollRectEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}