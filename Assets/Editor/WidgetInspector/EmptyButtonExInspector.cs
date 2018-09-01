
using UnityEditor;
using UnityEditor.UI;
using Framework.Code.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(EmptyButtonEx))]
        public class EmptyButtonExInspector : ButtonEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<EmptyButtonEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}