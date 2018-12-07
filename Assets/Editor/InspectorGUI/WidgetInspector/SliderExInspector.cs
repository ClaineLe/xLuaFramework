using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(SliderEx))]
        public class SliderExInspector : SliderEditor
        {
            protected override void OnHeaderGUI()
            {
                WidgetInspector.DrawHeaderGUI(target);
            }

            public override void OnInspectorGUI()
            {
                base.DrawHeader();
                base.OnInspectorGUI();
            }
        }
    }
}