
using UnityEditor;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(EmptyNode))]
        public class EmptyNodeInspector : UnityEditor.Editor
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