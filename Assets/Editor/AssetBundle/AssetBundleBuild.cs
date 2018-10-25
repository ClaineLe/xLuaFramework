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
			private static void BuildAssetBundle ()
			{
				/*
				string outputPath = Path.Combine (PathConst.StreamAssetPath, AppConst.GetPlatformName ());
				if (!Directory.Exists (outputPath))
					Directory.CreateDirectory (outputPath);

				UnityEditor.AssetBundleBuild[] builds = GetBuilds (assetTypes);
				if (builds != null && builds.Length > 0) {
					BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
					BuildPipeline.BuildAssetBundles (outputPath, builds, options, EditorUserBuildSettings.activeBuildTarget);
				}	
				*/
			}

			public static void BuildAllAssetBundle(){
				string outputPath = Path.Combine (PathConst.StreamAssetPath, AppConst.GetPlatformName ());
				if (!Directory.Exists (outputPath))
					Directory.CreateDirectory (outputPath);

				BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
				Debug.Log (EditorUserBuildSettings.activeBuildTarget);
				BuildPipeline.BuildAssetBundles (outputPath, options, EditorUserBuildSettings.activeBuildTarget);
			}

			private static UnityEditor.AssetBundleBuild[] GetBuilds(){
				/*
				List<UnityEditor.AssetBundleBuild> buildList = new List<UnityEditor.AssetBundleBuild> ();
				List<string> abNameList = new List<string>(AssetDatabase.GetAllAssetBundleNames ());
				abNameList = abNameList.FindAll (a => assetTypes.Exists (b => a.StartsWith (b.ToString ().ToLower())));
				for (int i = 0; i < abNameList.Count; i++) {
					UnityEditor.AssetBundleBuild build = new UnityEditor.AssetBundleBuild ();
					build.assetBundleName = abNameList [i];
					build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle (abNameList [i]);
					buildList.Add (build);
				}
				return buildList.ToArray ();
				*/
				return null;
			}
		}
	}
}