
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(DropdownEx))]
        public class DropdownExInspector : DropdownEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<DropdownEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}