
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(ScrollbarEx))]
        public class ScrollbarExInspector : ScrollbarEditor
        {
            protected override void OnHeaderGUI()
            {
                WidgetEditor.WidgetCommondInspector(target);
            }

            public override void OnInspectorGUI()
            {
                base.DrawHeader();
                base.OnInspectorGUI();
            }
        }
    }
}