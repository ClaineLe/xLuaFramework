using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Framework.Code.Widget;


namespace Framework.Editor
{
    using AssetBundle;
    namespace Common
    {
		public class EditorMenuItemsHub
        {
			[MenuItem ("FrameworkTools/AssetBundles/Build AssetBundleName/All")]
			static public void BuildAssetBundleName_All ()
			{
				AssetBundleMark.BuildAssetBundleName ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build AssetBundles/Android")]
			static public void BuildAssetBundles_Android ()
			{
				AssetBundleBuild.BuildAssetBundles (BuildTarget.Android);
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build AssetBundles/Windown")]
			static public void BuildAssetBundles_Windown ()
			{
				AssetBundleBuild.BuildAssetBundles (BuildTarget.StandaloneWindows64);
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build AssetBundles/iOS")]
			static public void BuildAssetBundles_iOS ()
			{
				AssetBundleBuild.BuildAssetBundles (BuildTarget.iOS);
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build AssetBundles/OSX")]
			static public void BuildAssetBundles_OSX ()
			{
				AssetBundleBuild.BuildAssetBundles (BuildTarget.StandaloneOSX);
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build Player/Android")]
			static public void BuildPlayer_Android ()
			{
				AssetBundlePacker.BuildPlayer (BuildTarget.Android);
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build Player/Windown")]
			static public void BuildPlayer_Windown ()
			{
				AssetBundlePacker.BuildPlayer (BuildTarget.StandaloneWindows64);
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build Player/iOS")]
			static public void BuildPlayer_iOS ()
			{
				AssetBundlePacker.BuildPlayer (BuildTarget.iOS);
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build Player/OSX")]
			static public void BuildPlayer_OSX ()
			{
				AssetBundlePacker.BuildPlayer (BuildTarget.StandaloneOSX);
			}
        }
    }
}