using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Framework.Core.Widget;
using Framework.Util;


namespace Framework.Editor
{
    using AssetBundle;
    namespace Common
    {
		public class EditorMenuItemsHub
        {
			[MenuItem("FrameworkTools/MarkAssetBundleName")]
            static public void BuildAssetBundleNam()
            {
				AssetBundleEditor.CleanAssetBundleName ();
                AssetBundleEditor.MarkAllAssetBundle();
            }

			[MenuItem ("FrameworkTools/BuildAssetBundles/Debug")]
			static public void BuildAssetBundles_Debug ()
			{
                int resVersion = AssetBundleEditor.BuildResVersion();
				AssetBundleEditor.BuildAssetBundle (false, resVersion);
			}

			[MenuItem ("FrameworkTools/BuildAssetBundles/Release")]
			static public void BuildAssetBundles_Release ()
			{
                int resVersion = AssetBundleEditor.BuildResVersion();
                AssetBundleEditor.BuildAssetBundle (true, resVersion);
			}

            [MenuItem ("FrameworkTools/BuildPlayer")]
			static public void BuildPlayer_Build ()
			{
                //BuildPlayer.Build("");
			}
        }
    }
}