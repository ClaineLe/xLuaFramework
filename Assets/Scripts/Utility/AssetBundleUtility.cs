using System.IO;
using UnityEngine;
using Framework.Game;
using System.Linq;
using System.Collections.Generic;
using Framework.Core.Assistant;
using Newtonsoft.Json;


namespace Framework
{
	namespace Util
	{
        public class AssetBundleUtility
        {
			public static int Merge(List<BundleBaseInfo> bundleBaseList,string scrDirPath, string dstFilePath, int buffSize = 1024 * 2){

				FileStream FileOut = new FileStream(dstFilePath, FileMode.Create);

                string bundleListStr = string.Empty;
                for (int i = 0; i < bundleBaseList.Count; i++) {
                 
                    FileInfo scrFileInfo = new FileInfo (Path.Combine(scrDirPath, bundleBaseList[i].name));
					if (!scrFileInfo.Exists) {
						Debug.LogError ("found out path. path:" + scrFileInfo.FullName);
						continue;
					}

                    BundleInfo bundleInfo = new BundleInfo();
                    bundleInfo.name = scrFileInfo.Name;
                    bundleInfo.md5 = bundleBaseList[i].md5;
                    bundleInfo.position = FileOut.Position;
                    bundleInfo.len = scrFileInfo.Length;
                    bundleListStr += bundleInfo.ToString() + "\n";

                    FileStream fileIn = new FileStream (scrFileInfo.FullName, FileMode.Open);
					byte[] data = new byte[buffSize];
					int read_len = 0;
					while ((read_len = fileIn.Read (data, 0, data.Length)) > 0) {
						FileOut.Write (data, 0, read_len);
						FileOut.Flush ();
					}
					fileIn.Close ();
				}

                byte[] listBytes =  System.Text.Encoding.UTF8.GetBytes (bundleListStr.Trim());
				FileOut.Write (listBytes, 0, listBytes.Length);
				FileOut.Close ();
                return listBytes.Length;

            }

			public static void Split(string scrFilePath, string dstDirPath){
			
			}

        }
    }
}