using System.IO;
using Framework.Core.Manager;
using Framework.Util;
using UnityEditor;
using UnityEngine;
using Framework.Game;
using System.Collections.Generic;
using Framework.Core.Manager;
namespace Framework.Editor
{
    namespace AssetBundle
    {
		public class AssetBundleMark
        {
			public static void CleanAssetBundleName(){
				foreach (string assetbundleName in AssetDatabase.GetAllAssetBundleNames())
					AssetDatabase.RemoveAssetBundleName (assetbundleName,true);
			}

			public static void MarkAssetBundle(string[] paths){
				string basepath = Path.Combine (Application.dataPath, PathConst.ExportResDirPath);
				for (int i = 0; i < paths.Length; i++) {
					string[] assetFiles = Directory.GetFiles (Path.Combine(basepath, paths[i]), "*.*", SearchOption.AllDirectories);
					List<string> assetFileList = new List<string> (assetFiles);
					assetFileList.RemoveAll (a=>a.EndsWith (".meta") || a.EndsWith (".DS_Store"));
					for (int j = 0; j < assetFileList.Count; j++) {
						string filePath = assetFileList [j];
						string bundleName = AssetPathController.GetAssetBundleName (filePath.Replace(basepath,string.Empty));
						AssetImporter importer = AssetImporter.GetAtPath (FileUtil.GetProjectRelativePath (filePath));
						if (importer != null){
							if(!string.IsNullOrEmpty(bundleName) && importer.assetBundleName != bundleName)
								importer.assetBundleName = bundleName;
						} else {
							Debug.LogError ("Found out File. Path:" + filePath);
						}
					}
				}
			}
        
			public static void MarkAllAssetBundle(){
				MarkAssetBundle (new []{
					"Lua",
					"Prefabs",
				});
			}
		}
    }
}