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
            public static void Build(string assetVer)
            {
                if (AssetBundleBuild.IsExistAssetVersion(assetVer))
                {
                    Debug.LogErrorFormat("Found Version:{0} Assets!!!", assetVer);
                    return;
                }

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


                string bundleStreamAssetPath = Application.streamingAssetsPath + "/" + PathConst.BundleDirName;
                AssetBundleUtility.CopyBundlesToStreamAsset(bundleStreamAssetPath, assetVer);
                AssetDatabase.Refresh();

                BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
                BuildPipeline.BuildPlayer(levels, Path.Combine(outputPath, targetName), target, option);
            }
        }
    }
}