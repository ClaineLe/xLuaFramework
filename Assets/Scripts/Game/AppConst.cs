using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Framework
{
    namespace Game
    {
        public class AppConst
        {
			public const string AppName = "xLuaFramework";

            public const int Change = 1;
            public const string AppVersion = "1.0.0";
            public const string ResVersion = "2.0.0";
            public const string LuaVersion = "3.0.0";

            public const string AppInfoFileName = "AASS.txt";
            public const string FileServerAddress = "https://source-1257834619.cos.ap-chengdu.myqcloud.com/HangUpGame/";

			public static bool ActiveAssetUpdater = true;
			/** AssetBundle仿真模式 */
			public static bool SimulateAssetBundleInEditor = false;

			public static string GetPlatformName ()
			{
				#if UNITY_EDITOR
				return GetPlatformForAssetBundles (UnityEditor.EditorUserBuildSettings.activeBuildTarget);
				#else
				return GetPlatformForAssetBundles(Application.platform);
				#endif
			}

			#if UNITY_EDITOR
			public static string GetPlatformForAssetBundles (UnityEditor.BuildTarget target)
			{
				switch (target) {
				case UnityEditor.BuildTarget.Android:
					return "Android";
				case UnityEditor.BuildTarget.iOS:
					return "iOS";
				case UnityEditor.BuildTarget.WebGL:
					return "WebGL";
				case UnityEditor.BuildTarget.StandaloneWindows:
				case UnityEditor.BuildTarget.StandaloneWindows64:
					return "Windows";
				case UnityEditor.BuildTarget.StandaloneOSX:
					return "OSX";
				default:
					return null;
				}
			}
			#endif

			public static string GetPlatformForAssetBundles (RuntimePlatform platform)
			{
				switch (platform) {
				case RuntimePlatform.Android:
					return "Android";
				case RuntimePlatform.IPhonePlayer:
					return "iOS";
				case RuntimePlatform.WebGLPlayer:
					return "WebGL";
				case RuntimePlatform.WindowsPlayer:
					return "Windows";
				case RuntimePlatform.OSXPlayer:
					return "OSX";
				default:
					return null;
				}
			}
        }
    }
}