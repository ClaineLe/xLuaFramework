using UnityEngine;
using UnityEditor;
using System.IO;
using Framework.Core.Manager;
using Framework.Game;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Framework.Editor
{
	namespace AssetBundle
	{
		public class AssetBundleBuild
		{
            private static string versionFilePath = Application.dataPath + "/" + PathConst.ExportResDirPath + PathConst.AssetVersionFileName;

            public static bool IsExistAssetVersion(string version){
                string[] versions = version.Trim().Split('.');
                string basePath = Path.Combine(PathConst.BuildBundleRootPath, PathConst.CurChangePlatformRelativePath);
                bool existRes = Directory.Exists(Path.Combine(basePath, "res", versions[0]));
                bool existLua = Directory.Exists(Path.Combine(basePath, "lua", versions[1]));
                bool existXls = Directory.Exists(Path.Combine(basePath, "xls", versions[2]));
                return existRes && existLua && existXls;
            }

            public enum VersionFlag
            {
                res,
                lua,
                xls
            };

            public static string GetAssetVersion(){
                JObject versionJson = LoadAssetVersionJson();
                return string.Format("{0}.{1}.{2}", versionJson["res"], versionJson["lua"], versionJson["xls"]);
            }

            private static JObject LoadAssetVersionJson(){
                if(!File.Exists(versionFilePath)){
                    SaveAssetVersion("0", "0", "0");
                }
                string versionStr = File.ReadAllText(versionFilePath).Trim();
                return JObject.Parse(versionStr);
            }

            private static void SaveAssetVersion(string resVer, string luaVer, string xlsVer){
                if (File.Exists(versionFilePath))
                    File.Delete(versionFilePath);

                JObject json = new JObject();
                json.Add(VersionFlag.res.ToString(), resVer);
                json.Add(VersionFlag.lua.ToString(), luaVer);
                json.Add(VersionFlag.xls.ToString(), xlsVer);
                File.WriteAllText(versionFilePath, json.ToString());
                AssetDatabase.Refresh();
            }

            private static int AddVersion(string assetFlag, bool release){
                JObject versionJson = LoadAssetVersionJson();
                int version = versionJson[assetFlag].Value<int>();
                if(release){
                    versionJson[assetFlag] = (++version).ToString();
                    File.WriteAllText(versionFilePath, versionJson.ToString());
                    AssetDatabase.Refresh();
                }
                return version;
            }

            public static void BuildAssetBundle_res (bool release)
			{
				BuildAssetBundleBase (release, "res", new []{
					"prefab",
					"scene",
                }, PathConst.BundleDirName);
			}

			public static void BuildAssetBundle_lua (bool release)
			{
				BuildAssetBundleBase (release, "xls", new []{
					"lua",
				});
			}

			public static void BuildAssetBundle_xls (bool release)
			{
				BuildAssetBundleBase (release, "lua", new []{
					"#xls",
				});
			}

			public static void BuildAssetBundle_all(bool release)
            {
                BuildAssetBundle_res(release);
                BuildAssetBundle_lua(release);
                BuildAssetBundle_xls(release);
			}

			private static UnityEditor.AssetBundleBuild[] GetBuilds(string[] filterStrs){
				List<UnityEditor.AssetBundleBuild> buildList = new List<UnityEditor.AssetBundleBuild> ();
				List<string> abNameList = new List<string>(AssetDatabase.GetAllAssetBundleNames ());

				List<string> filters = new List<string> (filterStrs);
				if (filters != null && filters.Count > 0) 
					abNameList = abNameList.FindAll (a => filters.Exists(b=>a.StartsWith(b)));

				for (int i = 0; i < abNameList.Count; i++) {
					UnityEditor.AssetBundleBuild build = new UnityEditor.AssetBundleBuild ();
					build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle (abNameList [i]);
					build.assetBundleName = abNameList [i];
					if(build.assetNames.Length > 0)
						buildList.Add (build);
				}
				return buildList.ToArray ();
			}

            private static void BuildAssetBundleBase(bool release, string assetFlag, string[] filterStrs, string manifestName = null){
				if (filterStrs.Length > 0) {
					UnityEditor.AssetBundleBuild[] builds = GetBuilds (filterStrs);
					if (builds.Length > 0) {
                        string version = AddVersion(assetFlag, release).ToString();
                        string outputPath = Path.Combine (PathConst.BuildBundleRootPath , PathConst.CurChangePlatformRelativePath, assetFlag, version);
						if (!Directory.Exists (outputPath))
							Directory.CreateDirectory (outputPath);
                         
                        BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;

                        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
                        if (builds == null)
							BuildPipeline.BuildAssetBundles (outputPath, options,target );
						else
							BuildPipeline.BuildAssetBundles (outputPath, builds, options, target);

                        string manifestFileName = version;
                        if (string.IsNullOrEmpty(manifestName))
                        {
                            File.Delete(Path.Combine(outputPath, manifestFileName));
                            File.Delete(Path.Combine(outputPath, manifestFileName + ".manifest"));
                        }
                        else
                        {
                            File.Move(Path.Combine(outputPath, manifestFileName), Path.Combine(outputPath, manifestName));
                            File.Move(Path.Combine(outputPath, manifestFileName + ".manifest"), Path.Combine(outputPath, manifestName + ".manifest"));
                        }
					}
				}
			}
		}
	}
}