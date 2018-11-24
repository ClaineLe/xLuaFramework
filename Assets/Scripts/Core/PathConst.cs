using UnityEngine;

namespace Framework
{
    namespace Game
    { 
        public class PathConst{
            public const string ViewRoot_Path = "Prefabs/#View/ViewRoot.prefab";
            public const string ViewLayer_Path = "Prefabs/#View/Layer.prefab";
            public const string ViewAsset_Path = "Prefabs/#View/{0}.prefab";
            public const string FORMAT_MODEL_NAME = "model.{0}.{1}";
            public const string FORMAT_VIEW_NAME = "view.{0}.{1}";
            public const string FORMAT_LUAROOT = "Lua/";
            public const string LUA_FRAMEWORK = "#core.Framework";
            public const string FORMAT_PRESENDER_NAME = "view.{0}.{1}Presender";

			public static string StreamAssetPathInAsset => Application.streamingAssetsPath + "/AssetBundles/";

            public static string LocalAssetCacheDirPath => "";
#if UNITY_EDITOR
            public const string ResRelativePath = "res/" + AppConst.ResVersion + "/";
            public const string XlsRelativePath = "xls/" + AppConst.XlsVersion + "/";
            public const string LuaRelativePath = "lua/" + AppConst.LuaVersion + "/";

            public const string ExportResDirPath = "AppAssets/";

            public static string StreamAssetPath
            {
                get
                {
                    return Application.dataPath + "/../AssetBundles/" + AppConst.Change.ToString() + "/";
                }
            }

            public static string PlayerOutPutPath
            {
                get
                {
                    return Application.dataPath + "/../PlayerOutPut/";
                }
            }
#endif
        }
    }
}