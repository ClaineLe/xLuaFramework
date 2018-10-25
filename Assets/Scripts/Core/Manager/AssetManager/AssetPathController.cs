using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Framework
{
	namespace Core.Manager
	{
		public class AssetPathController{

			public static string GetAssetBundleName (string assetPath)
			{
				string bundleName = Path.GetDirectoryName (assetPath);
				FileInfo fileInfo = new FileInfo (assetPath);
				if (fileInfo.Directory.Name.StartsWith ("#")) {
					bundleName = bundleName + "_" + Path.GetFileNameWithoutExtension (assetPath);
				}
				return bundleName.Replace ('/', '_').Replace ('\\', '_').ToLower ();
			}
		}
	}
}