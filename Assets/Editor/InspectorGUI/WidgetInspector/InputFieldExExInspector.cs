﻿
using UnityEditor;
using UnityEditor.UI;
using Framework.Core.Widget;

namespace Framework.Editor
{
    namespace Widget
    {
        [CustomEditor(typeof(InputFieldEx))]
        public class InputFieldExInspector : InputFieldEditor
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