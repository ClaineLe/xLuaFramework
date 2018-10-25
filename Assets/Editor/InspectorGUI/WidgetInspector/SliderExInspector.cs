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
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<SliderEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}