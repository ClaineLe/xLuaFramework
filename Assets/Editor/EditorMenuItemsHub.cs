﻿using UnityEngine;
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
			[MenuItem ("FrameworkTools/AssetBundles/MarkAssetBundleName/all")]
			static public void BuildAssetBundleName_All ()
			{
				AssetBundleMark.MarkAllAssetBundle ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/MarkAssetBundleName/clear")]
			static public void BuildAssetBundleName_Clear ()
			{
				AssetBundleMark.CleanAssetBundleName ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/lua")]
			static public void BuildAssetBundles_LUA ()
			{
				AssetBundleBuild.BuildAssetBundle_lua ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/res")]
			static public void BuildAssetBundles_RES ()
			{
				AssetBundleBuild.BuildAssetBundle_res ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/all")]
			static public void BuildAssetBundles_ALL ()
			{
				AssetBundleBuild.BuildAssetBundle_all ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/Build Player")]
			static public void BuildPlayer ()
			{
				AssetBundlePacker.BuildPlayer ();
			}
        }
    }
}