
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(InputFieldEx))]
        public class InputFieldExInspector : InputFieldEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<InputFieldEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}