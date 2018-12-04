using Framework.Game;
using UnityEngine;

namespace Framework
{
	namespace Core.Widget
	{
		public class SubView : EmptyNode
		{
			[HideInInspector]
			public string ViewScript;


			#if UNITY_EDITOR
			public TextAsset m_LuaScript;
			public string luaPath{
				get{ 
					string luaName = string.Format (PathConst.FORMAT_VIEW_NAME,ViewScript,ViewScript ).Replace(".","/") + ".txt";
					return  "Assets/" + PathConst.ExportResDirPath + PathConst.FORMAT_LUAROOT + luaName;
				}
			}
			public void InitSubView(){
				if (string.IsNullOrEmpty(ViewScript) || m_LuaScript == null) {
					ViewScript = name.Trim ().Split (' ') [0];
					m_LuaScript = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset> (luaPath);
				}
			}
			#endif
		}
	}
}