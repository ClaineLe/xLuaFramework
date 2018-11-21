using UnityEngine;
using UnityEditor;
using System.IO;
using Framework.Core.Manager;
using Framework.Game;
using System.Collections.Generic;

namespace Framework.Editor
{
	namespace AssetBundle
	{
		public class AssetBundleBuild
		{
			public static void BuildAssetBundle_res ()
			{
				BuildAssetBundleBase (ResPathConst.ResRelativePath, new []{
					"prefab",
					"scene",
				});
			}

			public static void BuildAssetBundle_lua ()
			{
				BuildAssetBundleBase (ResPathConst.LuaRelativePath, new []{
					"lua",
				});
			}

			public static void BuildAssetBundle_xls ()
			{
				BuildAssetBundleBase (ResPathConst.XlsRelativePath, new []{
					"#xls",
				});
			}

			public static void BuildAssetBundle_all(){
                BuildAssetBundle_res();
                BuildAssetBundle_lua();
                BuildAssetBundle_xls();
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

			private static void BuildAssetBundleBase(string outRelativePath, string[] filterStrs){
				if (filterStrs.Length > 0) {
					UnityEditor.AssetBundleBuild[] builds = GetBuilds (filterStrs);
					if (builds.Length > 0) {
						BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
						string outputPath = Path.Combine (PathConst.StreamAssetPath, AppConst.GetPlatformForAssetBundles (target));
						outputPath = Path.Combine (outputPath, outRelativePath);
						if (!Directory.Exists (outputPath))
							Directory.CreateDirectory (outputPath);

                        BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;

						if(builds == null)
							BuildPipeline.BuildAssetBundles (outputPath, options,target );
						else
							BuildPipeline.BuildAssetBundles (outputPath, builds, options, target);
					}
				}
			}
		}
	}
}