using UnityEngine;

namespace Framework
{
    namespace Game
    {
        public class PathConst
        {
            public const string ViewRoot_Path = "Prefabs/#View/ViewRoot.prefab";
            public const string ViewLayer_Path = "Prefabs/#View/Layer.prefab";
            public const string ViewAsset_Path = "Prefabs/#View/{0}.prefab";
            public const string FORMAT_MODEL_NAME = "model.{0}.{1}";
            public const string FORMAT_VIEW_NAME = "view.{0}.{1}";
            public const string FORMAT_LUAROOT = "Lua/";
            public const string LUA_FRAMEWORK = "#core.Framework";
            public const string FORMAT_PRESENDER_NAME = "view.{0}.{1}Presender";
            public const string BundleDirName = "AssetBundles";
            public static string StreamAssetPath
            {
                get
                {
#if UNITY_EDITOR
                    return Application.dataPath + "/../StreamingAssets/";
#else
                    return Application.streamingAssetsPath + "/";
#endif
                }
            }

            public static string PersistentDataPath{
                get{
#if UNITY_EDITOR
                    return Application.dataPath + "/../PersistentDatas/";
#else
                    return Application.persistentDataPath + "/";
#endif
                }
            }
#if UNITY_EDITOR
            public static string CurChangePlatformRelativePath{
                get{
                    return string.Format("{0}_{1}/", AppConst.Change, AppConst.GetPlatformName());
                }
            }
            public static string BuildBundleRootPath{
                get{
                    return Application.dataPath + "/../" + BundleDirName + "/";
                }
            }

            public const string ExportResDirPath = "AppAssets/";


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