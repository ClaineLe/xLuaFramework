﻿
using UnityEditor;
using UnityEditor.UI;
using Framework.Code.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(ImageEx))]
        public class ImageExInspector : ImageEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<ImageEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}