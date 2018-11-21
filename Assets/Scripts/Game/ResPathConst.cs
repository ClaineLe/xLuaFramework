using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
	namespace Game
	{
		public class ResPathConst 
		{
			public const string ViewRoot_Path = "Prefabs/#View/ViewRoot.prefab";
			public const string ViewLayer_Path = "Prefabs/#View/Layer.prefab";
			public const string ViewAsset_Path = "Prefabs/#View/{0}.prefab";
			public const string FORMAT_MODEL_NAME = "model.{0}.{1}";
			public const string FORMAT_VIEW_NAME = "view.{0}.{1}";
			public const string FORMAT_LUAROOT = "Lua/";
			public const string LUA_FRAMEWORK = "#core.Framework";
			public const string FORMAT_PRESENDER_NAME = "view.{0}.{1}Presender";

			public static string BaseResPath{
				get{
					return PathConst.StreamAssetPath + AppConst.GetPlatformName () + "/";
				}
			}

            public static string XlsRelativePath
            {
                get {
#if UNITY_EDITOR
                        return "xls/" + AppConst.XlsVersion + "/";
#endif
                    return string.Empty;
                }
            }

            public static string LuaRelativePath
            {
                get{
#if UNITY_EDITOR
                        return "lua/" + AppConst.LuaVersion + "/";
#endif
                    return string.Empty;
                }
            }

            public static string ResRelativePath
            {
                get{
#if UNITY_EDITOR
                        return "res/" + AppConst.ResVersion + "/";
#endif
                    return string.Empty;
                }
            }
        }
	}
}