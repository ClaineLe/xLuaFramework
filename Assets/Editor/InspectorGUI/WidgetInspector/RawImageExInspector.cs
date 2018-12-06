
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(RawImageEx))]
        public class RawImageExInspector : RawImageEditor
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