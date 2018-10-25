
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(TextEx))]
        public class TextExInspector : TextEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<TextEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}