
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(EmptyButtonEx))]
        public class EmptyButtonExInspector:UnityEditor.Editor
        {
            protected override void OnHeaderGUI()
            {
                WidgetInspector.DrawHeaderGUI(target);
            }

            public override void OnInspectorGUI()
            {
                base.DrawHeader();
            }
        }
    }
}