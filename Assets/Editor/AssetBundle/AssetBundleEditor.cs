using UnityEngine;
using UnityEditor;
using System.IO;
using Framework.Core.Manager;
using Framework.Game;
using System.Collections.Generic;
using Framework.Util;
using Framework.Core.Assistant;
using System.Linq;

namespace Framework.Editor
{
	namespace AssetBundle
	{
		public class AssetBundleEditor
		{
			public static string[] filterStrs = new []{
                "#Xls",
                "Lua",
                "Prefabs",
                "Scenes"
            };

            public static int GetResVersion()
            {
                int resVersion = 0;
                if (!File.Exists(PathConst.ResVersionPath))
                {
                    SaveResVersion(resVersion);
                }
                return int.Parse(File.ReadAllText(PathConst.ResVersionPath));
            }

            public static int BuildResVersion()
            {
                int resVersion = GetResVersion() + 1;
                SaveResVersion(resVersion);
                return resVersion;
            }

            private static void SaveResVersion(int version) {
                File.WriteAllText(PathConst.ResVersionPath, version.ToString());
                AssetDatabase.Refresh();
            }

            public static void CleanAssetBundleName()
            {
                foreach (string assetbundleName in AssetDatabase.GetAllAssetBundleNames())
                    AssetDatabase.RemoveAssetBundleName(assetbundleName, true);
            }

            public static void MarkAssetBundle(string[] paths)
            {
                string basepath = Path.Combine(Application.dataPath, PathConst.ExportResDirPath);
                for (int i = 0; i < paths.Length; i++)
                {
                    string[] assetFiles = Directory.GetFiles(Path.Combine(basepath, paths[i]), "*.*", SearchOption.AllDirectories);
                    List<string> assetFileList = new List<string>(assetFiles);
                    assetFileList.RemoveAll(a => a.EndsWith(".meta") || a.EndsWith(".DS_Store"));
                    for (int j = 0; j < assetFileList.Count; j++)
                    {
                        string filePath = assetFileList[j];
                        string bundleName = AssetPathController.GetAssetBundleName(filePath.Replace(basepath, string.Empty));
                        AssetImporter importer = AssetImporter.GetAtPath(FileUtil.GetProjectRelativePath(filePath));
                        if (importer != null)
                        {
                            if (!string.IsNullOrEmpty(bundleName) && importer.assetBundleName != bundleName)
                                importer.assetBundleName = bundleName;
                        }
                        else
                        {
                            Debug.LogError("Found out File. Path:" + filePath);
                        }
                    }
                }
            }

            public static void MarkAllAssetBundle()
            {
                MarkAssetBundle(AssetBundleEditor.filterStrs);
            }

            public static void BuildAssetBundle(bool release, int resVersion)
            {
				UnityEditor.AssetBundleBuild[] abBuilds = GetBuilds (filterStrs);
				BuildAssetBundleBase (release , resVersion, abBuilds);
			}

            private static AssetBundleBuild[] GetBuilds(string[] filterStrs){
				List<AssetBundleBuild> buildList = new List<AssetBundleBuild> ();
				List<string> abNameList = new List<string>(AssetDatabase.GetAllAssetBundleNames ());

				List<string> filters = new List<string> (filterStrs);
				if (filters != null && filters.Count > 0) 
					abNameList = abNameList.FindAll (a => filters.Exists(b=>a.StartsWith(b.ToLower())));

				for (int i = 0; i < abNameList.Count; i++) {
					AssetBundleBuild build = new AssetBundleBuild ();
					build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle (abNameList [i]);
					build.assetBundleName = abNameList [i];
					if(build.assetNames.Length > 0)
						buildList.Add (build);
				}
				return buildList.ToArray ();
			}

			private static void BuildAssetBundleBase(bool release, int resVersion, AssetBundleBuild[] builds){
				if (builds.Length > 0) {
					string outputPath = Path.Combine (PathConst.BuildBundleRootPath, PathConst.CurChangePlatformRelativePath, PathConst.BundleDirName);
					if (builds.Length > 0) {
						if (!Directory.Exists (outputPath))
							Directory.CreateDirectory (outputPath);
                         
                        BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
						if (release)
							options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;

                        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
                        if (builds == null)
							BuildPipeline.BuildAssetBundles (outputPath, options,target );
						else
							BuildPipeline.BuildAssetBundles (outputPath, builds, options, target);
					}
                    ClientBundleInfo clientBundleInfo = CreateClientBundleInf(resVersion, outputPath);
                    MergeAssetBundle (resVersion, clientBundleInfo.bundleList, outputPath);
					if(release)
						FileUtility.DirCopy(outputPath,PathConst.StreamAssetPath + "AssetBundles/",new []{".manifest"});
				}
			}

            private static ClientBundleInfo CreateClientBundleInf(int resVersion, string dirPath)
            {
                string[] bundleFiles = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);
                bundleFiles = bundleFiles.Where(s => (!s.EndsWith(".manifest") && !s.EndsWith(".txt"))).ToArray();
                ClientBundleInfo clientBundleInfo = new ClientBundleInfo();
                clientBundleInfo.resVersion = resVersion;
                for (int i = 0; i < bundleFiles.Length; i++)
                {
                    FileInfo scrFileInfo = new FileInfo(bundleFiles[i]);
                    if (!scrFileInfo.Exists)
                    {
                        Debug.LogError("found out path. path:" + scrFileInfo.FullName);
                        continue;
                    }
                    BundleBaseInfo baseInfo = new BundleBaseInfo();
                    baseInfo.name = scrFileInfo.Name;
                    baseInfo.md5 = MD5Helper.GetMD5HashFromFile(scrFileInfo.FullName);
                    clientBundleInfo.bundleList.Add(baseInfo);
                }
                File.WriteAllText(Path.Combine(dirPath, PathConst.BUNDLE_INFO_LIST_FILE_NAME), clientBundleInfo.ToString());
                return clientBundleInfo;
            }

            private static void MergeAssetBundle(int resVersion, List<BundleBaseInfo> bundleList, string dirPath){
				string dstPath = PathConst.ClientResourceDirPath + (PathConst.CurChangePlatformRelativePath + "_" + PathConst.BundleDirName + ".txt").ToLower();
				int listStrLen = AssetBundleUtility.Merge (bundleList, dirPath, dstPath);
                AppInfo appInfo = new AppInfo();
                appInfo.appVersion = Application.version;
                appInfo.resVersion = resVersion;
                appInfo.listContentSize = listStrLen;
                appInfo.change = AppConst.Change;
                appInfo.platform = AppConst.GetPlatform();
				DirectoryInfo dirInfo = new DirectoryInfo (dirPath);
				string bundleInfoPath = PathConst.ClientResourceDirPath + (PathConst.CurChangePlatformRelativePath + "_" + PathConst.BUNDLE_INFO_LIST_FILE_NAME).ToLower();
				File.WriteAllText (bundleInfoPath, appInfo.ToString());
			}
		}
	}
}