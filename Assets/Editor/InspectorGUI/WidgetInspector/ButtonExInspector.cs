
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(ButtonEx))]
        public class ButtonExInspector : ButtonEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<ButtonEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}