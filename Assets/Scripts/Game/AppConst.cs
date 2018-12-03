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

		public enum ePlatform{
			Null,
			Android,
			iOS,
			Windows,
			OSX,
		}

        public class AppConst
        {
			public const string AppName = "xLuaFramework";

			public const eChange Change = eChange.Official;

			public static bool ActiveAssetUpdater = true;

			public static string GetPlatformName ()
			{
				return GetPlatform ().ToString ();
			}

			public static ePlatform GetPlatform(){
				#if UNITY_EDITOR
				return GetPlatformForAssetBundles (UnityEditor.EditorUserBuildSettings.activeBuildTarget);
				#else
				return GetPlatformForAssetBundles(Application.platform);
				#endif
			}

			public static ePlatform GetPlatformForAssetBundles (RuntimePlatform platform)
			{
				switch (platform) {
					case RuntimePlatform.Android:
					return ePlatform.Android;
					case RuntimePlatform.IPhonePlayer:
					return ePlatform.iOS;
					case RuntimePlatform.WindowsPlayer:
					return ePlatform.Windows;
					case RuntimePlatform.OSXPlayer:
					return ePlatform.OSX;
					default:
					return ePlatform.Null;
				}
			}

#if UNITY_EDITOR
			public static ePlatform GetPlatformForAssetBundles(UnityEditor.BuildTarget target)
            {
                switch (target)
                {
                    case UnityEditor.BuildTarget.Android:
						return ePlatform.Android;
                    case UnityEditor.BuildTarget.iOS:
						return ePlatform.iOS;
                    case UnityEditor.BuildTarget.StandaloneWindows:
                    case UnityEditor.BuildTarget.StandaloneWindows64:
						return ePlatform.Windows;
                    case UnityEditor.BuildTarget.StandaloneOSX:
						return ePlatform.OSX;
                    default:
						return ePlatform.Null;
                }
            }
#endif

        }
    }
}