using UnityEngine;
using UnityEditor;
using Framework.Core.Widget;
using Framework.Game;
using System.Collections.Generic;

namespace Framework.Editor
{
	namespace Widget
	{
		[CustomEditor(typeof(SubView))]
		public class SubViewInspector : UnityEditor.Editor
		{
			private SubView m_Target{
				get{
					return target as SubView;
				}
			}

			public override void OnInspectorGUI()
			{
				WidgetEditor.WidgetCommondInspector<SubView>(target);
				if (m_Target.m_LuaScript == null) {
					EditorGUILayout.HelpBox ("没有找到对应Lua脚本. Path:" + m_Target.m_LuaScript, MessageType.Error);
				} else {
					EditorGUI.BeginDisabledGroup (true);
					EditorGUILayout.ObjectField ("s", m_Target.m_LuaScript, typeof(TextAsset));
					EditorGUI.EndDisabledGroup ();
				}
			}
		}
	}
}