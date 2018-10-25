
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(ToggleEx))]
        public class ToggleExInspector : ToggleEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<ToggleEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}