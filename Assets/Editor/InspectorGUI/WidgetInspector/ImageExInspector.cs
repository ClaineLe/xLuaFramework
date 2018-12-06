
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(ImageEx))]
        public class ImageExInspector : ImageEditor
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