using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Framework.Core.Widget;


namespace Framework.Editor
{
    using System.IO;
    using AssetBundle;
    using Framework.Game;

    namespace Common
    {
		public class EditorMenuItemsHub
        {
            private const bool releaseBuild = true;
            [MenuItem("FrameworkTools/AssetBundles/MarkAssetBundleName/all")]
            static public void BuildAssetBundleName_All()
            {
                AssetBundleMark.MarkAllAssetBundle();
            }

            [MenuItem ("FrameworkTools/AssetBundles/MarkAssetBundleName/clear")]
			static public void BuildAssetBundleName_Clear ()
			{
				AssetBundleMark.CleanAssetBundleName ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/res")]
			static public void BuildAssetBundles_RES ()
			{
				AssetBundleBuild.BuildAssetBundle_res (releaseBuild);
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/lua")]
			static public void BuildAssetBundles_LUA ()
			{
				AssetBundleBuild.BuildAssetBundle_lua (releaseBuild);
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/xls")]
			static public void BuildAssetBundles_XLS ()
			{
				AssetBundleBuild.BuildAssetBundle_xls (releaseBuild);
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/all")]
			static public void BuildAssetBundles_ALL ()
			{
				AssetBundleBuild.BuildAssetBundle_all (releaseBuild);
			}

            [MenuItem("FrameworkTools/AssetBundles/BuildPacker/lua")]
            static public void BuildPacker_lua()
            {
                //AssetBundlePacker.BuildPacker_lua("1111", "2222");
            }

            [MenuItem("FrameworkTools/AssetBundles/BuildPacker/all")]
            static public void BuildPacker_all()
            {
                string fromVersion = "1.1.1";
                string toVersion = "1.1.2";
                AssetBundlePacker.BuildPacker_all(fromVersion, toVersion);
            }

            [MenuItem ("FrameworkTools/AssetBundles/Build Player")]
			static public void BuildPlayer_Build ()
			{
                BuildPlayer.Build(AssetBundleBuild.GetAssetVersion());
			}

            [MenuItem("FrameworkTools/CopyBundleToRunTimeDir")]
            static public void CopyBundleToRunTimeDir(){

                string dstPath = PathConst.StreamAssetPath + PathConst.BundleDirName + "/";
                string version = AssetBundleBuild.GetAssetVersion();
                AssetBundleUtility.CopyBundlesToStreamAsset(dstPath, version);
            }
        }
    }
}