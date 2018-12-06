using UnityEngine;

namespace Framework
{
    namespace Game
    {
        public class PathConst
        {
            public const string ViewRoot_BasePath = "Prefabs/#View/";
            public const string ViewRoot_Path = ViewRoot_BasePath + "ViewRoot.prefab";
            public const string ViewLayer_Path = ViewRoot_BasePath + "Layer.prefab";
            public const string ViewAsset_Path = ViewRoot_BasePath + "{0}.prefab";
            public const string FORMAT_MODEL_NAME = "model.{0}.{1}";
            public const string FORMAT_VIEW_NAME = "view.{0}.{1}";
            public const string FORMAT_LUAROOT = "Lua/";
            public const string LUA_FRAMEWORK = "#core.Framework";
            public const string FORMAT_PRESENDER_NAME = "view.{0}.{1}Presender";
            public const string BundleDirName = "AssetBundles";
            public const string BUNDLE_INFO_LIST_FILE_NAME = "bundle_info.txt";

            //public const string FileServerAddress = "https://source-1257834619.cos.ap-chengdu.myqcloud.com/HangUpGame/AssetUpdateTest/";
            public const string FileServerAddress = "http://192.168.0.110:81/";

            public static string StreamAssetPath
            {
                get
                {
#if UNITY_EDITOR
                    string path = Application.dataPath + "/../StreamingAssets/";
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                    return path;
#else
                    return Application.streamingAssetsPath + "/";
#endif
                }
            }

            public static string PersistentDataPath {
                get {
#if UNITY_EDITOR
                    string path = Application.dataPath + "/../PersistentDatas/";
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                    return path;
#else
                    return Application.persistentDataPath + "/";
#endif
                }
            }
            public static string CurChangePlatformRelativePath {
                get {
                    return string.Format("{0}_{1}", AppConst.Change, AppConst.GetPlatformName());
                }
            }
#if UNITY_EDITOR

            public static string BuildBundleRootPath {
                get {
                    return Application.dataPath + "/../" + BundleDirName + "/";
                }
            }

            public static string ClientResourceDirPath {
                get {
                    string path = Application.dataPath + "/../client_resource/";
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                    return path;
                }
            }

            public const string ExportResDirPath = "AppAssets/";
            public static string ResVersionPath{
                get{
                    return Application.dataPath + "/" + PathConst.ExportResDirPath + "ResVersion.txt";
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