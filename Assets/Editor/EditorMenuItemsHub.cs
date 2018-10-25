using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Framework.Core.Widget;


namespace Framework.Editor
{
    using AssetBundle;
    namespace Common
    {
		public class EditorMenuItemsHub
        {
			[MenuItem ("FrameworkTools/AssetBundles/MarkAssetBundleName/All")]
			static public void BuildAssetBundleName_All ()
			{
				AssetBundleMark.MarkAllAssetBundle ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/MarkAssetBundleName/Clear")]
			static public void BuildAssetBundleName_Clear ()
			{
				AssetBundleMark.CleanAssetBundleName ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/All")]
			static public void BuildAssetBundles_Android ()
			{
				AssetBundleBuild.BuildAllAssetBundle ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build Player")]
			static public void BuildPlayer ()
			{
				AssetBundlePacker.BuildPlayer ();
			}
        }
    }
}