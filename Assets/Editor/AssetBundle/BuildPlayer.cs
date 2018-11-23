using Framework.Game;
using Framework.Util;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    namespace AssetBundle
    {
        public class BuildPlayer
        { 
            private static void CopyAssetBundlesTo(string relativePath)
            {

                string source = Path.Combine(PathConst.StreamAssetPath, AppConst.GetPlatformName(), relativePath);
                if (!Directory.Exists(source))
                    Debug.Log("No assetBundle output folder, try to build the assetBundles first.");

                FileUtility.DirCopy(source, PathConst.StreamAssetPathInAsset);
            }

            private static void CopyAssetBundlesToByVersion(string version) {

                if (Directory.Exists(PathConst.StreamAssetPathInAsset))
                    Directory.Delete(PathConst.StreamAssetPathInAsset, true);

                string[] versions = version.Trim().Split('.');
                CopyAssetBundlesTo("res/" + versions[0]);
                CopyAssetBundlesTo("lua/" + versions[1]);
                CopyAssetBundlesTo("xls/" + versions[2]);

                string[] manifests = Directory.GetFiles(PathConst.StreamAssetPathInAsset, "*.manifest");
                for (int i = 0; i < manifests.Length; i++)
                    File.Delete(manifests[i]);
            }

            public static void Build(string assetVer)
            {
                BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

                var outputPath = PathConst.PlayerOutPutPath + AppConst.GetPlatformForAssetBundles(target);
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
                AssetBundleBuild.BuildAssetBundle_all();
                CopyAssetBundlesToByVersion(assetVer);
                AssetDatabase.Refresh();

                BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
                BuildPipeline.BuildPlayer(levels, Path.Combine(outputPath, targetName), target, option);
            }
        }
    }
}