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

			private TextAsset luaScript;
			private string luaPath{
				get{
					return "Assets/" + PathConst.ExportResDirPath + PathConst.FORMAT_LUAROOT + string.Format (PathConst.FORMAT_VIEW_NAME.Replace(".","/") + ".txt", m_Target.name, m_Target.name);
				}
			}

			public override void OnInspectorGUI()
			{
				if (luaScript == null) {
					luaScript = AssetDatabase.LoadAssetAtPath<TextAsset> (luaPath);
					m_Target.ViewScript = m_Target.name;
				}

				WidgetEditor.WidgetCommondInspector<SubView>(target);
				if (luaScript == null) {
					EditorGUILayout.HelpBox ("没有找到对应Lua脚本. Path:" + luaPath, MessageType.Error);
				} else {
					EditorGUI.BeginDisabledGroup (true);
					luaScript = EditorGUILayout.ObjectField("luaScript",luaScript,typeof(TextAsset),true) as TextAsset;
					EditorGUI.EndDisabledGroup ();
				}
			}
		}
	}
}