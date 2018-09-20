using UnityEditor;
using UnityEngine;
using System.IO;
using Framework.Game;
using System.Collections.Generic;


namespace Framework.Editor
{
    namespace AssetBundle
    {
        public class AssetBundlePacker
        {
			private static void CopyAssetBundlesTo(string outputPath)
			{
				FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
				Directory.CreateDirectory(outputPath);

				string outputFolder = BundleUtility.GetPlatformName();

				string source = Path.Combine(BundleUtility.StreamAssetPath, outputFolder);
				if (!Directory.Exists(source))
					Debug.Log("No assetBundle output folder, try to build the assetBundles first.");

				string destination = Path.Combine(outputPath, outputFolder);
				if (Directory.Exists(destination))
					FileUtil.DeleteFileOrDirectory(destination);

				FileUtil.CopyFileOrDirectory(source, destination);
			}


			public static void BuildPlayer(BuildTarget target)
			{
				if (target != EditorUserBuildSettings.activeBuildTarget) {
					Debug.LogError("[BuildPlayer]Fail. ActiveBuildTarget:" + EditorUserBuildSettings.activeBuildTarget + ", destBuildTarget:" + target);
					return;
				}

				var outputPath = PathConst.PlayerOutPutPath + BundleUtility.GetPlatformForAssetBundles(target);
				if (!Directory.Exists(outputPath))
					Directory.CreateDirectory(outputPath);

				List<string> levelList = new List<string>();
				for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
				{
					if (EditorBuildSettings.scenes[i].enabled)
						levelList.Add(EditorBuildSettings.scenes[i].path);
				}
				string[] levels = levelList.ToArray();


				if (levels.Length == 0)
				{
					Debug.Log("Nothing to build.");
					return;
				}

				string targetName = AppConst.AppName;
				switch (target)
				{
				case BuildTarget.Android:
					{
						targetName += ".apk";
						break;
					}
				case BuildTarget.StandaloneWindows64:
					{
						targetName += ".exe";
						break;
					}
				case BuildTarget.StandaloneOSX:
					{
						targetName += ".app";
						break;
					}
				case BuildTarget.iOS:
					{
						break;
					}
				default:
					{
						targetName = string.Empty;
						break;
					}
				}


				if (string.IsNullOrEmpty(targetName))
					return;

				// Build and copy AssetBundles.
				AssetBundleBuild.BuildAssetBundles(target);
				CopyAssetBundlesTo(BundleUtility.StreamAssetPathInAsset);
				AssetDatabase.Refresh();

				BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
				BuildPipeline.BuildPlayer(levels, Path.Combine(outputPath, targetName), target, option);
			}

        }
    }
}