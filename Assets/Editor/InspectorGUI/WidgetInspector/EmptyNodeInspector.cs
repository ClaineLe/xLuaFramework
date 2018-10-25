
using UnityEditor;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {

        [CustomEditor(typeof(EmptyNode))]
        public class EmptyNodeInspector : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<EmptyNode>(target);
                base.OnInspectorGUI();
            }
        }
    }
}