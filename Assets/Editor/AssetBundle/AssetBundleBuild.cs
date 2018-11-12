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
			public static void BuildAssetBundle_lua ()
			{
				UnityEditor.AssetBundleBuild[] builds = GetBuilds (new List<string>(){
					"lua_",
				});
				BuildAssetBundleBase (builds);
			}

			public static void BuildAssetBundle_res ()
			{
				UnityEditor.AssetBundleBuild[] builds = GetBuilds (null,new List<string>(){
					"lua_",
				});
				BuildAssetBundleBase (builds);
			}

			public static void BuildAssetBundle_all(){
				BuildAssetBundle_lua ();
				BuildAssetBundle_res ();
				return;
				UnityEditor.AssetBundleBuild[] builds = GetBuilds ();
				BuildAssetBundleBase (builds);
			}

			private static UnityEditor.AssetBundleBuild[] GetBuilds(List<string> startWith = null, List<string> filterStrs = null ){
				List<UnityEditor.AssetBundleBuild> buildList = new List<UnityEditor.AssetBundleBuild> ();
				List<string> abNameList = new List<string>(AssetDatabase.GetAllAssetBundleNames ());

				if (startWith != null && startWith.Count > 0) 
					abNameList = abNameList.FindAll (a => startWith.Exists(b=>a.StartsWith(b)));

				if (filterStrs != null && filterStrs.Count > 0) 
					abNameList.RemoveAll (a => filterStrs.Exists(b=>a.StartsWith(b)));

				for (int i = 0; i < abNameList.Count; i++) {
					UnityEditor.AssetBundleBuild build = new UnityEditor.AssetBundleBuild ();
					build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle (abNameList [i]);
					build.assetBundleName = abNameList [i];
					if(build.assetNames.Length > 0)
						buildList.Add (build);
				}
				return buildList.ToArray ();
			}

			private static void BuildAssetBundleBase(UnityEditor.AssetBundleBuild[] builds = null){
				System.Version resVersion = new System.Version( AppConst.ResVersion + "." + AppConst.LuaVersion);
				BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
				string outputPath = Path.Combine (PathConst.StreamAssetPath, AppConst.GetPlatformForAssetBundles (target));
				if (!Directory.Exists (outputPath))
					Directory.CreateDirectory (outputPath);

				BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
				Debug.Log (EditorUserBuildSettings.activeBuildTarget);
				if(builds == null)
					BuildPipeline.BuildAssetBundles (outputPath, options,target );
				else
					BuildPipeline.BuildAssetBundles (outputPath, builds, options, target);
			}
		}
	}
}