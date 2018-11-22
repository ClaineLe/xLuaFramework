using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Framework.Core.Widget;


namespace Framework.Editor
{
    using AssetBundle;
    using System.Threading.Tasks;

    namespace Common
    {
		public class EditorMenuItemsHub
        {
            [MenuItem("FrameworkTools/CompressionHelper")]
            static public void Compression()
            {
                string scrDir = @"D:\Android";
                string scrDirX = @"D:\AndroidXXX\";
                string zipFullPath = @"D:\Android.gzip";
                CompressionHelper.Compress(scrDir, zipFullPath);
                CompressionHelper.DeCompress(zipFullPath, scrDirX);
            }

            static async Task Async1()
            {
                var r = await Async2();
                var x = await Async3(r);
                Debug.LogErrorFormat("结果是 {0}", r + x);
            }

            static async Task<int> Async2()
            {
                Debug.Log("Async21");
                await Task.Delay(1000);
                Debug.Log("Async22");

                return 100;
            }

            static async Task<int> Async3(int x)
            {
                Debug.Log("Async31");
                await Task.Delay(1000);
                Debug.Log("Async32");
                return x % 7;
            }






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
				AssetBundleBuild.BuildAssetBundle_res ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/lua")]
			static public void BuildAssetBundles_LUA ()
			{
				AssetBundleBuild.BuildAssetBundle_lua ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/xls")]
			static public void BuildAssetBundles_XLS ()
			{
				AssetBundleBuild.BuildAssetBundle_xls ();
			}

			[MenuItem ("FrameworkTools/AssetBundles/BuildAssetBundles/all")]
			static public void BuildAssetBundles_ALL ()
			{
				AssetBundleBuild.BuildAssetBundle_all ();
			}

            [MenuItem("FrameworkTools/AssetBundles/BuildPacker/lua")]
            static public void BuildPacker_lua()
            {
                AssetBundlePacker.BuildPacker_lua("1111", "2222");
            }

            [MenuItem("FrameworkTools/AssetBundles/BuildPacker/all")]
            static public void BuildPacker_all()
            {
                string fromVersion = "1111.1111.3333";
                string toVersion = "1111.2222.3333";
                AssetBundlePacker.BuildPacker_all(fromVersion, toVersion);
            }

            [MenuItem ("FrameworkTools/AssetBundles/Build Player")]
			static public void BuildPlayer_Build ()
			{
                BuildPlayer.Build();
			}
        }
    }
}