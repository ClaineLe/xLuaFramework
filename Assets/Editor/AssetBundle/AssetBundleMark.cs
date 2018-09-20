using System.IO;
using Framework.Code.Manager;
using Framework.Util;
using UnityEditor;
using UnityEngine;
using Framework.Game;

namespace Framework.Editor
{
    namespace AssetBundle
    {
		public class AssetBundleMark
        {
			public static void BuildAssetBundleName(){

				foreach (string assetbundleName in AssetDatabase.GetAllAssetBundleNames())
					AssetDatabase.RemoveAssetBundleName (assetbundleName,true);

				string path = Application.dataPath + "/" + PathConst.ExportResDirPath;
				string[] resFile = Directory.GetFiles (path, "*.*", SearchOption.AllDirectories);
				for (int i = 0; i < resFile.Length; i++) {
					if (Path.GetExtension (resFile [i]) == ".meta")
						continue;
					if (Path.GetExtension (resFile [i]) == ".DS_Store")
						continue;
					string dirName = Path.GetDirectoryName (resFile [i]);
					string fileName = Path.GetFileNameWithoutExtension (resFile [i]);
					AssetImporter assetImporter = AssetImporter.GetAtPath ("Assets/" + PathConst.ExportResDirPath + resFile[i].Replace (path, string.Empty));
					Debug.Log ("Assets/" + PathConst.ExportResDirPath + resFile[i].Replace (path, string.Empty));
					string assetPath = Path.Combine (dirName, fileName).Replace (path, string.Empty);
					assetPath = assetPath.Replace('/','_').Replace('\\','_');
					if (assetImporter.assetBundleName != assetPath) {
						assetImporter.assetBundleName = assetPath;
					}
				}
			}
        }
    }
}