﻿
using UnityEditor;
using UnityEditor.UI;
using Framework.Code.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(RawImageEx))]
        public class RawImageExInspector : RawImageEditor
        {
            public override void OnInspectorGUI()
            {
                WidgetEditor.WidgetCommondInspector<RawImageEx>(target);
                base.OnInspectorGUI();
            }
        }
    }
}