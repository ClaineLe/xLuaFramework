using UnityEngine;

namespace Framework
{
    namespace Game
    { 
        public class PathConst{
			public const string ExportResDirPath = "AppAssets/";
			public static string PlayerOutPutPath {
				get {
					return Application.dataPath + "/../PlayerOutPut/";
				}
			}

			private const string AssetBundlesOutputPath = "AssetBundles";

			public static string StreamAssetPathInAsset {
				get {
					return Application.streamingAssetsPath + "/" + AssetBundlesOutputPath + "/";
				}
			}

			public static string StreamAssetPath {
				get {
					#if UNITY_EDITOR
					return Application.dataPath + "/../" + AssetBundlesOutputPath + "/";
					#else
					return StreamAssetPathInAsset;
					#endif
				}
			}
        }
    }
}