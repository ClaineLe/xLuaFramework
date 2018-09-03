
using UnityEditor;
using UnityEditor.UI;
using Framework.Code.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(ScrollbarEx))]
        public class ScrollbarExInspector : ScrollbarEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<ScrollbarEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}