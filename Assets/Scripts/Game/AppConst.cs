using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Framework
{
    namespace Game
    {
		public enum eChange{
			Official = 1,
		}

        public class AppConst
        {
			public const string AppName = "xLuaFramework";

			public const string ResVersion = "1111";
			public const string LuaVersion = "2222";
			public const string XlsVersion = "3333";
			public static string AssetVersion{
				get{ 
					return string.Format ("{0}.{1}.{2}", ResVersion, LuaVersion, XlsVersion);
				}
			}

			public const eChange Change = eChange.Official;

            public const string AppInfoFileName = "AASS.txt";
            public const string FileServerAddress = "https://source-1257834619.cos.ap-chengdu.myqcloud.com/HangUpGame/";

			public static bool ActiveAssetUpdater = true;

			public static string GetPlatformName ()
			{
				#if UNITY_EDITOR
				return GetPlatformForAssetBundles (UnityEditor.EditorUserBuildSettings.activeBuildTarget);
				#else
				return GetPlatformForAssetBundles(Application.platform);
				#endif
			}

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

            /** AssetBundle仿真模式 */
            public static bool AssetBundleModel = true;

#if UNITY_EDITOR

            public static string GetPlatformForAssetBundles(UnityEditor.BuildTarget target)
            {
                switch (target)
                {
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

        }
    }
}