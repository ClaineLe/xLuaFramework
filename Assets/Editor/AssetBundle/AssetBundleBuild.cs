using System.Collections.Generic;
using Framework.Util;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Framework.Editor
{
    namespace AssetBundle
    {
        public class AssetBundleBuild
        {
			public static void BuildAssetBundles(BuildTarget target, UnityEditor.AssetBundleBuild[] builds = null)
			{
				if (target != EditorUserBuildSettings.activeBuildTarget) {
					Debug.LogError("[BuildAssetBundles]Fail. ActiveBuildTarget:" + EditorUserBuildSettings.activeBuildTarget + ", destBuildTarget:" + target);
					return;
				}

				string outputPath = Path.Combine(BundleUtility.StreamAssetPath, BundleUtility.GetPlatformName());
				if (!Directory.Exists(outputPath))
					Directory.CreateDirectory(outputPath);

				BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
				if (builds == null || builds.Length == 0)
				{
					BuildPipeline.BuildAssetBundles(outputPath, options, EditorUserBuildSettings.activeBuildTarget);
				}
				else
				{
					BuildPipeline.BuildAssetBundles(outputPath, builds, options, EditorUserBuildSettings.activeBuildTarget);
				}
			}
        }
    }
}