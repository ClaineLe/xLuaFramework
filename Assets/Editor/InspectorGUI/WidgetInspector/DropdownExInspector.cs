
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